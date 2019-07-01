using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // RET - Return from Subroutine.
    // 1001 0101 0000 1000
    public class ret: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public ret() { info = new OpInfo( "RET", "Return from Subroutine",
            new Regex( @"1001010100001000", RegexOptions.Compiled ) ); }

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
