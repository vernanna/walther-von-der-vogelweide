using System.Diagnostics.CodeAnalysis;

namespace Vernanna.WaltherVonDerVogelweide.Shared.Extensions;

public static class ObjectExtensions
{
    [return: NotNull]
    public static T ThrowIfNull<T>(this T? value, string? paramName = null) =>
        value is null
            ? throw new ArgumentNullException(paramName ?? nameof(value), "Value cannot be null.")
            : value;

    public static T? As<T>(this object? value) where T : class => value as T;
}