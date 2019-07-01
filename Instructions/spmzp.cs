using System.Collections.Generic;

namespace objdump.Instructions
{
    // SPM - Store Program Memory.
    // 1001 0101 1111 1000
    public static class spmzp
    {
        public static string Disassemble( OpInfo opInfo, List< Record > list, ref int pc )
        { 
            // Формируем ассемблерный вид команды.
            var op = opInfo.Name.PadRight( Program.ArgumentsPad, ' ' );
                
            op += "Z+".PadRight( Program.CommentsPad, ' ' );
                
            op += opInfo.Description;
                
            return op;
        }
    }
}
