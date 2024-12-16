using System;
using System.Collections.Generic;

namespace Minsk.CodeAnalysis.Syntax
{
    public abstract class StatementSyntax : SyntaxNode
    {
        protected StatementSyntax(SyntaxTree syntaxTree)
            : base(syntaxTree)
        {
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
    }
}