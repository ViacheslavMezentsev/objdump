using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SEN - Set Negative Flag.
    // 1001 0100 0010 1000
    public class sen: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public sen() { info = new OpInfo( "SEN", "Set Negative Flag",
            new Regex( @"1001010000101000", RegexOptions.Compiled ) ); }

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
