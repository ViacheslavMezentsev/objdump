using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // RCALL - Relative Call to Subroutine.
    // 1101 kkkk kkkk kkkk
    public class rcall: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public rcall() { info = new OpInfo( "RCALL", "Relative Call to Subroutine",
            new Regex( @"1101\d{12}", RegexOptions.Compiled ) ); }

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1101 kkkk kkkk kkkk
                // PC <- PC + k + 1
                uint k = Convert.ToUInt16( code.Substring( 4, 12 ), 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                if ( ( k & ( 1 << 11 ) ) == 0 ) {

                    k =  ++k << 1;
                    op += String.Format( "$+{0:X4} ({1:X4})", k, item.Address + k ).PadRight( Program.CommentsPad, ' ' );

                } else {

                    k = ~k << 1 & 0xFFF;
                    op += String.Format( "$-{0:X4} ({1:X4})", k, item.Address - k ).PadRight( Program.CommentsPad, ' ' );
                }
                
                // Описание.
                op += info.Description;

            } catch( Exception ex ) {

                // Отладочное сообщение в случае ошибки.
                op = String.Format( "line: {0}, addr: ${1:X4}, opcode: ${2:X4} - {3}",
                    item.LineNumber, item.Address, item.OpCode, ex.Message );

                throw new Exception( op );
            }

            return op;
        }

    }

}
