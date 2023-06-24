using Project6502;
using System.Diagnostics;

namespace StatusFlags
{
    [TestClass]
    public class StatusFlagChange4s
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("CLC : SEC Clear/SetCarry Flag")]
        public void ClearSetCarryFlag()
        {

            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {

                0x38, // SEC 
                0x18  // CLC
            };

            processor.LoadProgram(program);
            processor.InstructionStep();
            var registers = processor.Registers();

            Trace.WriteLine($"PC = {registers["PC"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Assert.IsTrue(registers["C"] == "True");

            processor.InstructionStep();
            registers = processor.Registers();
            Trace.WriteLine($"C = {registers["C"]}");
            Assert.IsTrue(registers["C"] == "False");

        }
        [TestMethod("CLD : SED Clear/Set Decimal flag")]
        public void ClearSetDecimalFlag()
        {

            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {

                0xF8, // SED 
                0xD8  // CLD
            };

            processor.LoadProgram(program);
            processor.InstructionStep();
            var registers = processor.Registers();

            Trace.WriteLine($"PC = {registers["PC"]}");
            Trace.WriteLine($"D = {registers["D"]}");
            Assert.IsTrue(registers["D"] == "True");

            processor.InstructionStep();
            registers = processor.Registers();
            Trace.WriteLine($"D = {registers["D"]}");
            Assert.IsTrue(registers["D"] == "False");

        }

        [TestMethod("CLI : SEI Clear/Set Interrupt flag")]
        public void ClearSetInterruptFlag()
        {

            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {

                0x78, // SEI
                0x58  // CLI
            };

            processor.LoadProgram(program);
            processor.InstructionStep();
            var registers = processor.Registers();

            Trace.WriteLine($"PC = {registers["PC"]}");
            Trace.WriteLine($"I = {registers["I"]}");
            Assert.IsTrue(registers["I"] == "True");

            processor.InstructionStep();
            registers = processor.Registers();
            Trace.WriteLine($"I = {registers["I"]}");
            Assert.IsTrue(registers["I"] == "False");

        }

        [TestMethod("CLV : Clear overflow flag")]
        public void ClearOverflowInterruptFlag()
        {

            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {
                0xa9, // LDA #255
                0xFF,
                0x69, // ADC #05
                0x05,
                0xB8, // CLV
            };

            processor.LoadProgram(program);
            processor.InstructionStep();
            processor.InstructionStep();
            var registers = processor.Registers();

            Trace.WriteLine($"PC = {registers["PC"]}");
            Trace.WriteLine($"V = {registers["V"]}");
            Assert.IsTrue(registers["V"] == "True");

            processor.InstructionStep();
            registers = processor.Registers();
            Trace.WriteLine($"V = {registers["V"]}");

            Assert.IsTrue(registers["V"] == "False");

        }

        [TestMethod("SEC : Set carry Flag")]
        public void SetCarryFlat()
        {

            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {
                0x38
            };
            processor.LoadProgram(program);
            processor.InstructionStep();
            var regs = processor.Registers();
            Trace.WriteLine($"Carry = {regs["C"]}");
            Assert.IsTrue(regs["C"] == "True");
        }

        [TestMethod("SED : Set Decimal Flag")]
        public void SetDecimalFlag()
        {

            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0xF8
            };
            processor.LoadProgram(program);
            processor.InstructionStep();
            var regs = processor.Registers();
            Trace.WriteLine($"Carry = {regs["D"]}");
            Assert.IsTrue(regs["D"] == "True");
        }

        [TestMethod("SEI : Set interrupt  Flag")]
        public void SetInterruprtFlag()
        {

            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0x78
            };
            processor.LoadProgram(program);
            processor.InstructionStep();
            var regs = processor.Registers();
            Trace.WriteLine($"Carry = {regs["I"]}");
            Assert.IsTrue(regs["I"] == "True");
        }
    }
}
