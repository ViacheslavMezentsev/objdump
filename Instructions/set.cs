using System.Collections.Generic;

namespace objdump.Instructions
{
    // SET - Set T Flag.
    // 1001 0100 0110 1000
    public static class set
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
