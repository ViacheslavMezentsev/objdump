using System.Collections.Generic;

namespace objdump.Instructions
{
    // RETI - Return from Interrupt.
    // 1001 0101 0001 1000
    public static class reti
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
