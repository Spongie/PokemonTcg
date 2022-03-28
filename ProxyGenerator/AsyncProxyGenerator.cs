using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyGenerator
{
    [Generator]
    public class AsyncProxyGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = (AsyncProxySyntaxReceiver)context.SyntaxReceiver;

            foreach (var item in syntaxReceiver.ClassToAugment)
            {
                var attribute = item.AttributeLists.SelectMany(a => a.Attributes).First(a => a.Name.ToString() == "MakeAsyncProxy");
                var x = (IdentifierNameSyntax)attribute.ArgumentList.Arguments.First().Expression.DescendantNodes().First(v => v.IsKind(SyntaxKind.IdentifierName));

                var model = context.Compilation.GetSemanticModel(item.SyntaxTree).GetSymbolInfo(x);
                var typeSymbol = (ITypeSymbol)model.Symbol;

                var methods = new List<string>();

                foreach (var member in typeSymbol.GetMembers().OfType<IMethodSymbol>())
                {
                    var paremeters = string.Join(",", member.Parameters.Select(p => $"{GetFullSymbolName(p.Type)} {p.Name}"));

                    var method = $@"
        public NetworkingCore.NetworkId {member.Name}({paremeters})
        {{
            var message = new NetworkingCore.Messages.GenericMessageData
            {{
                TargetMethod = {"\""}{member.Name}{"\""},
                TargetClass = {"\""}{item.Identifier.ValueText.Replace("Async", "")}{"\""},
                Parameters = new object[] {{ {string.Join(",", member.Parameters.Select(p => p.Name))} }}
            }}.ToNetworkMessage(networkPlayer.Id);

            networkPlayer.Send(message);
            return message.MessageId;
        }}
";
                    methods.Add(method);
                }

                var klass = $@"
public partial class {item.Identifier.ValueText}
{{
    {string.Join(Environment.NewLine, methods)}
}}
";

                context.AddSource($"{item.Identifier.ValueText}.g.cs", klass);

                if (model.CandidateReason == CandidateReason.Ambiguous)
                {
                    return;
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(i =>
            {
                var attributeSource = @"
namespace Proxy
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class MakeAsyncProxyAttribute: System.Attribute 
    {
        public MakeAsyncProxyAttribute(System.Type type) { TargetType = type; }
        public System.Type TargetType { get; set;}
    } 
}";
                i.AddSource("MakeAsyncProxyAttribute.g.cs", attributeSource);
            });

            context.RegisterForSyntaxNotifications(() => new AsyncProxySyntaxReceiver());
        }

        private string GetFullSymbolName(ITypeSymbol symbol)
        {
            string value = symbol.Name;

            if (symbol.TypeKind == TypeKind.Array)
            {
                return symbol.ToDisplayString();
            }
            if (symbol is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                value += $"<{GetFullSymbolName(namedType.TypeArguments.First())}> ";
            }

            var ns = symbol.ContainingNamespace;

            while (ns != null)
            {
                if (!string.IsNullOrEmpty(ns.Name))
                {
                    value = value.Insert(0, ns.Name + ".");
                }
                ns = ns.ContainingNamespace;
            }

            return value;
        }
    }

    class AsyncProxySyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> ClassToAugment { get; private set; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax cds)
            {
                if (cds.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString() == "MakeAsyncProxy"))
                {
                    ClassToAugment.Add(cds);
                }
            }
        }
    }
}
