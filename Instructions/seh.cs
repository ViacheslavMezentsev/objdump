using System.Collections.Generic;

namespace objdump.Instructions
{
    // SEH - Set Half Carry Flag.
    // 1001 0100 0101 1000
    public static class seh
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
