using System;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp.Class
{
    public class MethodDeclarationListener : CSharpParserBaseListener
    {
        public MethodInfo MethodInfo { get; private set; }

        private readonly ClassName _parentClassName;
        private readonly TypeName _typeName;
        
        public MethodDeclarationListener(ClassName parentClassName, TypeName typeName)
        {
            _parentClassName = parentClassName;
            _typeName = typeName;
        }

        public override void EnterMethod_declaration(CSharpParser.Method_declarationContext context)
        {
            Console.WriteLine();
            Console.WriteLine("method declaration: " + context.GetText());

            var methodNameString = context.method_member_name().GetText();
            
            Console.WriteLine("TODO: implement method arguments");
            Console.WriteLine("TODO: implement method accessflag");
            Console.WriteLine("TODO: check code snippet");
            
            MethodInfo = new MethodInfo(
                new MethodName(
                    _parentClassName,
                    methodNameString,
                    _typeName.ToString(),
                    new Argument[0]
                    ),
                AccessFlags.None,
                _parentClassName,
                new Argument[0],
                _typeName,
                new SourceCodeSnippet(context.GetText(), SourceCodeLanguage.CSharp));
        }
    }
}