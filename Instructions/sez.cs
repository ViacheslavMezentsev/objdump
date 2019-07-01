using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SEZ - Set Zero Flag.
    // 1001 0100 0001 1000
    public class sez: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public sez() { info = new OpInfo( "SEZ", "Set Zero Flag",
            new Regex( @"1001010000011000", RegexOptions.Compiled ) ); }

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
