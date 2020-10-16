using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp.Class
{
    public class NumericTypeListener : CSharpParserBaseListener
    {
        public PrimitiveTypeName TypeName;
        
        public NumericTypeListener()
        {
        }

        public override void EnterNumeric_type(CSharpParser.Numeric_typeContext context)
        {
            if (context.integral_type() != null)
            {
                if (context.integral_type().INT() != null)
                {
                    TypeName = PrimitiveTypeName.Int;
                }
                else if (context.integral_type().BYTE() != null)
                {
                    TypeName = PrimitiveTypeName.Byte;
                }
                else if (context.integral_type().CHAR() != null)
                {
                    TypeName = PrimitiveTypeName.Char;
                }
                else if (context.integral_type().LONG() != null)
                {
                    TypeName = PrimitiveTypeName.Long;
                }
                else if (context.integral_type().UINT() != null)
                {
                    TypeName = PrimitiveTypeName.Int;
                }
                else if (context.integral_type().SBYTE() != null)
                {
                    TypeName = PrimitiveTypeName.Byte;
                }
                else if (context.integral_type().SHORT() != null)
                {
                    TypeName = PrimitiveTypeName.Short;
                }
                else if (context.integral_type().ULONG() != null)
                {
                    TypeName = PrimitiveTypeName.Long;
                }
                else if (context.integral_type().USHORT() != null)
                {
                    TypeName = PrimitiveTypeName.Short;
                }
            } 
            else if (context.floating_point_type() != null)
            {
                if (context.floating_point_type().FLOAT() != null)
                {
                    TypeName = PrimitiveTypeName.Float;
                }
                else if (context.floating_point_type().DOUBLE() != null)
                {
                    TypeName = PrimitiveTypeName.Double;
                }
            }
        }
    }
}