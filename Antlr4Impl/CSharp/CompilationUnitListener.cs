using System;
using System.Collections.Generic;
using System.Diagnostics;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public class CompilationUnitListener : CSharpParserBaseListener
    {
        public List<ClassInfo> OuterClassInfos { get; private set; }
        private readonly string FilePath;

        public CompilationUnitListener(string filePath)
        {
            FilePath = filePath;
            OuterClassInfos = new List<ClassInfo>();
        }

        public override void EnterCompilation_unit(CSharpParser.Compilation_unitContext context)
        {
            var namespaceDeclarationListener = new NamespaceDeclarationMemberListener(OuterClassInfos);
            
            foreach (var namespaceMemberDeclaration in context.namespace_member_declarations().namespace_member_declaration())
            {
                namespaceMemberDeclaration.EnterRule(namespaceDeclarationListener);
                
                /*Console.WriteLine();
                Console.WriteLine(namespaceMemberDeclaration.type_declaration().class_definition().identifier().GetText());
                Console.WriteLine(namespaceMemberDeclaration.type_declaration().class_definition().GetText());
                Console.WriteLine();
                Console.WriteLine(namespaceMemberDeclaration.type_declaration().class_definition().class_base().GetText());
                Console.WriteLine();
                Console.WriteLine(namespaceMemberDeclaration.type_declaration().class_definition().class_body().GetText());*/
                //Console.WriteLine(namespaceMemberDeclaration.namespace_declaration().GetText());
                
                //Console.WriteLine(namespaceMemberDeclaration);
            }
        }
    }
}