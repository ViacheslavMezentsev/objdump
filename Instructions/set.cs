using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SET - Set T Flag.
    // 1001 0100 0110 1000
    public class set: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public set() { info = new OpInfo( "SET", "Set T Flag",
            new Regex( @"1001010001101000", RegexOptions.Compiled ) ); }

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
