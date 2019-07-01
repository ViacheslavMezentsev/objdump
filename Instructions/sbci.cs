using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // SBCI - Subtract Immediate with Carry.
    // 0100 KKKK dddd KKKK
    public static class sbci
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
                // 0100 KKKK dddd KKKK
                var Rd = 16 + Convert.ToByte( code.Substring( 8, 4 ), 2 );
                var K = Convert.ToByte( code.Substring( 4, 4 ) + code.Substring( 12, 4 ), 2 );

                op += $"R{Rd}, ${K:X2}".PadRight( Program.CommentsPad, ' ' );

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
