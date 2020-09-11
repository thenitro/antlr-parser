using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.Go
{
    public class AntlrParseGo
    {
        public static IEnumerable<ClassInfo> OuterClassInfosFromGoSource(string source, string filePath)
        {
            try
            {
                char[] codeArray = source.ToCharArray();
                AntlrInputStream inputStream = new AntlrInputStream(codeArray, codeArray.Length);

                GoLexer lexer = new GoLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                GoParser parser = new GoParser(commonTokenStream);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ErrorListener()); // add ours

                // TODO go through each member type in the parser object and traverse with listeners
                FunctionDeclListener functionDeclListener = new FunctionDeclListener();
                parser.functionDecl().EnterRule(functionDeclListener);
                
                // TODO return assembled class infos
                return new List<ClassInfo>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new List<ClassInfo>();
        }
    }
}