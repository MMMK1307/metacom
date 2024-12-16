using System;
using System.Collections.Generic;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed partial class ElseClauseSyntax : SyntaxNode
    {
        internal ElseClauseSyntax(SyntaxTree syntaxTree, SyntaxToken elseKeyword, StatementSyntax elseStatement)
            : base(syntaxTree)
        {
            ElseKeyword = elseKeyword;
            ElseStatement = elseStatement;
        }

        public override SyntaxKind Kind => SyntaxKind.ElseClause;
        public SyntaxToken ElseKeyword { get; }
        public StatementSyntax ElseStatement { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
    }
}