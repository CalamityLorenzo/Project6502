using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection.Emit;

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
        // D ecimal Mode
        // B reak command
        // V Overfow
        // Negative 
        bool[] _processorStatusFlags = new bool[8];

        ushort _programCounter;

        // Memory page 1
        // $0100-$01ff = 256->511`
        byte[] stackData = new byte[255];
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
            var index = 0;
            var pos = 1;
            while (index < 8)
            {
                pos = index == 0 ? 1 : pos = pos * 2;
                _processorStatusFlags[index] = (stackValue & pos) != 0 ? true : false;
                index++;
            }
        }

        void Reset() { }


        #region Address Modes
        private byte ImmediateConstant() => _programBuffer[_programCounter++];

        private int ZeroPage() => _programBuffer[_programCounter++];

        private int ZeroPage_X() => (_programBuffer[_programCounter++] + XRegister) & 0xFF;
        private int ZeroPage_Y() => (_programBuffer[_programCounter++] + YRegister) & 0xFF;

        private int Absolute() => _programBuffer.Absolute(ref _programCounter);
        private int Absolute_X() => _programBuffer.Absolute(ref _programCounter) + XRegister;
        private int Absolute_Y() => _programBuffer.Absolute(ref _programCounter) + YRegister;

        /// <summary>
        /// This is a pig but (val),x = val+x.
        /// msb = mem[val+x+1 &0xFF]
        /// lsb = mem[val+x & 0xFF]
        /// The indirect bit is having to query the memory
        /// </summary>
        /// <returns></returns>
        private int Indirect_X() => (memory[((_programBuffer[_programCounter] + XRegister) & 0xFF) + 1] << 8 | memory[((_programBuffer[_programCounter++] + XRegister) & 0xFF)]); // Indexed Indirect x ($,X)
        private int Indirect_Y() => (memory[_programBuffer[_programCounter] + 1] << 8 | memory[_programBuffer[_programCounter++]]) + YRegister;
        #endregion

        // We program is passed in as bytesm and thus already parsed.
        public void Process(byte[] buffer)
        {
            _programCounter = 0; ;
            _programBuffer = buffer;
            while (_programCounter < buffer.Length)
            {
                // do many things.
                var instruction = buffer[_programCounter++];
                switch (instruction)
                {
                    case 0x50:
                        BranchIfOverflowClear(); // BVC;
                        break;

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

                    case 0xA0:
                    case 0xA4:
                    case 0xB4:
                    case 0xAC:
                    case 0xBC:
                        LoaDIntoYregister(instruction);
                        break;

                    case 0x85:
                    case 0x95:
                    case 0x8D:
                    case 0x9D:
                    case 0x99:
                    case 0x81:
                    case 0x91:
                        StoreTheAccumulator(instruction);
                        break;

                    case 0x86:
                    case 0x96:
                    case 0x8E:
                        StoreTheXregister(instruction);
                        break;
                    case 0x84:
                    case 0x94:
                    case 0x8C:
                        StoreTheYregister(instruction);
                        break;
                    #endregion Load Store Operations
                    #region Logical Operations
                    case 0x29:
                    case 0x25:
                    case 0x35:
                    case 0x2D:
                    case 0x3D:
                    case 0x39:
                    case 0x21:
                    case 0x31:
                        LogicalAND(instruction);
                        break;

                    case 0x49:
                    case 0x45:
                    case 0x55:
                    case 0x4D:
                    case 0x5D:
                    case 0x59:
                    case 0x41:
                    case 0x51:
                        ExclusiveOR(instruction);
                        break;

                    case 0x09:
                    case 0x05:
                    case 0x15:
                    case 0x0D:
                    case 0x1D:
                    case 0x19:
                    case 0x01:
                    case 0x11:
                        LogicalInclusiveOR(instruction);
                        break;

                    case 0x24:
                    case 0x2C:
                        BIT(instruction);
                        break;
                    #endregion Logical Operations

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


                }

                // 16 bit addressing
               // _programCounter += 1;
            }
        }
    }
}