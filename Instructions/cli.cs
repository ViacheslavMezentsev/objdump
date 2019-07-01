using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CLI - Clear Global Interrupt Flag.
    // 1001 0100 1111 1000
    public class cli: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public cli() { info = new OpInfo( "CLI", "Clear Global Interrupt Flag",
            new Regex( @"1001010011111000", RegexOptions.Compiled ) ); }

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
