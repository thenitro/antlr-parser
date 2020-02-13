using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4Gen.CSharp;
using System;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public class MemberDeclarationListener : CSharpParserBaseListener
    {
        ClassName parentClassName;
        public MethodInfo MethodInfo;
        public ClassInfo InnerClass;

        public MemberDeclarationListener(ClassName _parentClassName)
        {
            parentClassName = _parentClassName;
        }
        public override void EnterClass_member_declaration([NotNull] CSharpParser.Class_member_declarationContext context)
        {
            //base.EnterClass_member_declaration(context);
            if (context.common_member_declaration() != null)
            {
                MethodDeclarationListener methodDeclarationListener = new MethodDeclarationListener(parentClassName);
                context.common_member_declaration().EnterRule(methodDeclarationListener);
                if (methodDeclarationListener.InnerClass != null)
                {
                    InnerClass = methodDeclarationListener.InnerClass;
                }
                if (methodDeclarationListener.MethodInfo != null)
                {
                    MethodInfo = methodDeclarationListener.MethodInfo;
                }

            }


        }
    }

    public class MethodDeclarationListener : CSharpParserBaseListener
    {
        ClassName parentClassName;
        public MethodInfo MethodInfo;
        public ClassInfo InnerClass;

        public MethodDeclarationListener(ClassName _parentClassName)
        {
            parentClassName = _parentClassName;
        }
        public override void EnterCommon_member_declaration([NotNull] CSharpParser.Common_member_declarationContext context)
        {
            //base.EnterCommon_member_declaration(context);
            //method declaration
            if(context.method_declaration() != null)
            {
                MethodMemberListener methodMemberListener = new MethodMemberListener(parentClassName);
                context.method_declaration().EnterRule(methodMemberListener);
                if(methodMemberListener.MethodInfo != null)
                {
                    MethodInfo = methodMemberListener.MethodInfo;
                }
            }
            //TODO: handle inner class definition
            if(context.class_definition() != null)
            {
                
                ClassDeclarationListener classDeclarationListener = new ClassDeclarationListener(parentClassName.FullyQualified);
                context.class_definition().EnterRule(classDeclarationListener);
                if(classDeclarationListener.ClassInfo != null)
                {
                    InnerClass = classDeclarationListener.ClassInfo;

                }
            }

            
        }
    }

    public class MethodMemberListener : CSharpParserBaseListener
    {
        public MethodInfo MethodInfo;
        ClassName parentClassName;
        public MethodMemberListener(ClassName _parentClassName)
        {
            parentClassName = _parentClassName;
        }
        public override void EnterMethod_declaration([NotNull] CSharpParser.Method_declarationContext context)
        {
            //base.EnterMethod_declaration(context);
            string name = context.method_member_name().GetText();
            TypeName returnType = PrimitiveTypeName.Void;
            MethodName methodName = new MethodName(parentClassName, name, returnType.FullyQualified, new List<string> { "1" });
            MethodInfo = new MethodInfo(methodName, AccessFlags.AccPublic, parentClassName, new List<Argument>(), returnType, context.method_body().GetFullText());
        }
    }
}