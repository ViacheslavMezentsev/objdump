using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SPM - Store Program Memory.
    // 1001 0101 1110 1000
    public class spm: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public spm() { info = new OpInfo( "SPM", "Store Program Memory",
            new Regex( @"1001010111101000", RegexOptions.Compiled ) ); }

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
