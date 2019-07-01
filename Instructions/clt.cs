using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CLT - Clear T Flag.
    // 1001 0100 1110 1000
    public class clt: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public clt() { info = new OpInfo( "CLT", "Clear T Flag",
            new Regex( @"1001010011101000", RegexOptions.Compiled ) ); }

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
