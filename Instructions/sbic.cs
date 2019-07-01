using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // SBIC - Skip if Bit in I/O Register is Cleared.
    // 1001 1001 AAAA Abbb
    public class sbic
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
                // 1001 1001 AAAA Abbb
                var A = Convert.ToByte( code.Substring( 8, 5 ), 2 );
                var b = Convert.ToByte( code.Substring( 13, 3 ), 2 );

                op += $"${A:X2}, {b}".PadRight( Program.CommentsPad, ' ' );

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
