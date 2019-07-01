using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SBRS - Skip if Bit in Register is Set.
    // 1111 111r rrrr 0bbb
    public class sbrs: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public sbrs() { info = new OpInfo( "SBRS", "Skip if Bit in Register is Set",
            new Regex( @"1111111\d{5}0\d{3}", RegexOptions.Compiled ) ); }

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1111 111r rrrr 0bbb
                var Rr = Convert.ToByte( code.Substring( 7, 5 ), 2 );
                var b = Convert.ToByte( code.Substring( 13, 3 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "R{0}, {1}", Rr, b ).PadRight( Program.CommentsPad, ' ' );

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
