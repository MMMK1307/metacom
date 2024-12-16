using System;
using System.Collections.Generic;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed partial class GlobalStatementSyntax : MemberSyntax
    {
        internal GlobalStatementSyntax(SyntaxTree syntaxTree, StatementSyntax statement)
            : base(syntaxTree)
        {
            Statement = statement;
        }

        public override SyntaxKind Kind => SyntaxKind.GlobalStatement;
        public StatementSyntax Statement { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
    }
}