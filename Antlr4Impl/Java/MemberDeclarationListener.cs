using System.Collections.Generic;
using System.Linq;

namespace antlr_parser.Antlr4Impl.Java
{
    #region TOP LEVEL - Member Listener

    /// <summary>
    /// Listener for Method, Field, and Inner Class declarations 
    /// </summary>
    public class MemberDeclarationListener : JavaParserBaseListener
    {
        readonly ClassName parentClassName;
        readonly string package;
        readonly string packageFqn;

        public MethodInfo MethodInfo;
        public FieldInfo FieldInfo;
        public ClassInfo InnerClassInfo;

        public MemberDeclarationListener(ClassName parentClassName, string package, string packageFqn)
        {
            this.parentClassName = parentClassName;
            this.package = package;
            this.packageFqn = packageFqn;
        }

        public override void EnterMemberDeclaration(JavaParser.MemberDeclarationContext context)
        {
            // this member could be a method, class, or field
            if (context.methodDeclaration() != null)
            {
                MethodDeclarationListener methodDeclarationListener =
                    new MethodDeclarationListener(parentClassName);
                context.methodDeclaration().EnterRule(methodDeclarationListener);
                MethodInfo = methodDeclarationListener.MethodInfo;
            }

            if (context.constructorDeclaration() != null)
            {
                ConstructorDeclarationListener constructorDeclarationListener =
                    new ConstructorDeclarationListener(parentClassName);
                context.constructorDeclaration().EnterRule(constructorDeclarationListener);
                MethodInfo = constructorDeclarationListener.MethodInfo;
            }

            if (context.fieldDeclaration() != null)
            {
                FieldDeclarationListener fieldDeclarationListener =
                    new FieldDeclarationListener(parentClassName);
                context.fieldDeclaration().EnterRule(fieldDeclarationListener);
                FieldInfo = fieldDeclarationListener.FieldInfo;
            }

            if (context.classDeclaration() != null)
            {
                ClassDeclarationListener classDeclarationListener =
                    new ClassDeclarationListener(
                        package,
                        packageFqn,
                        AccessFlags.AccPrivate, // TODO
                        parentClassName);
                context.classDeclaration().EnterRule(classDeclarationListener);
                InnerClassInfo = classDeclarationListener.ClassInfo;
            }
        }
    }

    public class InterfaceMemberDeclarationListener : JavaParserBaseListener
    {
        readonly ClassName parentClassName;
        public MethodInfo MethodInfo;

        public InterfaceMemberDeclarationListener(ClassName parentClassName)
        {
            this.parentClassName = parentClassName;
        }

        public override void EnterInterfaceMemberDeclaration(
            JavaParser.InterfaceMemberDeclarationContext context)
        {
            // this member could be a method, class, or field
            if (context.interfaceMethodDeclaration() != null)
            {
                InterfaceMethodDeclarationListener methodDeclarationListener =
                    new InterfaceMethodDeclarationListener(parentClassName);
                context.interfaceMethodDeclaration().EnterRule(methodDeclarationListener);
                MethodInfo = methodDeclarationListener.MethodInfo;
            }
        }
    }

    #endregion

    #region METHOD Listeners

    public class MethodDeclarationListener : BaseMethodDeclarationListener
    {
        public MethodDeclarationListener(ClassName parentClassName) : base(parentClassName)
        {
        }

        public override void EnterMethodDeclaration(JavaParser.MethodDeclarationContext context)
        {
            ParseMethodFromContext(
                context.formalParameters(),
                context.typeTypeOrVoid(),
                context.qualifiedNameList(),
                context.IDENTIFIER().GetText(),
                context.GetFullText());
        }
    }

    public class ConstructorDeclarationListener : BaseMethodDeclarationListener
    {
        public ConstructorDeclarationListener(ClassName parentClassName) : base(parentClassName)
        {
        }

        public override void EnterConstructorDeclaration(JavaParser.ConstructorDeclarationContext context)
        {
            ParseMethodFromContext(
                context.formalParameters(),
                null,
                context.qualifiedNameList(),
                context.IDENTIFIER().GetText(),
                context.GetFullText());
        }
    }


    public class InterfaceMethodDeclarationListener : BaseMethodDeclarationListener
    {
        public InterfaceMethodDeclarationListener(ClassName parentClassName) : base(parentClassName)
        {
        }

        public override void EnterInterfaceMethodDeclaration(JavaParser.InterfaceMethodDeclarationContext context)
        {
            ParseMethodFromContext(
                context.formalParameters(),
                context.typeTypeOrVoid(),
                context.qualifiedNameList(),
                context.IDENTIFIER().GetText(),
                context.GetFullText());
        }
    }

