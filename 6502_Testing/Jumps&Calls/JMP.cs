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

            Six502Processor processor = createProcessor();
            var program = new byte[]
            {
                0x4C,  // LDA #32
                0x80,
                0x6A
            };

            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"PC = {registers["PC"]}");

            Assert.IsTrue(registers["PC"] == "32874");
        }

        [TestMethod("JMP : Indirect")]
        public void Jmp_Indirect()
        {
            var mem=new byte[ushort.MaxValue];
            mem[32874] = 0xFF;
            mem[32875] = 0x01;
            Six502Processor processor = createProcessor(mem);
            var program = new byte[]
            {
                0x6C,  // LDA #32
                0x80,
                0x6A
            };

            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"PC = {registers["PC"]}");

            Assert.IsTrue(registers["PC"] == "255");
        }
    }
}
