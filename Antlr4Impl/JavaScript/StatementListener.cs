using System;

namespace antlr_parser.Antlr4Impl.JavaScript
{
    public class StatementListener : JavaScriptParserBaseListener
    {
        public override void EnterStatement(JavaScriptParser.StatementContext context)
        {
            DoStatement(context);
        }

        public static void DoStatement(JavaScriptParser.StatementContext context)
        {
            if (context.classDeclaration() != null)
            {
                ClassDeclarationListener classDeclarationListener = new ClassDeclarationListener();
                context.classDeclaration().EnterRule(classDeclarationListener);
            }

            if (context.functionDeclaration() != null)
            {
                FunctionDeclarationListener functionDeclarationListener = new FunctionDeclarationListener();
                context.functionDeclaration().EnterRule(functionDeclarationListener);
            }

            if (context.expressionStatement() != null)
            {
                ExpressionStatementListener expressionStatementListener = new ExpressionStatementListener();
                context.expressionStatement().EnterRule(expressionStatementListener);
            }

            if (context.block() != null)
            {
                BlockListener blockListener = new BlockListener();
                context.block().EnterRule(blockListener);
            }

            if (context.variableStatement() != null)
            {
                VariableStatementListener variableStatementListener = new VariableStatementListener();
                context.variableStatement().EnterRule(variableStatementListener);
            }
        }
    }

    public class ClassDeclarationListener : JavaScriptParserBaseListener
    {
        public override void EnterClassDeclaration(JavaScriptParser.ClassDeclarationContext context)
        {
            Console.WriteLine($"Enter Class : {context.identifier().Identifier().GetText()}");
            
            
        }
    }

    public class FunctionDeclarationListener : JavaScriptParserBaseListener
    {
        public override void EnterFunctionDeclaration(JavaScriptParser.FunctionDeclarationContext context)
        {
            Console.WriteLine($"Enter Function : {context.identifier().Identifier().GetText()}");
            
            FunctionBodyListener functionBodyListener = new FunctionBodyListener();
            context.functionBody().EnterRule(functionBodyListener);
        }
    }

    public class FunctionBodyListener : JavaScriptParserBaseListener
    {
        public override void EnterFunctionBody(JavaScriptParser.FunctionBodyContext context)
        {
            SourceElementsListener sourceElementsListener = new SourceElementsListener();
            context.sourceElements().EnterRule(sourceElementsListener);
        }
    }

    public class ExpressionStatementListener : JavaScriptParserBaseListener
    {
        public override void EnterExpressionStatement(JavaScriptParser.ExpressionStatementContext context)
        {
            ExpressionSequenceListener expressionSequenceListener = new ExpressionSequenceListener();
            context.expressionSequence().EnterRule(expressionSequenceListener);
            
            // end of statement
            EosListener eosListener = new EosListener();
            context.eos().EnterRule(eosListener);
        }
    }

    public class ExpressionSequenceListener : JavaScriptParserBaseListener
    {
        public override void EnterExpressionSequence(JavaScriptParser.ExpressionSequenceContext context)
        {
            foreach (JavaScriptParser.SingleExpressionContext singleExpressionContext in context.singleExpression())
            {
                Console.WriteLine($"Expression: {singleExpressionContext.GetFullText()}");                
            }
        }
    }

    public class EosListener : JavaScriptParserBaseListener
    {
        public override void EnterEos(JavaScriptParser.EosContext context)
        {
            
        }
    }

    public class BlockListener : JavaScriptParserBaseListener
    {
        public override void EnterBlock(JavaScriptParser.BlockContext context)
        {
            StatementListListener statementListListener = new StatementListListener();
            context.statementList().EnterRule(statementListListener);
        }
    }

    public class StatementListListener : JavaScriptParserBaseListener
    {
        public override void EnterStatementList(JavaScriptParser.StatementListContext context)
        {
            foreach (JavaScriptParser.StatementContext statementContext in context.statement())
            {
                StatementListener.DoStatement(statementContext);
            }
        }
    }
}