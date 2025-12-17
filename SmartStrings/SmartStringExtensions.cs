// SmartStrings - String templating helpers for C#
// https://github.com/jonatasolmartins/smart-strings
//
// Copyright (c) 2025 Jonatas Olziris Martins
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("SmartStrings.Tests")]

namespace SmartStrings
{

    /// <summary>
    /// Provides static access to template filling methods, as an alternative to extension syntax.
    /// </summary>
    public static class TemplateString
    {
         /// <summary>
        /// Replaces the first placeholder in the string with the provided value.
        /// </summary>
        /// <remarks>
        /// Returns the original template if it is null or empty.
        /// </remarks>
        /// <param name="template">The template string containing a single placeholder.</param>
        /// <param name="value">The value to insert into the template.</param>
        /// <returns>The filled string.</returns>
        public static string Fill(string template, string value) =>
            SmartStringExtensions.Fill(template, value);

        /// <summary>
        /// Replaces placeholders in the string with the corresponding values in the order provided.
        /// </summary>
        /// <remarks>
        /// Returns the original template if it is null or empty.
        /// </remarks>
        /// <param name="template">The template string containing placeholders.</param>
        /// <param name="values">The values to insert into the placeholders in order.</param>
        /// <returns>The filled string.</returns>
        public static string Fill(string template, params string[] values) =>
            SmartStringExtensions.Fill(template, values);

        /// <summary>
        /// Replaces named placeholders in the template with values from a dictionary.
        /// Supports fallback values in the form <c>{key:fallback}</c>.
        /// </summary>
        /// <remarks>
        /// Returns the original template if it is null or empty.
        /// </remarks>
        /// <param name="template">The template string containing named placeholders.</param>
        /// <param name="values">A dictionary of keys and values to fill the template.</param>
        /// <returns>The filled string with all placeholders replaced.</returns>
        public static string Fill(string template, Dictionary<string, string?> values) =>
            SmartStringExtensions.Fill(template, values);

        /// <summary>
        /// Replaces placeholders in the template with values from a model.
        /// Supports both flat objects (with public properties) and primitive values.
        /// Named placeholders support fallback values in the form <c>{key:fallback}</c>.
        /// </summary>
        /// <remarks>
        /// If the model is a primitive or string, replaces the first placeholder only.
        /// If the template is null or empty, it is returned as is.
        /// </remarks>
        /// <param name="template">The template string containing named or positional placeholders.</param>
        /// <param name="values">A value or object whose data is used to fill the template.</param>
        /// <returns>The filled string with placeholders replaced accordingly.</returns>
        public static string Fill<T>(string template, T values) =>
            SmartStringExtensions.Fill(template, values);

        /// <summary>
        /// Replaces placeholders in the template with values from a model using the specified culture.
        /// Supports both flat objects (with public properties) and primitive values.
        /// Named placeholders support format specifiers in the form <c>{key:format}</c> for IFormattable types,
        /// or fallback values for non-formattable types.
        /// </summary>
        /// <remarks>
        /// If the model is a primitive or string, replaces the first placeholder only.
        /// If the template is null or empty, it is returned as is.
        /// Format specifiers work with DateTime, decimal, int, and other IFormattable types.
        /// </remarks>
        /// <param name="template">The template string containing named or positional placeholders.</param>
        /// <param name="values">A value or object whose data is used to fill the template.</param>
        /// <param name="culture">The culture to use for formatting. If null, uses configured culture or Thread.CurrentCulture.</param>
        /// <returns>The filled string with placeholders replaced accordingly.</returns>
        public static string Fill<T>(string template, T values, CultureInfo culture) =>
            SmartStringExtensions.Fill(template, values, culture);

        /// <summary>
        /// Fills a template using the flat properties of a model, allowing custom overrides for nested or formatted values.
        /// </summary>
        public static string Fill<T>(string template, T model, Action<TemplateMap<T>> map) =>
            SmartStringExtensions.Fill(template, model, map);
    }
    /// <summary>
    /// Provides extension methods for filling string templates with values.
    /// </summary>
    public static class SmartStringExtensions
    {
        private static readonly Regex NamedPlaceholderRegex = new Regex(@"\{(\w+)(?::([^}]*))?\}", RegexOptions.Compiled);
        private static readonly Regex GenericPlaceholderRegex = new Regex(@"\{[^{}]+\}", RegexOptions.Compiled);

