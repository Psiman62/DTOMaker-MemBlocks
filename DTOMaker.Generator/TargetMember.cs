using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace DTOMaker.Generator
{
    public sealed class TargetMember : TargetBase
    {
        public TargetMember(string name, Location location) : base(name, location) { }
        public int Sequence { get; set; }
        public string MemberType { get; set; } = "";
        public int FieldOffset { get; set; }
        public int FieldLength { get; set; }
        public bool IsBigEndian { get; set; } = false;
        public string CodecTypeName => $"DTOMaker.Runtime.Codec_{MemberType}_{(IsBigEndian ? "BE" : "LE")}";


        public bool CanEmit()
        {
            return !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(MemberType)
                && Sequence > 0
                && FieldOffset >= 0
                && FieldLength > 0;
        }

        protected override IEnumerable<SyntaxDiagnostic> OnGetValidationDiagnostics()
        {
            if (string.IsNullOrWhiteSpace(MemberType))
            {
                yield return new SyntaxDiagnostic(_location, DiagnosticSeverity.Error, $"MemberType'{MemberType}' must be defined");
            }
            if (Sequence <= 0)
            {
                yield return new SyntaxDiagnostic(_location, DiagnosticSeverity.Error, $"Sequence ({Sequence}) must be > 0");
            }
            if (FieldOffset < 0)
            {
                yield return new SyntaxDiagnostic(_location, DiagnosticSeverity.Error, $"FieldOffset ({FieldOffset}) must be >= 0");
            }
            if (FieldLength <= 0)
            {
                yield return new SyntaxDiagnostic(_location, DiagnosticSeverity.Error, $"FieldLength ({FieldLength}) must be > 0");
            }
            yield break; // todo
        }
    }
}