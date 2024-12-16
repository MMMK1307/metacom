using Minsk.CodeAnalysis.Syntax;

namespace Metacom
{
    public class MetaTranslator
    {
        public static string TranslateSyntax(SyntaxToken token)
        {
            var translatedText = TranslateSyntaxToken(token.Kind);

            if (translatedText == "")
                return TranslateIdentifierToken(token);

            return translatedText;
        }

        public static string TranslateIdentifierToken(SyntaxToken token)
        {
            return token.Text switch
            {
                "Math" => "Math",
                "Max" => "max",
                "Length" => "length",
                _ => token.Text
            };
        }

        public static string TranslateSyntaxToken(SyntaxKind? kind)
        {
            return kind switch
            {
                SyntaxKind.PlusToken => "+",
                SyntaxKind.PlusPlusToken => "++",
                SyntaxKind.PlusEqualsToken => "+=",
                SyntaxKind.MinusToken => "-",
                SyntaxKind.MinusMinusToken => "--",
                SyntaxKind.MinusEqualsToken => "-=",
                SyntaxKind.StarToken => "*",
                SyntaxKind.StarEqualsToken => "*=",
                SyntaxKind.SlashToken => "/",
                SyntaxKind.SlashEqualsToken => "/=",
                SyntaxKind.BangToken => "!",
                SyntaxKind.EqualsToken => "=",
                SyntaxKind.TildeToken => "~",
                SyntaxKind.LessToken => "<",
                SyntaxKind.LessOrEqualsToken => "<=",
                SyntaxKind.GreaterToken => ">",
                SyntaxKind.GreaterOrEqualsToken => ">=",
                SyntaxKind.AmpersandToken => "&",
                SyntaxKind.AmpersandAmpersandToken => "&&",
                SyntaxKind.AmpersandEqualsToken => "&=",
                SyntaxKind.PipeToken => "|",
                SyntaxKind.PipeEqualsToken => "|=",
                SyntaxKind.PipePipeToken => "||",
                SyntaxKind.HatToken => "^",
                SyntaxKind.HatEqualsToken => "^=",
                SyntaxKind.EqualsEqualsToken => "==",
                SyntaxKind.BangEqualsToken => "!=",
                SyntaxKind.OpenParenthesisToken => "(",
                SyntaxKind.CloseParenthesisToken => ")",
                SyntaxKind.OpenBraceToken => "{{",
                SyntaxKind.CloseBraceToken => "}}",
                SyntaxKind.ColonToken => ":",
                SyntaxKind.CommaToken => ",",
                SyntaxKind.BreakKeyword => "break",
                SyntaxKind.ContinueKeyword => "continue",
                SyntaxKind.ElseKeyword => "else",
                SyntaxKind.FalseKeyword => "false",
                SyntaxKind.ForKeyword => "for",
                SyntaxKind.FunctionKeyword => "function",
                SyntaxKind.IfKeyword => "if",
                SyntaxKind.LetKeyword => "let",
                SyntaxKind.ReturnKeyword => "return",
                SyntaxKind.ToKeyword => "to",
                SyntaxKind.TrueKeyword => "true",
                SyntaxKind.VarKeyword => "var",
                SyntaxKind.IntKeyword => "int",
                SyntaxKind.FloatKeyword => "float",
                SyntaxKind.StringKeyword => "String",
                SyntaxKind.DoubleKeyword => "double",
                SyntaxKind.WhileKeyword => "while",
                SyntaxKind.DoKeyword => "do",
                SyntaxKind.PublicKeyword => "public",
                SyntaxKind.PrivateKeyword => "private",
                SyntaxKind.SealedKeyword => "sealed",
                SyntaxKind.StaticKeyword => "static",
                SyntaxKind.ListKeyword => "List",
                SyntaxKind.DictionaryKeyword => "HashMap",
                SyntaxKind.CloseSquareBracket => "]",
                SyntaxKind.ContainsKeyCall => "containsKey",
                SyntaxKind.NewKeyword => "new",
                _ => "",
            };
        }
    }
}