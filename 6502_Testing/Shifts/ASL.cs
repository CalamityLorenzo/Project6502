using Project6502;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Shifts
{
    [TestClass]
    public class ASL
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("ASL : Accumulator")]
        public void ASL_Accumulator()
        {
            Six502Processor processor = createProcessor();

            var program = new byte[]
            {
                0xA9,
                0x20,

                0x0A
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "64");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
        }

        [TestMethod("ASL : ZeroPage")]
        public void ASL_ZeroPage()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0xFF] = 0x20;
            Six502Processor processor = createProcessor(mem);
            
            var program = new byte[]
            {
                0x06,
                0xFF
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0xFF}] = {mem[0xFF]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(mem[255] == 64);
        }


        [TestMethod("ASL : ZeroPage.X")]
        public void ASL_ZeroPage_X()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0xFE+0x01] = 0x20;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0xA2,
                0x01,
                0x16,
                0xFE
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0xFF}] = {mem[0xFF]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(mem[255] == 64);
        }

        [TestMethod("ASL : Absolute")]
        public void ASL_Absolute()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0x16<<8 | 0xFE] = 129;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0x0E,
                0xFE,
                0x16,
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0x16 << 8 | 0xFE}] = {mem[0x16 << 8 | 0xFE]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "True");
            Assert.IsTrue(mem[5886] == 2);
        }

        [TestMethod("ASL : Absolute.X")]
        public void ASL_Absolute_X()
        {
            var mem = new byte[ushort.MaxValue];

            mem[(0x16 << 8 | 0xFE)+1] = 129;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0xA2,
                0x01,
                0x1E,
                0xFE,
                0x16,
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{(0x16 << 8 | 0xFE)+1}] = {mem[(0x16 << 8 | 0xFE) + 1]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "True");
            Assert.IsTrue(mem[5887] == 2);
        }
    }
}
