using Project6502;
using System.Diagnostics;

namespace LoadStoreOperations
{
    [TestClass]
    public class STX
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("STX : Zero Page")]
        public void STXZeroPage()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA9, // LDA 33
                0x21,
                0xAA, // TAX

                0x86,
                0x25
            };

            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["X"]);
            Trace.WriteLine($"mem[{0x25}] =  {mem[0x25]}");
            Assert.IsTrue(registers["X"] == "33");
            Assert.IsTrue(mem[37] == 33);
        }

        [TestMethod("STX : Zero Page.Y")]
        public void STXZeroPage_Y()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA9, // LDA 33
                0x21,
                0xAA, // TAX

                0xA9,
                0xAA,
                0xA8, // TAY

                0x96,
                0x25
            };

            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine($"X {registers["X"]}");
            Trace.WriteLine($"Y {registers["Y"]}");
            Trace.WriteLine($"mem[{0x25 + 0xAA}] =  {mem[0x25 + 0xAA]}");
            Assert.IsTrue(registers["X"] == "33");
            Assert.IsTrue(registers["Y"] == "170");
            Assert.IsTrue(mem[207] == 33);
        }


        [TestMethod("STX : Absolute")]
        public void STXAbsolute()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];

            var processor = createProcessor(mem);

            // LDA $25
            var program = new byte[]
            {
                0xA9, // LDA 33
                0xFF,
                0xAA, // TAX

                0x8E,
                0xFF,
                0x25,
            };

            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine($"X {registers["X"]}");
            Trace.WriteLine($"mem[{(0x25 << 8 | 0xFF)}] =  {mem[(0x25 << 8 | 0xFF)]}");
            Assert.IsTrue(registers["X"] == "255");
            Assert.IsTrue(mem[9727] == 255);
        }
    }
}
