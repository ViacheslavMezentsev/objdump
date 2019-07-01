using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // IN - Load an I/O Location to Register.
    // 1011 0AAd dddd AAAA
    public static class In
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
                // 1011 0AAd dddd AAAA
                var Rd = Convert.ToByte( code.Substring( 7, 5 ), 2 );
                var A = Convert.ToByte( code.Substring( 5, 2 ) + code.Substring( 12, 4 ), 2 );

                op += $"R{Rd}, ${A:X2}".PadRight( Program.CommentsPad, ' ' );

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
