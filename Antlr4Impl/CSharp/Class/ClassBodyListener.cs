using System;
using System.Collections.Generic;
using System.Linq;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp.Class
{
    public class ClassBodyListener : CSharpParserBaseListener
    {
        public List<MethodInfo> MethodInfos { get; private set; }
        public List<FieldInfo> FieldInfos { get; private set; }
        
        private readonly ClassName _className;
        private readonly string _parentFileName;
        
        public ClassBodyListener(ClassName className, string parentFileName)
        {
            _className = className;
            _parentFileName = parentFileName;
            
            MethodInfos = new List<MethodInfo>();
            FieldInfos = new List<FieldInfo>();
        }

        public override void EnterClass_body(CSharpParser.Class_bodyContext context)
        {
            Console.WriteLine();
            Console.WriteLine(context.GetText());

            foreach (var classMemberDeclaration in context.class_member_declarations().class_member_declaration())
            {
                var typedMemberDeclarationListener = new TypedMemberDeclarationListener(_className);
                classMemberDeclaration.common_member_declaration().typed_member_declaration().EnterRule(typedMemberDeclarationListener);
                
                if (typedMemberDeclarationListener.MethodInfo != null)
                {
                    MethodInfos.Add(typedMemberDeclarationListener.MethodInfo);
                }
                
                if (typedMemberDeclarationListener.FieldInfo != null)
                {
                    FieldInfos.Add(typedMemberDeclarationListener.FieldInfo);
                }
                
                /*Console.WriteLine();
                Console.WriteLine(classMemberDeclaration.attributes().GetText());
                Console.WriteLine();
                Console.WriteLine(classMemberDeclaration.all_member_modifiers().GetText());
                Console.WriteLine(classMemberDeclaration.common_member_declaration().GetText());*/
            }
        }
    }
}