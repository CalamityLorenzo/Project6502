using Project6502;
using System.Diagnostics;

namespace LoadStoreOperations
{
    [TestClass]
    [TestCategory("Load/Store Operations")]
    public class STY
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("STY : Zero Page")]
        public void STYZeroPage()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA9, // LDA 33
                0x21,
                0xA8, // TAY y = $21

                0x84,
                0x25
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine($"Y {registers["Y"]}");
            Trace.WriteLine($"mem[{0x25}] =  {mem[0x25]}");
            Assert.IsTrue(registers["Y"] == "33");
            Assert.IsTrue(mem[37] == 33);
        }

        [TestMethod("STY : Zero Page.X")]
        public void STYZeroPage_X()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA9, // LDA 255
                0xFF,
                0xA8, // TAY

                0xA9,
                0xBB,
                0xAA, // TAX

                0x94,
                0x25
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine($"X {registers["X"]}");
            Trace.WriteLine($"Y {registers["Y"]}");
            Trace.WriteLine($"mem[{0x25 + 0xBB}] =  {mem[0x25 + 0xBB]}");
            Assert.IsTrue(registers["X"] == "187");
            Assert.IsTrue(registers["Y"] == "255");
            Assert.IsTrue(mem[224] == 255);
        }


        [TestMethod("STY : Absolute")]
        public void STYAbsolute()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA9, // LDA 33
                0xFF,
                0xA8, // TAX

                0x8C, // STY
                0x25,
                0xFF
            };

            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine($"Y {registers["Y"]}");
            Trace.WriteLine($"mem[{(0x25 << 8 | 0xFF)}] =  {mem[(0x25 << 8 | 0xFF)]}");
            Assert.IsTrue(registers["Y"] == "255");
            Assert.IsTrue(mem[9727] == 255);
        }
    }
}

