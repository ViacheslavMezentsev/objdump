using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SEV - Set Overflow Flag.
    // 1001 0100 0011 1000
    public class sev: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public sev() { info = new OpInfo( "SEV", "Set Overflow Flag", 
            new Regex( @"1001010000111000", RegexOptions.Compiled ) ); }

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
