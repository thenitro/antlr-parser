using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4Gen.CSharp;

namespace antlr_parser.Antlr4Impl.CSharp
{
    //With this, I'm basically attempting to mimic the Java implementation of this listener to then make my way to the class and member methods
    public class TypeDeclarationListener : CSharpParserBaseListener
    {
        readonly string parentFilePath;
        public ClassInfo OuterClassInfo;
        public List<ClassInfo> OuterClassInfos;

        public TypeDeclarationListener(string _parentFilePath)
        {
            this.parentFilePath = _parentFilePath;
        }
        
        //I think I should be using a different method here?
        public override void EnterNamespace_member_declaration([NotNull] CSharpParser.Namespace_member_declarationContext context)
        {
            //base.EnterNamespace_member_declaration(context);
            //enter namespace
            if (context.type_declaration() != null)
            {
                TypeDefinitionListener typeDefinitionListener = new TypeDefinitionListener(parentFilePath);
                context.type_declaration().EnterRule(typeDefinitionListener);
                if(typeDefinitionListener.OuterClassInfo != null)
                {
                    OuterClassInfo = typeDefinitionListener.OuterClassInfo;

                }
            }
            if(context.namespace_declaration() != null)
            {
                NamespaceDeclarationListener namespaceDeclarationListener = new NamespaceDeclarationListener(parentFilePath);
                context.namespace_declaration().EnterRule(namespaceDeclarationListener);
                //return;
                if(namespaceDeclarationListener.OuterClassInfos != null)
                {
                    OuterClassInfos = namespaceDeclarationListener.OuterClassInfos;
                }
            }
        }
        

    }

    public class NamespaceDeclarationListener : CSharpParserBaseListener
    {
        readonly string filePath;
        //public ClassInfo OuterClassInfo;
        public List<ClassInfo> OuterClassInfos;

        public NamespaceDeclarationListener(string _filePath)
        {
            filePath = _filePath;
        }
        public override void EnterNamespace_declaration([NotNull] CSharpParser.Namespace_declarationContext context)
        {
            //WE GO HERE
            if(context.namespace_body() != null)
            {
                NamespaceBodyListener namespaceBodyListener = new NamespaceBodyListener(filePath);
                context.namespace_body().EnterRule(namespaceBodyListener);
                if(namespaceBodyListener.OuterClassInfos != null)
                {
                    OuterClassInfos = namespaceBodyListener.OuterClassInfos;
                }
            }
        }

        
    }

    public class NamespaceBodyListener : CSharpParserBaseListener
    {
        readonly string filePath;
        //public ClassInfo OuterClassInfo;
        public List<ClassInfo> OuterClassInfos;

        public NamespaceBodyListener(string _filePath)
        {
            filePath = _filePath;
        }
        public override void EnterNamespace_body([NotNull] CSharpParser.Namespace_bodyContext context)
        {
            //base.EnterNamespace_body(context);

            if(context.namespace_member_declarations() != null)
            {
                NamespaceMemberListener namespaceMemberListener = new NamespaceMemberListener(filePath);
                context.namespace_member_declarations().EnterRule(namespaceMemberListener);
                if (namespaceMemberListener.OuterClassInfos != null)
                {
                    OuterClassInfos = namespaceMemberListener.OuterClassInfos;
                }
            }
        }
    }

    public class NamespaceMemberListener : CSharpParserBaseListener
    {
        //public ClassInfo OuterClassInfo;
        public List<ClassInfo> OuterClassInfos = new List<ClassInfo>();
        readonly string filePath;

