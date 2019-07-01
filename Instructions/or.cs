using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // OR - Logical OR.
    // 0010 10rd dddd rrrr
    public class Or: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public Or() { info = new OpInfo( "OR", "Logical OR",
            new Regex( @"001010\d{10}", RegexOptions.Compiled ) ); }        

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 0010 10rd dddd rrrr
                var Rd = Convert.ToByte( code.Substring( 7, 5 ), 2 );
                var Rr = Convert.ToByte( code.Substring( 6, 1 ) + code.Substring( 12, 4 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "R{0}, R{1}", Rd, Rr ).PadRight( Program.CommentsPad, ' ' );

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
