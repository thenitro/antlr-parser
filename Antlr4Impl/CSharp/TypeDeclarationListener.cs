using System;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp
{
    public class TypeDeclarationListener : CSharpParserBaseListener
    {
        public ClassInfo OuterClassInfo { get; private set; }
        
        public TypeDeclarationListener()
        {
        }

        public override void EnterType_declaration(CSharpParser.Type_declarationContext context)
        {
            if (context.class_definition() != null)
            {
                var classDefinitionListener = new ClassDefinitionListener();
                context.class_definition().EnterRule(classDefinitionListener);
                OuterClassInfo = classDefinitionListener.ClassInfo;
            }
            
            if (context.interface_definition() != null)
            {
                throw new NotImplementedException();
            }
            
            if (context.enum_definition() != null)
            {
                throw new NotImplementedException();
            }
            
            if (context.struct_definition() != null)
            {
                throw new NotImplementedException();
            }
            
            if (context.delegate_definition() != null)
            {
                throw new NotImplementedException();
            }
        }
    }
}