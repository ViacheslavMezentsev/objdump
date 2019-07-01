using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SEH - Set Half Carry Flag.
    // 1001 0100 0101 1000
    public class seh: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public seh() { info = new OpInfo( "SEH", "Set Half Carry Flag",
            new Regex(  @"1001010001011000", RegexOptions.Compiled ) ); }

        public string Disassemble( List< Record > list, ref int counter ) { 
        
            // Формируем ассемблерный вид команды.

            // Название инструкции.
            var op = info.Name.PadRight( Program.ArgumentsPad + Program.CommentsPad, ' ' );
                
            // Описание.
            op += info.Description;
                
            return op;
        
        }

    }

}
