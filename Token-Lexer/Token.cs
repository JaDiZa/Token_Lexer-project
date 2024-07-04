using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum TokenType
{
    keyword,
    Identifier,
    Literal,
    Operator,
    Separator,
    Comment,
    PreprocessorDirective,
    Unknown,
    Keyword
}

public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }

    public Token(TokenType type, string value)
    {  Type = type; Value = value; }

    public override string ToString()
    {
        return $"{Type}: {Value}";
    }


}

