using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp.Class
{
    public class SimpleTypeListener : CSharpParserBaseListener
    {
        public PrimitiveTypeName TypeName { get; private set; }
        
        public override void EnterSimple_type(CSharpParser.Simple_typeContext context)
        {
            if (context.BOOL() != null)
            {
                TypeName = PrimitiveTypeName.Boolean;
            } 
            else if (context.numeric_type() != null)
            {
                var numericTypeListener = new NumericTypeListener();
                
                context.numeric_type().EnterRule(numericTypeListener);

                TypeName = numericTypeListener.TypeName;
            }
        }
    }
}