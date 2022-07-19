using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace ServiceCollectionGenerators.Helpers;

internal static class SymbolExtensions
{
    /// <summary>
    ///     Retrieves an attribute data based on its <paramref name="fullName"/>
    /// </summary>
    public static AttributeData? GetAttribute(this ISymbol symbol, string fullName)
    {
        return symbol.GetAttributes(fullName).FirstOrDefault();
    }

    /// <summary>
    ///     Retrieves multiple attribute data based on its <paramref name="fullName"/>
    /// </summary>
    public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, string fullName)
    {
        return symbol.GetAttributes().Where(a => a.AttributeClass?.ToString() == fullName);
    }
        
    /// <summary>
    ///     Retrieves an attribute named argument based on its <paramref name="name"/>
    /// </summary>
    public static T? GetNamedArgument<T>(this AttributeData attributeData, string name)
    {
        TypedConstant typedConstant = attributeData.NamedArguments.FirstOrDefault(kp => kp.Key == name).Value;
        return !typedConstant.IsNull ? (T?)typedConstant.Value : default;
    }
}