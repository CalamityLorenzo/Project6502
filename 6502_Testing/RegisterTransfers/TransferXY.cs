using LoadStoreOperations;
using Project6502;
using System.Diagnostics;

namespace RegisterTransfers
{
    [TestClass]
    public class TransferXY
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
            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine($"A {registers["A"]}");
            Trace.WriteLine($"X {registers["X"]}");
            Assert.IsTrue(registers["A"] == "37");
            Assert.IsTrue(registers["X"] == "37");
        }

        [TestMethod("TXA : Implied")]
        public void TXAImplied()
        {
            var processor = createProcessor();

            var program = new byte[]
            {
                0xA2, // LDx #70
                0xFF,

                0xA9,
                0x25, // 37 in decimal money,
                
                0x8A
            };
            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine($"A {registers["A"]}");
            Trace.WriteLine($"X {registers["X"]}");
            Trace.WriteLine($"N {registers["N"]}");
            Assert.IsTrue(registers["A"] == "255");
            Assert.IsTrue(registers["X"] == "255");
            Assert.IsTrue(registers["N"] == "True");
            

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
            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine($"A {registers["A"]}");
            Trace.WriteLine($"Y {registers["Y"]}");
            Assert.IsTrue(registers["A"] == "37");
            Assert.IsTrue(registers["Y"] == "37");
        }

        [TestMethod("TYA : Implied")]
        public void TYAImplied()
        {
            var processor = createProcessor();

            var program = new byte[]
            {
                0xA0, // LDY #79
                0x46,

                0x98
            };
            processor.AdhocProcess(program);
            var registers = processor.Registers();
            Trace.WriteLine($"A {registers["A"]}");
            Trace.WriteLine($"Y {registers["Y"]}");
            Assert.IsTrue(registers["A"] == "70");
            Assert.IsTrue(registers["Y"] == "70");
        }
    }
}