    public abstract class BaseMethodDeclarationListener : JavaParserBaseListener
    {
        readonly ClassName parentClassName;
        public MethodInfo MethodInfo;

        protected BaseMethodDeclarationListener(ClassName parentClassName)
        {
            this.parentClassName = parentClassName;
        }

        protected void ParseMethodFromContext(
            JavaParser.FormalParametersContext formalParametersContext,
            JavaParser.TypeTypeOrVoidContext typeTypeOrVoidContext,
            JavaParser.QualifiedNameListContext qualifiedNameListContext,
            string methodNameText,
            string methodBody)
        {
            FormalParametersListener formalParametersListener =
                new FormalParametersListener();
            formalParametersContext.EnterRule(formalParametersListener);

            TypeName returnType = PrimitiveTypeName.Void;
            if (typeTypeOrVoidContext != null)
            {
                TypeTypeOrVoidListener typeOrVoidListener = new TypeTypeOrVoidListener();
                typeTypeOrVoidContext.EnterRule(typeOrVoidListener);
                returnType = typeOrVoidListener.TypeName;
            }

            QualifiedNameListListener qualifiedNameListListener = new QualifiedNameListListener();
            if (qualifiedNameListContext != null)
            {
                // Exceptions
                qualifiedNameListContext.EnterRule(qualifiedNameListListener);
                List<string> qualifiedNames = qualifiedNameListListener.QualifiedNames;
            }

            MethodName methodName = new MethodName(
                parentClassName,
                methodNameText,
                returnType.FullyQualified,
                formalParametersListener
                    .Arguments
                    .Select(arg => new Argument(
                        arg.Type.FullyQualified, 
                        TypeName.For(arg.Type.FullyQualified))).ToList());
            MethodInfo = new MethodInfo(
                methodName,
                AccessFlags.AccPublic, // TODO
                parentClassName,
                formalParametersListener.Arguments,
                returnType,
                new SourceCodeSnippet(methodBody, SourceCodeLanguage.Java));
        }
    }

    public class QualifiedNameListListener : JavaParserBaseListener
    {
        public readonly List<string> QualifiedNames = new List<string>();

        public override void EnterQualifiedNameList(JavaParser.QualifiedNameListContext context)
        {
            QualifiedNameListener qualifiedNameListener = new QualifiedNameListener();
            foreach (JavaParser.QualifiedNameContext qualifiedNameContext in context.qualifiedName())
            {
                qualifiedNameContext.EnterRule(qualifiedNameListener);
                QualifiedNames.Add(qualifiedNameListener.QualifiedName);
            }
        }
    }

    public class QualifiedNameListener : JavaParserBaseListener
    {
        public string QualifiedName;

        public override void EnterQualifiedName(JavaParser.QualifiedNameContext context)
        {
            QualifiedName = context.IDENTIFIER().ToString();
        }
    }

    public class TypeTypeOrVoidListener : JavaParserBaseListener
    {
        public TypeName TypeName;

        public override void EnterTypeTypeOrVoid(JavaParser.TypeTypeOrVoidContext context)
        {
            if (context.typeType() == null)
            {
                TypeName = PrimitiveTypeName.Void;
                return;
            }

            TypeTypeListener typeTypeListener = new TypeTypeListener();
            context.typeType().EnterRule(typeTypeListener);

            if (typeTypeListener.PrimitiveTypeName != null)
            {
                TypeName = typeTypeListener.PrimitiveTypeName;
            }
            else if (!string.IsNullOrEmpty(typeTypeListener.ID))
            {
                TypeName = TypeName.For(typeTypeListener.ID);
            }
            else
            {
                TypeName = PrimitiveTypeName.Void;
            }
        }
    }

    public class FormalParametersListener : JavaParserBaseListener
    {
        public List<Argument> Arguments = new List<Argument>();

        public override void EnterFormalParameters(JavaParser.FormalParametersContext context)
        {
            if (context.formalParameterList() != null)
            {
                FormalParameterListListener formalParameterListListener = new FormalParameterListListener();
                context.formalParameterList().EnterRule(formalParameterListListener);
                Arguments = formalParameterListListener.Arguments;
            }
        }
    }

    public class FormalParameterListListener : JavaParserBaseListener
    {
        public readonly List<Argument> Arguments = new List<Argument>();

