using Project6502;
using System.Diagnostics;

namespace Reset
{

    [TestClass]
    public class ResetVector
    {
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);


        [TestMethod("Reset")]
        public void Reset_SixFive0HToo()
        {
            var mem = new byte[ushort.MaxValue];
            // Assume the program starts at 516
            var testProgram = new byte[]
            {
                0xA9, 0x0D // LOAD 14 into the accumulator
            };

            LoadRomData(mem);
            LoadProgramData(mem, testProgram, 0x0204);

            var p = createProcessor(mem);
            p.Reset();
            p.InstructionStep();
            p.InstructionStep();
            var currentX = p.Registers()["X"];
            p.InstructionStep();
            var finalX = p.Registers()["X"];
            p.InstructionStep();
            p.InstructionStep();
            var finalA= p.Registers()["A"];

            Trace.WriteLine($"startX={currentX}");
            Trace.WriteLine($"finalX={finalX}");
            Trace.WriteLine($"A={finalA}");

        }

        private void LoadProgramData(byte[] mem, byte[] testProgram, int startPos)
        {
            testProgram.CopyTo(mem, startPos);
        }

        /// <summary>
        /// the 6502 is a cpu and knows very very little.
        /// We need to provide it with the routines to act on.
        /// Including for startup.
        /// </summary>
        /// <param name="mem"></param>
        private void LoadRomData(byte[] mem)
        {
            // This is the reset instructions
            // So this is where the PRogram Counter jumps to when we first (re)start
            var reset = new byte[]
            {
                0xA2, 0xFF, // Load 255 into X register
                0x9A,       // Load 255 into stack pointer
                0xA2, 00,   // Load 0 into X register

                // This is the start address for our program
                0x4C, 0x04,0x02,  // JMP 0x0204
            };

            reset.CopyTo(mem, 0xE0E0);

            mem[0xFFFB + 1] = (0xE0E0>>8);
            mem[0xFFFB] = (0xE0E0 & 0xFF);

        }
    }
}
