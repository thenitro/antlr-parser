using System;

namespace antlr_parser.Antlr4Impl.JavaScript
{
    public class VariableStatementListener : JavaScriptParserBaseListener
    {
        public override void EnterVariableStatement(JavaScriptParser.VariableStatementContext context)
        {
            VariableDeclarationListListener variableDeclarationListListener = new VariableDeclarationListListener();
            context.variableDeclarationList().EnterRule(variableDeclarationListListener);
        }
    }

    public class VariableDeclarationListListener : JavaScriptParserBaseListener
    {
        public override void EnterVariableDeclarationList(JavaScriptParser.VariableDeclarationListContext context)
        {
            VarModifierListener varModifierListener = new VarModifierListener();
            context.varModifier().EnterRule(varModifierListener);
            
            foreach (JavaScriptParser.VariableDeclarationContext variableDeclarationContext in context.variableDeclaration())
            {
                VariableDeclarationListener variableDeclarationListener = new VariableDeclarationListener();
                variableDeclarationContext.EnterRule(variableDeclarationListener);
            }
        }
    }

    public class VarModifierListener : JavaScriptParserBaseListener
    {
        public override void EnterVarModifier(JavaScriptParser.VarModifierContext context)
        {
            
        }
    }

    public class VariableDeclarationListener : JavaScriptParserBaseListener
    {
        public override void EnterVariableDeclaration(JavaScriptParser.VariableDeclarationContext context)
        {
            Console.WriteLine($"Single Expression: {context.singleExpression().GetFullText()}");
        }
    }
}