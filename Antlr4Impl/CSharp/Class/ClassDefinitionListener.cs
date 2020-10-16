using System;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp.Class
{
    public class ClassDefinitionListener : CSharpParserBaseListener
    {
        public ClassInfo ClassInfo { get; private set; }
        private readonly string _parentFileName;

        public ClassDefinitionListener(string parentFileName)
        {
            _parentFileName = parentFileName;
        }

        public override void EnterClass_definition(CSharpParser.Class_definitionContext context)
        {
            /*Console.WriteLine(context.GetText());
            Console.WriteLine();
            Console.WriteLine(context.class_base().GetText());
            */

            var nameString = $"{_parentFileName}|{context.identifier().GetText()}";
            var name = new ClassName(nameString);
            
            Console.WriteLine($"Parse class {nameString}");

            var classBodyListener = new ClassBodyListener(name, _parentFileName);
            context.class_body().EnterRule(classBodyListener);
            
            Console.WriteLine($"TODO: Implement innerClasses");
            Console.WriteLine($"TODO: Implement flags");

            var headerText = context.GetText();
            
            ClassInfo = new ClassInfo(
                name, 
                classBodyListener.MethodInfos, 
                classBodyListener.FieldInfos,
                AccessFlags.None,
                new ClassInfo[0], 
                new SourceCodeSnippet(headerText, SourceCodeLanguage.CSharp), 
                false);
        }
    }
}