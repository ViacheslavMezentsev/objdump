using System;
using System.Collections.Generic;

namespace objdump
{
    public class OpInfo
    {
        public delegate TResult DisasmFunc<in T1, in T2, T3, out TResult>( T1 arg1, T2 arg2, ref T3 arg3 );

        public string Name;
        public string Description;
        public uint Opcode;
        public uint Mask;
        public DisasmFunc<OpInfo, List<Record>, int, string> Disassemble;

        public OpInfo( uint opcode, uint mask, DisasmFunc<OpInfo, List<Record>, int, string> func, string name, string description )
        {
            Opcode = opcode;
            Mask = mask;
            Name = name;
            Description = description;
            Disassemble = func;
        }
    }
}
