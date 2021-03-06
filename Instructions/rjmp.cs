﻿using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // RJMP - Relative Jump.
    // 1100 kkkk kkkk kkkk
    public class rjmp
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
                // 1100 kkkk kkkk kkkk
                // PC <- PC + k + 1
                uint k = Convert.ToUInt16( code.Substring( 4, 12 ), 2 );

                if ( ( k & ( 1 << 11 ) ) == 0 )
                {
                    k = ( k + 1 ) << 1;
                    op += $"$+{k:X4} ({item.Address + k:X4})".PadRight( Program.CommentsPad, ' ' );
                }
                else
                {
                    k = ( ~k & 0xFFF ) << 1;
                    op += $"$-{k:X4} ({item.Address - k:X4})".PadRight( Program.CommentsPad, ' ' );
                }

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