        /// <summary>
        /// Global configuration options for SmartStrings.
        /// </summary>
        internal static SmartStringsOptions GlobalOptions { get; private set; } = new SmartStringsOptions();

        /// <summary>
        /// Configures global defaults for SmartStrings. Used internally by DI extensions.
        /// </summary>
        /// <param name="options">The options to apply globally.</param>
        internal static void ConfigureDefaults(SmartStringsOptions options)
        {
            GlobalOptions = options ?? new SmartStringsOptions();
        }

        /// <summary>
        /// Resolves the culture to use for formatting based on configuration and parameters.
        /// </summary>
        /// <param name="explicitCulture">Explicitly provided culture (highest priority).</param>
        /// <returns>The culture to use for formatting.</returns>
        private static CultureInfo ResolveCulture(CultureInfo? explicitCulture)
        {
            // Priority: explicit > configured > Thread.CurrentCulture > InvariantCulture
            if (explicitCulture != null)
                return explicitCulture;

            if (GlobalOptions.DefaultCulture != null)
                return GlobalOptions.DefaultCulture;

            if (GlobalOptions.InheritThreadCulture)
                return CultureInfo.CurrentCulture;

            return CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Replaces the first placeholder in the string with the provided value.
        /// </summary>
        /// <remarks>
        /// Returns the original template if it is null or empty.
        /// </remarks>
        /// <param name="template">The template string containing a single placeholder.</param>
        /// <param name="value">The value to insert into the template.</param>
        /// <returns>The filled string.</returns>
        public static string Fill(this string template, string value)
        {
            if (string.IsNullOrEmpty(template)) return template;

            return GenericPlaceholderRegex.Replace(template, value ?? string.Empty, 1);
        }

        /// <summary>
        /// Replaces placeholders in the string with the corresponding values in the order provided.
        /// </summary>
        /// <remarks>
        /// Returns the original template if it is null or empty.
        /// </remarks>
        /// <param name="template">The template string containing placeholders.</param>
        /// <param name="values">The values to insert into the placeholders in order.</param>
        /// <returns>The filled string.</returns>
        public static string Fill(this string template, params string[] values)
        {
            if (string.IsNullOrEmpty(template)) return template;

            int i = 0;
            return GenericPlaceholderRegex.Replace(template, _ =>
            {
                if (i < values.Length)
                    return values[i++];
                return _?.Value ?? string.Empty;
            });
        }

        /// <summary>
        /// Replaces named placeholders in the template with values from a dictionary.
        /// Supports fallback values in the form <c>{key:fallback}</c>.
        /// </summary>
        /// <remarks>
        /// Returns the original template if it is null or empty.
        /// </remarks>
        /// <param name="template">The template string containing named placeholders.</param>
        /// <param name="values">A dictionary of keys and values to fill the template.</param>
        /// <returns>The filled string with all placeholders replaced.</returns>
        public static string Fill(this string template, Dictionary<string, string?> values)
        {
            if (string.IsNullOrEmpty(template)) return template;

            return NamedPlaceholderRegex.Replace(template, match =>
            {
                var key = match.Groups[1].Value;
                var fallback = match.Groups[2].Success ? match.Groups[2].Value : string.Empty;

                return values.TryGetValue(key, out var value) && !string.IsNullOrEmpty(value)
                    ? value
                    : fallback;
            });
        }

        /// <summary>
        /// Replaces placeholders in the template with values from a model.
        /// Supports both flat objects (with public properties) and primitive values.
        /// Named placeholders support format specifiers in the form <c>{key:format}</c> for IFormattable types,
        /// or fallback values for non-formattable types.
        /// </summary>
        /// <remarks>
        /// If the model is a primitive or string, replaces the first placeholder only.
        /// If the template is null or empty, it is returned as is.
        /// Format specifiers work with DateTime, decimal, int, and other IFormattable types.
        /// </remarks>
        /// <param name="template">The template string containing named or positional placeholders.</param>
        /// <param name="values">A value or object whose data is used to fill the template.</param>
        /// <returns>The filled string with placeholders replaced accordingly.</returns>
        public static string Fill<T>(this string template, T values)
        {
            return Fill(template, values, (CultureInfo?)null);
        }

        /// <summary>
        /// Replaces placeholders in the template with values from a model using the specified culture.
        /// Supports both flat objects (with public properties) and primitive values.
        /// Named placeholders support format specifiers in the form <c>{key:format}</c> for IFormattable types,
        /// or fallback values for non-formattable types.
        /// </summary>
        /// <remarks>
        /// If the model is a primitive or string, replaces the first placeholder only.
        /// If the template is null or empty, it is returned as is.
        /// Format specifiers work with DateTime, decimal, int, and other IFormattable types.
        /// </remarks>
        /// <param name="template">The template string containing named or positional placeholders.</param>
        /// <param name="values">A value or object whose data is used to fill the template.</param>
        /// <param name="culture">The culture to use for formatting. If null, uses configured culture or Thread.CurrentCulture.</param>
        /// <returns>The filled string with placeholders replaced accordingly.</returns>
        public static string Fill<T>(this string template, T values, CultureInfo? culture)
        {

            if (string.IsNullOrEmpty(template)) return template;

            if (values is null)
                return template.FillWithObjects(new Dictionary<string, object?>(), culture);

            var type = typeof(T);

            if (type.IsPrimitive || values is string || values is decimal)
            {
                return template.Fill(values?.ToString() ?? string.Empty);
            }

            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                dict[prop.Name] = prop.GetValue(values);
            }

            return template.FillWithObjects(dict, culture);

        }


