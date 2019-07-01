using System.Collections.Generic;

namespace objdump.Instructions
{
    // CLT - Clear T Flag.
    // 1001 0100 1110 1000
    public static class clt
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
