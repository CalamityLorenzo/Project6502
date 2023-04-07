using Project6502;
using System.Diagnostics;

namespace _6502_Testing
{
    [TestClass]
    public class LDATests
    {

            
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("LDA : Immediate")]
        public void LDAImmediate()
        {
            // Empty memory
            var processor = createProcessor();

            // LDA #15 = $0F
            var program = new byte[]
            {
                0xA9,
                0x0F
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Assert.IsTrue(registers["A"] == "15");
        }

        [TestMethod("LDA : Zero Page")]
        public void LDAZeroPage()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[37] = 100;
            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA5,
                0x25
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Assert.IsTrue(registers["A"] == "100");
        }

        [TestMethod("LDA : Zero Page.X")]
        public void LDAZeroPageX()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[37] = 100;
            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA5,
                0x25
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Assert.IsTrue(registers["A"] == "100");
        }
    }
}