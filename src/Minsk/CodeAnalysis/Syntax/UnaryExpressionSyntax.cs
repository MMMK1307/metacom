namespace Minsk.CodeAnalysis.Syntax
{
    public sealed partial class UnaryExpressionSyntax : ExpressionSyntax
    {
        internal UnaryExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken operatorToken, ExpressionSyntax operand)
            : base(syntaxTree)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Operand { get; }
    }

    public sealed partial class SingleExpressionSyntax : ExpressionSyntax
    {
        public SingleExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken identifier, SyntaxToken operatorToken)
            : base(syntaxTree)
        {
            Identifier = identifier;
            Operator = operatorToken;
        }

        public override SyntaxKind Kind => SyntaxKind.SingleExpression;
        public SyntaxToken Identifier { get; }
        public SyntaxToken Operator { get; }
    }
}