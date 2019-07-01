using System.Collections.Generic;

namespace objdump.Instructions
{
    // IJMP - Indirect Jump.
    // 1001 0100 0000 1001
    public static class ijmp
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
