using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public static class AntlrParseCSharp
    {
        public static IEnumerable<ClassInfo> OuterClassInfosFromCSharpSource(string source, string filePath)
        {
            try
            {
                char[] codeArray = source.ToCharArray();
                AntlrInputStream inputStream = new AntlrInputStream(codeArray, codeArray.Length);

                CSharpLexer lexer = new CSharpLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                CSharpParser parser = new CSharpParser(commonTokenStream);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ErrorListener()); // add ours

                // TODO find the highest level compilation unity and start a listener
                CompilationUnitListener compilationUnitListener = new CompilationUnitListener(filePath);

                //Tried this, learned I couldn't iterate over what I wanted to
                //parser.compilation_unit().EnterRule(compilationUnitListener);
                //using this works for entering namespace
                parser.namespace_member_declarations().EnterRule(compilationUnitListener);

                parser.compilation_unit().EnterRule(compilationUnitListener);
                
                // replace this
                //return new List<ClassInfo>();
                return compilationUnitListener.OuterClassInfos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}