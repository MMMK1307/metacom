using System;
using System.Collections.Generic;

namespace Minsk.CodeAnalysis.Syntax
{
    public abstract class ExpressionSyntax : SyntaxNode
    {
        protected ExpressionSyntax(SyntaxTree syntaxTree)
            : base(syntaxTree)
        {
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
    }
}