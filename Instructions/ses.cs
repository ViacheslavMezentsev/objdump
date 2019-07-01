using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SES - Set Signed Flag.
    // 1001 0100 0100 1000
    public class ses: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public ses() { info = new OpInfo( "SES", "Set Signed Flag",
            new Regex( @"1001010001001000", RegexOptions.Compiled ) ); }

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
