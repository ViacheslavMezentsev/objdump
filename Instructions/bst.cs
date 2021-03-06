﻿using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // BST - Bit Store from Bit in Register to T Flag in SREG.
    // 1111 101d dddd 0bbb
    public static class bst
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
                // 1111 101d dddd 0bbb
                var Rd = Convert.ToByte( code.Substring( 7, 5 ), 2 );
                var b = Convert.ToByte( code.Substring( 13, 3 ), 2 );

                op += $"R{Rd}, {b}".PadRight( Program.CommentsPad, ' ' );

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
