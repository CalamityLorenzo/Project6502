using System.Reflection.Metadata.Ecma335;

namespace Six502Assembler
{
    public class S502Assembler
    {
        char[] sourceCode = null;
        private int currentCharPosition;
        private List<Tokens6502> tokenList = new List<Tokens6502>();    

        public S502Assembler() { }

        public List<Tokens6502> ParseSource(string input)
        {
            // Okay we get a string, and what we really want to do is treat it as a stream of chars.
            // Effectively just moving forward one char at a time almost like a stack.
            sourceCode = input.ToCharArray();

            while (!AtEnd())
            {
                Parse();
            }
            return this.tokenList;
        }


        void Parse()
        {
            switch (sourceCode[currentCharPosition])
            {
                // Label text
                case '.' or '_':
                    var p = Peek();
                    if (Char.IsLetter(p))
                    {
                        Read();
                        CreateLabel();
                    }
                    break;
                case char a when char.IsLetter(a):
                    CreateLabel();
                    break;
                case char a when char.IsDigit(a):
                    {
                        var startPos = currentCharPosition;
                        while (char.IsDigit(sourceCode[currentCharPosition]))
                        {
                            Read();
                        }
                        tokenList.Add(new Tokens6502(TokenType.Number, new string(sourceCode[startPos..currentCharPosition])));
                    }
                    break;
                // Immediate Value or Address
                case '#':
                    tokenList.Add(new Tokens6502(TokenType.Splat, "#"));
                    Read();
                    break;
                // It's an address, but is it numericalv or label?
                case '$':
                    {
                        if (char.IsLetterOrDigit(Peek()))
                        {
                            var tokenType = char.IsLetter(Read()) ? TokenType.AddressLabel : TokenType.Address;
                            var startPos = currentCharPosition;
                            while (char.IsLetterOrDigit(Peek()))
                            {
                                Read();
                            }
                            Read();
                            tokenList.Add(new Tokens6502(tokenType, new string(sourceCode[startPos..currentCharPosition])));
                        }
                    }
                    break;
                // Do not care new lines
                case ' ':
                case '\n':
                case '\r':
                    Read();
                    break;
                // comments
                case ';':
                    {
                        // Comments are the rest of the line
                        var startPos = currentCharPosition;
                        while (sourceCode[currentCharPosition] != '\n')
                        {
                            Read();
                        }
                        tokenList.Add(new Tokens6502(TokenType.Comment, new string(sourceCode[startPos..currentCharPosition])));
                    }
                    break;
            }
        }

        private void CreateLabel()
        {
            // This is hopefully a label
            var currentPos = currentCharPosition;
            while (char.IsLetter(Peek()))
            {
                // Read the next char until!
                Read();
            }
            // Then move past the last token
            Read();
            tokenList.Add(new Tokens6502(TokenType.Label, new string(sourceCode[currentPos..currentCharPosition])));
        }


        char Read()
        {
            if (currentCharPosition +1 < this.sourceCode.Length)
            {
                var item = sourceCode[currentCharPosition];
                this.currentCharPosition += 1;
                return item;
            }
            else
            {
                return '\0';// throw new ArgumentOutOfRangeException("Passed end of stream");
            }
        }

        char Peek()
        {
            if (currentCharPosition < this.sourceCode.Length-1)
            {
                var item = sourceCode[currentCharPosition + 1];
                return item;
            }
            else
            {
                return '\0';
            }
        }

        char Next()
        {
            this.currentCharPosition += 1;
            if (currentCharPosition < this.sourceCode.Length)
            {
                return sourceCode[currentCharPosition];
            }
            else
            {
                throw new ArgumentOutOfRangeException("Passed end of stream");
            }
        }

        bool AtEnd()
        {
            return this.currentCharPosition == sourceCode.Length - 1;
        }
    }
}