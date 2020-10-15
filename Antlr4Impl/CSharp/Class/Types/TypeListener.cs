using System;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.CSharp.Class
{
    public class TypeListener : CSharpParserBaseListener
    {
        public string QualifiedName { get; private set; }
        public string Id { get; private set; }
        public PrimitiveTypeName Type { get; private set; }
        
        public TypeListener()
        {
            Type = PrimitiveTypeName.Void;
        }

        public override void EnterType_(CSharpParser.Type_Context context)
        {
            Console.WriteLine("");
            Console.WriteLine("enter type " + context.GetText());
            
            if (context.base_type().VOID() != null)
            {
                return;
            }
            
            if (context.base_type().simple_type() != null)
            {
                Console.WriteLine("");
                Console.WriteLine("simple type " + context.base_type().simple_type().GetText());                
            }
            
            if (context.base_type().class_type() != null)
            {
                Console.WriteLine("");
                Console.WriteLine("class type " + context.base_type().class_type().GetText());                
            }
            
            if (context.base_type().tuple_type() != null)
            {
                Console.WriteLine("");
                Console.WriteLine("tuple_type " + context.base_type().tuple_type().GetText());                
            }
        }
    }
}