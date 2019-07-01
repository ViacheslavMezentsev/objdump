using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CLH - Clear Half Carry Flag.
    // 1001 0100 1101 1000
    public class clh: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public clh() { info = new OpInfo( "CLH", "Clear Half Carry Flag",
            new Regex( @"1001010011011000", RegexOptions.Compiled ) ); }

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
