using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // NOP - No Operation.
    // 0000 0000 0000 0000
    public class nop: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public nop() { info = new OpInfo( "NOP", "No Operation",
            new Regex( @"0000000000000000", RegexOptions.Compiled ) ); }

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
