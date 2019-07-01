using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // LDS (16-bit) – Load Direct from Data Space.
    // 1010 0kkk dddd kkkk
    public class lds16
    {
        public static string Disassemble( OpInfo opInfo, List< Record > list, ref int pc )
        {
            string op;

            var item = list[ pc ];

            try
            {
                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Формируем ассемблерный вид команды.
                op = opInfo.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1010 0kkk dddd kkkk
                var Rd = 16 + Convert.ToByte( code.Substring( 8, 4 ), 2 );
                var k = Convert.ToByte( code.Substring( 5, 3 ) + code.Substring( 12, 4 ), 2 );

                op += $"R{Rd}, ${k:X2}".PadRight( Program.CommentsPad, ' ' );

                op += opInfo.Description;
            }
            catch ( Exception ex )
            {
                op = $"line: {item.LineNumber}, addr: ${item.Address:X4}, opcode: ${item.OpCode:X4} - {ex.Message}";

                throw new Exception( op );
            }

            return op;
        }
    }
}
