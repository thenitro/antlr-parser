using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public static class AntlrParseCSharp
    {
        public static IEnumerable<ClassInfo> OuterClassInfosFromCSharpSource(
            string source, string filePath)
        {
            try
            {
                var codeArray = source.ToCharArray();
                var inputStream = new AntlrInputStream(codeArray, codeArray.Length);
                
                var lexer = new CSharpLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(lexer);
                var compilationUnitListener = new CompilationUnitListener(filePath);
                
                
                var parser = new CSharpParser(commonTokenStream);
                    parser.RemoveErrorListeners();
                    parser.AddErrorListener(new ErrorListener());
                    parser.compilation_unit().EnterRule(compilationUnitListener);
                    
                return compilationUnitListener.OuterClassInfos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }
    }
}