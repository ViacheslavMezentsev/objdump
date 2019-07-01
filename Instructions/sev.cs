using System.Collections.Generic;

namespace objdump.Instructions
{
    // SEV - Set Overflow Flag.
    // 1001 0100 0011 1000
    public static class sev
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
