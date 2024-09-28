using DTOMaker.Gentime;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace DTOMaker.Generator
{
    internal sealed class MemBlockMember : TargetMember
    {
        public MemBlockMember(string name, Location location) : base(name, location) { }

        public string CodecTypeName => $"DTOMaker.Runtime.Codec_{MemberType}_{(IsBigEndian ? "BE" : "LE")}";

        private SyntaxDiagnostic? CheckFieldOffset()
        {
            return FieldOffset switch
            {
                null => null, // todo not allowed when required
                >= 0 => null,
                _ => new SyntaxDiagnostic(Location, DiagnosticSeverity.Error, $"FieldOffset ({FieldOffset}) must be >= 0")
            };
        }

        private SyntaxDiagnostic? CheckFieldLength()
        {
            return FieldLength switch
            {
                null => null, // todo not allowed when required
                > 0 => null,
                _ => new SyntaxDiagnostic(Location, DiagnosticSeverity.Error, $"FieldLength ({FieldLength}) must be > 0")
            };
        }

        protected override IEnumerable<SyntaxDiagnostic> OnGetValidationDiagnostics()
        {
            foreach (var diagnostic1 in base.OnGetValidationDiagnostics())
            {
                yield return diagnostic1;
            }

            SyntaxDiagnostic? diagnostic2;
            if ((diagnostic2 = CheckFieldOffset()) is not null) yield return diagnostic2;
            if ((diagnostic2 = CheckFieldLength()) is not null) yield return diagnostic2;
        }


    }
}
