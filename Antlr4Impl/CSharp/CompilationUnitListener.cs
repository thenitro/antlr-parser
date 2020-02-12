using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4Gen.CSharp;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public class CompilationUnitListener : CSharpParserBaseListener
    {
        public readonly List<ClassInfo> OuterClassInfos = new List<ClassInfo>();
        readonly string filePath;

        public CompilationUnitListener(string _filePath)
        {
            this.filePath = _filePath;
        }

       
        public override void EnterNamespace_member_declarations([NotNull] CSharpParser.Namespace_member_declarationsContext context)
        {
            //base.EnterNamespace_member_declaration(context);
            //ENTER NAMESPACE??
            TypeDeclarationListener typeDeclarationListener = new TypeDeclarationListener(filePath);
            foreach (CSharpParser.Namespace_member_declarationContext namespaceMemberDeclaration in context.namespace_member_declaration())
            {
                namespaceMemberDeclaration.EnterRule(typeDeclarationListener);

                if(typeDeclarationListener.OuterClassInfo != null)
                {
                    OuterClassInfos.Add(typeDeclarationListener.OuterClassInfo);
                    typeDeclarationListener.OuterClassInfo = null;
                }
            }
        }
        //couldn't get this to follow through
        public override void EnterCompilation_unit([NotNull] CSharpParser.Compilation_unitContext context)
        {
            //base.EnterCompilation_unit(context);
            //foreach (CSharpParser.Namespace_member_declarationContext namespaceMemberDeclaration in context.namespace_member_declaration())


        }


    }
}