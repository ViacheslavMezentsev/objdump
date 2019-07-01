using System.Collections.Generic;

namespace objdump.Instructions
{
    // ICALL - Clear Global Interrupt Flag.
    // 1001 0101 0000 1001
    public static class icall
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
