using System;
using System.Collections.Generic;

namespace Minsk.CodeAnalysis.Syntax
{
    public sealed partial class FunctionDeclarationSyntax : MemberSyntax
    {
        internal FunctionDeclarationSyntax(SyntaxTree syntaxTree, SyntaxToken functionKeyword, SyntaxToken identifier, SyntaxToken openParenthesisToken, SeparatedSyntaxList<ParameterSyntax> parameters, SyntaxToken closeParenthesisToken, TypeClauseSyntax? type, BlockStatementSyntax body, SyntaxToken accessMod)
            : base(syntaxTree)
        {
            AccessModifier = accessMod;
            FunctionKeyword = functionKeyword;
            Identifier = identifier;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            Type = type;
            Body = body;
        }

        public override SyntaxKind Kind => SyntaxKind.FunctionDeclaration;
        public SyntaxToken AccessModifier { get; }
        public SyntaxToken FunctionKeyword { get; }
        public SyntaxToken Identifier { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public TypeClauseSyntax? Type { get; }
        public BlockStatementSyntax Body { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Array.Empty<SyntaxNode>();
        }
    }
}