using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // BLD - Bit Load from the T Flag in SREG to a Bit in Register.
    // 1111 100d dddd 0bbb
    public class bld: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public bld() { info = new OpInfo( "BLD", "Bit Load from the T Flag in SREG to a Bit in Register",
            new Regex( @"1111100\d{5}0\d{3}", RegexOptions.Compiled ) ); }        

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1111 100d dddd 0bbb
                var Rd = Convert.ToByte( code.Substring( 7, 5 ), 2 );
                var b = Convert.ToByte( code.Substring( 13, 3 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "R{0}, {1}", Rd, b ).PadRight( Program.CommentsPad, ' ' );

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
