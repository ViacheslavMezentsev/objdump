using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CBR - Clear Bits in Register.
    // 0111 KKKK dddd KKKK
    public class cbr: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public cbr() { info = new OpInfo( "CBR", "Clear Bits in Register",
            new Regex( @"0111\d{12}", RegexOptions.Compiled ) ); }

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 0111 KKKK dddd KKKK
                var Rd = 16 + Convert.ToByte( code.Substring( 8, 4 ), 2 );
                var K = Convert.ToByte( code.Substring( 4, 4 ) + code.Substring( 12, 4 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "R{0}, ${1:X2}", Rd, K ).PadRight( Program.CommentsPad, ' ' );

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
