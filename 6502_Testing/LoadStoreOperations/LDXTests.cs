using Project6502;
using System.Diagnostics;

namespace LoadStoreOperations
{
    [TestClass]
    public class LDX
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("LDX : Immediate")]
        public void LDXImmediate()
        {
            // Empty memory
            var processor = createProcessor();

            // LDA #15 = $0F
            var program = new byte[]
            {
                0xA2,
                0x1F
            };

            processor.AdhocProgram(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["X"]);
            Assert.IsTrue(registers["X"] == "31");
        }

        [TestMethod("LDX : Zero Page")]
        public void LDXZeroPage()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[37] = 100;
            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA6,
                0x25
            };

            processor.AdhocProgram(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["X"]);
            Assert.IsTrue(registers["X"] == "100");
        }

        [TestMethod("LDX : Zero Page.Y")]
        public void LDXZeroPageY()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[108] = 255;
            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                //!-- Preamble
                0xA9, // LDA #37 (Immediate)
                0x25,
                0xA8,  // TAY x = 37
                //!-- Preamble
                0xB6,  // LDX $47,X Load the content of ($47 + $25) 108 into the accumulator
                0x47
            };

            processor.AdhocProgram(program);
            var registers = processor.Registers();
            Trace.WriteLine($"A  {registers["A"]}");
            Trace.WriteLine($"X  {registers["X"]}");
            Assert.IsTrue(registers["X"] == "255");
        }

        [TestMethod("LDX : Absolute")]
        public void LDXAbsolute()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[65281] = 100;
            var processor = createProcessor(mem);


            var program = new byte[]
            {
                0xAE,
                0x01,
                0xFF,
            };

            processor.AdhocProgram(program);
            var registers = processor.Registers();
            Trace.WriteLine($"X = {registers["X"]}");
            Assert.IsTrue(registers["X"] == "100");
        }

   
        [TestMethod("LDX : Absolute.Y")]
        public void LDXAbsoluteY()
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
                0xA8,  // TAY x = 16

                0xBE,
                0x01,
                0xFF,
            };

            processor.AdhocProgram(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["X"]);
            Trace.WriteLine(registers["Y"]);
            Assert.IsTrue(registers["X"] == "100");
            Assert.IsTrue(registers["Y"] == "16");
        }

    }
}
