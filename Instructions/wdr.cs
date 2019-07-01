using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // WDR - Watchdog Reset.
    // 1001 0101 1010 1000
    public class wdr: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public wdr() { info = new OpInfo( "WDR", "Watchdog Reset",
            new Regex( @"1001010110101000", RegexOptions.Compiled ) ); }

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
