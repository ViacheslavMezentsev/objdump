using System.Collections.Generic;

namespace objdump.Instructions
{
    // CLI - Clear Global Interrupt Flag.
    // 1001 0100 1111 1000
    public static class cli
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
