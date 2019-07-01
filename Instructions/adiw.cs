using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // ADIW - Add Immediate to Word.
    // 1001 0110 KKdd KKKK
    public static class adiw
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
                // 1001 0110 KKdd KKKK
                var Rd = 24 + ( Convert.ToByte( code.Substring( 10, 2 ), 2 ) << 1 );
                var K = Convert.ToByte( code.Substring( 8, 2 ) + code.Substring( 12, 4 ), 2 );

                op += $"R{Rd + 1}:R{Rd}, {K}".PadRight( Program.CommentsPad, ' ' );

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
