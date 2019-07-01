using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // FMULSU - Fractional Multiply Signed with Unsigned.
    // 0000 0011 1ddd 1rrr
    public static class fmulsu
    {
        public static string Disassemble( OpInfo opInfo, List<Record> list, ref int pc )
        {
            string op;

            var item = list[ pc ];

            try
            {
                // Формируем ассемблерный вид команды.
                op = opInfo.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 0000 0011 1ddd 1rrr
                var Rd = 16 + Convert.ToByte( code.Substring( 9, 3 ), 2 );
                var Rr = 16 + Convert.ToByte( code.Substring( 13, 3 ), 2 );

                op += $"R{Rd}, R{Rr}".PadRight( Program.CommentsPad, ' ' );

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
