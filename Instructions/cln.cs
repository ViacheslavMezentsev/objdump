using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CLN - Clear Negative Flag.
    // 1001 0100 1010 1000
    public class cln: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public cln() { info = new OpInfo( "CLN", "Clear Negative Flag",
            new Regex( @"1001010010101000", RegexOptions.Compiled ) ); }

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
