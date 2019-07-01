using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // IJMP - Indirect Jump.
    // 1001 0100 0000 1001
    public class ijmp: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public ijmp() { info = new OpInfo( "IJMP", "Indirect Jump",
            new Regex( @"1001010000001001", RegexOptions.Compiled ) ); }

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
