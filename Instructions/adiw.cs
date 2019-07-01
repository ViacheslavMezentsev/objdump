using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // ADIW - Add Immediate to Word.
    // 1001 0110 KKdd KKKK
    public class adiw: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public adiw() { info = new OpInfo( "ADIW", "Add Immediate to Word",
            new Regex( @"10010110\d{8}", RegexOptions.Compiled ) ); }

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1001 0110 KKdd KKKK
                var Rd = 24 + ( Convert.ToByte( code.Substring( 10, 2 ), 2 ) << 1 );
                var K = Convert.ToByte( code.Substring( 8, 2 ) + code.Substring( 12, 4 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );
                
                // Параметры.
                op += String.Format( "R{0}:R{1}, {2}", Rd + 1, Rd, K ).PadRight( Program.CommentsPad, ' ' );

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
