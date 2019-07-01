using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // EICALL - Extended Indirect Call to Subroutine.
    // 1001 0101 0001 1001
    public class eicall: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public eicall() { info = new OpInfo( "EICALL", "Extended Indirect Call to Subroutine",
            new Regex( @"1001010100011001", RegexOptions.Compiled ) ); }

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
