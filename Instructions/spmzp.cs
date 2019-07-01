using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SPM - Store Program Memory.
    // 1001 0101 1111 1000
    public class spmzp: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public spmzp() { info = new OpInfo( "SPM", "Store Program Memory",        
            new Regex( @"1001010111111000", RegexOptions.Compiled ) ); }
        
        public string Disassemble( List< Record > list, ref int counter ) { 
        
            // Формируем ассемблерный вид команды.

            // Название инструкции.
            var op = info.Name.PadRight( Program.ArgumentsPad, ' ' );
                
            // Параметры.
            op += "Z+".PadRight( Program.CommentsPad, ' ' );
                
            // Описание.
            op += info.Description;
                
            return op;
        
        }

    }

}
