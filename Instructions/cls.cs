using System.Collections.Generic;

namespace objdump.Instructions
{
    // CLS - Clear Signed Flag.
    // 1001 0100 1100 1000
    public static class cls
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
