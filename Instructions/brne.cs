﻿using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // BRNE - Branch if Minus.
    // 1111 01kk kkkk k001
    public static class brne
    {
        public static string Disassemble( OpInfo opInfo, List< Record > list, ref int pc )
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
                // 1111 01kk kkkk k001
                // PC <- PC + k + 1
                uint k = Convert.ToByte( code.Substring( 6, 7 ), 2 );

                k = ( uint ) ( ( ( k & ( 1 << 6 ) ) == 0 ) ? ++k << 1 : - ( ~k << 1 & 0x7F ) );

                op += $"${item.Address + k:X4}".PadRight( Program.CommentsPad, ' ' );

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
