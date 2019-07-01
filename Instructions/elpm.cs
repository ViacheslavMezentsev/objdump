using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // ELPM - Extended Load Program Memory.
    // 1001 0101 1101 1000
    public class elpm: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public elpm() { info = new OpInfo( "ELPM", "Extended Load Program Memory",
            new Regex( @"1001010111011000", RegexOptions.Compiled ) ); }

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
