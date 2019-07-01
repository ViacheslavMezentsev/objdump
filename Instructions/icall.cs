using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // ICALL - Clear Global Interrupt Flag.
    // 1001 0101 0000 1001
    public class icall: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public icall() { info = new OpInfo( "ICALL", "Clear Global Interrupt Flag",
            new Regex( @"1001010100001001", RegexOptions.Compiled ) ); }

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
