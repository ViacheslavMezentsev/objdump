using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // MULSU - Multiply Signed with Unsigned.
    // 0000 0011 0ddd 0rrr
    public static class mulsu
    {
        public static string Disassemble( OpInfo opInfo, List<Record> list, ref int pc )
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
                // 0000 0011 0ddd 0rrr
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
