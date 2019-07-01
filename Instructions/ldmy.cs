using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // LD - Load Indirect from Data Space to Register using Index Y.
    // 1001 000d dddd 1010
    public class ldmy: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public ldmy() { info = new OpInfo( "LD", "Load Indirect from Data Space to Register using Index Y",
            new Regex( @"1001000\d{5}1010", RegexOptions.Compiled ) ); }
        
        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1001 000d dddd 1010
                var Rd = Convert.ToByte( code.Substring( 7, 5 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "R{0}, -Y", Rd ).PadRight( Program.CommentsPad, ' ' );

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
