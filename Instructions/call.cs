using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace objdump.Instructions {

    // CALL - Long Call to a Subroutine.
    // 1001 010k kkkk 111k kkkk kkkk kkkk kkkk
    public class call: IInstruction {

        public OpInfo info;
        public OpInfo OpInfo { get { return info; } }

        public call() { info = new OpInfo( "CALL", "Long Call to a Subroutine",
            new Regex( @"1001010\d{5}111\d", RegexOptions.Compiled ) ); }

        public string Disassemble( List< Record > list, ref int counter ) {

            string op;

            var item = list[ counter++ ];

            try {

                // Преобразуем в двоичное представление.
                var code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // Узнаём параметры инструкции.
                // 0123 4567 8901 2345
                // 1001 010k kkkk 111k
                uint k = Convert.ToUInt16( code.Substring( 7, 5 ) + code.Substring( 15, 1 ), 2 );

                item = list[ counter ];
                code = Convert.ToString( item.OpCode, 2 ).PadLeft( 16, '0' );

                // 0123 4567 8901 2345
                // kkkk kkkk kkkk kkkk
                // PC <- PC + k + 1
                k = ( k << 16 ) + Convert.ToUInt16( code, 2 );                

                // Формируем ассемблерный вид команды.

                // Название инструкции.
                op = info.Name.PadRight( Program.ArgumentsPad, ' ' );

                // Параметры.
                op += String.Format( "${0:X4}", k << 1 ).PadRight( Program.CommentsPad, ' ' );

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
