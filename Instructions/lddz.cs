using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // LD - Load Indirect From Data Space to Register using Index Z.
    // 10q0 qq0d dddd 0qqq
    public static class lddz
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
                // 10q0 qq0d dddd 0qqq
                var Rd = Convert.ToByte( code.Substring( 7, 5 ), 2 );
                var q = Convert.ToByte( code.Substring( 2, 1 ) + code.Substring( 4, 2 ) + code.Substring( 13, 3 ), 2 );

                op += $"R{Rd}, Z+{q}".PadRight( Program.CommentsPad, ' ' );

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
