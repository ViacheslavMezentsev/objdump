using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // OUT - Store Register to I/O Location.
    // 1011 1AAr rrrr AAAA
    public class Out: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public Out() { info = new OpInfo( "OUT", "Store Register to I/O Location",
            new Regex( @"10111\d{11}", RegexOptions.Compiled ) ); }

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1011 1AAr rrrr AAAA
                var Rr = Convert.ToByte( code.Substring( 7, 5 ), 2 );
                var A = Convert.ToByte( code.Substring( 5, 2 ) + code.Substring( 12, 4 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "${0:X2}, R{1}", A, Rr ).PadRight( Program.CommentsPad, ' ' );

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
