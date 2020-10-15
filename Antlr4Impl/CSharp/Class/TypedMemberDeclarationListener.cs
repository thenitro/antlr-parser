using System;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp.Class
{
    public class TypedMemberDeclarationListener : CSharpParserBaseListener
    {
        public MethodInfo MethodInfo { get; private set; }
        public FieldInfo FieldInfo { get; private set; }

        private readonly ClassName _parentClassName;

        public TypedMemberDeclarationListener(ClassName parentClassName)
        {
            _parentClassName = parentClassName;
        }

        public override void EnterTyped_member_declaration(CSharpParser.Typed_member_declarationContext context)
        {
            var type = PrimitiveTypeName.Void;
            
            if (context.type_() != null)
            {
                var typeListener = new TypeListener();
                context.type_().EnterRule(typeListener);

                type = typeListener.Type;
            }
            
            if (context.field_declaration() != null)
            {
                var fieldDeclarationListener = new FieldDeclarationListener(_parentClassName, type);
                context.field_declaration().EnterRule(fieldDeclarationListener);

                FieldInfo = fieldDeclarationListener.FieldInfo;
            }
                
            if (context.method_declaration() != null)
            {
                Console.WriteLine();
                Console.WriteLine("method declaration: " + context.method_declaration().GetText());
                Console.WriteLine();
                Console.WriteLine("method declaration Name: " + context.method_declaration().method_member_name().GetText());
            } 
        }
    }
}