using System;
using System.Collections.Immutable;
using System.Text;
using Metacom;
using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Syntax;

namespace MetaCompiler
{
    public class Translator
    {
        public Translator()
        {
            _translationBuilder = new StringBuilder();
        }

        private StringBuilder _translationBuilder { get; set; }
        private string TranslatedText => _translationBuilder.ToString();

        public static Compilation Translate(Compilation compilation)
        {
            var translator = new Translator();
            var trees = compilation.SyntaxTrees;

            for (int i = 0; i < trees.Length; i++)
            {
                if (trees[i].Root.Kind == SyntaxKind.CompilationUnit)
                {
                    translator.TranslateCompilationMembers(trees[i].Root.Members);
                }
            }
            Console.Write(translator.TranslatedText);
            return compilation;
        }

        private void TranslateCompilationMembers(ImmutableArray<MemberSyntax> members)
        {
            for (int i = 0; i < members.Length; i++)
            {
                var member = members[i];
                switch (member.Kind)
                {
                    case SyntaxKind.FunctionDeclaration:

                        if (member is FunctionDeclarationSyntax)
                        {
                            TranslateFunctionDeclaration((member as FunctionDeclarationSyntax));
                        }
                        break;

                    default:
                        if (member is GlobalStatementSyntax)
                        {
                            TranslateGlobalDeclaration(member as GlobalStatementSyntax);
                            _translationBuilder.Append(";");
                        }
                        break;
                }
                _translationBuilder.AppendLine("\n");
            }
            return;
        }

        private void TranslateFunctionDeclaration(FunctionDeclarationSyntax member)
        {
            WriteFunctionDeclaration(accessMod: member.AccessModifier, identifier: member.Identifier, type: member.Type?.Identifier);
            WriteFunctionParameters(member.Parameters);
            WriteBlockStatement(member.Body);
            return;
        }

        private void WriteFunctionDeclaration(SyntaxToken accessMod, SyntaxToken identifier, SyntaxToken? type)
        {
            //_translationBuilder.AppendLine($"{1} {0} ({2}) \n {{\treturn\n}}", identifier, type, parameters);
            var stType = MetaTranslator.TranslateSyntaxToken(type.Kind);
            var access = MetaTranslator.TranslateSyntaxToken(accessMod.Kind);
            _translationBuilder.Append($"{access} {stType} {identifier.Text}");
        }

        private void WriteFunctionParameters(SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            _translationBuilder.Append("(");

            for (int i = 0; i < parameters.Count; i++)
            {
                if (i != 0)
                    _translationBuilder.Append(", ");

                _translationBuilder.Append($"{MetaTranslator.TranslateSyntaxToken(parameters[i].Type.Identifier.Kind)} {parameters[i].Identifier.Text}");
            }
            _translationBuilder.Append(")");
        }

        private void WriteBlockStatement(BlockStatementSyntax body)
        {
            _translationBuilder.AppendLine("{");
            for (int i = 0; i < body.Statements.Length; i++)
            {
                WriteStatement(body.Statements[i]);
            }
            _translationBuilder.AppendLine("\n}");
        }

        private void WriteStatement(StatementSyntax statement)
        {
            switch (statement.Kind)
            {
                case SyntaxKind.VariableDeclaration:
                    WriteVariableDeclaration(statement as VariableDeclarationSyntax);
                    return;

                case SyntaxKind.ExpressionStatement:
                    WriteExpressionStatement(statement as ExpressionStatementSyntax);
                    return;

                case SyntaxKind.WhileStatement:
                    WriteWhileStatement(statement as WhileStatementSyntax);
                    return;

                case SyntaxKind.IfStatement:
                    WriteIfStatement(statement as IfStatementSyntax);
                    return;

                case SyntaxKind.BlockStatement:
                    WriteBlockStatement(statement as BlockStatementSyntax);
                    return;

                default:
                    return;
            }
        }

        private void WriteIfStatement(IfStatementSyntax ifStatement)
        {
            _translationBuilder.Append(MetaTranslator.TranslateSyntaxToken(ifStatement.IfKeyword.Kind));
            WriteExpression(ifStatement.Condition);
            WriteStatement(ifStatement.ThenStatement);
        }

        private void WriteWhileStatement(WhileStatementSyntax whileStatement)
        {
            _translationBuilder.Append(MetaTranslator.TranslateSyntaxToken(whileStatement.Kind));
            _translationBuilder.Append("(");
            WriteExpression(whileStatement.Condition);
            _translationBuilder.Append(")");
            WriteStatement(whileStatement.Body);
        }

        private void WriteExpressionStatement(ExpressionStatementSyntax expression)
        {
            WriteExpression(expression.Expression);
        }

