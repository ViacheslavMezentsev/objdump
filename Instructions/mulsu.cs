using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // MULSU - Multiply Signed with Unsigned.
    // 0000 0011 0ddd 0rrr
    public class mulsu: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public mulsu() { info = new OpInfo( "MULSU", "Multiply Signed with Unsigned",
            new Regex( @"000000110\d{3}0\d{3}", RegexOptions.Compiled ) ); }
            
        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 0000 0011 0ddd 0rrr
                var Rd = 16 + Convert.ToByte( code.Substring( 9, 3 ), 2 );
                var Rr = 16 + Convert.ToByte( code.Substring( 13, 3 ), 2 );

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