        public override void EnterFormalParameterList(JavaParser.FormalParameterListContext context)
        {
            FormalParameterListener formalParameterListener = new FormalParameterListener();
            foreach (JavaParser.FormalParameterContext formalParameterContext in context.formalParameter())
            {
                formalParameterContext.EnterRule(formalParameterListener);
                Arguments.Add(formalParameterListener.Argument);
            }
        }
    }

    public class FormalParameterListener : JavaParserBaseListener
    {
        public Argument Argument;

        public override void EnterFormalParameter(JavaParser.FormalParameterContext context)
        {
            // type of parameter
            TypeTypeListener typeTypeListener = new TypeTypeListener();
            context.typeType().EnterRule(typeTypeListener);

            TypeName typeName = null;
            if (typeTypeListener.PrimitiveTypeName != null)
            {
                typeName = typeTypeListener.PrimitiveTypeName;
            }
            else if (!string.IsNullOrEmpty(typeTypeListener.ID))
            {
                typeName = TypeName.For(typeTypeListener.ID);
            }
            else
            {
                typeName = PrimitiveTypeName.Void;
            }

            // name of parameter
            VariableDeclaratorIdListener variableDeclaratorIdListener = new VariableDeclaratorIdListener();
            context.variableDeclaratorId().EnterRule(variableDeclaratorIdListener);

            Argument = new Argument(variableDeclaratorIdListener.ID, typeName);
        }
    }

    #endregion

    #region FIELD Listeners

    public class FieldDeclarationListener : JavaParserBaseListener
    {
        readonly ClassName parentClassName;

        public FieldDeclarationListener(ClassName parentClassName)
        {
            this.parentClassName = parentClassName;
        }

        public FieldInfo FieldInfo;

        public override void EnterFieldDeclaration(JavaParser.FieldDeclarationContext context)
        {
            PrimitiveTypeName primitiveTypeName = null;
            string qualifiedName = "";
            if (context.typeType() != null)
            {
                TypeTypeListener typeTypeListener = new TypeTypeListener();
                context.typeType().EnterRule(typeTypeListener);
                primitiveTypeName = typeTypeListener.PrimitiveTypeName;
                qualifiedName = typeTypeListener.QualifiedName;
            }

            List<string> IDs = new List<string>();
            if (context.variableDeclarators() != null)
            {
                VariableDeclaratorsListener variableDeclaratorsListener = new VariableDeclaratorsListener();
                context.variableDeclarators().EnterRule(variableDeclaratorsListener);
                IDs = variableDeclaratorsListener.IDs;
            }

            if (primitiveTypeName == null && string.IsNullOrEmpty(qualifiedName))
            {
                primitiveTypeName = PrimitiveTypeName.Void;
            }

            string fieldName = IDs.FirstOrDefault();
            FieldName fieldFqn = FieldName.FieldFqnFromNames(
                fieldName,
                parentClassName.FullyQualified,
                primitiveTypeName != null
                    ? primitiveTypeName.FullyQualified
                    : TypeName.For(qualifiedName).FullyQualified);
            FieldInfo = new FieldInfo(
                fieldFqn,
                parentClassName,
                AccessFlags.AccPublic,
                new SourceCodeSnippet(context.GetFullText(), SourceCodeLanguage.Java));
        }
    }

    public class VariableDeclaratorsListener : JavaParserBaseListener
    {
        public readonly List<string> IDs = new List<string>();

        public override void EnterVariableDeclarators(JavaParser.VariableDeclaratorsContext context)
        {
            VariableDeclaratorListener variableDeclaratorListener = new VariableDeclaratorListener();
            foreach (JavaParser.VariableDeclaratorContext variableDeclaratorContext in context.variableDeclarator())
            {
                variableDeclaratorContext.EnterRule(variableDeclaratorListener);
                IDs.Add(variableDeclaratorListener.ID);
                variableDeclaratorListener.ID = null;
            }
        }
    }

    public class VariableDeclaratorListener : JavaParserBaseListener
    {
        public string ID;

        public override void EnterVariableDeclarator(JavaParser.VariableDeclaratorContext context)
        {
            VariableDeclaratorIdListener variableDeclaratorIdListener = new VariableDeclaratorIdListener();
            context.variableDeclaratorId().EnterRule(variableDeclaratorIdListener);
            ID = variableDeclaratorIdListener.ID;
        }
    }

    public class VariableDeclaratorIdListener : JavaParserBaseListener
    {
        public string ID;

        public override void EnterVariableDeclaratorId(JavaParser.VariableDeclaratorIdContext context)
        {
            ID = context.IDENTIFIER().GetText();
        }
    }

    #endregion
}