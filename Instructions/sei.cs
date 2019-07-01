using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SEI - Set Global Interrupt Flag.
    // 1001 0100 0111 1000
    public class sei: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public sei() { info = new OpInfo( "SEI", "Set Global Interrupt Flag",
            new Regex( @"1001010001111000", RegexOptions.Compiled ) ); }

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
