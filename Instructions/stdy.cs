using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // STD - Store Indirect From Register to Data Space using Index Y.
    // 10q0 qq1r rrrr 1qqq
    public class stdy: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public stdy() { info = new OpInfo( "STD", "Store Indirect From Register to Data Space using Index Y",
            new Regex( @"10\d0\d\d1\d{5}1\d{3}", RegexOptions.Compiled ) ); }

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 10q0 qq1r rrrr 1qqq
                var Rr = Convert.ToByte( code.Substring( 7, 5 ), 2 );
                var q = Convert.ToByte( code.Substring( 2, 1 ) + code.Substring( 4, 2 ) + code.Substring( 13, 3 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "Y+{1}, R{0}", q, Rr ).PadRight( Program.CommentsPad, ' ' );

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