        private void WriteAdditionalType(AdditionalTypeSyntax? additionalType)
        {
            if (additionalType is null)
            {
                return;
            }

            _translationBuilder.Append('<');
            WriteAdditionalTypeIdentifiers(additionalType.Identifiers);
            _translationBuilder.Append('>');
        }

        private void WriteAdditionalTypeIdentifiers(SeparatedSyntaxList<SyntaxToken> identifiers)
        {
            for (int i = 0; i < identifiers.Count; i++)
            {
                if (i != 0)
                    _translationBuilder.Append(", ");

                var type = MetaTranslator.TranslateSyntaxToken(identifiers[i].Kind);
                _translationBuilder.Append(type);
            }
        }

        private void WriteVariableDeclaration(VariableDeclarationSyntax variableDeclaration)
        {
            var type = MetaTranslator.TranslateSyntaxToken(variableDeclaration.TypeClause?.Identifier.Kind);

            _translationBuilder.Append($"{type}");
            WriteAdditionalType(additionalType: variableDeclaration.AditionalType);
            _translationBuilder.Append($" {variableDeclaration.Identifier.Text} = ");
            WriteExpression(variableDeclaration.Initializer);
        }

        private void WriteExpression(ExpressionSyntax expression)
        {
            if (expression is BinaryExpressionSyntax)
                WriteBinaryExpression((expression as BinaryExpressionSyntax)!);

            if (expression is LiteralExpressionSyntax)
                WriteLiteralExpression((expression as LiteralExpressionSyntax)!);

            if (expression is CallExpressionSyntax)
                WriteCallExpression((expression as CallExpressionSyntax)!);

            if (expression is NameExpressionSyntax)
                WriteNameExpression((expression as NameExpressionSyntax)!);

            if (expression is UnaryExpressionSyntax)
                WriteUnaryExpression((expression as UnaryExpressionSyntax)!);

            if (expression is AssignmentExpressionSyntax)
                WriteAssignmentExpression((expression as AssignmentExpressionSyntax)!);

            if (expression is ParenthesizedExpressionSyntax)
                WriteParenthesizedExpression((expression as ParenthesizedExpressionSyntax)!);
        }

        private void WriteUnaryExpression(UnaryExpressionSyntax expression)
        {
            var operatorToken = MetaTranslator.TranslateSyntaxToken(expression.OperatorToken.Kind);
            if (expression.OperatorToken.Kind == SyntaxKind.MinusToken)
            {
                _translationBuilder.Append(operatorToken);
                WriteExpression(expression.Operand);
                return;
            }
            WriteExpression(expression.Operand);
            _translationBuilder.Append(operatorToken);
        }

        private void WriteAssignmentExpression(AssignmentExpressionSyntax expression)
        {
            var assignmentToken = MetaTranslator.TranslateSyntaxToken(expression.AssignmentToken.Kind);
            _translationBuilder.Append(expression.IdentifierToken.Text);
            _translationBuilder.Append($" {assignmentToken} ");
            WriteExpression(expression.Expression);
        }

        private void WriteParenthesizedExpression(ParenthesizedExpressionSyntax expression)
        {
            _translationBuilder.Append("(");
            WriteExpression(expression.Expression);
            _translationBuilder.Append(")");
        }

        private void WriteNameExpression(NameExpressionSyntax expression)
        {
            _translationBuilder.Append(expression.IdentifierToken.Text);
        }

        private void WriteBinaryExpression(BinaryExpressionSyntax expression)
        {
            WriteExpression(expression.Left);

            var expressionOperator = MetaTranslator.TranslateSyntaxToken(expression.OperatorToken.Kind);

            _translationBuilder.Append($" {expressionOperator} ");

            WriteExpression(expression.Right);
        }

        private void WriteLiteralExpression(LiteralExpressionSyntax expression)
        {
            _translationBuilder.Append(expression.LiteralToken.Text);
        }

        private void WriteCallExpressionParameters(SeparatedSyntaxList<ExpressionSyntax> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                WriteExpression(parameters[i]);
            }
        }

        private void WriteCallExpressionMembers(SeparatedSyntaxList<SyntaxToken> members)
        {
            for (int i = 0; i < members.Count; i++)
            {
                _translationBuilder.Append(".");
                var member = MetaTranslator.TranslateSyntaxToken(members[i].Kind);
                _translationBuilder.Append(member);
            }
        }

        private void WriteCallExpression(CallExpressionSyntax expression)
        {
            _translationBuilder.Append(expression.Identifier.Text);

            if (expression.InnerMembers != null)
            {
                WriteCallExpressionMembers(expression.InnerMembers);
            }

            _translationBuilder.Append("(");
            WriteCallExpressionParameters(expression.Arguments);
            _translationBuilder.Append(")");
        }

        private void TranslateGlobalDeclaration(GlobalStatementSyntax member)
        {
            WriteStatement(member.Statement);
            return;
        }
    }
}