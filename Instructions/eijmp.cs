using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // EIJMP - Extended Indirect Jump.
    // 1001 0100 0001 1001
    public class eijmp: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public eijmp() { info = new OpInfo( "EIJMP", "Extended Indirect Jump",
            new Regex( @"1001010000011001", RegexOptions.Compiled ) ); }

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
