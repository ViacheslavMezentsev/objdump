using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


namespace objdump {

    public class Record {
              
        public byte IsMacro;
        public UInt16 LineNumber;    
        public UInt32 Address;
        public UInt32 OpCode;        

        public Record( UInt16 LineNumber, UInt32 Address, UInt32 OpCode, byte IsMacro ) {
        
            this.LineNumber = LineNumber;
            this.Address = Address;
            this.OpCode = OpCode;
            this.IsMacro = IsMacro;                

        }

    }


    public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>> {

        public void Add( TKey key, TValue value ) {

            Add( new KeyValuePair<TKey, TValue>( key, value ) );
        }

    }


    public class OpInfo {

        public string Name;        
        public string Description;
        public Regex Pattern;

        public OpInfo( string Name, string Description, Regex Pattern ) {
            
            this.Name = Name;
            this.Pattern = Pattern;
            this.Description = Description;
        }

    }


    public interface IInstruction {

        OpInfo OpInfo { get; }
        string Disassemble( List< Record > list, ref int counter );
    }


    public class AVRObjectFile {

        private byte RecordLength;
        private byte FilesCount;
        private UInt32 FileNamesOffset;
        private UInt32 RecordsOffset;
        private string ObjFilePath;

        private string[] FileNames;
        private List< List< Record > > Records;

        public AVRObjectFile( string FileName ) {

            // Сохраняем полный путь к объектному файлу.
            ObjFilePath = Path.GetDirectoryName( Path.GetFullPath( FileName ) );

            var obj = new EndianAwareBinaryReader( File.Open( FileName, FileMode.Open, FileAccess.Read ), false );

            FileNamesOffset = obj.ReadUInt32();
            RecordsOffset = obj.ReadUInt32();
            RecordLength = obj.ReadByte();
            FilesCount = obj.ReadByte();

            Records = new List< List< Record > >();

            for ( var n = 0; n < FilesCount; n++ ) Records.Add( new List< Record >() );

            // Анализируем сигнатуру объектного файла.
            if ( Encoding.Default.GetString( obj.ReadBytes( 15 ) ).Equals( "AVR Object File" ) ) {
                        
                // Пропускаем нулевой байт.
                obj.ReadByte();

                // Считываем таблицу команд.
                var Count = ( int ) ( ( FileNamesOffset - RecordsOffset ) / RecordLength );

                for ( var n = 0; n < Count; n++ ) {
                    
                    var addr = ( UInt32) ( ( obj.ReadUInt16() << 8 ) | obj.ReadByte() ) << 1;

                    var opcode = obj.ReadUInt16();
                    var filenum = obj.ReadByte();
                    var line = obj.ReadUInt16();
                    var ismacro = obj.ReadByte();

                    Records[ filenum ].Add( new Record( line, addr, opcode, ismacro ) );

                }

                // Считываем имена файлов.
                var Text = Encoding.Default.GetString( obj.ReadBytes( ( ( int ) obj.BaseStream.Length - ( int ) FileNamesOffset - 2 ) ) );
                
                FileNames = Text.Split( new[] { '\0' } );

                // Проверяем наличие файлов.
                for ( var n = 0; n < FileNames.Length; n++ ) {

                    // Если путь подключаемого файла относительный.
                    if ( !Path.IsPathRooted( FileNames[n] ) ) {

                        FileNames[n] = Path.Combine( ObjFilePath, FileNames[n] );
                    }

                    //Program.LogInfo(FileNames[n]);
                }
                    
            } else {

                Program.LogInfo( String.Format( "[ERROR] Unknown object file: {0}", Path.GetFullPath( FileName ) ) );
            }

            obj.Close();

        }

        
        public void WriteLine( string FileName, string Text ) {
                      
            File.AppendAllText( FileName, Text + Environment.NewLine, Encoding.Default );
        }


        public string Disassemble( List< Record > list, ref int counter ) {
            
            string op;
            
            var item = list[ counter ];

            try {

                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                var cmd = Program.instructions.First( x => x.OpInfo.Pattern.IsMatch( code ) );
                 
                // TODO: Заменять длинные команды на команды с короткой мнемоникой.

                op = cmd.Disassemble( list, ref counter );

            // Инструкция не поддерживается.
            } catch ( InvalidOperationException ex ) {

                op =  String.Format( ".DW  ${0:X4}", item.OpCode );

            // Прочие ошибки.
            } catch( Exception ex ) {
                
                op = ex.Message;
            }
                
            return op;

        }


