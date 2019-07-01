using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CLZ - Clear Zero Flag.
    // 1001 0100 1001 1000
    public class clz: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public clz() { info = new OpInfo( "CLZ", "Clear Zero Flag",
        new Regex( @"1001010010011000", RegexOptions.Compiled ) ); }

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
