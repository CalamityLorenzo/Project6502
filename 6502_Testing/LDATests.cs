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

            mem[79] = 100;
            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                //!-- Preamble
                0xA9, // LDA #37 (Immediate)
                0x25,
                0xAA,  // TAX x = 37
                //!-- Preamble
                0xB5,  // LDA $2A,X Load the content of ($2A + $25) 79 into the accumulator
                0x2A
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine($"A  {registers["A"]}");
            Trace.WriteLine($"X  {registers["X"]}");
            Assert.IsTrue(registers["A"] == "100");
        }

        [TestMethod("LDA : Absolute")]
        public void LDAAbsolute()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[65281] = 100;
            var processor = createProcessor(mem);


            var program = new byte[]
            {
                0xAD,
                0xFF,
                0x01
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Assert.IsTrue(registers["A"] == "100");
        }

        [TestMethod("LDA : Absolute.X")]
        public void LDAAbsoluteX()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[65281 + 16] = 100;
            var processor = createProcessor(mem);


            var program = new byte[]
            {
                //!-- Preamble
                0xA9, // LDA #16 (Immediate)
                0x10,
                0xAA,  // TAX x = 16

                0xBD,
                0xFF,
                0x01
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Trace.WriteLine(registers["X"]);
            Assert.IsTrue(registers["A"] == "100");
            Assert.IsTrue(registers["X"] == "16");
        }

        [TestMethod("LDA : Absolute.Y")]
        public void LDAAbsoluteY()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[65281 + 16] = 100;
            var processor = createProcessor(mem);


            var program = new byte[]
            {
                //!-- Preamble
                0xA9, // LDA #16 (Immediate)
                0x10,
                0xA8,  // TAX x = 16

                0xB9,
                0xFF,
                0x01
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Trace.WriteLine(registers["Y"]);
            Assert.IsTrue(registers["A"] == "100");
            Assert.IsTrue(registers["Y"] == "16");
        }

        [TestMethod("LDA : Indirect Indexed.X")]
        public void LDAIndirectX()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];
            // this should become $64 $65
            // Thus LDA = 6465
            mem[(255+16) & 0xFF] = 100;
            mem[(255+17) & 0xFF] = 101;
            mem[(0x65 <<8) | 0x64] = 8;
            var processor = createProcessor(mem);


            var program = new byte[]
            {
                //!-- Preamble
                0xA9, // LDA #16 (Immediate)
                0x10,
                0xAA,  // TAX x = 16

                0xA1,
                0xFF   // 255
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Trace.WriteLine(registers["X"]);
            Assert.IsTrue(registers["A"] == "8");
            Assert.IsTrue(registers["X"] == "16");
        }
    }
}