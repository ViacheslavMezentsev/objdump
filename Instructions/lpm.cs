using System.Collections.Generic;

namespace objdump.Instructions
{
    // LPM - Load Program Memory.
    // 1001 0101 1100 1000
    public static class lpm
    {
        public static string Disassemble( OpInfo opInfo, List< Record > list, ref int pc )
        { 
            // Формируем ассемблерный вид команды.
            var op = opInfo.Name.PadRight( Program.ArgumentsPad + Program.CommentsPad, ' ' );
                
            op += opInfo.Description;
                
            return op;
        }
    }
}
