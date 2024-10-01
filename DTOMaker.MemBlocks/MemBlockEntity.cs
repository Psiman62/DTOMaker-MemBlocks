using DTOMaker.Gentime;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace DTOMaker.MemBlocks
{
    internal static class DiagnosticId
    {
        public const string DMMB0001 = nameof(DMMB0001); // Invalid block size
        public const string DMMB0002 = nameof(DMMB0002); // Invalid field offset
        public const string DMMB0003 = nameof(DMMB0003); // Invalid field length
    }
    internal sealed class MemBlockEntity : TargetEntity
    {
        public MemBlockEntity(string name, Location location) : base(name, location) { }

        private SyntaxDiagnostic? CheckBlockSizeIsValid()
        {
            return BlockSize switch
            {
                1 => null,
                2 => null,
                4 => null,
                8 => null,
                16 => null,
                32 => null,
                64 => null,
                128 => null,
                256 => null,
                512 => null,
                1024 => null,
                _ => new SyntaxDiagnostic(
                        DiagnosticId.DMMB0001, "Invalid block size", DiagnosticCategory.Design, Location, DiagnosticSeverity.Error,
                        $"BlockSize ({BlockSize}) is invalid. BlockSize must be a whole power of 2 between 1 and 1024")
            };
        }

        protected override IEnumerable<SyntaxDiagnostic> OnGetValidationDiagnostics()
        {
            foreach (var diagnostic1 in base.OnGetValidationDiagnostics())
            {
                yield return diagnostic1;
            }

            SyntaxDiagnostic? diagnostic2;
            if ((diagnostic2 = CheckBlockSizeIsValid()) is not null) yield return diagnostic2;
        }

    }
}
