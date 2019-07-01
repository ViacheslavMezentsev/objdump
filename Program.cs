using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using objdump.Instructions;


namespace objdump
{
    public class Program
    {
        public static List<OpInfo> Instructions = new List<OpInfo>
        {
            // ADC: 0001 11rd dddd rrrr
            new OpInfo( 0b0001_1100_0000_0000, 0b1111_1100_0000_0000, adc.Disassemble, "ADC", "Add with Carry" ),
            // ADD: 0000 11rd dddd rrrr
            new OpInfo( 0b0000_1100_0000_0000, 0b1111_1100_0000_0000, add.Disassemble, "ADD", "Add without Carry" ),
            // ADIW: 1001 0110 KKdd KKKK
            new OpInfo( 0b1001_0110_0000_0000, 0b1111_1111_0000_0000, adiw.Disassemble, "ADIW", "Add Immediate to Word" ),
            // AND: 0010 00rd dddd rrrr
            new OpInfo( 0b0010_0000_0000_0000, 0b1111_1100_0000_0000, and.Disassemble, "AND", "Logical AND" ),
            // ANDI: 0111 KKKK dddd KKKK
            new OpInfo( 0b0111_0000_0000_0000, 0b1111_0000_0000_0000, andi.Disassemble, "ANDI", "Add without Carry" ),
            // ASR: 1001 010d dddd 0101
            new OpInfo( 0b1001_0100_0000_0101, 0b1111_1110_0000_1111, asr.Disassemble, "ASR", "Arithmetic Shift Right" ),
            // BCLR: 1001 0100 1sss 1000
            new OpInfo( 0b1001_0100_1000_1000, 0b1111_1111_1000_1111, bclr.Disassemble, "BCLR", "Bit Clear in SREG" ),
            // BLD: 1111 100d dddd 0bbb
            new OpInfo( 0b1111_1000_0000_0000, 0b1111_1110_0000_1000, bld.Disassemble, "BLD", "Bit Load from the T Flag in SREG to a Bit in Register" ),
            // BRBC: 1111 01kk kkkk ksss
            new OpInfo( 0b1111_0100_0000_0000, 0b1111_1100_0000_0000, brbc.Disassemble, "BRBC", "Branch if Bit in SREG is Cleared" ),
            // BRBS: 1111 00kk kkkk ksss
            new OpInfo( 0b1111_0000_0000_0000, 0b1111_1100_0000_0000, brbs.Disassemble, "BRBS", "Branch if Bit in SREG is Set" ),
            // BRCC: 1111 01kk kkkk k000
            new OpInfo( 0b1111_0100_0000_0000, 0b1111_1100_0000_0111, brcc.Disassemble, "BRCC", "Branch if Carry Cleared" ),
            // BRCS: 1111 00kk kkkk k000
            new OpInfo( 0b1111_0000_0000_0000, 0b1111_1100_0000_0111, brcs.Disassemble, "BRCS", "Branch if Carry Set" ),
            // BREAK: 1001 0101 1001 1000
            new OpInfo( 0b1001_0101_1001_1000, 0b1111_1111_1111_1111, Break.Disassemble, "BREAK", "Break" ),
            // BREQ: 1111 00kk kkkk k001
            new OpInfo( 0b1111_0000_0000_0001, 0b1111_1100_0000_0111, breq.Disassemble, "BREQ", "Branch if Equal" ),
            // BRGE: 1111 01kk kkkk k100
            new OpInfo( 0b1111_0000_0000_0100, 0b1111_1100_0000_0111, brge.Disassemble, "BRGE", "Branch if Greater or Equal (Signed)" ),
            // BRHC: 1111 01kk kkkk k101
            new OpInfo( 0b1111_0100_0000_0101, 0b1111_1100_0000_0111, brhc.Disassemble, "BRHC", "Branch if Half Carry Flag is Cleared" ),
            // BRHS: 1111 00kk kkkk k101
            new OpInfo( 0b1111_0100_0000_0101, 0b1111_1100_0000_0111, brhs.Disassemble, "BRHS", "Branch if Half Carry Flag is Set" ),
            // BRID: 1111 01kk kkkk k111
            new OpInfo( 0b1111_0100_0000_0111, 0b1111_1100_0000_0111, brid.Disassemble, "BRID", "Branch if Global Interrupt is Disabled" ),
            // BRIE: 1111 00kk kkkk k111
            new OpInfo( 0b1111_0000_0000_0111, 0b1111_1100_0000_0111, brie.Disassemble, "BRIE", "Branch if Global Interrupt is Enabled" ),
            // BRLO: 1111 00kk kkkk k000
            new OpInfo( 0b1111_0000_0000_0000, 0b1111_1100_0000_0111, brlo.Disassemble, "BRLO", "Branch if Lower (Unsigned)" ),
            // BRLT: 1111 00kk kkkk k100
            new OpInfo( 0b1111_0000_0000_0100, 0b1111_1100_0000_0111, brlt.Disassemble, "BRLT", "Branch if Lower (Unsigned)" ),
            // BRMI: 1111 00kk kkkk k010
            new OpInfo( 0b1111_0000_0000_0010, 0b1111_1100_0000_0111, brmi.Disassemble, "BRMI", "Branch if Minus" ),
            // BRNE: 1111 01kk kkkk k001
            new OpInfo( 0b1111_0000_0000_0001, 0b1111_1100_0000_0111, brne.Disassemble, "BRNE", "Branch if Minus" ),
            // BRPL: 1111 01kk kkkk k010
            new OpInfo( 0b1111_0100_0000_0010, 0b1111_1100_0000_0111, brpl.Disassemble, "BRPL", "Branch if Plus" ),
            // BRSH: 1111 01kk kkkk k000
            new OpInfo( 0b1111_0100_0000_0000, 0b1111_1100_0000_0111, brsh.Disassemble, "BRSH", "Branch if Same or Higher (Unsigned)" ),
            // BRTC: 1111 01kk kkkk k110
            new OpInfo( 0b1111_0100_0000_0110, 0b1111_1100_0000_0111, brtc.Disassemble, "BRTC", "Branch if the T Flag is Cleared" ),
            // BRTS: 1111 00kk kkkk k110
            new OpInfo( 0b1111_0000_0000_0110, 0b1111_1100_0000_0111, brts.Disassemble, "BRTS", "Branch if Minus" ),
            // BRVC: 1111 01kk kkkk k011
            new OpInfo( 0b1111_0100_0000_0011, 0b1111_1100_0000_0111, brvc.Disassemble, "BRVC", "Branch if Overflow Cleared" ),
            // BRVS: 1111 00kk kkkk k011
            new OpInfo( 0b1111_0000_0000_0011, 0b1111_1100_0000_0111, brvs.Disassemble, "BRVS", "Branch if Overflow Set" ),
            // BST: 1111 101d dddd 0bbb
            new OpInfo( 0b1111_1010_0000_0000, 0b1111_1110_0000_1000, bst.Disassemble, "BST", "Bit Store from Bit in Register to T Flag in SREG" ),
            // CALL: 1001 010k kkkk 111k kkkk kkkk kkkk kkkk
            new OpInfo( 0b1001_0100_0000_1110, 0b1111_1110_0000_1110, call.Disassemble, "CALL", "Long Call to a Subroutine" ),
            // CBI: 1001 1000 AAAA Abbb
            new OpInfo( 0b1001_1000_0000_1110, 0b1111_1111_0000_0000, cbi.Disassemble, "CBI", "Clear Bit in I/O Register" ),
            // CBR: 0111 KKKK dddd KKKK
            new OpInfo( 0b0111_0000_0000_0000, 0b1111_0000_0000_0000, cbr.Disassemble, "CBR", "Clear Bits in Register" ),
            // CLC: 1001 0100 1000 1000
            new OpInfo( 0b1001_0100_1000_1000, 0b1111_1111_1111_1111, clc.Disassemble, "CLC", "Clear Carry Flag" ),
            // CLH: 1001 0100 1101 1000
            new OpInfo( 0b1001_0100_1101_1000, 0b1111_1111_1111_1111, clh.Disassemble, "CLH", "Clear Half Carry Flag" ),
            // CLI: 1001 0100 1111 1000
            new OpInfo( 0b1001_0100_1111_1000, 0b1111_1111_1111_1111, cli.Disassemble, "CLI", "Clear Global Interrupt Flag" ),
            // CLN: 1001 0100 1010 1000
            new OpInfo( 0b1001_0100_1010_1000, 0b1111_1111_1111_1111, cln.Disassemble, "CLN", "Clear Negative Flag" ),
            // CLR: 0010 01dd dddd dddd
            new OpInfo( 0b0010_0100_0000_0000, 0b1111_1100_0000_0000, clr.Disassemble, "CLR", "Clear Register" ),
            // CLS: 1001 0100 1100 1000
            new OpInfo( 0b1001_0100_1100_1000, 0b1111_1111_1111_1111, cls.Disassemble, "CLS", "Clear Signed Flag" ),
            // CLT: 1001 0100 1110 1000
            new OpInfo( 0b1001_0100_1110_1000, 0b1111_1111_1111_1111, clt.Disassemble, "CLT", "Clear T Flag" ),
            // CLV: 1001 0100 1011 1000
            new OpInfo( 0b1001_0100_1011_1000, 0b1111_1111_1111_1111, clv.Disassemble, "CLV", "Clear Overflow Flag" ),
            // CLZ: 1001 0100 1001 1000
            new OpInfo( 0b1001_0100_1001_1000, 0b1111_1111_1111_1111, clz.Disassemble, "CLZ", "Clear Zero Flag" ),
            // COM: 1001 010d dddd 0000
            new OpInfo( 0b1001_0100_0000_0000, 0b1111_1110_0000_1111, com.Disassemble, "COM", "One’s Complement" ),
            // CP: 0001 01rd dddd rrrr
            new OpInfo( 0b0001_0100_0000_0000, 0b1111_1100_0000_0000, cp.Disassemble, "CP", "Compare" ),
            // CPC: 0000 01rd dddd rrrr
            new OpInfo( 0b0000_0100_0000_0000, 0b1111_1100_0000_0000, cpc.Disassemble, "CPC", "Compare with Carry" ),
            // CPI: 0011 KKKK dddd KKKK
            new OpInfo( 0b0011_0000_0000_0000, 0b1111_0000_0000_0000, cpi.Disassemble, "CPI", "Compare with Immediate" ),
            // CPSE: 0001 00rd dddd rrrr
            new OpInfo( 0b0001_0000_0000_0000, 0b1111_1100_0000_0000, cpse.Disassemble, "CPSE", "Compare Skip if Equal" ),
            // DEC: 1001 010d dddd 1010
            new OpInfo( 0b1001_0100_0000_1010, 0b1111_1110_0000_1111, dec.Disassemble, "DEC", "Decrement" ),
            // DES: 1001 0100 KKKK 1011
            new OpInfo( 0b1001_0100_0000_1011, 0b1111_1111_0000_1111, des.Disassemble, "DES", "Data Encryption Standard" ),
            // EICALL: 1001 0101 0001 1001
            new OpInfo( 0b1001_0101_0001_1001, 0b1111_1111_1111_1111, eicall.Disassemble, "EICALL", "Extended Indirect Call to Subroutine" ),
            // EIJMP: 1001 0100 0001 1001
            new OpInfo( 0b1001_0100_0001_1001, 0b1111_1111_1111_1111, eijmp.Disassemble, "EIJMP", "Extended Indirect Jump" ),
            // ELPM(1): 1001 0101 1101 1000
            new OpInfo( 0b1001_0101_1101_1000, 0b1111_1111_1111_1111, elpm.Disassemble, "ELPM", "Extended Load Program Memory" ),
            // ELPM(2): 1001 000d dddd 0110
            new OpInfo( 0b1001_0000_0000_0110, 0b1111_1110_0000_1111, elpmz.Disassemble, "ELPM", "Extended Load Program Memory" ),
            // ELPM(3): 1001 000d dddd 0111
            new OpInfo( 0b1001_0000_0000_0111, 0b1111_1110_0000_1111, elpmzp.Disassemble, "ELPM", "Extended Load Program Memory" ),
            // EOR: 0010 01rd dddd rrrr
            new OpInfo( 0b0010_0100_0000_0000, 0b1111_1100_0000_0000, eor.Disassemble, "EOR", "Exclusive OR" ),
            // FMUL: 0000 0011 0ddd 1rrr
            new OpInfo( 0b0000_0011_0000_1000, 0b1111_1111_1000_1000, fmul.Disassemble, "FMUL", "Fractional Multiply Unsigned" ),
            // FMULS: 0000 0011 1ddd 0rrr
            new OpInfo( 0b0000_0011_1000_0000, 0b1111_1111_1000_1000, fmuls.Disassemble, "FMULS", "Fractional Multiply Signed" ),
            // FMULSU: 0000 0011 1ddd 1rrr
            new OpInfo( 0b0000_0011_1000_1000, 0b1111_1111_1000_1000, fmulsu.Disassemble, "FMULSU", "Fractional Multiply Signed with Unsigned" ),
            // ICALL: 1001 0101 0000 1001
            new OpInfo( 0b1001_0101_0000_1001, 0b1111_1111_1111_1111, icall.Disassemble, "ICALL", "Clear Global Interrupt Flag" ),
            // IJMP: 1001 0100 0000 1001
            new OpInfo( 0b1001_0100_0000_1001, 0b1111_1111_1111_1111, ijmp.Disassemble, "IJMP", "Indirect Jump" ),
            // IN: 1011 0AAd dddd AAAA
            new OpInfo( 0b1011_0000_0000_0000, 0b1111_1000_0000_0000, In.Disassemble, "IN", "Load an I/O Location to Register" ),
            // INC: 1001 010d dddd 0011
            new OpInfo( 0b1001_0100_0000_0011, 0b1111_1110_0000_1111, inc.Disassemble, "INC", "Increment" ),
            // JMP: 1001 010k kkkk 110k kkkk kkkk kkkk kkkk
            new OpInfo( 0b1001_0100_0000_1100, 0b1111_1110_0000_1110, jmp.Disassemble, "JMP", "Jump" ),
            // LAC: 1001 001r rrrr 0110
            new OpInfo( 0b1001_0010_0000_0110, 0b1111_1110_0000_1111, lac.Disassemble, "LAC", "Load And Clear" ),
            // LAS: 1001 001r rrrr 0101
            new OpInfo( 0b1001_0010_0000_0101, 0b1111_1110_0000_1111, las.Disassemble, "LAS", "Load And Set" ),
            // LAT: 1001 001r rrrr 0111
            new OpInfo( 0b1001_0010_0000_0111, 0b1111_1110_0000_1111, lat.Disassemble, "LAT", "Load And Toggle" ),
            // LDI: 1110 KKKK dddd KKKK
            new OpInfo( 0b1110_0000_0000_0000, 0b1111_0000_0000_0000, ldi.Disassemble, "LDI", "Load Immediate" ),
            // LDS: 1010 0kkk dddd kkkk
            new OpInfo( 0b1010_0000_0000_0000, 0b1111_1000_0000_0000, lds16.Disassemble, "LDS", "Load Direct from Data Space" ),
            // LDS: 1001 000d dddd 0000 kkkk kkkk kkkk kkkk
            new OpInfo( 0b1001_0000_0000_0000, 0b1111_1110_0000_1111, lds32.Disassemble, "LDS", "Load Direct from Data Space" ),

            // LD(1): 1001 000d dddd 1100
            new OpInfo( 0b1001_0000_0000_1100, 0b1111_1110_0000_1111, ldx.Disassemble, "LD", "Load Indirect from Data Space to Register using Index X" ),
            // LD(2): 1001 000d dddd 1101
            new OpInfo( 0b1001_0000_0000_1101, 0b1111_1110_0000_1111, ldxp.Disassemble, "LD", "Load Indirect from Data Space to Register using Index X" ),
            // LD(3): 1001 000d dddd 1110
            new OpInfo( 0b1001_0000_0000_1110, 0b1111_1110_0000_1111, ldmx.Disassemble, "LD", "Load Indirect from Data Space to Register using Index X" ),

            // LD(1): 1000 000d dddd 1000
            new OpInfo( 0b1000_0000_0000_1000, 0b1111_1110_0000_1111, ldy.Disassemble, "LD", "Load Indirect from Data Space to Register using Index Y" ),
            // LD(2): 1001 000d dddd 1001
            new OpInfo( 0b1001_0000_0000_1001, 0b1111_1110_0000_1111, ldyp.Disassemble, "LD", "Load Indirect from Data Space to Register using Index Y" ),
            // LD(3): 1001 000d dddd 1010
            new OpInfo( 0b1001_0000_0000_1010, 0b1111_1110_0000_1111, ldmy.Disassemble, "LD", "Load Indirect from Data Space to Register using Index Y" ),
            // LD(4): 10q0 qq0d dddd 1qqq
            new OpInfo( 0b1000_0000_0000_1000, 0b1101_0010_0000_1000, lddy.Disassemble, "LDD", "Load Indirect from Data Space to Register using Index Y" ),

            // LD(1): 1000 000d dddd 0000
            new OpInfo( 0b1000_0000_0000_0000, 0b1111_1110_0000_1111, ldz.Disassemble, "LD", "Load Indirect From Data Space to Register using Index Z" ),
            // LD(2): 1001 000d dddd 0001
            new OpInfo( 0b1001_0000_0000_0001, 0b1111_1110_0000_1111, ldzp.Disassemble, "LD", "Load Indirect From Data Space to Register using Index Z" ),
            // LD(3): 1001 000d dddd 0010
            new OpInfo( 0b1001_0000_0000_0010, 0b1111_1110_0000_1111, ldmz.Disassemble, "LD", "Load Indirect From Data Space to Register using Index Z" ),
            // LD(4): 10q0 qq0d dddd 0qqq
            new OpInfo( 0b1000_0000_0000_0000, 0b1101_0010_0000_1000, lddz.Disassemble, "LDD", "Load Indirect From Data Space to Register using Index Z" ),

            // LPM(1): 1001 0101 1100 1000
            new OpInfo( 0b1001_0101_1100_1000, 0b1111_1111_1111_1111, lpm.Disassemble, "LPM", "Load Program Memory" ),
            // LPM(2): 1001 000d dddd 0100
            new OpInfo( 0b1001_0000_0000_0100, 0b1111_1110_0000_1111, lpmz.Disassemble, "LPM", "Load Program Memory" ),
            // LPM(3): 1001 000d dddd 0101
            new OpInfo( 0b1001_0000_0000_0101, 0b1111_1110_0000_1111, lpmzp.Disassemble, "LPM", "Load Program Memory" ),

            // LSL: 0000 11dd dddd dddd
            new OpInfo( 0b0000_1100_0000_0000, 0b1111_1100_0000_0000, lsl.Disassemble, "LSL", "Logical Shift Left" ),
            // LSR: 1001 010d dddd 0110
            new OpInfo( 0b1001_0100_0000_0110, 0b1111_1110_0000_1111, lsr.Disassemble, "LSR", "Logical Shift Right" ),
            // MOV: 0010 11rd dddd rrrr
            new OpInfo( 0b0010_1100_0000_0000, 0b1111_1100_0000_0000, mov.Disassemble, "MOV", "Copy Register" ),
            // MOVW: 0000 0001 dddd rrrr
            new OpInfo( 0b0000_0001_0000_0000, 0b1111_1111_0000_0000, movw.Disassemble, "MOVW", "Copy Register Word" ),
            // MUL: 1001 11rd dddd rrrr
            new OpInfo( 0b1001_1100_0000_0000, 0b1111_1100_0000_0000, mul.Disassemble, "MUL", "Multiply Unsigned" ),
            // MULS: 0000 0010 dddd rrrr
            new OpInfo( 0b0000_0010_0000_0000, 0b1111_1111_0000_0000, muls.Disassemble, "MULS", "Multiply Signed" ),
            // MULSU: 0000 0011 0ddd 0rrr
            new OpInfo( 0b0000_0011_0000_0000, 0b1111_1111_1000_1000, mulsu.Disassemble, "MULSU", "Multiply Signed with Unsigned" ),
            // NEG: 1001 010d dddd 0001
            new OpInfo( 0b1001_0100_0000_0001, 0b1111_1110_0000_0001, neg.Disassemble, "NEG", "Two’s Complement" ),
            // NOP: 0000 0000 0000 0000
            new OpInfo( 0b0000_0000_0000_0000, 0b1111_1111_1111_1111, nop.Disassemble, "NOP", "No Operation" ),
            // OR: 0010 10rd dddd rrrr
            new OpInfo( 0b0010_1000_0000_0000, 0b1111_1100_0000_0000, Or.Disassemble, "OR", "Logical OR" ),
            // ORI: 0110 KKKK dddd KKKK
            new OpInfo( 0b0110_0000_0000_0000, 0b1111_0000_0000_0000, ori.Disassemble, "ORI", "Logical OR with Immediate" ),
            // OUT: 1011 1AAr rrrr AAAA
            new OpInfo( 0b1011_1000_0000_0000, 0b1111_1000_0000_0000, Out.Disassemble, "OUT", "Store Register to I/O Location" ),
            // POP: 1001 000d dddd 1111
            new OpInfo( 0b1001_0000_0000_1111, 0b1111_1110_0000_1111, pop.Disassemble, "POP", "Pop Register from Stack" ),
            // PUSH: 1001 001d dddd 1111
            new OpInfo( 0b1001_0010_0000_1111, 0b1111_1110_0000_1111, push.Disassemble, "PUSH", "Push Register on Stack" ),
            // RCALL: 1101 kkkk kkkk kkkk
            new OpInfo( 0b1101_0000_0000_0000, 0b1111_0000_0000_0000, rcall.Disassemble, "RCALL", "Relative Call to Subroutine" ),
            // RET: 1001 0101 0000 1000
            new OpInfo( 0b1001_0101_0000_1000, 0b1111_1111_1111_1111, ret.Disassemble, "RET", "Return from Subroutine" ),
            // RETI: 1001 0101 0001 1000
            new OpInfo( 0b1001_0101_0001_1000, 0b1111_1111_1111_1111, reti.Disassemble, "RETI", "Return from Interrupt" ),
            // RJMP: 1100 kkkk kkkk kkkk
            new OpInfo( 0b1100_0000_0000_0000, 0b1111_0000_0000_0000, rjmp.Disassemble, "RJMP", "Relative Jump" ),
            // ROL: 0001 11dd dddd dddd
            new OpInfo( 0b0001_1100_0000_0000, 0b1111_1100_0000_0000, rol.Disassemble, "ROL", "Rotate Left trough Carry" ),
            // ROR: 1001 010d dddd 0111
            new OpInfo( 0b1001_0100_0000_0111, 0b1111_1110_0000_1111, ror.Disassemble, "ROR", "Rotate Right through Carry" ),
            // SBC: 0000 10rd dddd rrrr
            new OpInfo( 0b0000_1000_0000_0000, 0b1111_1100_0000_0000, sbc.Disassemble, "SBC", "Subtract with Carry" ),
            // SBCI: 0100 KKKK dddd KKKK
            new OpInfo( 0b0100_0000_0000_0000, 0b1111_0000_0000_0000, sbci.Disassemble, "SBCI", "Subtract Immediate with Carry" ),
            // SBI: 1001 1010 AAAA Abbb
            new OpInfo( 0b1001_1010_0000_0000, 0b1111_1111_0000_0000, sbi.Disassemble, "SBI", "Set Bit in I/O Register" ),
            // SBIC: 1001 1001 AAAA Abbb
            new OpInfo( 0b1001_1001_0000_0000, 0b1111_1111_0000_0000, sbic.Disassemble, "SBIC", "Skip if Bit in I/O Register is Cleared" ),
            // SBIS: 1001 1011 AAAA Abbb
            new OpInfo( 0b1001_1011_0000_0000, 0b1111_1111_0000_0000, sbis.Disassemble, "SBIS", "Skip if Bit in I/O Register is Set" ),
            // SBIW: 1001 0111 KKdd KKKK
            new OpInfo( 0b1001_0111_0000_0000, 0b1111_1111_0000_0000, sbiw.Disassemble, "SBIW", "Subtract Immediate from Word" ),
            // SBR: 0110 KKKK dddd KKKK
            new OpInfo( 0b0110_0000_0000_0000, 0b1111_0000_0000_0000, sbr.Disassemble, "SBR", "Set Bits in Register" ),
            // SBRC: 1111 110r rrrr 0bbb
            new OpInfo( 0b1111_1100_0000_0000, 0b1111_1110_0000_1000, sbrc.Disassemble, "SBRC", "Skip if Bit in Register is Cleared" ),
            // SBRS: 1111 111r rrrr 0bbb
            new OpInfo( 0b1111_1110_0000_0000, 0b1111_1110_0000_1000, sbrs.Disassemble, "SBRS", "Skip if Bit in Register is Set" ),
            // SEC: 1001 0100 0000 1000
            new OpInfo( 0b1001_01000_0000_1000, 0b1111_1111_1111_1111, sec.Disassemble, "SEC", "Set Carry Flag" ),
            // SEH: 1001 0100 0101 1000
            new OpInfo( 0b1001_01000_0101_1000, 0b1111_1111_1111_1111, seh.Disassemble, "SEH", "Set Half Carry Flag" ),
            // SEI: 1001 0100 0111 1000
            new OpInfo( 0b1001_01000_0111_1000, 0b1111_1111_1111_1111, sei.Disassemble, "SEI", "Set Global Interrupt Flag" ),
            // SEN: 1001 0100 0010 1000
            new OpInfo( 0b1001_01000_0010_1000, 0b1111_1111_1111_1111, sen.Disassemble, "SEN", "Set Negative Flag" ),
            // SER: 1110 1111 dddd 1111
            new OpInfo( 0b1110_1111_0000_1111, 0b1111_1111_0000_1111, ser.Disassemble, "SER", "Set all Bits in Register" ),
            // SES: 1001 0100 0100 1000
            new OpInfo( 0b1001_0100_0100_1000, 0b1111_1111_1111_1111, ses.Disassemble, "SES", "Set Signed Flag" ),
            // SET: 1001 0100 0110 1000
            new OpInfo( 0b1001_0100_0110_1000, 0b1111_1111_1111_1111, set.Disassemble, "SET", "Set T Flag" ),
            // SEV: 1001 0100 0011 1000
            new OpInfo( 0b1001_0100_0011_1000, 0b1111_1111_1111_1111, sev.Disassemble, "SEV", "Set Overflow Flag" ),
            // SEZ: 1001 0100 0001 1000
            new OpInfo( 0b1001_0100_0001_1000, 0b1111_1111_1111_1111, sez.Disassemble, "SEZ", "Set Zero Flag" ),
            // SLEEP: 1001 0101 1000 1000
            new OpInfo( 0b1001_0101_1000_1000, 0b1111_1111_1111_1111, sleep.Disassemble, "SLEEP", "Set the circuit in sleep mode" ),
            // SPM: 1001 0101 1110 1000
            new OpInfo( 0b1001_0101_1110_1000, 0b1111_1111_1111_1111, spm.Disassemble, "SPM", "Store Program Memory" ),
            // SPM: 1001 0101 1111 1000
            new OpInfo( 0b1001_0101_1111_1000, 0b1111_1111_1111_1111, spmzp.Disassemble, "SPM", "Store Program Memory" ),

            // ST: 1001 001r rrrr 1100
            new OpInfo( 0b1001_0010_0000_1100, 0b1111_1110_0000_1111, stx.Disassemble, "ST", "Store Indirect From Register to Data Space using Index X" ),
            // ST: 1001 001r rrrr 1101
            new OpInfo( 0b1001_0010_0000_1101, 0b1111_1110_0000_1111, stxp.Disassemble, "ST", "Store Indirect From Register to Data Space using Index X" ),
            // ST: 1001 001r rrrr 1110
            new OpInfo( 0b1001_0010_0000_1110, 0b1111_1110_0000_1111, stmx.Disassemble, "ST", "Store Indirect From Register to Data Space using Index X" ),

            // ST: 1000 001r rrrr 1000
            new OpInfo( 0b1000_0010_0000_1000, 0b1111_1110_0000_1111, sty.Disassemble, "ST", "Store Indirect From Register to Data Space using Index Y" ),
            // ST: 1001 001r rrrr 1001
            new OpInfo( 0b1001_0010_0000_1001, 0b1111_1110_0000_1111, styp.Disassemble, "ST", "Store Indirect From Register to Data Space using Index Y" ),
            // ST: 1001 001r rrrr 1010
            new OpInfo( 0b1001_0010_0000_1010, 0b1111_1110_0000_1111, stmy.Disassemble, "ST", "Store Indirect From Register to Data Space using Index Y" ),

            // ST: 1000 001r rrrr 0000
            new OpInfo( 0b1000_0010_0000_0000, 0b1111_1110_0000_1111, stz.Disassemble, "ST", "Store Indirect From Register to Data Space using Index Z" ),
            // ST: 1001 001r rrrr 0001
            new OpInfo( 0b1001_0010_0000_0001, 0b1111_1110_0000_1111, stzp.Disassemble, "ST", "Store Indirect From Register to Data Space using Index Z" ),
            // ST: 1001 001r rrrr 0010
            new OpInfo( 0b1001_0010_0000_0010, 0b1111_1110_0000_1111, stmz.Disassemble, "ST", "Store Indirect From Register to Data Space using Index Z" ),

            // STD(4): 10q0 qq1r rrrr 1qqq
            new OpInfo( 0b1000_0010_0000_1000, 0b1101_0010_0000_1000, stdy.Disassemble, "STD", "Store Indirect From Register to Data Space using Index Y" ),
            // STD(4): 10q0 qq1r rrrr 0qqq
            new OpInfo( 0b1000_0010_0000_0000, 0b1101_0010_0000_1000, stdz.Disassemble, "STD", "Store Indirect From Register to Data Space using Index Z" ),

            // STS: 1010 1kkk dddd kkkk
            new OpInfo( 0b1010_1000_0000_0000, 0b1111_1000_0000_0000, sts16.Disassemble, "STS", "Store Direct to Data Space" ),
            // STS: 1001 001d dddd 0000 kkkk kkkk kkkk kkkk
            new OpInfo( 0b1001_0010_0000_0000, 0b1111_1110_0000_1111, sts32.Disassemble, "STS", "Store Direct to Data Space" ),

            // SUB: 0001 10rd dddd rrrr
            new OpInfo( 0b0001_1000_0000_0000, 0b1111_1100_0000_0000, sub.Disassemble, "SUB", "Subtract without Carry" ),
            // SUBI: 0101 KKKK dddd KKKK
            new OpInfo( 0b0101_0000_0000_0000, 0b1111_0000_0000_0000, subi.Disassemble, "SUBI", "Subtract Immediate" ),
            // SWAP: 1001 010d dddd 0010
            new OpInfo( 0b1001_0100_0000_0010, 0b1111_1110_0000_1111, swap.Disassemble, "SWAP", "Swap Nibbles" ),
            // TST: 0010 00dd dddd dddd
            new OpInfo( 0b0010_0000_0000_0000, 0b1111_1100_0000_0000, tst.Disassemble, "TST", "Test for Zero or Minus" ),
            // WDR: 1001 0101 1010 1000
            new OpInfo( 0b1001_0101_1010_1000, 0b1111_1111_1111_1111, wdr.Disassemble, "WDR", "Watchdog Reset" ),
            // XCH: 1001 001r rrrr 0100
            new OpInfo( 0b1001_0010_0000_0100, 0b1111_1110_0000_1111, xch.Disassemble, "XCH", "Exchange" ),
        };

