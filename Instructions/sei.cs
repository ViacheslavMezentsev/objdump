using System.Collections.Generic;

namespace objdump.Instructions
{
    // SEI - Set Global Interrupt Flag.
    // 1001 0100 0111 1000
    public class sei
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
