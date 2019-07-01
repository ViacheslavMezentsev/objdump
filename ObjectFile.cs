using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace objdump
{
    public class ObjectFile
    {
        private byte RecordLength;
        private byte FilesCount;
        private uint FileNamesOffset;
        private uint RecordsOffset;
        private string ObjFilePath;

        private string[] _fileNames;
        private List<List<Record>> _records;

        public ObjectFile( string fileName )
        {
            // Сохраняем полный путь к объектному файлу.
            ObjFilePath = Path.GetDirectoryName( Path.GetFullPath( fileName ) );

            var obj = new EndianAwareBinaryReader( File.Open( fileName, FileMode.Open, FileAccess.Read ), false );

            FileNamesOffset = obj.ReadUInt32();
            RecordsOffset = obj.ReadUInt32();
            RecordLength = obj.ReadByte();
            FilesCount = obj.ReadByte();

            _records = new List<List<Record>>();

            for ( var n = 0; n < FilesCount; n++ ) _records.Add( new List<Record>() );

            // Анализируем сигнатуру объектного файла.
            if ( Encoding.Default.GetString( obj.ReadBytes( 15 ) ).Equals( "AVR Object File" ) )
            {
                // Пропускаем нулевой байт.
                obj.ReadByte();

                // Считываем таблицу команд.
                var count = ( int ) ( ( FileNamesOffset - RecordsOffset ) / RecordLength );

                for ( var n = 0; n < count; n++ )
                {
                    var addr = ( uint ) ( ( obj.ReadUInt16() << 8 ) | obj.ReadByte() ) << 1;

                    var opcode = obj.ReadUInt16();
                    var filenum = obj.ReadByte();
                    var line = obj.ReadUInt16();
                    var ismacro = obj.ReadByte();

                    _records[ filenum ].Add( new Record( line, addr, opcode, ismacro ) );
                }

                // Считываем имена файлов.
                var text = Encoding.Default.GetString( obj.ReadBytes( ( int ) obj.BaseStream.Length - ( int ) FileNamesOffset - 2 ) );

                _fileNames = text.Split( '\0' );

                // Проверяем наличие файлов.
                for ( var n = 0; n < _fileNames.Length; n++ )
                {
                    // Если путь подключаемого файла относительный.
                    if ( !Path.IsPathRooted( _fileNames[n] ) )
                    {
                        _fileNames[n] = Path.Combine( ObjFilePath, _fileNames[n] );
                    }

                    //Program.LogInfo(FileNames[n]);
                }
            }
            else
            {
                Program.LogError( "Unknown object file: {0}", Path.GetFullPath( fileName ) );
            }

            obj.Close();
        }


        public void WriteLine( string FileName, string Text )
        {
            File.AppendAllText( FileName, Text + Environment.NewLine, Encoding.Default );
        }


        public static string Disassemble( List< Record > list, ref int pc )
        {
            string op;

            var item = list[ pc ];

            try
            {
                var cmd = Program.Instructions.Find( x => ( item.OpCode & x.Mask ) == x.Opcode );

                // TODO: Заменять длинные команды на команды с короткой мнемоникой.
                op = cmd != null ? cmd.Disassemble( cmd, list, ref pc ) : $".DW  ${item.OpCode:X4}";
            }
            catch ( Exception ex )
            {
                op = ex.Message;
            }

            return op;
        }


        public void WriteListing()
        {
            if ( _records.Count == 0 ) return;

            for ( var n = 0; n < _records.Count; n++ )
            {
                // Располагаем все листинги в папке целевого объектного файла.
                var listFileName = Path.Combine( ObjFilePath, Path.ChangeExtension( Path.GetFileName( _fileNames[n] ), ".lst" ) );

                try
                {
                    File.Delete( listFileName );
                }
                catch {}

                Program.LogInfo( "records: {0}, file: {1}", _records[n].Count, listFileName );

                // Пропускаем пустые файлы.
                if ( _records[n].Count == 0 ) continue;

                int m;
                var lastline = 0;
                var counter = 0;
                var list = _records[n];

                while ( counter < list.Count )
                {
                    var record = list[ counter ];
                    var line = record.LineNumber;

                    // Если строки отличаются, то выводим новую строку.
                    if ( line != lastline )
                    {
                        var src = File.ReadLines( _fileNames[n], Encoding.Default ).Skip( lastline ).Take( line - lastline );

                        m = 1;

                        foreach ( var s in src ) WriteLine( listFileName, $"{lastline + m++}:".PadRight( 10 ) + s );

                        lastline = line;
                    }

                    WriteLine( listFileName, $"{record.Address:X8}: {record.OpCode:X4}     {Disassemble(list, ref counter)}" );

                    counter++;
                }

                // Считываем остаток файла.
                var end = File.ReadLines( _fileNames[n], Encoding.Default ).Count();
                var tail = File.ReadLines( _fileNames[n], Encoding.Default ).Skip( lastline ).Take( end - lastline );

                m = 1;

                foreach ( var s in tail ) WriteLine( listFileName, $"{lastline + m++}:".PadRight( 10 ) + s );
            }
        }
    }
}
