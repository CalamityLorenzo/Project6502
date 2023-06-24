using Project6502;
using System.Diagnostics;

namespace LoadStoreOperations
{
    [TestClass]
    public class STA
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);


        [TestMethod("STA : Zero Page")]
        public void STA_ZeroPage()
        {

            var memory = new byte[ushort.MaxValue];
            memory[162] = 100;

            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9, // LDA 200
                0xC8,

                0x85, // STA 
                0xA2,
            };

            processor.AdhocProcess(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            Trace.WriteLine($"mem[{0xA2}] =  {memory[0xA2]}");
            Assert.IsTrue(memory[0xA2] == 0xC8);

        }

        [TestMethod("STA : Zero Page.X")]
        public void STA_ZeroPage_X()
        {
            var memory = new byte[ushort.MaxValue];

            //memory[162] = 100;
            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9, // LDA 64
                0x40,
                0xAA, // Put 64 in X TAX

                0xA9, // LDA 200
                0xC8,

                0x95, // STA -> (A2+64) = e2 = 200;
                0xA2,
            };

            processor.AdhocProcess(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            Trace.WriteLine($"mem[{(0xA2 + 0x40)}] =  {memory[0xA2 + 0x40]}");
            Assert.IsTrue(memory[0xA2 + 0x40] == 0xC8);
        }

        [TestMethod("STA : Absolute")]
        public void STA_Absolute()
        {

            var memory = new byte[ushort.MaxValue];

            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9, // LDA 200
                0xC8,

                0x8D, // STA 
                0x40,
                0xA2, // Remeber A full 16bit address = 0xA240 (Not 0xA2 + 0x40)
            };

            processor.AdhocProcess(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            var memAddress = (0xA2 << 8 | 0x40);
            Trace.WriteLine($"mem[{memAddress}] =  {memory[memAddress]}");
            Assert.IsTrue(memory[memAddress] == 200);

        }

        [TestMethod("STA : Absolute,X")]
        public void STA_Absolute_X()
        {

            var memory = new byte[ushort.MaxValue];

            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9, // LDA 64
                0x40,
                0xAA, // Put 64 in X TAX

                0xA9, // LDA 200
                0xC8,

                0x9D, // STA (mem),X = ($A2,$40), $40
                0x40,
                0xA2, // Remeber A full 16bit address = 0xA240 (Not 0xA2 + 0x40)
            };

            processor.AdhocProcess(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            var memAddress = (0xA2 << 8 | 0x40)+ 0x40;
            Trace.WriteLine($"mem[{memAddress}] =  {memory[memAddress]}");
            Assert.IsTrue(memory[memAddress] == 0xC8);

        }
        [TestMethod("STA : Absolute,Y")]
        public void STA_Absolute_Y()
        {

            var memory = new byte[ushort.MaxValue];

            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9, // LDA 64
                0x32,
                0xA8, // Put 64 in X TAX

                0xA9, // LDA 200
                0xC8,

                0x99, // STA (mem),X = ($A2,$40), $40
                0x40,
                0xA2, // Remeber A full 16bit address = 0xA240 (Not 0xA2 + 0x40)
            };

            processor.AdhocProcess(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            var memAddress = (0xA2 << 8 | 0x40) + 0x32;
            Trace.WriteLine($"mem[{memAddress}] =  {memory[memAddress]}");
            Assert.IsTrue(memory[memAddress] == 0xC8);

        }

        [TestMethod("STA : Indirect,X")]
        public void STA_Indirext_X()
        {

            var memory = new byte[ushort.MaxValue];
            
            memory[212] = 00;
            memory[213] = 10;
            
            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9, // LDA 64
                0x32,
                0xAA, // Put 64 in X TAX

                0xA9, // LDA 200
                0xC8,

                0x81, // STA (mem,X) = ($A2, $32) = $A2+ $32 =  $C4 Which = $c5 << $c4
                0xA2, // Remeber A full 16bit address = 0xA240 (Not 0xA2 + 0x40)
            };

            processor.AdhocProcess(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            var memAddress = (0xA2 + 0x32);

            Trace.WriteLine($"lsb = {memory[memAddress]}");
            Trace.WriteLine($"msb = {memory[memAddress+1]}");

            var lsb = memory[memAddress];
            var msb = memory[memAddress+1];

            Trace.WriteLine($"mem[{msb << 8 | lsb}] =  {memory[msb << 8 | lsb]}");
            Assert.IsTrue(memory[msb << 8 | lsb] == 0xC8);

        }

        [TestMethod("STA : Indirect,Y")]
        public void STA_Indirext_Y()
        {

            var memory = new byte[ushort.MaxValue];

            memory[0xA1] = 0x90; //lsb 
            memory[0xA2] = 01;   // msb
            var processor = createProcessor(memory);
            var program = new byte[]
            {
                0xA9,
                0x96, // 150 in Acc

                0xA0,
                0x07, // 7 in Y

                0x91, // STA 150 in (00 + 200) + 7 =mem207
                0xA1, // (mem[161] mem[162])
            };

            processor.AdhocProcess(program);

            var registers = processor.Registers();

            Trace.WriteLine($"A {registers["A"]}");
            Trace.WriteLine($"Y = {registers["Y"]}");
            var memAddress = (407);
            var lsb = memory[0xA1];
            var msb = memory[0xA2];
            var yReg = int.Parse(registers["Y"]);
            Trace.WriteLine($"lsb = {memory[0xA1]}");
            Trace.WriteLine($"msb = {memory[0xA2 + 1]}");
            Trace.WriteLine($"Address = {(msb<<8 | lsb) + yReg }");


            Trace.WriteLine($"mem[{(msb << 8 | lsb) + yReg}] =  {memory[(msb << 8 | lsb) + yReg]}");
            Assert.IsTrue(memory[(msb << 8 | lsb)+yReg   ] == 0x96);

        }
    }
}
