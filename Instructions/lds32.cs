using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // LDS - Load Direct from Data Space.
    // 1001 000d dddd 0000 kkkk kkkk kkkk kkkk
    public class lds32: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public lds32() { info = new OpInfo( "LDS", "Load Direct from Data Space",
            new Regex( @"1001000\d{5}0000", RegexOptions.Compiled ) ); }
            
        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter++ ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1001 000d dddd 0000
                var Rd = Convert.ToByte( code.Substring( 7, 5 ), 2 );

                item = list[ counter ];
                code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // 0123 4567 8901 2345
                // kkkk kkkk kkkk kkkk
                var k = Convert.ToUInt16( code, 2 );

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "R{0}, ${1:X4}", Rd, k ).PadRight( Program.CommentsPad, ' ' );

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
