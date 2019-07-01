using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CLS - Clear Signed Flag.
    // 1001 0100 1100 1000
    public class cls: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public cls() { info = new OpInfo( "CLS", "Clear Signed Flag",
            new Regex( @"1001010011001000", RegexOptions.Compiled ) ); }

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