        /// <summary>
        /// Fills a template using the flat properties of a model, allowing custom overrides for nested or formatted values.
        /// </summary>
        public static string Fill<T>(this string template, T model, Action<TemplateMap<T>> map)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;

            var baseDict = SmartStringExtensions.ExtractValues(model);

            var binder = new TemplateMap<T>(model);
            map?.Invoke(binder);

            foreach (var kv in binder.GetValues())
            {
                baseDict[kv.Key] = kv.Value;
            }

            return template.Fill(baseDict);
        }

        /// <summary>
        /// Extracts public instance property values from the model into a dictionary.
        /// </summary>
        private static Dictionary<string, string?> ExtractValues<T>(T model)
        {
            var result = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            if (model == null) return result;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var val = prop.GetValue(model)?.ToString();
                result[prop.Name] = val;
            }

            return result;
        }

        /// <summary>
        /// Replaces named placeholders in the template with values from a dictionary of objects.
        /// Supports format specifiers for IFormattable types (DateTime, decimal, etc.) in the form <c>{key:format}</c>.
        /// For non-formattable types, the colon value is used as a fallback.
        /// </summary>
        private static string FillWithObjects(this string template, Dictionary<string, object?> values, CultureInfo? culture = null)
        {
            if (string.IsNullOrEmpty(template)) return template;

            var resolvedCulture = ResolveCulture(culture);

            return NamedPlaceholderRegex.Replace(template, match =>
            {
                var key = match.Groups[1].Value;
                var formatOrFallback = match.Groups[2].Success ? match.Groups[2].Value : string.Empty;

                if (values.TryGetValue(key, out var value) && value != null)
                {
                    // Check if value implements IFormattable and has a format/fallback specified
                    if (value is IFormattable formattable && !string.IsNullOrEmpty(formatOrFallback))
                    {
                        try
                        {
                            // Try to apply format - if it fails, treat as fallback
                            return formattable.ToString(formatOrFallback, resolvedCulture);
                        }
                        catch
                        {
                            // Format string was invalid, use as fallback instead
                            return formatOrFallback;
                        }
                    }
                    return value.ToString() ?? string.Empty;
                }

                // Key not found or value is null, use fallback
                return formatOrFallback;
            });
        }
    }

    /// <summary>
    /// Enables manual binding of values from a complex object to string placeholders.
    /// </summary>
    /// <typeparam name="T">The source object type.</typeparam>
    public class TemplateMap<T>
    {
        private readonly Dictionary<string, string?> _values = new();

        /// <summary>
        /// The original source object passed to Fill.
        /// </summary>
        public T Source { get; }

        public TemplateMap(T source)
        {
            Source = source;
        }

        /// Binds a value to a placeholder key.
        /// If the resolved value is null or empty, and the template has a fallback (e.g., {KEY:Fallback}),
        /// the fallback will be used instead.
        /// <example>
        /// map.Bind("USERNAME", r => r.User?.FullName);
        /// </example>
        public void Bind(string key, Func<T, object?> resolver)
        {
            _values[key] = resolver(Source)?.ToString();
        }

        /// <summary>
        /// Binds or overrides a placeholder value manually.
        /// </summary>
        public string? this[string key]
        {
            get => _values.TryGetValue(key, out var val) ? val : null;
            set => _values[key] = value;
        }

        internal Dictionary<string, string?> GetValues() => _values;
    }
}