namespace Minsk.CodeAnalysis.Syntax
{
    public sealed partial class TypeClauseSyntax : SyntaxNode
    {
        internal TypeClauseSyntax(SyntaxTree syntaxTree, SyntaxToken colonToken, SyntaxToken identifier)
            : base(syntaxTree)
        {
            ColonToken = colonToken;
            Identifier = identifier;
        }

        public override SyntaxKind Kind => SyntaxKind.TypeClause;
        public SyntaxToken ColonToken { get; }
        public SyntaxToken Identifier { get; }
    }

    public sealed partial class AdditionalTypeSyntax : SyntaxNode
    {
        internal AdditionalTypeSyntax(SyntaxTree syntaxTree, SeparatedSyntaxList<SyntaxToken> identifiers)
            : base(syntaxTree)
        {
            Identifiers = identifiers;
        }

        public override SyntaxKind Kind => SyntaxKind.TypeClause;
        public SeparatedSyntaxList<SyntaxToken> Identifiers { get; }
    }
}