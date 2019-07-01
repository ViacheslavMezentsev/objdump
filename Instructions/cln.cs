using System.Collections.Generic;

namespace objdump.Instructions
{
    // CLN - Clear Negative Flag.
    // 1001 0100 1010 1000
    public static class cln
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
