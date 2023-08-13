using Six502Assembler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6502_Testing.Parser
{
    [TestClass]
    public class ParserTesting
    {
        [TestMethod]
        public void BasicTokenParsing()
        {
            var program = """
                LDA #100
                STA #$100
                BNE $80
                .pidgeon
                Fred
                _colin
                LDA colin
                STX $Fred
                """;

            var p = new S502Assembler();

            var tokens = p.ParseSource(program);

            Assert.IsTrue(tokens.Count > 0);
        }
    }
}
