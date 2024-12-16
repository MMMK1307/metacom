namespace Minsk.CodeAnalysis.Syntax
{
    public sealed partial class CallExpressionSyntax : ExpressionSyntax
    {
        internal CallExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken identifier, SyntaxToken openParenthesisToken, SeparatedSyntaxList<ExpressionSyntax> arguments, SyntaxToken closeParenthesisToken, SeparatedSyntaxList<SyntaxToken> innerMember)
            : base(syntaxTree)
        {
            Identifier = identifier;
            OpenParenthesisToken = openParenthesisToken;
            Arguments = arguments;
            CloseParenthesisToken = closeParenthesisToken;
            InnerMembers = innerMember;
        }

        public override SyntaxKind Kind => SyntaxKind.CallExpression;
        public SyntaxToken Identifier { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public SeparatedSyntaxList<SyntaxToken>? InnerMembers { get; }
    }

    public sealed partial class NewExpressionSyntax : ExpressionSyntax
    {
        public NewExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken newKeyword, ExpressionSyntax expression) :
            base(syntaxTree)
        {
            NewKeyword = newKeyword;
            Expression = expression;
        }

        public override SyntaxKind Kind => SyntaxKind.NewExpression;
        public SyntaxToken NewKeyword { get; }
        public ExpressionSyntax Expression { get; }
    }

    public sealed partial class ArrayAccessExpression : ExpressionSyntax
    {
        public ArrayAccessExpression(SyntaxTree syntaxTree, SyntaxToken identifier, SyntaxToken openSquaredBracket, ExpressionSyntax expression, SeparatedSyntaxList<SyntaxToken>? innerMembers, SyntaxToken closeSquaredBracket)
        : base(syntaxTree)
        {
            Identifier = identifier;
            OpenSquaredBracket = openSquaredBracket;
            Expression = expression;
            InnerMembers = innerMembers;
            CloseSquaredBracket = closeSquaredBracket;
        }

        public override SyntaxKind Kind => SyntaxKind.ArrayAccessExpression;
        public SyntaxToken Identifier { get; }
        public SyntaxToken OpenSquaredBracket { get; }
        public ExpressionSyntax Expression { get; }
        public SeparatedSyntaxList<SyntaxToken>? InnerMembers { get; }
        public SyntaxToken CloseSquaredBracket { get; }
    }

    public sealed partial class ArrayDeclarationExpression : ExpressionSyntax
    {
        public ArrayDeclarationExpression(SyntaxTree syntaxTree, SyntaxToken expressionType, SyntaxToken openSquaredBracket, ExpressionSyntax expression, SyntaxToken closeSquaredBracket)
        : base(syntaxTree)
        {
            Type = expressionType;
            OpenSquaredBracket = openSquaredBracket;
            Expression = expression;
            CloseSquaredBracket = closeSquaredBracket;
        }

        public override SyntaxKind Kind => SyntaxKind.ArrayDeclarationExpression;
        public SyntaxToken Type { get; }
        public SyntaxToken OpenSquaredBracket { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseSquaredBracket { get; }
    }
}