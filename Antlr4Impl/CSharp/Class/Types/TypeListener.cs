using System;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp.Class
{
    public class TypeListener : CSharpParserBaseListener
    {
        public string ClassName { get; private set; }
        public PrimitiveTypeName Type { get; private set; }
        
        public TypeListener()
        {
        }

        public override void EnterType_(CSharpParser.Type_Context context)
        {
            if (context.base_type().VOID() != null)
            {
                Type = PrimitiveTypeName.Void;
                return;
            }
            
            if (context.base_type().simple_type() != null)
            {
                var simpleTypeListener = new SimpleTypeListener();
                context.base_type().simple_type().EnterRule(simpleTypeListener);

                Type = simpleTypeListener.TypeName;                
            }
            
            if (context.base_type().class_type() != null)
            {
                ClassName = context.base_type().class_type().GetText();
            }
            
            if (context.base_type().tuple_type() != null)
            {
                Console.WriteLine("");
                Console.WriteLine("tuple_type " + context.base_type().tuple_type().GetText());                
            }
        }
    }
}