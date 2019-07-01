using System.Collections.Generic;

namespace objdump.Instructions
{
    // NOP - No Operation.
    // 0000 0000 0000 0000
    public static class nop
    {
        public static string Disassemble( OpInfo opInfo, List<Record> list, ref int pc )
        {
            // Формируем ассемблерный вид команды.
            var op = opInfo.Name.PadRight( Program.ArgumentsPad + Program.CommentsPad, ' ' );

            op += opInfo.Description;

            return op;
        }
    }
}
