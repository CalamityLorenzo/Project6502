using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Project6502
{

    /*
     * 8bt processor
     * little endian
     * 16vt address bus
     * registers
     * program counter
     * 11 fucking addressing modes.
     */
    public partial class Six502Processor
    {
        private bool _abortTriggered = false;

        byte Accumulator;
        byte XRegister;
        byte YRegister;
        /// <summary>
        /// 1 byte but functionally starts at $100-$1FF
        /// DECREMENTED on push
        /// INCREMETED on Pop/pull
        /// </summary>
        byte _stackPointer = 255;
        // C arry
        // Z ero 
        // I nterrupt disable
        // I AM 1
        // D ecimal Mode
        // B reak command
        // V Overfow
        // Negative 
        bool[] _processorStatusFlags = new bool[8];

        ushort _programCounter;
        public ushort ProgramCounter => _programCounter;
        public byte Databus => memory[_programCounter];
        // Memory page 1
        // $0100-$01ff = 256->511`
        byte[] stackData = new byte[255];
        private readonly InterruptStructure? _BRK;

        /// <summary>
        /// Our memory.
        /// </summary>
        readonly byte[] memory;
        /// This is the program as supplied via the process method.
        private byte[] _programBuffer;
        /// <summary>
        /// This is the computer memory.
        /// This can be as big as it needs, we can only see the first 16bits
        /// </summary>
        /// <param name="memory"></param>
        public Six502Processor(byte[] memory)
        {
            this.memory = memory;
        }

        public Six502Processor(byte[] memory, InterruptStructure? BRK) : this(memory)
        {
            this._BRK = BRK;
            ConfigureInterrupts();
        }

        private void ConfigureInterrupts()
        {
            if (_BRK != null)
            {
                memory[0xFFFD] = (byte)(_BRK.Value.Address & 0xFF);
                memory[0xFFFE] = (byte)(_BRK.Value.Address >> 8);
                _BRK.Value.Method.CopyTo(memory, _BRK.Value.Address);
            }
        }

        public Dictionary<string, string> Registers() => new Dictionary<string, string>
        {
            {"A", Accumulator.ToString()},
            {"X", XRegister.ToString()},
            {"Y", YRegister.ToString()},
            {"SP", _stackPointer.ToString()},
            {"PC", _programCounter.ToString()},
            {"C",_processorStatusFlags[0].ToString()},
            {"Z",_processorStatusFlags[1].ToString()},
            {"I",_processorStatusFlags[2].ToString()},
            {"D",_processorStatusFlags[3].ToString()},
            {"B",_processorStatusFlags[5].ToString()},
            {"V",_processorStatusFlags[6].ToString()},
            {"N",_processorStatusFlags[7].ToString()}
};

        /// <summary>
        /// Helper for sett status a value for 
        /// Zero (psp[1])
        /// Negative (psp[7])
        /// </summary>
        /// <param name="registerValue"></param>
        private void CheckNegativeZeroFlags(byte registerValue)
        {
            // Zero
            _processorStatusFlags[1] = (registerValue == 0);
            // Negative.
            _processorStatusFlags[7] = (registerValue > 127);
        }

        private byte ConvertFromProcessorStatus()
        {
            byte result = 0;
            var index = 0;
            foreach (var b in _processorStatusFlags)
            {
                // binary concat 
                if (b) // We move the lowest bool index 0 to 7.
                       // reversing as we go as binary digits are read 'backwards'/reversed.
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }

        private void ConvertToProcessorStatus(byte stackValue)
        {
            var bitIndex = 0;
            var arrIndex = 7;
            var pos = 1;
            while (bitIndex < 8)
            {
                pos = bitIndex == 0 ? 1 : pos = pos * 2;
                _processorStatusFlags[arrIndex] = (stackValue & pos) != 0 ? true : false;
                bitIndex++;
                arrIndex--;
            }
        }

        public void Reset()
        {
            // Un-used processor flag is always set 
            _processorStatusFlags[5] = true;
            // Stack pointer must be at the top
            _stackPointer = 0xFF;
            ConfigureInterrupts();
        }


        #region Address Modes
        private byte ImmediateConstant() => memory[_programCounter++];

        private int ZeroPage() => memory[_programCounter++];

        private int ZeroPage_X() => (memory[_programCounter++] + XRegister) & 0xFF;
        private int ZeroPage_Y() => (memory[_programCounter++] + YRegister) & 0xFF;

        private int Absolute() => memory.Absolute(ref _programCounter);
        private int Absolute_X() => memory.Absolute(ref _programCounter) + XRegister;
        private int Absolute_Y() => memory.Absolute(ref _programCounter) + YRegister;

        /// <summary>
        /// This is a pig but (val),x = val+x.
        /// msb = mem[val+x+1 &0xFF]
        /// lsb = mem[val+x & 0xFF]
        /// The indirect bit is having to query the memory
        /// </summary>
        /// <returns></returns>
        private int Indirect_X()
        {
            //byte 1
            var t = (memory[((memory[_programCounter] + XRegister) & 0xFF) + 1] << 8
                  // byte 2
                  | memory[((memory[_programCounter++] + XRegister) & 0xFF)]); // Indexed Indirect x ($,X)
            return t;
        }

        // Indirect Indexed Get 16 bit address from memory locations pointed at. + value of y to result.
        private int Indexed_Y() => (memory[memory[_programCounter] + 1] << 8 | memory[memory[_programCounter++]]) + YRegister; // Indirect  + Index Y
        private int Indirect() => (memory[(memory[_programCounter]) << 8 | (memory[++_programCounter])]); // Straight indirection 16bit address points to lsb where the actul thing is happening.
        #endregion

        /// <summary>
        /// Load our program into memory, at the specifed location
        /// </summary>
        /// <param name="buffer">The constructed program</param>
        /// <param name="startMemory">Where in memory the instructions are loaded.</param>
        public void LoadProgram(byte[] buffer, ushort startMemory = 0x200)
        {
            _programCounter = startMemory;
            _programBuffer = buffer;
            this._programBuffer.CopyTo(memory, startMemory);
        }



        // We program is passed in as bytesm and thus already parsed.
        public void AdhocProgram(byte[] buffer, ushort startMemory = 0x200)
        {
            this.LoadProgram(buffer, startMemory);
            _programCounter= startMemory;
            while (_programCounter < memory.Length)
            {

                InstructionStep();
                // Not actually recorded anywhere...
                if (_abortTriggered) break;
            }
        }

        public void AdhocInstruction(byte[] instruction)
        {
            this.LoadProgram(instruction, _programCounter);
            InstructionStep();
        }



        /// <summary>
        /// 1 instruction step
        /// nb: no relationship to the 'ticks' per instruction
        /// (For a start I jhaven't even considered how I'm doing that and odn't get me started on the page banks).
        /// 
        /// </summary>
        public void InstructionStep()
        {
            // do many things.
            var instruction = memory[_programCounter++];
            switch (instruction)
            {
                case 0x03:  // astop
                    _abortTriggered = true;
                    return;
                #region status_flag_changes
                case 0x18: // CLC
                    CLearCarry();
                    break;
                case 0xD8: // CLD
                    CLearDecimal();
                    break;
                case 0x58: // CLI
                    CLearInterrupt();
                    break;
                case 0xB8: // CLV
                    CLearoVerflow();
                    break;
                case 0x38:
                    SetCarryFlag();
                    break;
                case 0xF8:
                    SetDecimalFlag();
                    break;
                case 0x78:
                    SetInterruptFlag();
                    break;
                #endregion status_flag_changes

                #region Stack Operations
                case 0x9A: // TSX
                    TransferStackpointertoX();
                    break;
                case 0xBA: // TXS
                    TransferXtoStackPointer();
                    break;
                case 0x48: //PHA
                    PusHAccumulator();
                    break;
                case 0x68: // PLA
                    PulLAccumulator();
                    break;
                case 0x08: // PHP
                    PusHProcessorStatus();
                    break;
                case 0x28: // PLP
                    PulLProcessorstatus();
                    break;
                #endregion Stack Operations

                #region Register Transfers
                case 0x98: // TYA
                    TransferYToAccumulator();
                    break;
                case 0xA8: // TAY
                    TransferAccumulatorToY();
                    break;

                case 0xAA: // TAX
                    TransferAccumulatorToX();
                    break;

                case 0x8A: // TXA
                    TransferXToAccumulator();
                    break;
                #endregion Register Transfers
                #region Load Store Operations
                case 0xA9: //LDA
                case 0xA5:
                case 0xB5:
                case 0xAD:
                case 0xBD:
                case 0xB9:
                case 0xA1:
                case 0xB1:
                    LoaDtheAccumulator(instruction);
                    break;
                case 0xA2: // LDX
                case 0xA6:
                case 0xB6:
                case 0xAE:
                case 0xBE:
                    LoaDIntoXregister(instruction);
                    break;

                case 0xA0: //LDY
                case 0xA4:
                case 0xB4:
                case 0xAC:
                case 0xBC:
                    LoaDIntoYregister(instruction);
                    break;

                case 0x85: // STA
                case 0x95:
                case 0x8D:
                case 0x9D:
                case 0x99:
                case 0x81:
                case 0x91:
                    StoreTheAccumulator(instruction);
                    break;

                case 0x86: // STX
                case 0x96:
                case 0x8E:
                    StoreTheXregister(instruction);
                    break;
                case 0x84: // STY
                case 0x94:
                case 0x8C:
                    StoreTheYregister(instruction);
                    break;
                #endregion Load Store Operations
                #region Logical Operations
                case 0x29: // AND
                case 0x25:
                case 0x35:
                case 0x2D:
                case 0x3D:
                case 0x39:
                case 0x21:
                case 0x31:
                    LogicalAND(instruction);
                    break;

                case 0x49: //EOR
                case 0x45:
                case 0x55:
                case 0x4D:
                case 0x5D:
                case 0x59:
                case 0x41:
                case 0x51:
                    ExclusiveOR(instruction);
                    break;

                case 0x09: // ORA
                case 0x05:
                case 0x15:
                case 0x0D:
                case 0x1D:
                case 0x19:
                case 0x01:
                case 0x11:
                    LogicalInclusiveOR(instruction);
                    break;

                case 0x24: // BIT
                case 0x2C:
                    BIT(instruction);
                    break;
                #endregion Logical Operations
                #region Shift
                case 0x0A:
                case 0x06:
                case 0x16:
                case 0x0E:
                case 0x1E:
                    ASL(instruction);
                    break;
                case 0x4A:
                case 0x46:
                case 0x56:
                case 0x4E:
                case 0x5E:
                    LSR(instruction);
                    break;

                case 0x2A: // ROL
                case 0x26:
                case 0x36:
                case 0x2E:
                case 0x3E:
                    ROtateLeft(instruction);
                    break;
                case 0x6A: // ROR
                case 0x66:
                case 0x76:
                case 0x6E:
                case 0x7E:
                    ROtateRight(instruction);
                    break;
                #endregion Shift
                #region Increments&Decrements
                case 0xE6:  // INC
                case 0xF6:
                case 0xEE:
                case 0xFE:
                    {
                        IncrementMemory(instruction);
                        break;
                    }
                case 0xC6: // DEC
                case 0xD6:
                case 0xCE:
                case 0xDE:
                    {
                        DecrementMemory(instruction);
                        break;
                    }
                case 0xCA:  //DEX
                    {
                        DecrementXRegister(instruction);
                        break;
                    }
                case 0x88:  //DEY
                    {
                        DecrementYRegister(instruction);
                        break;
                    }
                case 0xE8:  // INX
                    {
                        IncrementXRegister(instruction);
                        break;
                    }
                case 0xC8: // INY
                    {
                        IncrementYRegister(instruction);
                        break;
                    }
                #endregion
                #region Jumps and Calls
                case 0x4C: //JMP
                case 0x6C:
                    Jump(instruction);
                    break;
                case 0x20: // JSR
                    JumptoSubRoutine();
                    break;
                case 0x60: // RTS
                    ReturnfromSubroutine();
                    break;
                #endregion Jumps and Calls

                #region Arithmetic
                case 0x69: // ADC
                case 0x65:
                case 0x75:
                case 0x6D:
                case 0x7D:
                case 0x79:
                case 0x61:
                case 0x71:
                    ADwithCarry(instruction);
                    break;

                case 0xe9: //SBC
                case 0xe5:
                case 0xF5:
                case 0xED:
                case 0xFD:
                case 0xF9:
                case 0xE1:
                case 0xF1:
                    SuBtractwithCarry(instruction);
                    break;

                #endregion

                #region Branches
                case 0x90: // BCC
                    BranchIfCarryClear();
                    break;
                case 0xB0: //BCS
                    BranchIfCarrySet();
                    break;
                case 0xF0: // BEQ
                    BranchIfZeroSet();
                    break;
                case 0x30: // BMI
                    BranchIfNegativeSet();
                    break;
                case 0xD0: // BNE
                    BranchIfZeroClear();
                    break;
                case 0x10: // BPL
                    BranchIfNegativeClear();
                    break;
                case 0x50: // BVC
                    BranchIfOverflowClear();
                    break;
                case 0x70: // BVS
                    BranchIfOverflowSet();
                    break;

                #endregion Branches

                #region System
                case 0xEA:
                    NoOperation();
                    break;
                case 0x00:
                    Break();
                    break;
                case 0x40:
                    ReturnFromInterrupt();
                    break;
                    #endregion
            }

        }
    }
}