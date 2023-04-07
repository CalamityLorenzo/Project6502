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
        byte IndexX;
        byte IndexY;
        byte _stackPointer;
        // C arry 
        // Z ero 
        // Interrupt disable
        // Decimal Mode
        // B reak command
        // V Overfow
        // Negative 
        bool[] _processorStatusFlags = new bool[8];
        short _programCounter;

        // Memory page 1
        // $0100-$01ff = 256->511`
        byte[] stackData = new byte[255];

        public Six502Processor() { }
        
        void Reset() { }

        // We program is passed in as bytesm and thus already parsed.
        void Process(byte[] buffer)
        {
            _programCounter = 0; ;
            while(_programCounter < buffer.Length)
            {
                // do many things.

                switch (buffer[_programCounter])
                {
                    case 0x50:
                        BranchIfOverflowClear((sbyte)buffer[_programCounter+=1]); // BVC;
                        break;

                    case 0x18: // CLC
                        CLearCarry();
                        break;
                    case 0xD8: // CLD
                        CLearDecimal();
                        break;
                    case 0x58:
                        CLearInterrupt();
                        break;
                    case 0xB8:
                        CLearoVerflow();
                        break;

                }

                // 16 bit addressing
                _programCounter += 1;
            }
        }
    }
}