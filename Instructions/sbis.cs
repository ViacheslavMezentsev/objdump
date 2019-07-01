using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SBIS - Skip if Bit in I/O Register is Set.
    // 1001 1011 AAAA Abbb
    public class sbis: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public sbis() { info = new OpInfo( "SBIS", "Skip if Bit in I/O Register is Set",
            new Regex( @"10011011\d{8}", RegexOptions.Compiled ) ); }
        
        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1001 1011 AAAA Abbb
                var A = Convert.ToByte( code.Substring( 8, 5 ), 2 );
                var b = Convert.ToByte( code.Substring( 13, 3 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "${0:X2}, {1}", A, b ).PadRight( Program.CommentsPad, ' ' );

                // Описание.
                op += info.Description;

            } catch( Exception ex ) {

                // Отладочное сообщение в случае ошибки.
                op = String.Format( "line: {0}, addr: ${1:X4}, opcode: ${2:X4} - {3}",
                    item.LineNumber, item.Address, item.OpCode, ex.Message );

                throw new Exception( op );
            }

            return op;
        }

    }

}
