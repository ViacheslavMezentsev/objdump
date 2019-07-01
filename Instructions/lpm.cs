using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // LPM - Load Program Memory.
    // 1001 0101 1100 1000
    public class lpm: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public lpm() { info = new OpInfo( "LPM", "Load Program Memory",
            new Regex( @"1001010111001000", RegexOptions.Compiled ) ); }        

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
