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
    public class Six502Processor
    {
        byte Accumulator;
        byte IndexX;
        byte IndexY;
        int _stackPointer;
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
            var pos = 0;
            while(pos< buffer.Length)
            {
                // do many things.
            }
        }
    }
}