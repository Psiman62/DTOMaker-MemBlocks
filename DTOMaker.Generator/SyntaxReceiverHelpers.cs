using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace DTOMaker.Generator
{
    internal static class SyntaxReceiverHelpers
    {
        public static bool IsIdentifierForAttributeName(this IdentifierNameSyntax ins, string attributeName)
        {
            var prefix = ins.Identifier.Text.AsSpan();
            var suffix = nameof(Attribute).AsSpan();
            var candidate = attributeName.AsSpan();
            return candidate.Length == (prefix.Length + suffix.Length)
                && candidate.StartsWith(prefix)
                && candidate.EndsWith(suffix);
        }

    }
}
