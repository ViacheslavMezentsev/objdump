using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CLV - Clear Overflow Flag.
    // 1001 0100 1011 1000
    public class clv: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public clv() { info = new OpInfo( "CLV", "Clear Overflow Flag",
            new Regex( @"1001010010111000", RegexOptions.Compiled ) ); }        

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
