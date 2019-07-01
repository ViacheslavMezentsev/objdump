using System.Collections.Generic;

namespace objdump.Instructions
{
    // ELPM - Extended Load Program Memory.
    // 1001 0101 1101 1000
    public static class elpm
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
