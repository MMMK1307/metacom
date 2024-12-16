namespace Minsk.CodeAnalysis.Syntax
{
    public sealed partial class NameExpressionSyntax : ExpressionSyntax
    {
        internal NameExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken identifierToken, SeparatedSyntaxList<SyntaxToken> innerMembers)
            : base(syntaxTree)
        {
            IdentifierToken = identifierToken;
            InnerMembers = innerMembers;
        }

        public override SyntaxKind Kind => SyntaxKind.NameExpression;
        public SyntaxToken IdentifierToken { get; }
        public SeparatedSyntaxList<SyntaxToken> InnerMembers { get; }
    }

    public sealed partial class VariableDeclarationExpression : ExpressionSyntax
    {
        internal VariableDeclarationExpression(SyntaxTree syntaxTree, TypeClauseSyntax? typeClause, AdditionalTypeSyntax? aditionalType)
            : base(syntaxTree)
        {
            TypeClause = typeClause;
            AditionalType = aditionalType;
        }

        public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;
        public TypeClauseSyntax? TypeClause { get; }
        public AdditionalTypeSyntax? AditionalType { get; }
    }
}