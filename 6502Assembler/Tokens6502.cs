namespace Six502Assembler
{
    public enum TokenType{
    Unknown =0,
    Label,
    Address,
    Splat,
    Number,
    Operator,
    Keyword,
        Comment,
        AddressLabel
    }
    public record Tokens6502 (TokenType Type, String Value );
    
}