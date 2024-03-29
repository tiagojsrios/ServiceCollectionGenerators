﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using ServiceCollectionGenerators.Helpers;
using ServiceCollectionGenerators.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCollectionGenerators.Generators;

[Generator]
public class OptionsGenerator : ISourceGenerator
{
    public const string OptionsAttribute = nameof(OptionsAttribute);

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(ctx => ctx.AddSource($"{OptionsAttribute}.g.cs", EmbeddedResourceHelper.GetEmbeddedResource($"{OptionsAttribute}.cs")));
        context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver(false, OptionsAttribute));
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not AttributeSyntaxReceiver syntaxReceiver) { return; }

        List<OptionsRegistrations> options = new();

        foreach (var candidateTypeNode in syntaxReceiver.Candidates)
        {
            SemanticModel model = context.Compilation.GetSemanticModel(candidateTypeNode.SyntaxTree);

            if (ModelExtensions.GetDeclaredSymbol(model, candidateTypeNode) is not INamedTypeSymbol type) { continue; }

            AttributeData? attribute = type.GetAttribute($"ServiceCollectionGenerators.Attributes.{OptionsAttribute}");

            if (attribute == null) { continue; }

            string configurationSectionName = attribute.GetNamedArgument<string>("ConfigurationSectionName") ?? type.Name;
            bool validateDataAnnotations = attribute.GetNamedArgument<bool?>("ValidateDataAnnotations") ?? true;
            bool validateOnStart = attribute.GetNamedArgument<bool?>("ValidateOnStart") ?? false;

            if (!validateDataAnnotations && validateOnStart)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor("OSG001",
                            "Invalid Configuration",
                            "ValidateDataAnnotations can't be false, when ValidateOnStart is true", 
                            nameof(OptionsGenerator), DiagnosticSeverity.Error, isEnabledByDefault: true), 
                        Location.None
                    )
                );

                continue;
            }

            options.Add(new OptionsRegistrations(type.ToDisplayString(), configurationSectionName, validateDataAnnotations, validateOnStart));
        }

        if (options.Any())
        {
            context.AddSource("ServiceCollectionExtensions.g.cs", GenerateOptionsServiceCollection(context.Compilation.AssemblyName!, options));
        }
    }

    private static SourceText GenerateOptionsServiceCollection(string @namespace, IEnumerable<OptionsRegistrations> options)
    {
        return SyntaxFactory
            .ParseCompilationUnit(OptionsRegistrations.RenderClass(@namespace, options))
            .NormalizeWhitespace()
            .GetText(Encoding.UTF8);
    }
}