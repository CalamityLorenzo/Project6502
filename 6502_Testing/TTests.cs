using Project6502;
using System.Diagnostics;

namespace _6502_Testing
{
    [TestClass]
    public class TTests
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("TAX : Implied")]
        public void TAXImplied()
        {
            var processor = createProcessor();

            var program = new byte[]
            {
                0xA9,
                0x25, // 37 in decimal money,
                0xAA
            };
            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Trace.WriteLine(registers["X"]);
            Assert.IsTrue(registers["A"] == "37");
            Assert.IsTrue(registers["X"] == "37");
        }

        [TestMethod("TXA : Implied")]
        public void TXAImplied()
        {
            var processor = createProcessor();

            var program = new byte[]
            {
                0xA9,
                0x25, // 37 in decimal money,
                0x8A
            };
            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Trace.WriteLine(registers["X"]);
            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["X"] == "0");
        }

        [TestMethod("TAY : Implied")]
        public void TAYImplied()
        {
            var processor = createProcessor();

            var program = new byte[]
            {
                0xA9,
                0x25, // 37 in decimal money,
                0xA8
            };
            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Trace.WriteLine(registers["Y"]);
            Assert.IsTrue(registers["A"] == "37");
            Assert.IsTrue(registers["Y"] == "37");
        }

        [TestMethod("TYA : Implied")]
        public void TYAImplied()
        {
            var processor = createProcessor();

            var program = new byte[]
            {
                0xA9,
                0x25, // 37 in decimal money,
                0x98
            };
            processor.Process(program);
            var registers = processor.Registers();
            Trace.WriteLine(registers["A"]);
            Trace.WriteLine(registers["Y"]);
            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["Y"] == "0");
        }
    }
}
