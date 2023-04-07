using System.ComponentModel;
using System.Net.Http.Headers;

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

        short _programCounter;

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
        public Six502Processor(byte[] memory) {
            this.memory = memory;
        }

        public Dictionary<string, string> Registers() => new Dictionary<string, string>
        {
            {"A", Accumulator.ToString()},
            {"X", XRegister.ToString()},
            {"Y", YRegister.ToString()},
            {"SP", YRegister.ToString()},
            {"PC", _programCounter.ToString()},
            {"ProcessorStatus", String.Join("\n",new String []{
                        $"C : {_processorStatusFlags[0]}",
                        $"Z : {_processorStatusFlags[1]}",
                        $"I : {_processorStatusFlags[2]}",
                        $"D : {_processorStatusFlags[3]}",
                        $"B : {_processorStatusFlags[4]}",
                        $"V : {_processorStatusFlags[5]}" })}
        };


        void Reset() { }

        // We program is passed in as bytesm and thus already parsed.
        public void Process(byte[] buffer)
        {
            _programCounter = 0; ;
            _programBuffer = buffer;
            while(_programCounter < buffer.Length)
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
                    case 0x9A: // TSX
                        TransferStackPointertoX();
                        break;
                    case 0xBA: // TXS
                        TransferXtoStackPointer();
                        break;

                    case 0x98: // TYA
                        TransferYToAccumulator();
                        break; 
                    case 0xA8: // TAY
                        TransferAccumulatorToY();
                        break;

                    case 0xAA:
                        TransferAccumulatorToX();
                    break;

                    case 0x8A:
                        TransferXToAccumulator();
                    break;
                    

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
                //_programCounter += 1;
            }
        }
    }
}