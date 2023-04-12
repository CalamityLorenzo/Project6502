using Project6502;
using System.Diagnostics;

namespace LoadStoreOperations
{
    [TestClass]
    public class LDY
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("LDY : Immediate")]
        public void LDYImmediate()
        {
            // Empty memory
            var processor = createProcessor();

            // LDA #15 = $0F
            var program = new byte[]
            {
                0xA0,
                0x1F
            };

            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["Y"]);
            Assert.IsTrue(registers["Y"] == "31");
        }

        [TestMethod("LDY : Zero Page")]
        public void LDYZeroPage()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[37] = 100;
            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA4,
                0x25
            };

            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["Y"]);
            Assert.IsTrue(registers["Y"] == "100");
        }

        [TestMethod("LDX : Zero Page.X")]
        public void LDAZeroPageX()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[108] = 255;
            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                //!-- Preamble Stuff X
                0xA9, // LDA #37 (Immediate)
                0x25,
                0xAA,  // TAX x = 37
                //!-- Preamble
                0xB4,  // LDY $47,X Load the content of ($47 + $25) 108 into the accumulator
                0x47
            };

            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine($"A  {registers["A"]}");
            Trace.WriteLine($"X  {registers["X"]}");
            Trace.WriteLine($"Y  {registers["Y"]}");
            Assert.IsTrue(registers["Y"] == "255");
        }

        [TestMethod("LDY : Absolute")]
        public void LDYAbsolute()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            mem[65281] = 100;
            var processor = createProcessor(mem);


            var program = new byte[]
            {
                0xAC,
                0xFF,
                0x01
            };

            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["Y"]);
            Assert.IsTrue(registers["Y"] == "100");
        }


        [TestMethod("LDX : Absolute.Y")]
        public void LDYAbsoluteX()
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

                0xBC,
                0xFF,
                0x01
            };

            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine($"Y {registers["Y"]}");
            Trace.WriteLine($"X {registers["X"]}");
            Assert.IsTrue(registers["Y"] == "100");
            Assert.IsTrue(registers["X"] == "16");
        }

    }
}
