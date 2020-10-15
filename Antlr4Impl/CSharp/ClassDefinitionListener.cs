using System;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public class ClassDefinitionListener : CSharpParserBaseListener
    {
        public ClassInfo ClassInfo;
        
        public ClassDefinitionListener()
        {
        }

        public override void EnterClass_definition(CSharpParser.Class_definitionContext context)
        {
            /*Console.WriteLine(context.GetText());
            Console.WriteLine();
            Console.WriteLine(context.class_base().GetText());
            Console.WriteLine();
            Console.WriteLine(context.class_body().GetText());*/

            var nameString = context.identifier().GetText();
            var name = new ClassName(nameString);
            
            Console.WriteLine($"Parse class {nameString}");
            Console.WriteLine($"Implement methods");
            Console.WriteLine($"Implement fields");
            Console.WriteLine($"Implement flags");
            Console.WriteLine($"Implement innerClasses");
            Console.WriteLine($"Implement headerSource");
            
            ClassInfo = new ClassInfo(
                name, 
                null, 
                null, 
                AccessFlags.None, 
                null, 
                null, 
                false);
        }
    }
}