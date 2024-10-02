using DTOMaker.Models;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace DTOMaker.MemBlocks.Tests
{
    public class GeneratorTests
    {
        //[Fact]
        //public void ConstantsValueChecks()
        //{
        //    nameof(DomainAttribute).Should().Be(Constants.DomainAttribute);
        //    nameof(EntityAttribute).Should().Be(Constants.EntityAttribute);
        //    nameof(MemberAttribute).Should().Be(Constants.MemberAttribute);
        //}

        [Fact]
        public async Task Happy01_NoMembers()
        {
            var inputSource =
                """
                using DTOMaker.Models;
                namespace MyOrg.Models
                {
                    [Entity()]
                    [EntityLayout(LayoutMethod.Explicit, 64)]
                    public interface IMyDTO
                    {
                    }
                }
                """;

            var generatorResult = GeneratorTestHelper.RunSourceGenerator(inputSource, LanguageVersion.LatestMajor);
            generatorResult.Exception.Should().BeNull();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Info).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Should().BeEmpty();
            generatorResult.GeneratedSources.Length.Should().Be(1);
            GeneratedSourceResult outputSource = generatorResult.GeneratedSources[0];

            // custom generation checks
            outputSource.HintName.Should().Be("MyOrg.Models.MyDTO.MemBlocks.g.cs");
            string outputCode = string.Join(Environment.NewLine, outputSource.SourceText.Lines.Select(tl => tl.ToString()));
            await Verifier.Verify(outputCode);
        }

        [Fact]
        public async Task Happy02_OneMember()
        {
            var inputSource =
                """
                using DTOMaker.Models;
                namespace MyOrg.Models
                {
                    [Entity()]
                    [EntityLayout(LayoutMethod.Explicit, 64)]
                    public interface IMyDTO
                    {
                        [Member(1)] 
                        [MemberLayout(0, 8)]
                        double Field1 { get; set; }
                    }
                }
                """;

            var generatorResult = GeneratorTestHelper.RunSourceGenerator(inputSource, LanguageVersion.LatestMajor);
            generatorResult.Exception.Should().BeNull();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Info).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Should().BeEmpty();
            generatorResult.GeneratedSources.Length.Should().Be(1);
            GeneratedSourceResult outputSource = generatorResult.GeneratedSources[0];

            // custom generation checks
            string outputCode = string.Join(Environment.NewLine, outputSource.SourceText.Lines.Select(tl => tl.ToString()));
            await Verifier.Verify(outputCode);
        }

        [Fact]
        public async Task Happy03_TwoMembers()
        {
            var inputSource =
                """
                using DTOMaker.Models;
                namespace MyOrg.Models
                {
                    [Entity()]
                    [EntityLayout(LayoutMethod.Explicit, 64)]
                    public interface IMyDTO
                    {
                        [Member(1)]
                        [MemberLayout(0, 8)] 
                        double Field1 { get; set; }

                        [Member(2)]
                        [MemberLayout(8, 8)] 
                        long Field2 { get; set; }
                    }
                }
                """;

            var generatorResult = GeneratorTestHelper.RunSourceGenerator(inputSource, LanguageVersion.LatestMajor);
            generatorResult.Exception.Should().BeNull();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Info).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Should().BeEmpty();
            generatorResult.GeneratedSources.Length.Should().Be(1);
            GeneratedSourceResult outputSource = generatorResult.GeneratedSources[0];

            // custom generation checks
            string outputCode = string.Join(Environment.NewLine, outputSource.SourceText.Lines.Select(tl => tl.ToString()));
            await Verifier.Verify(outputCode);
        }

        [Fact]
        public async Task Happy04_TwoEntities()
        {
            var inputSource =
                """
                using DTOMaker.Models;
                namespace MyOrg.Models
                {
                    [Entity()]
                    [EntityLayout(LayoutMethod.Explicit, 64)]
                    public interface IMyFirstDTO
                    {
                        [Member(1)]
                        [MemberLayout(0, 8)] 
                        double Field1 { get; set; }
                    }

                    [Entity()]
                    [EntityLayout(LayoutMethod.Explicit, 64)]
                    public interface IMyOtherDTO
                    {
                        [Member(1)]
                        [MemberLayout(0, 8)]
                        long Field1 { get; set; }
                    }
                }
                """;

            var generatorResult = GeneratorTestHelper.RunSourceGenerator(inputSource, LanguageVersion.LatestMajor);
            generatorResult.Exception.Should().BeNull();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Info).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Should().BeEmpty();

            // custom generation checks
            generatorResult.GeneratedSources.Length.Should().Be(2);
            {
                GeneratedSourceResult outputSource = generatorResult.GeneratedSources[0];
                outputSource.HintName.Should().Be("MyOrg.Models.MyFirstDTO.MemBlocks.g.cs");
            }
            {
                GeneratedSourceResult outputSource = generatorResult.GeneratedSources[1];
                outputSource.HintName.Should().Be("MyOrg.Models.MyOtherDTO.MemBlocks.g.cs");
                string outputCode = string.Join(Environment.NewLine, outputSource.SourceText.Lines.Select(tl => tl.ToString()));
                await Verifier.Verify(outputCode);
            }
        }

        [Fact]
        public void Fault01_InvalidLayoutMethod()
        {
            var inputSource =
                """
                using DTOMaker.Models;
                namespace MyOrg.Models
                {
                    [Entity()]
                    [EntityLayout(LayoutMethod.Undefined, 64)]
                    public interface IMyDTO
                    {
                    }
                }
                """;

            var generatorResult = GeneratorTestHelper.RunSourceGenerator(inputSource, LanguageVersion.LatestMajor);
            generatorResult.Exception.Should().BeNull();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Info).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Should().BeEmpty();

            var errors = generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
            errors.Length.Should().Be(1);
            errors[0].GetMessage().Should().StartWith("LayoutMethod is not defined.");
        }

        [Fact]
        public void Fault02_InvalidBlockSize()
        {
            var inputSource =
                """
                using DTOMaker.Models;
                namespace MyOrg.Models
                {
                    [Entity()]
                    [EntityLayout(LayoutMethod.Explicit, 63)]
                    public interface IMyDTO
                    {
                    }
                }
                """;

            var generatorResult = GeneratorTestHelper.RunSourceGenerator(inputSource, LanguageVersion.LatestMajor);
            generatorResult.Exception.Should().BeNull();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Info).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Should().BeEmpty();

            var errors = generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
            errors.Length.Should().Be(1);
            errors[0].GetMessage().Should().StartWith("BlockSize (63) is invalid.");
        }

        [Fact]
        public void Fault03_OrphanMember()
        {
            // note: [Entity] attribute is missing
            var inputSource =
                """
                using DTOMaker.Models;
                namespace MyOrg.Models
                {
                    public interface IMyDTO
                    {
                        [Member(1)]
                        [MemberLayout(0, 8)] 
                        double Field1 { get; set; }
                    }
                }
                """;

            var generatorResult = GeneratorTestHelper.RunSourceGenerator(inputSource, LanguageVersion.LatestMajor);
            generatorResult.Exception.Should().BeNull();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Info).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Should().BeEmpty();

            // custom generation checks
            generatorResult.GeneratedSources.Length.Should().Be(0);
        }

        [Fact]
        public void Fault04_InvalidMemberOffset_Lo()
        {
            var inputSource =
                """
                using DTOMaker.Models;
                namespace MyOrg.Models
                {
                    [Entity()]
                    [EntityLayout(LayoutMethod.Explicit, 64)]
                    public interface IMyDTO
                    {
                        [Member(1)] 
                        [MemberLayout(-1, 8)]
                        double Field1 { get; set; }
                    }
                }
                """;

            var generatorResult = GeneratorTestHelper.RunSourceGenerator(inputSource, LanguageVersion.LatestMajor);
            generatorResult.Exception.Should().BeNull();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Info).Should().BeEmpty();
            generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning).Should().BeEmpty();

            var errors = generatorResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
            errors.Length.Should().Be(1);
            errors[0].GetMessage().Should().Be("FieldOffset (-1) must be >= 0");
        }

        // todo field offset too hi

    }
}