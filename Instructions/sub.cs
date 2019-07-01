﻿using System;
using System.Collections.Generic;

namespace objdump.Instructions
{
    // SUB - Subtract without Carry.
    // 0001 10rd dddd rrrr
    public static class sub
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
                // 0001 10rd dddd rrrr
                var Rd = Convert.ToByte( code.Substring( 7, 5 ), 2 );
                var Rr = Convert.ToByte( code.Substring( 6, 1 ) + code.Substring( 12, 4 ), 2 );

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
