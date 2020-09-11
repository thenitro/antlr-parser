using System;

namespace antlr_parser.Antlr4Impl.Go
{
    public class FunctionDeclListener : GoParserBaseListener
    {
        public override void EnterFunctionDecl(GoParser.FunctionDeclContext context)
        {
            Console.WriteLine(context.IDENTIFIER().ToString());
        }
    }
}