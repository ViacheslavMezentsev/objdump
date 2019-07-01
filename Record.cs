using System;

namespace objdump
{
    public class Record
    {
        public byte IsMacro;
        public UInt16 LineNumber;
        public UInt32 Address;
        public UInt32 OpCode;

        public Record( UInt16 LineNumber, UInt32 Address, UInt32 OpCode, byte IsMacro )
        {
            this.LineNumber = LineNumber;
            this.Address = Address;
            this.OpCode = OpCode;
            this.IsMacro = IsMacro;
        }
    }
}
