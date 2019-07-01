using System.Collections.Generic;

namespace objdump.Instructions
{
    // EICALL - Extended Indirect Call to Subroutine.
    // 1001 0101 0001 1001
    public static class eicall
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