        public void WriteListing() {
         
            if ( Records.Count == 0 ) return;            
            
            for ( var n = 0; n < Records.Count; n++ ) {

                // Располагаем все листинги в папке целевого объектного файла.
                var ListFileName = Path.Combine( ObjFilePath, Path.ChangeExtension( Path.GetFileName( FileNames[n] ), ".lst" ) );

                try { File.Delete( ListFileName ); } catch {}

                Program.LogInfo( String.Format( "[INFO ] records: {0}, file: {1}", Records[n].Count, ListFileName ) );

                // Пропускаем пустые файлы.
                if ( Records[n].Count == 0 ) continue;

                int m;
                var lastline = 0;
                var counter = 0;
                var list = Records[ n ];

                while ( counter < list.Count ) {
            
                    var record = list[ counter ];
                    var line = record.LineNumber;

                    // Если строки отличаются, то выводим новую строку.
                    if ( line != lastline ) {

                        var src = File.ReadLines( FileNames[n], Encoding.Default ).Skip( lastline ).Take( line - lastline );

                        m = 1;

                        foreach ( var s in src ) WriteLine( ListFileName, String.Format( "{0}:", lastline + m++ ).PadRight( 10 ) + s );

                        lastline = line;
                    }

                    WriteLine( ListFileName, String.Format( "{0:X8}: {1:X4}     {2}", 
                        record.Address, record.OpCode, Disassemble( list, ref counter ) ) );

                    // Переходим на следующую команду.
                    counter++;
                } 

                // Считываем остаток файла.
                var end = File.ReadLines( FileNames[n], Encoding.Default ).Count();
                var tail = File.ReadLines( FileNames[n], Encoding.Default ).Skip( lastline ).Take( end - lastline );


                m = 1;

                foreach ( var s in tail ) WriteLine( ListFileName, String.Format( "{0}:", lastline + m++ ).PadRight( 10 ) + s );
                  
            }

        }

    }
    

    public class Program {

        private static string logFile = Path.Combine( AssemblyDirectory, "objbump.log" );

        public static List< OpInfo > infos;
        public static List< IInstruction > instructions;

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <value>The assembly title.</value>
        public static string AssemblyTitle {

            get {

                // Get all Title attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes( typeof( AssemblyTitleAttribute ), false );

                // If there is at least one Title attribute
                if ( attributes.Length > 0 ) {

                    // Select the first one
                    var titleAttribute = ( AssemblyTitleAttribute ) attributes[ 0 ];

                    // If it is not an empty string, return it
                    if ( titleAttribute.Title != "" ) return titleAttribute.Title;

                }

                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return Path.GetFileNameWithoutExtension( Assembly.GetExecutingAssembly().CodeBase );

            }

        }

        static public string AssemblyDirectory {

            get {
                var filePath = new Uri( Assembly.GetExecutingAssembly().CodeBase ).LocalPath;
                return Path.GetDirectoryName( filePath );
            }

        }


        static public string LogFile {

            get { return logFile; }
        }


        static public void LogInfo( string text ) {

            File.AppendAllText( LogFile, String.Format( "{0} {1} {2}{3}",
                DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(),
                text, Environment.NewLine ), Console.OutputEncoding );
        }


        public static void Initialize() {

            instructions = new List< IInstruction >();
            infos = new List< OpInfo >();

            try {

                foreach ( var type in Assembly.GetExecutingAssembly().GetTypes() ) {

                    if ( !type.IsClass || !typeof( IInstruction ).IsAssignableFrom( type ) ) continue;

                    instructions.Add( ( IInstruction ) Activator.CreateInstance( type, new object[] {} ) );
                }

                foreach ( var item in instructions.Select( op => op.OpInfo ) ) infos.Add( item );

            } catch ( Exception ex ) {

                LogInfo( "[ERROR] [Initialize()] " + ex.Message );
            }

            LogInfo( String.Format( "[INFO ] {0} instructions loaded.", infos.Count ) );

        }

        static public int ArgumentsPad = 10;
        static public int CommentsPad = 20;

        static void Main( string[] args ) {
            
            try {

                // Третья цифра сборки (build) будет равна числу дней начиная с 1 января 2000 года по местному времени. 
                // Четвертая цифра ревизии (revision) будет установлена в количество секунд от полуночи по местному времени,
                // делёное пополам.
                var build = Assembly.GetExecutingAssembly().GetName().Version.Build;
                var revision = Assembly.GetExecutingAssembly().GetName().Version.Revision;

                var bdate = new DateTime( 2000, 1, 1 );
                bdate = bdate.AddDays( build );
                bdate = bdate.AddSeconds( 2 * revision );

                // Удаляем файл лога.
                try { File.Delete( LogFile ); } catch {}

                // Выводим информацию о версии и дате сборки.
                LogInfo( String.Format( "[INFO ] {0} version {1} ({2} {3})", AssemblyTitle, 
                    Assembly.GetExecutingAssembly().GetName().Version, bdate.ToLongDateString(), 
                    bdate.ToLongTimeString() ) );

                // Выводим опции командной строки.
                LogInfo( "[INFO ] options: " + args.Aggregate( ( first, second ) => String.Format( "{0}, {1}", first, second ) ) );

                // Загружаем список поддерживаемых инструкций.
                Initialize();

                // Анализируем командную строку.
                var OptionS_Enabled = false;

                // Перебираем параметры.
                var objfiles = new List< string >();

                foreach ( var arg in args ) {
                
                    // Игнорируем любые другие параметры.
                    if ( !( new Regex( @"(-\w+|--\w+)" ).IsMatch( arg ) ) ) objfiles.Add( arg );

                    if ( arg.Equals( "-S" ) ) OptionS_Enabled = true;
                }
                 
                if ( OptionS_Enabled && ( objfiles.Count > 0 ) ) { 

                    var ObjectFile = objfiles[0];

                    if ( !File.Exists( ObjectFile ) ) {
                    
                        LogInfo( "[ERROR] File not found: " + Path.GetFullPath( ObjectFile ) );
                        return;
                    }
                                    
                    logFile = Path.Combine( Path.GetDirectoryName( ObjectFile ), "objdump.log" );

                    new AVRObjectFile( ObjectFile ).WriteListing();
                }

            } catch ( Exception ex ) {
                
                LogInfo( "[ERROR] " + ex.Message );

            }

        }

    }

}
