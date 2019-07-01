using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // SLEEP - This instruction sets the circuit in sleep mode defined by the MCU Control Register.
    // 1001 0101 1000 1000
    public class sleep: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public sleep() { info = new OpInfo( "SLEEP", "This instruction sets the circuit in sleep mode defined by the MCU Control Register",
            new Regex( @"1001010110001000", RegexOptions.Compiled ) ); }

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
