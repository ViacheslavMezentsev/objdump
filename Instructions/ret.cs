using System.Collections.Generic;

namespace objdump.Instructions
{
    // RET - Return from Subroutine.
    // 1001 0101 0000 1000
    public static class ret
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
