using Project6502;
using System.Diagnostics;

namespace IncrementsDecrements
{
    [TestClass]
    public class IncrementDecrement
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("INC : ZeroPage")]
        public void INC_ZP()
        {

            var memory = new byte[ushort.MaxValue];
            memory[1] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xE6,
                0x01,

                0xA6,
                0x01
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"X = {registers["X"]}");

            Assert.IsTrue(registers["X"] == (11).ToString());
        }

        [TestMethod("INC : ZeroPage + X")]
        public void INC_ZP_X()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xA2, // LDX 255
                0xFF,

                0xF6, // INC 255
                0xFF,


               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"memory[254] = {memory[254]}");

            Assert.IsTrue(memory[254] == 11);
        }
        [TestMethod("INC : ABS")]
        public void INC_ZP_ABS()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xEE, // LDX 255
                0x00,
                0x02,
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"memory[512] = {memory[512]}");

            Assert.IsTrue(memory[512] == 0xEE+1);
        }

        [TestMethod("INC : ABS + X")]
        public void INC_ABS()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xA2, // LDX
                0x01,
                0xFE, // INC
                0x00,
                0x02,
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"memory[513] = {memory[513]}");

            Assert.IsTrue(memory[513] == 0x01+ 1);
        }

        [TestMethod("INX")]
        public void INX()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xA2, // LDX
                0x01,
                0xE8
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"X = {registers["X"]}");

            Assert.IsTrue(registers["X"]=="2");
        }

        [TestMethod("INY")]
        public void INY()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xA0, // LDX
                0x01,
                0xC8
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"Y = {registers["Y"]}");

            Assert.IsTrue(registers["Y"] == "2");
        }
        [TestMethod("DEX")]
        public void DEX()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xA2, // LDX
                0x0A,
                0xCA
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"X = {registers["X"]}");

            Assert.IsTrue(registers["X"] == "9");
        }

        [TestMethod("DEY")]
        public void DEY()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xA0, // LDX
                0x0A,
                0x88
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"Y = {registers["Y"]}");

            Assert.IsTrue(registers["Y"] == "9");
        }


        [TestMethod("DEC : ZeroPage")]
        public void DEC_ZP()
        {

            var memory = new byte[ushort.MaxValue];
            memory[1] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xC6,
                0x01,

                0xA6,
                0x01
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"X = {registers["X"]}");

            Assert.IsTrue(registers["X"] == (9).ToString());
        }

        [TestMethod("DEC : ZeroPage + X")]
        public void DEC_ZP_X()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xA2, // LDX 255
                0xFF,

                0xD6, // INC 255
                0xFF,


               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"memory[254] = {memory[254]}");

            Assert.IsTrue(memory[254] == 9);
        }
        [TestMethod("DEC : ABS")]
        public void DEC_ABS()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xCE, // LDX 255
                0x00,
                0x02,
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"memory[512] = {memory[512]}");

            Assert.IsTrue(memory[512] == 0xCE - 1);
        }

        [TestMethod("DEC : ABS + X")]
        public void DEC_ABS_X()
        {

            var memory = new byte[ushort.MaxValue];
            memory[254] = 10;
            var p = createProcessor(memory);

            var program = new byte[]
                {
                0xA2, // LDX
                0x01,
                0xDE, // INC
                0x00,
                0x02,
               };

            p.AdhocProgram(program);
            var registers = p.Registers();
            Trace.WriteLine($"memory[513] = {memory[513]}");

            Assert.IsTrue(memory[513] == 0x01 - 1);
        }

    }
}
