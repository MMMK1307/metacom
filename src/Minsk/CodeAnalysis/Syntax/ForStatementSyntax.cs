namespace Minsk.CodeAnalysis.Syntax
{
    public sealed partial class ForStatementSyntax : StatementSyntax
    {
        public ForStatementSyntax(SyntaxTree syntaxTree, SyntaxToken keyword, StatementSyntax declaration, ExpressionSyntax condition, ExpressionSyntax? modifier, StatementSyntax body)
            : base(syntaxTree)
        {
            Keyword = keyword;
            Declaration = declaration;
            Condition = condition;
            Modifier = modifier;
            Body = body;
        }

        public SyntaxToken Keyword { get; }
        public StatementSyntax Declaration { get; }
        public ExpressionSyntax Condition { get; }
        public ExpressionSyntax? Modifier { get; }
        public override SyntaxKind Kind => SyntaxKind.ForStatement;
        public StatementSyntax Body { get; }
    }
}