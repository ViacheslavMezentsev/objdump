using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // STS (16-bit) – Store Direct to Data Space.
    // 1010 1kkk dddd kkkk
    public class sts16: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public sts16() { info = new OpInfo( "STS", "Store Direct to Data Space",
            new Regex( @"10101\d{11}", RegexOptions.Compiled ) ); }

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1010 1kkk dddd kkkk
                var Rr = 16 + Convert.ToByte( code.Substring( 8, 4 ), 2 );
                var k = Convert.ToByte( code.Substring( 5, 3 ) + code.Substring( 12, 4 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "${0:X2}, R{1}", k, Rr ).PadRight( Program.CommentsPad, ' ' );

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
