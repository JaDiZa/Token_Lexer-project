using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Lexer
{
    private static readonly HashSet<string> Keywords = new HashSet<string>
    {
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue",
        "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally",
        "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long",
        "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public",
        "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct",
        "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual",
        "void", "volatile", "while"
    };

    private static readonly HashSet<string> Operators = new HashSet<string>
    {
        "+", "-", "*", "/", "%", "==", "!=", "<", ">", "<=", ">=", "&&", "||", "!", "&", "|", "^", "~", "<<", ">>",
        "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^=", "<<=", ">>=", "++", "--", "?", ":", "??", "??=", "=>"
    };

    private static readonly HashSet<string> Separators = new HashSet<string> { "(", ")", "{", "}", "[", "]", ";", ":", ",", "." };

    //^ asserts postion at the start of the string
    //[a-zA-Z_] mathces single character that is a letter or underscore | [a-zA-Z0-9_] mathces zero or more charecters that are either letter (a-z,A-z) digits(0-9) or underscore allow identifier to be followed by any combination of characters
    private static readonly Regex IdentifierRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*");
    //\d+ mathces one or more digits 
    private static readonly Regex IntegerLiteralRegex = new Regex(@"^\d+");
    //usedto match string literals
    // \" matches a double qoute char 0 or more times
    private static readonly Regex StringLiteralRegex = new Regex("^\".*?\"");
    //used to match character literals in C#
    private static readonly Regex CharLiteralRegex = new Regex(@"^'.'");

    public List<Token> Tokenize(string sourceCode)
    { 
        List<Token> tokens = new List<Token>();
        int index = 0;

        while (index < sourceCode.Length) 
        { 
            char currentChar = sourceCode[index];

            //skips the whitespacesin the code
            if (char.IsWhiteSpace(currentChar))
            {
                index++;
                continue;
            }

            //check for keywords and identifiers
            if (char.IsLetter(currentChar) || currentChar == '_')
            {
                var match = IdentifierRegex.Match(sourceCode.Substring(index));
                if (match.Success)
                {
                    string value = match.Value;
                    TokenType type = Keywords.Contains(value) ? TokenType.Keyword : TokenType.Identifier;
                    tokens.Add(new Token(type, value));
                    index += value.Length;
                    continue;
                }
            }

            // Check for literals
            if (char.IsDigit(currentChar))
            {
                var match = IntegerLiteralRegex.Match(sourceCode.Substring(index));
                if (match.Success)
                {
                    tokens.Add(new Token(TokenType.Literal, match.Value));
                    index += match.Value.Length;
                    continue;
                }
            }
            if (currentChar == '"')
            {
                var match = StringLiteralRegex.Match(sourceCode.Substring(index));
                if (match.Success)
                {
                    tokens.Add(new Token(TokenType.Literal, match.Value));
                    index += match.Value.Length;
                    continue;
                }
            }
            if (currentChar == '\'')
            {
                var match = CharLiteralRegex.Match(sourceCode.Substring(index));
                if (match.Success)
                {
                    tokens.Add(new Token(TokenType.Literal, match.Value));
                    index += match.Value.Length;
                    continue;
                }
            }

            // Check for operators
            bool isOp = false;
            foreach (var op in Operators)
            {
                if (sourceCode.Substring(index).StartsWith(op))
                {
                    tokens.Add(new Token(TokenType.Operator, op));
                    index += op.Length;
                    isOp = true;
                    break;
                }
            }

            // Check for separators
            if (Separators.Contains(currentChar.ToString()))
            {
                tokens.Add(new Token(TokenType.Separator, currentChar.ToString()));
                index++;
                continue;
            }
            if (isOp) { continue; }
            // Unknown token
            tokens.Add(new Token(TokenType.Unknown, currentChar.ToString()));
            index++;
        }

        return tokens;
    }
}


