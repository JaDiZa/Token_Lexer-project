class Program
{
    static void Main()
    {
        string code = "int x = 42; if (x > 10) { x += 5; }";

        Lexer lexer = new Lexer();
        List<Token> tokens = lexer.Tokenize(code);

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}
