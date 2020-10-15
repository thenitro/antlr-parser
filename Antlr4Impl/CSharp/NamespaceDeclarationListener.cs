using System;
using System.Collections.Generic;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public class NamespaceDeclarationListener : CSharpParserBaseListener
    {
        private List<ClassInfo> _classes;
        
        public NamespaceDeclarationListener(List<ClassInfo> classes)
        {
            _classes = classes;
        }

        public override void EnterNamespace_member_declaration(CSharpParser.Namespace_member_declarationContext context)
        {
            base.EnterNamespace_member_declaration(context);
            
            Console.WriteLine();
            Console.WriteLine(context.type_declaration().class_definition().identifier().GetText());
            Console.WriteLine(context.type_declaration().class_definition().GetText());
            Console.WriteLine();
            Console.WriteLine(context.type_declaration().class_definition().class_base().GetText());
            Console.WriteLine();
            Console.WriteLine(context.type_declaration().class_definition().class_body().GetText());
            
            Console.WriteLine();
            Console.WriteLine(context.namespace_declaration().GetText());
            
            
        }
    }
}