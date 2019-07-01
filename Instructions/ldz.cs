using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // LD - Load Indirect From Data Space to Register using Index Z.
    // 1000 000d dddd 0000
    public class ldz: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public ldz() { info = new OpInfo( "LD", "Load Indirect From Data Space to Register using Index Z",
            new Regex( @"1000000\d{5}0000", RegexOptions.Compiled ) ); }
        
        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1000 000d dddd 0000
                var Rd = Convert.ToByte( code.Substring( 7, 5 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "R{0}, Z", Rd ).PadRight( Program.CommentsPad, ' ' );

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
