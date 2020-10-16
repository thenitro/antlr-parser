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
            var className = string.Empty;
            
            if (context.type_() != null)
            {
                var typeListener = new TypeListener();
                context.type_().EnterRule(typeListener);

                type = typeListener.Type;
                className = typeListener.ClassName;
            }
            
            var typeName = className != null
                ? TypeName.For(className)
                : type; 
            
            if (context.field_declaration() != null)
            {
                var fieldDeclarationListener = new FieldDeclarationListener(_parentClassName, typeName.FullyQualified);
                context.field_declaration().EnterRule(fieldDeclarationListener);

                FieldInfo = fieldDeclarationListener.FieldInfo;
            }
                
            if (context.method_declaration() != null)
            {
                var methodDeclarationListener = new MethodDeclarationListener(_parentClassName, typeName);
                context.method_declaration().EnterRule(methodDeclarationListener);
                MethodInfo = methodDeclarationListener.MethodInfo;
            } 
        }
    }
}