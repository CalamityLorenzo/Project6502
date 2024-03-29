﻿using Project6502;
using System.Diagnostics;

namespace Arithmetic
{
    [TestClass]
    public class SBC
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("SBC : Immediate")]
        public void SBC_Immediate()
        {
            var mem = new byte[ushort.MaxValue];
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #121
                0x79,

                0xE9,   
                0x12,  // Val not memory location
                0x03,
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"V = {registers["V"]}");
            Trace.WriteLine($"PC = {registers["PC"]}");

            Assert.IsTrue(registers["A"] == "103");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(registers["V"] == "True");

        }

        [TestMethod("SBC : Zero Page")]
        public void SBC_ZeroPage()
        {
            var mem = new byte[ushort.MaxValue];
            mem[22] = 250;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0x11,

                0xe5,
                0x16  //  memory location
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"V = {registers["V"]}");

            Assert.IsTrue(registers["A"] == "23");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(registers["V"] == "True");

        }


        [TestMethod("SBC : Zero Page.X")]
        public void SBC_ZeroPage_X()
        {
            var mem = new byte[ushort.MaxValue];
            mem[22 + 9] = 80;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0xF5,

                0xA2, // LDX #09
                0x09,

                0xF5,
                0x16
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "165");
            Assert.IsTrue(registers["X"] == "9");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(registers["C"] == "False");

        }

        [TestMethod("SBC : Absolute")]
        public void SBC_Absolute()
        {
            var mem = new byte[ushort.MaxValue];
            mem[8726] = 80;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0xF5,

                0xA2, // LDX #09
                0x09,

                0xED,
                0x16,
                0x22
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "165");
            Assert.IsTrue(registers["X"] == "9");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(registers["C"] == "False");

        }

        [TestMethod("SBC : Absolute.X")]
        public void SBC_Absolute_X()
        {
            var mem = new byte[ushort.MaxValue];
            mem[8726 + 9] = 80;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0xF5,

                0xA2, // LDX #09
                0x09,

                0xFD,
                0x16,
                0x22
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "165");
            Assert.IsTrue(registers["X"] == "9");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(registers["C"] == "False");

        }
        [TestMethod("SBC : Absolute.Y")]
        public void SBC_Absolute_Y()
        {
            var mem = new byte[ushort.MaxValue];
            mem[8726 + 9] = 80;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0xF5,

                0xA0, // LDY #09
                0x09,

                0xF9,
                0x16,
                0x22
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"Y = {registers["Y"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "165");
            Assert.IsTrue(registers["X"] == "0");
            Assert.IsTrue(registers["Y"] == "9");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(registers["C"] == "False");

        }

        [TestMethod("SBC : Indirect.X")]
        public void SBC_Indirect_X()
        {
            // put a value in the zero page (0->FF)
            var mem = new byte[ushort.MaxValue];
            
             // A - 100- C
            mem[64 + 16] = 100;  // 0x64
            mem[64 + 17] = 101;  // 0x65

            mem[0x6564] = 85;
            var processor = createProcessor(mem);


            var program = new byte[]
            {
                0xA9,
                0x69,
                0xA2,
                0x10,

                0xE1,
                0x40

            };
            processor.AdhocProgram(program);


            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"Y = {registers["Y"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "20");
            Assert.IsTrue(registers["X"] == "16");
            Assert.IsTrue(registers["Y"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["C"] == "False");


        }
    }
}
