using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Data.Common;

namespace DTOMaker.Generator
{

    internal class SyntaxReceiver : ISyntaxContextReceiver
    {
        public ConcurrentDictionary<string, TargetDomain> Domains { get; } = new ConcurrentDictionary<string, TargetDomain>();


        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            SyntaxReceiverHelper.ProcessNode(context, Domains);
        }
    }
}
