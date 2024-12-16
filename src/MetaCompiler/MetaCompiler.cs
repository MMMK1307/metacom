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
                            TranslateFunctionDeclaration((member as FunctionDeclarationSyntax)!);
                        }
                        break;

                    default:
                        if (member is GlobalStatementSyntax)
                        {
                            TranslateGlobalDeclaration((member as GlobalStatementSyntax)!);
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
            WriteFunctionDeclaration(accessMod: member.AccessModifier, identifier: member.Identifier, type: member.Type);
            WriteFunctionParameters(member.Parameters);
            WriteBlockStatement(member.Body);
            return;
        }

        private void WriteFunctionDeclaration(SyntaxToken accessMod, SyntaxToken identifier, TypeClauseSyntax? type)
        {
            //_translationBuilder.AppendLine($"{1} {0} ({2}) \n {{\treturn\n}}", identifier, type, parameters);
            _translationBuilder.Append($"{MetaTranslator.TranslateSyntaxToken(accessMod.Kind)} ");
            WriteDeclarationType(type);
            _translationBuilder.Append($"{identifier.Text}");
        }

        private void WriteDeclarationType(TypeClauseSyntax? typeClause)
        {
            if (typeClause == null)
                return;

            var type = MetaTranslator.TranslateSyntaxToken(typeClause.Identifier.Kind);
            _translationBuilder.Append($"{type}");
            if (typeClause.IsList)
            {
                _translationBuilder.Append("[] ");
            }
        }

        private void WriteFunctionParameters(SeparatedSyntaxList<ParameterSyntax> parameters)
        {
            _translationBuilder.Append("(");

            for (int i = 0; i < parameters.Count; i++)
            {
                if (i != 0)
                    _translationBuilder.Append(", ");

                WriteDeclarationType(parameters[i].Type);

                _translationBuilder.Append($" {parameters[i].Identifier.Text}");
            }
            _translationBuilder.Append(")");
        }

        private void WriteBlockStatement(BlockStatementSyntax body)
        {
            _translationBuilder.Append("{\n");
            for (int i = 0; i < body.Statements.Length; i++)
            {
                WriteStatement(body.Statements[i]);
                var t = _translationBuilder[_translationBuilder.Length - 1];
                if (t != '}')
                    _translationBuilder.Append(";");
                _translationBuilder.Append("\n");
            }
            _translationBuilder.Append("\n}");
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

                case SyntaxKind.ReturnStatement:
                    WriteReturnStatement((statement as ReturnStatementSyntax)!);
                    return;

                case SyntaxKind.ForStatement:
                    WriteForStatement((statement as ForStatementSyntax)!);
                    return;

                default:
                    return;
            }
        }

        private void WriteForStatement(ForStatementSyntax statement)
        {
            var forKeyword = MetaTranslator.TranslateSyntaxToken(statement.Keyword.Kind);
            _translationBuilder.Append(forKeyword);
            _translationBuilder.Append("(");
            WriteStatement(statement.Declaration);
            _translationBuilder.Append("; ");
            WriteExpression(statement.Condition);
            _translationBuilder.Append("; ");
            WriteExpression(statement.Modifier);
            _translationBuilder.Append(")");
            WriteStatement(statement.Body);
        }

        private void WriteReturnStatement(ReturnStatementSyntax statement)
        {
            var returnKeyword = MetaTranslator.TranslateSyntaxToken(statement.ReturnKeyword.Kind);

            _translationBuilder.Append($"{returnKeyword} ");

            if (statement.Expression is null)
                return;

            WriteExpression(statement.Expression);
            _translationBuilder.Append(";");
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
            WriteDeclarationType(variableDeclaration.TypeClause);
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

            if (expression is SingleExpressionSyntax)
                WriteSingleExpression((expression as SingleExpressionSyntax)!);

            if (expression is AssignmentExpressionSyntax)
                WriteAssignmentExpression((expression as AssignmentExpressionSyntax)!);

            if (expression is ParenthesizedExpressionSyntax)
                WriteParenthesizedExpression((expression as ParenthesizedExpressionSyntax)!);

            if (expression is NewExpressionSyntax)
                WriteNewExpression((expression as NewExpressionSyntax)!);

            if (expression is ArrayDeclarationExpression)
                WriteArrayDeclarationExpression((expression as ArrayDeclarationExpression)!);

            if (expression is ArrayAccessExpression)
                WriteArrayAccessExpression((expression as ArrayAccessExpression)!);
        }

        private void WriteArrayAccessExpression(ArrayAccessExpression expression)
        {
            _translationBuilder.Append(expression.Identifier.Text);
            _translationBuilder.Append("[");
            WriteExpression(expression.Expression);
            _translationBuilder.Append("]");
        }

        private void WriteSingleExpression(SingleExpressionSyntax expression)
        {
            var identifier = MetaTranslator.TranslateSyntax(expression.Identifier);
            _translationBuilder.Append(identifier);
            _translationBuilder.Append(MetaTranslator.TranslateSyntax(expression.Operator));
        }

        private void WriteArrayDeclarationExpression(ArrayDeclarationExpression expression)
        {
            var typeKeyword = MetaTranslator.TranslateSyntaxToken(expression.Type.Kind);
            _translationBuilder.Append($"{typeKeyword}[");
            WriteExpression(expression.Expression);
            _translationBuilder.Append("]");
        }

        private void WriteNewExpression(NewExpressionSyntax expression)
        {
            var newKeyword = MetaTranslator.TranslateSyntaxToken(expression.NewKeyword.Kind);
            _translationBuilder.Append($"{newKeyword} ");
            WriteExpression(expression.Expression);
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
            var identifierToken = MetaTranslator.TranslateSyntax(expression.IdentifierToken);
            _translationBuilder.Append(expression.IdentifierToken.Text);

            if (expression.InnerMembers.Count < 1)
                return;

            WriteCallExpressionMembers(expression.InnerMembers);
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
                if (i != 0)
                    _translationBuilder.Append(", ");
                WriteExpression(parameters[i]);
            }
        }

        private void WriteCallExpressionMembers(SeparatedSyntaxList<SyntaxToken> members)
        {
            for (int i = 0; i < members.Count; i++)
            {
                _translationBuilder.Append(".");
                var member = MetaTranslator.TranslateSyntax(members[i]);
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