using System;
using System.Collections.Generic;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed partial class TypeClauseSyntax : SyntaxNode
    {
        internal TypeClauseSyntax(SyntaxTree syntaxTree, SyntaxToken colonToken, SyntaxToken identifier, bool isList)
            : base(syntaxTree)
        {
            ColonToken = colonToken;
            Identifier = identifier;
            IsList = isList;
        }

        public override SyntaxKind Kind => SyntaxKind.TypeClause;
        public SyntaxToken ColonToken { get; }
        public bool IsList { get; }
        public SyntaxToken Identifier { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
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

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
    }
}