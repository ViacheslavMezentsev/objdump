using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CLC - Clear Carry Flag.
    // 1001 0100 1000 1000
    public class clc: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public clc() { info = new OpInfo( "CLC", "Clear Carry Flag",
            new Regex( @"1001010010001000", RegexOptions.Compiled ) ); }

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
