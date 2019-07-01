using System.Text;
using System.IO;

// Исходник взят с одного из форумов http://social.msdn.microsoft.com/

namespace objdump {
    
    class EndianAwareBinaryReader : BinaryReader {

        private bool isLittleEndian;
        private byte[] buffer = new byte[8];
 
        public EndianAwareBinaryReader( Stream input, Encoding encoding, bool isLittleEndian ) : base( input, encoding ) {
            
            this.isLittleEndian = isLittleEndian;
        }
 
        public EndianAwareBinaryReader( Stream input, bool isLittleEndian ) : this( input, Encoding.UTF8, isLittleEndian ) {
        }
 
        public bool IsLittleEndian {
            
            get { return isLittleEndian; }
            set { isLittleEndian = value; }
            
        }
 
        public override unsafe double ReadDouble() {
        
            if ( isLittleEndian ) return base.ReadDouble();
            
            FillMyBuffer(8);
            
            uint num = ( uint ) ( ( ( buffer[3] | ( buffer[2] << 8 ) ) | ( buffer[1] << 0x10 ) ) | ( buffer[0] << 0x18 ) );
            uint num2 = ( uint ) ( ( ( buffer[7] | ( buffer[6] << 8 ) ) | ( buffer[5] << 0x10 ) ) | ( buffer[4] << 0x18 ) );
            ulong num3 = ( ( ulong ) num << 0x20 ) | num2;
            
            return *( ( ( double * ) &num3 ) );
            
        }
 
        public override short ReadInt16() {
        
            if ( isLittleEndian ) return base.ReadInt16();
            
            FillMyBuffer(2);
            
            return ( short ) ( buffer[1] | ( buffer[0] << 8 ) );
            
        }
 
        public override int ReadInt32() {
            
            if ( isLittleEndian ) return base.ReadInt32();
            
            FillMyBuffer(4);
            
            return ( ( ( buffer[3] | ( buffer[2] << 8 ) ) | ( buffer[1] << 0x10 ) ) | ( buffer[0] << 0x18 ) );
            
        }
 
        public override long ReadInt64() {
            
            if ( isLittleEndian ) return base.ReadInt64();
            
            FillMyBuffer(8);
            
            ulong num = ( ulong ) ( ( ( buffer[3] | ( buffer[2] << 8 ) ) | ( buffer[1] << 0x10 ) ) | ( buffer[0] << 0x18 ) );
            ulong num2 = ( ulong ) ( ( ( buffer[7] | ( buffer[6] << 8 ) ) | ( buffer[5] << 0x10 ) ) | ( buffer[4] << 0x18 ) );
            
            return ( long ) ( ( num2 << 0x20 ) | num );
            
        }
 
        public override unsafe float ReadSingle() {
            
            if ( isLittleEndian ) return base.ReadSingle();
            
            FillMyBuffer(4);
            
            uint num = ( uint ) ( ( ( buffer[3] | ( buffer[2] << 8 ) ) | ( buffer[1] << 0x10 ) ) | ( buffer[0] << 0x18 ) );
            
            return *( ( ( float * ) &num ) );
            
        }
 
        public override ushort ReadUInt16() {
        
            if ( isLittleEndian ) return base.ReadUInt16();
            
            FillMyBuffer(2);
            
            return ( ushort ) ( buffer[1] | ( buffer[0] << 8 ) );
            
        }
 
        public override uint ReadUInt32() {
            
            if ( isLittleEndian ) return base.ReadUInt32();
            
            FillMyBuffer(4);
            
            return ( uint ) ( ( ( buffer[3] | ( buffer[2] << 8 ) ) | ( buffer[1] << 0x10 ) ) | ( buffer[0] << 0x18 ) );
            
        }
 
        public override ulong ReadUInt64() {
        
            if ( isLittleEndian ) return base.ReadUInt64();
            
            FillMyBuffer(8);
            
            ulong num = ( ulong ) ( ( ( buffer[3] | ( buffer[2] << 8 ) ) | ( buffer[1] << 0x10 ) ) | ( buffer[0] << 0x18 ) );
            ulong num2 = ( ulong ) ( ( ( buffer[7] | ( buffer[6] << 8 ) ) | ( buffer[5] << 0x10 ) ) | ( buffer[4] << 0x18 ) );
            
            return ( ( num2 << 0x20 ) | num );
            
        }
 
        private void FillMyBuffer( int numBytes ){
            
            int offset = 0;
            int num2 = 0;
            
            if ( numBytes == 1 ) {
            
                num2 = BaseStream.ReadByte();
                
                if ( num2 == -1 ) {
                    
                    throw new EndOfStreamException( "Attempted to read past the end of the stream." );
                }
                
                buffer[0] = ( byte ) num2;
            
            } else {
                
                do {
                
                    num2 = BaseStream.Read( buffer, offset, numBytes - offset );
                    
                    if ( num2 == 0 ) {
                    
                        throw new EndOfStreamException( "Attempted to read past the end of the stream." );
                    }
                    
                    offset += num2;
                    
                } while ( offset < numBytes );
                
            }
            
        }
        
    }
}
