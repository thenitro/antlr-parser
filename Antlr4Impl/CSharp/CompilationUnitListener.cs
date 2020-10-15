using System.Collections.Generic;
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
            var namespaceDeclarationListener = new NamespaceDeclarationMemberListener(OuterClassInfos, FilePath);
            
            foreach (var namespaceMemberDeclaration in context.namespace_member_declarations().namespace_member_declaration())
            {
                namespaceMemberDeclaration.EnterRule(namespaceDeclarationListener);
            }
        }
    }
}