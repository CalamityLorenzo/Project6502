using Project6502;
using System.Diagnostics;

namespace _6502_Testing
{
    [TestClass]
    public class STATests
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);


        [TestMethod("STA : Zero Page")]
        public void STA_ZeroPage()
        {

            var memory = new byte[byte.MaxValue];
            memory[162] = 100;

            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9, // LDA 200
                0xC8,

                0x85, // STA 
                0xA2,
            };

            processor.Process(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            Trace.WriteLine($"mem[100] =  {memory[100]}");
            Assert.IsTrue(memory[100] == 0xC8);

        }

        [TestMethod("STA : Zero Page.X")]
        public void STA_ZeroPage_X()
        {
            var memory = new byte[ushort.MaxValue];

            //memory[162] = 100;
            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9, // LDA 64
                0x40,
                0xAA, // Put 64 in X TAX

                0xA9, // LDA 200
                0xC8,

                0x95, // STA -> (A2+64) = e2 = 200;
                0xA2,
            };

            processor.Process(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            Trace.WriteLine($"mem[{(0xA2 + 0x40)}] =  {memory[0xA2 + 0x40]}");
            Assert.IsTrue(memory[0xA2 + 0x40] == 0xC8);
        }

        [TestMethod("STA : Absolute")]
        public void STA_Absolute()
        {

            var memory = new byte[ushort.MaxValue];

            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9, // LDA 200
                0xC8,

                0x8D, // STA 
                0xA2, // Remeber A full 16bit address = 0xA240 (Not 0xA2 + 0x40)
                0x40
            };

            processor.Process(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            var memAddress = (0xA2 << 8 | 0x40);
            Trace.WriteLine($"mem[{memAddress}] =  {memory[memAddress]}");
            Assert.IsTrue(memory[memAddress] == 200);

        }

    }
}
