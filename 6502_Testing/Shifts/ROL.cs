using Project6502;
using System.Diagnostics;

namespace Shifts
{
    [TestClass]
    public class ROL
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("ROL : Accumulator")]
        public void ROL_Accumulator()
        {
            Six502Processor processor = createProcessor();
            var program = new byte[]
            {
                0xA9,  // LDA #32
                0x80,

                0x2A
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "1");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "True");
        }

        [TestMethod("ROL : ZeroPage")]
        public void ROL_ZeroPage()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0xFF] = 0x80;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0x26,
                0xFF
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0xFF}] = {mem[0xFF]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "True");
            Assert.IsTrue(mem[255] == 1);
        }

        [TestMethod("ROL : ZeroPage.X")]
        public void ROL_ZeroPage_X()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0xFE + 0x01] = 0x80;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0xA2, //LDX 01
                0x01,

                0x36,
                0xFE
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0xFF}] = {mem[0xFF]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "True");
            Assert.IsTrue(mem[255] == 1);
        }

        [TestMethod("ROL : Absolute")]
        public void ROL_Absolute()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0x16 << 8 | 0xFE] = 10;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0x2E,
                0x16,
                0xFE
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0x16 << 8 | 0xFE}] = {mem[0x16 << 8 | 0xFE]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(mem[5886] == 20);
        }

        [TestMethod("ROL : Absolute.X")]
        public void ROL_Absolute_X()
        {
            var mem = new byte[ushort.MaxValue];

            mem[(0x16 << 8 | 0xFE) + 1] = 18;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0xA2, // LDX  #01
                0x01,

                0x3E,
                0x16,
                0xFE
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{(0x16 << 8 | 0xFE) + 1}] = {mem[(0x16 << 8 | 0xFE) + 1]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(mem[5887] == 36);
        }

    }
}