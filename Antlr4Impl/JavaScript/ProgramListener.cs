using System.Collections.Generic;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser.Antlr4Impl.JavaScript
{
    public class ProgramListener : JavaScriptParserBaseListener
    {
        public readonly List<ClassInfo> OuterClassInfos = new List<ClassInfo>(); 
        readonly string filePath;
        public ProgramListener(string filePath)
        {
            this.filePath = filePath;
        }
        
        public override void EnterProgram(JavaScriptParser.ProgramContext context)
        {
            SourceElementsListener sourceElementsListener = new SourceElementsListener();
            context.sourceElements().EnterRule(sourceElementsListener);
        }
    }

    public class SourceElementsListener : JavaScriptParserBaseListener
    {
        public override void EnterSourceElements(JavaScriptParser.SourceElementsContext context)
        {
            foreach (JavaScriptParser.SourceElementContext sourceElementContext in context.sourceElement())
            {
                SourceElementListener sourceElementListener = new SourceElementListener();
                sourceElementContext.EnterRule(sourceElementListener);
            }
        }
    }

    public class SourceElementListener : JavaScriptParserBaseListener
    {
        public override void EnterSourceElement(JavaScriptParser.SourceElementContext context)
        {
            StatementListener statementListener = new StatementListener();
            context.statement().EnterRule(statementListener);
        }
    }
}