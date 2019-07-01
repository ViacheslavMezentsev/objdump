using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // RETI - Return from Interrupt.
    // 1001 0101 0001 1000
    public class reti: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public reti() { info = new OpInfo( "RETI", "Return from Interrupt",
            new Regex( @"1001010100011000", RegexOptions.Compiled ) ); }

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