        #region Properties

        public static Assembly ExecAssembly => Assembly.GetExecutingAssembly();

        public static string AssemblyTitle
        {
            get
            {
                var attribute = ExecAssembly.GetCustomAttributes( false ).OfType<AssemblyTitleAttribute>().FirstOrDefault();

                return attribute != null && attribute.Title != "" ? attribute.Title : Path.GetFileNameWithoutExtension( ExecAssembly.CodeBase );
            }
        }

        public static string AssemblyDirectory
        {
            get
            {
                var filePath = new Uri( Assembly.GetExecutingAssembly().CodeBase ).LocalPath;
                return Path.GetDirectoryName( filePath );
            }
        }


        public static string LogFile => Path.ChangeExtension( new Uri( ExecAssembly.CodeBase ).LocalPath, ".log" );

        #endregion

        private static void Log( string format, params object[] list )
        {
            var text = string.Format( format, list );

            text = $"{DateTime.Now:dd.MM.yyyy HH:mm:ss} {text}{Environment.NewLine}";

            try
            {
                File.AppendAllText( LogFile, text, Encoding.UTF8 );
            }
            catch { }
        }

        public static void LogInfo( string format, params object[] list )
        {
            Log( "[INFO ] " + format, list );
        }


        public static void LogError( string format, params object[] list )
        {
            Log( "[ERROR] " + format, list );
        }

