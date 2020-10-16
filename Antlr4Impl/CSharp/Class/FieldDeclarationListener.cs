using System;
using System.Linq;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp.Class
{
    public class FieldDeclarationListener : CSharpParserBaseListener
    {
        public FieldInfo FieldInfo { get; private set; }

        private readonly ClassName _parentClassName;
        private readonly string _typeName;
        
        public FieldDeclarationListener(ClassName parentClassName, string typeName)
        {
            _parentClassName = parentClassName;
            _typeName = typeName;
        }

        public override void EnterField_declaration(CSharpParser.Field_declarationContext context)
        {
            var fieldNameStr = context.variable_declarators().variable_declarator().First().GetText();
            var fieldName = FieldName.FieldFqnFromNames(
                fieldNameStr,
                _parentClassName.FullyQualified,
                _typeName);

            Console.WriteLine($"Parse Field: {fieldNameStr} of {_typeName}");
            
            FieldInfo = new FieldInfo(
                fieldName,
                _parentClassName,
                AccessFlags.None,
                new SourceCodeSnippet(context.GetFullText(), SourceCodeLanguage.CSharp));
        }
    }
}