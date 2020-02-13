using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4Gen.CSharp;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public class CompilationUnitListener : CSharpParserBaseListener
    {
        public List<ClassInfo> OuterClassInfos = new List<ClassInfo>();
        readonly string filePath;

        public CompilationUnitListener(string _filePath)
        {
            this.filePath = _filePath;
        }

       
        //public override void EnterNamespace_member_declarations([NotNull] CSharpParser.Namespace_member_declarationsContext context)
        //{
        //    //base.EnterNamespace_member_declaration(context);
        //    //ENTER NAMESPACE??
        //    TypeDeclarationListener typeDeclarationListener = new TypeDeclarationListener(filePath);
        //    foreach (CSharpParser.Namespace_member_declarationContext namespaceMemberDeclaration in context.namespace_member_declaration())
        //    {
        //        namespaceMemberDeclaration.EnterRule(typeDeclarationListener);

        //        if(typeDeclarationListener.OuterClassInfo != null)
        //        {
        //            OuterClassInfos.Add(typeDeclarationListener.OuterClassInfo);
        //            typeDeclarationListener.OuterClassInfo = null;
        //        }
        //    }
        //}
        //couldn't get this to follow through
        public override void EnterCompilation_unit([NotNull] CSharpParser.Compilation_unitContext context)
        {
            if(context.using_directives() != null)
            {
                UsingDirectivesListener usingDirectivesListener = new UsingDirectivesListener();
                context.using_directives().EnterRule(usingDirectivesListener);
            }

            if(context.namespace_member_declarations() != null)
            {
                //return;
                NamespaceOrTypeListener namespaceOrTypeListener = new NamespaceOrTypeListener(filePath);
                context.namespace_member_declarations().EnterRule(namespaceOrTypeListener);
                if(namespaceOrTypeListener.OuterClassInfos.Count > 0)
                {
                    OuterClassInfos = new List<ClassInfo>(namespaceOrTypeListener.OuterClassInfos);
                }
            }

        }


    }

    public class UsingDirectivesListener : CSharpParserBaseListener
    {
        
        public override void EnterUsing_directives([NotNull] CSharpParser.Using_directivesContext context)
        {
            //base.EnterUsing_directives(context);
            return;
            //WE GO HERE
        }
    }


    public class NamespaceOrTypeListener : CSharpParserBaseListener
    {
        public List<ClassInfo> OuterClassInfos;
        readonly string filePath;

        public NamespaceOrTypeListener(string _filePath)
        {
            filePath = _filePath;
            OuterClassInfos = new List<ClassInfo>();
        }
        
        public override void EnterNamespace_member_declarations([NotNull] CSharpParser.Namespace_member_declarationsContext context)
        {
            // base.EnterNamespace_member_declarations(context);
            //return;
            //WE GO HERE
            TypeDeclarationListener typeDeclarationListener = new TypeDeclarationListener(filePath);
            foreach (CSharpParser.Namespace_member_declarationContext namespaceMemberDeclaration in context.namespace_member_declaration())
            {
                namespaceMemberDeclaration.EnterRule(typeDeclarationListener);

                if (typeDeclarationListener.OuterClassInfo != null)
                {
                    OuterClassInfos.Add(typeDeclarationListener.OuterClassInfo);
                    //typeDeclarationListener.OuterClassInfo = null;
                }
                if(typeDeclarationListener.OuterClassInfos.Count > 0)
                {
                    OuterClassInfos = new List<ClassInfo>(typeDeclarationListener.OuterClassInfos);
                }
            }

            
        }
    }

}