        public static void Initialize()
        {
            Instructions = Instructions.OrderBy( x => x.Opcode ).ToList();

            LogInfo( "[Initialize()] {0} instructions loaded.", Instructions.Count );
        }

        public static int ArgumentsPad = 10;
        public static int CommentsPad = 20;

        static void Main( string[] args )
        {
            try
            {
                var version = ExecAssembly.GetName().Version;

                var bdate = new DateTime( 2000, 1, 1 ).AddDays( version.Build ).AddSeconds( 2 * version.Revision );

                // Удаляем файл лога.
                try
                {
                    File.Delete( LogFile );
                }
                catch { }

                // Выводим информацию о версии и дате сборки.
                var lines = new[]
                        {
                            new { Text = "OS: " + Environment.OSVersion },
                            new { Text = ".Net: " + Environment.Version },
                            new { Text = $"{AssemblyTitle}, version {version}, {bdate:dd-MMM-yyyy HH:mm:ss}"}
                        };

                foreach ( var line in lines ) LogInfo( line.Text );

                // Выводим опции командной строки.
                LogInfo( "options: " + args.Aggregate( ( first, second ) => $"{first}, {second}") );

                // Загружаем список поддерживаемых инструкций.
                Initialize();

                // Анализируем командную строку.
                var OptionS_Enabled = false;

                // Перебираем параметры.
                var objfiles = new List<string>();

                foreach ( var arg in args )
                {
                    // Игнорируем любые другие параметры.
                    if ( !new Regex( @"(-\w+|--\w+)" ).IsMatch( arg ) ) objfiles.Add( arg );

                    if ( arg.Equals( "-S" ) ) OptionS_Enabled = true;
                }

                if ( OptionS_Enabled && objfiles.Count > 0 )
                {
                    var objectFile = objfiles[0];

                    if ( !File.Exists( objectFile ) )
                    {
                        LogError( "File not found: " + Path.GetFullPath( objectFile ) );
                        return;
                    }

                    new ObjectFile( objectFile ).WriteListing();
                }
            }
            catch ( Exception ex )
            {
                LogError( ex.Message );
            }
        }
    }
}
