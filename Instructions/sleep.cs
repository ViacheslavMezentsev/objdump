using System.Collections.Generic;

namespace objdump.Instructions
{
    // SLEEP - This instruction sets the circuit in sleep mode defined by the MCU Control Register.
    // 1001 0101 1000 1000
    public static class sleep
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