        public NamespaceMemberListener(string _filePath)
        {
            filePath = _filePath;
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
            }


        }
    }

    public class TypeDefinitionListener : CSharpParserBaseListener
    {
        public ClassInfo OuterClassInfo;
        readonly string parentFilePath;

        public TypeDefinitionListener(string _parentFilePath)
        {
            parentFilePath = _parentFilePath;
        }
        public override void EnterType_declaration([NotNull] CSharpParser.Type_declarationContext context)
        {
            // base.EnterType_declaration(context);

            //if theres a class defined
            if (context.class_definition() != null)
            {
                ClassDeclarationListener classDeclarationListener = new ClassDeclarationListener(parentFilePath);
                context.class_definition().EnterRule(classDeclarationListener);
                if(classDeclarationListener.ClassInfo != null)
                {
                    OuterClassInfo = classDeclarationListener.ClassInfo;
                }
            }
            //TODO: handle interface and enums?
            if(context.interface_definition() != null)
            {

            }
            if(context.enum_definition() != null)
            {

            }
        }

    }

    public class ClassDeclarationListener : CSharpParserBaseListener
    {
        string parentFile;
        public ClassInfo ClassInfo;
        public ClassDeclarationListener(string _parentFile)
        {
            this.parentFile = _parentFile;
        }
        public override void EnterClass_definition([NotNull] CSharpParser.Class_definitionContext context)
        {
            //base.EnterClass_definition(context);
            ClassName className = new ClassName($"{parentFile}|{context.identifier().GetText()}");


            string headerText = context.GetFullText();
            if (headerText.Contains("{"))
            {
                headerText = headerText.Substring(0, headerText.IndexOf("{", System.StringComparison.Ordinal));
            }
            ClassBodyListener classBodyListener = new ClassBodyListener(className);
            context.class_body().EnterRule(classBodyListener);
            //entering nulls for now, I only care about class info
            ClassInfo = new ClassInfo(className, classBodyListener.MethodInfos, new List<FieldInfo>(), new AccessFlags(), classBodyListener.InnerClasses, headerText, false);
        }
    }

    public class ClassBodyListener : CSharpParserBaseListener
    {
        ClassName parentClass;
        public List<MethodInfo> MethodInfos;
        public List<ClassInfo> InnerClasses;
        public ClassBodyListener(ClassName _parentClass)
        {
            parentClass = _parentClass;
        }
        public override void EnterClass_body([NotNull] CSharpParser.Class_bodyContext context)
        {
            //base.EnterClass_body(context);
            ClassMemberDeclarationListener classMemberDeclarationListener = new ClassMemberDeclarationListener(parentClass);
            context.class_member_declarations().EnterRule(classMemberDeclarationListener);
            if(classMemberDeclarationListener.MethodInfos != null)
            {
                MethodInfos = classMemberDeclarationListener.MethodInfos;

            }
            if (classMemberDeclarationListener.InnerClasses != null)
            {
                InnerClasses = classMemberDeclarationListener.InnerClasses;

            }
        }
    }

    public class ClassMemberDeclarationListener : CSharpParserBaseListener
    {
        ClassName parentClassName;
        public List<MethodInfo> MethodInfos = new List<MethodInfo>();
        public List<ClassInfo> InnerClasses = new List<ClassInfo>();

        public ClassMemberDeclarationListener(ClassName _parentClassName)
        {
            parentClassName = _parentClassName;
        }
        public override void EnterClass_member_declarations([NotNull] CSharpParser.Class_member_declarationsContext context)
        {
            if (context.class_member_declaration() == null) return;


            //base.EnterClass_member_declarations(context);
            MemberDeclarationListener memberDeclarationListener = new MemberDeclarationListener(parentClassName);
            foreach(CSharpParser.Class_member_declarationContext memberDeclaration in context.class_member_declaration())
            {
                memberDeclaration.EnterRule(memberDeclarationListener);
                if (memberDeclarationListener.MethodInfo != null)
                {
                    MethodInfos.Add(memberDeclarationListener.MethodInfo);
                }
                if (memberDeclarationListener.InnerClass != null)
                {
                    InnerClasses.Add(memberDeclarationListener.InnerClass);
                }
            }

            
        }
    }
}