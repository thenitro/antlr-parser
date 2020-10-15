using System;
using System.Collections.Generic;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public class NamespaceDeclarationMemberListener : CSharpParserBaseListener
    {
        private List<ClassInfo> _classes;
        
        public NamespaceDeclarationMemberListener(List<ClassInfo> classes)
        {
            _classes = classes;
        }

        public override void EnterNamespace_member_declaration(CSharpParser.Namespace_member_declarationContext context)
        {            
            var typeDeclarationListener = new TypeDeclarationListener();
            context.type_declaration().EnterRule(typeDeclarationListener);
            
            _classes.Add(typeDeclarationListener.OuterClassInfo);
            
            /*Console.WriteLine();
            Console.WriteLine(context.namespace_declaration().GetText());*/
        }
    }
}