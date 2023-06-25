using Project6502;
using System.Diagnostics;

namespace Jumps_Calls
{
    [TestClass]
    public class JMP
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);
        [TestMethod("JMP : Absolute")]
        public void Jmp_Absolute()
        {

            var mem = new byte[ushort.MaxValue];

            mem[(0x80 << 8 |
                0x6A) + 1] = 0x03;
            Six502Processor processor = createProcessor(mem);
            var program = new byte[]
            {
                0x4C,  // JMP 80 (<<8 | 6a)
                0x80,
                0x6A,
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"PC = {registers["PC"]}");

            Assert.IsTrue(registers["PC"] == "32876");
        }

        [TestMethod("JMP : Indirect")]
        public void Jmp_Indirect()
        {
            var mem=new byte[ushort.MaxValue];
            mem[32874] = 0xFF;
            mem[32875] = 0x00;
            mem[255] = 0x03;
            Six502Processor processor = createProcessor(mem);
            var program = new byte[]
            {
                0x6C,  // JMP #32
                0x80,
                0x6A
            };

            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"PC = {registers["PC"]}");

            Assert.IsTrue(registers["PC"] == "256");
        }
    }
}
