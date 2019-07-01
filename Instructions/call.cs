using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // CALL - Long Call to a Subroutine.
    // 1001 010k kkkk 111k kkkk kkkk kkkk kkkk
    public static class call
    {
        public static string Disassemble( OpInfo opInfo, List< Record > list, ref int pc )
        {
            string op;

            var item = list[ pc++ ];

            try
            {
                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Формируем ассемблерный вид команды.
                op = opInfo.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1001 010k kkkk 111k
                uint k = Convert.ToUInt16( code.Substring( 7, 5 ) + code.Substring( 15, 1 ), 2 );

                item = list[ pc ];
                code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // 0123 4567 8901 2345
                // kkkk kkkk kkkk kkkk
                // PC <- PC + k + 1
                k = ( k << 16 ) + Convert.ToUInt16( code, 2 );                

                op += $"${k << 1:X4}".PadRight( Program.CommentsPad, ' ' );

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
