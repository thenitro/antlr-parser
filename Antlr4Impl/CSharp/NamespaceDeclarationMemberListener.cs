using System.Collections.Generic;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public class NamespaceDeclarationMemberListener : CSharpParserBaseListener
    {
        private readonly List<ClassInfo> _classes;
        private readonly string _parentFileName;
        
        public NamespaceDeclarationMemberListener(List<ClassInfo> classes, string parentFileName)
        {
            _classes = classes;
            _parentFileName = parentFileName;
        }

        public override void EnterNamespace_member_declaration(CSharpParser.Namespace_member_declarationContext context)
        {            
            var typeDeclarationListener = new TypeDeclarationListener(_parentFileName);
            context.type_declaration().EnterRule(typeDeclarationListener);
            
            _classes.Add(typeDeclarationListener.OuterClassInfo);
            
            /*Console.WriteLine();
            Console.WriteLine(context.namespace_declaration().GetText());*/
        }
    }
}