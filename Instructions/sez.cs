using System.Collections.Generic;

namespace objdump.Instructions
{
    // SEZ - Set Zero Flag.
    // 1001 0100 0001 1000
    public static class sez
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
