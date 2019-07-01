using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // BREAK - Break.
    // 1001 0101 1001 1000
    public class Break: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public Break() { info = new OpInfo( "BREAK", "Break",
            new Regex( @"1001010110011000", RegexOptions.Compiled ) ); }

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
