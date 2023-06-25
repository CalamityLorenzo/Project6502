using Project6502;
using System.Diagnostics;

namespace Arithmetic
{
    [TestClass]
    public class ADC
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("ADC : Immediate")]
        public void ADC_Immediate()
        {
            var mem = new byte[ushort.MaxValue];
            var processor = createProcessor(mem);
            
            var program = new byte[]{
                0xA9,  // LDA #245
                0x73,
                
                0x69,
                0x12  // Val not memory location
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"V = {registers["V"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "133");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(registers["V"] == "True");

        }

        [TestMethod("ADC : Zero Page")]
        public void ADC_ZeroPage()
        {
            var mem = new byte[ushort.MaxValue];
            mem[22] = 250;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0x11,

                0x65,
                0x16  //  memory location
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"V = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "11");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["C"] == "True");
            Assert.IsTrue(registers["V"] == "True");

        }


        [TestMethod("ADC : Zero Page.X")]
        public void ADC_ZeroPage_X()
        {
            var mem = new byte[ushort.MaxValue];
            mem[22+9] = 80;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0xF5,

                0xA2, // LDX #09
                0x09,

                0x75, 
                0x16  
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "69");
            Assert.IsTrue(registers["X"] == "9");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["C"] == "True");

        }

        [TestMethod("ADC : Absolute")]
        public void ADC_Absolute()
        {
            var mem = new byte[ushort.MaxValue];
            mem[8726] = 80;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0xF5,

                0xA2, // LDX #09
                0x09,

                0x6D,
                0x16,
                0x22
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "69");
            Assert.IsTrue(registers["X"] == "9");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["C"] == "True");

        }

        [TestMethod("ADC : Absolute.X")]
        public void ADC_Absolute_X()
        {
            var mem = new byte[ushort.MaxValue];
            mem[8726+9] = 80;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0xF5,

                0xA2, // LDX #09
                0x09,

                0x7D,
                0x16,
                0x22
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "69");
            Assert.IsTrue(registers["X"] == "9");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["C"] == "True");

     }
        [TestMethod("ADC : Absolute.Y")]
        public void ADC_Absolute_Y()
        {
            var mem = new byte[ushort.MaxValue];
            mem[8726 + 9] = 80;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #245
                0xF5,

                0xA0, // LDY #09
                0x09,

                0x79,
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

            Assert.IsTrue(registers["A"] == "69");
            Assert.IsTrue(registers["X"] == "0");
            Assert.IsTrue(registers["Y"] == "9");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["C"] == "True");

        }

        [TestMethod("ADC : Indirect.X")]
        public void ADC_Indirect_X()
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
                0xA9, // LDA #105
                0x69,
                0xA2, // LDX #16
                0x10,

                0x61, // ADC 0x40
                0x40,
                0x03
            };
            processor.AdhocProgram(program);


            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"Y = {registers["Y"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "190");
            Assert.IsTrue(registers["X"] == "16");
            Assert.IsTrue(registers["Y"] == "0");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(registers["C"] == "False");


        }
    }
}
