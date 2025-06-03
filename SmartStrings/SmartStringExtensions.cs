// SmartStrings - String templating helpers for C#
// https://github.com/jonatasolmartins/smart-strings
//
// Copyright (c) 2025 Jonatas Olziris Martins
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SmartStrings
{

    /// <summary>
    /// Provides extension methods for filling string templates with values.
    /// </summary>
    public static class SmartStringExtensions
    {
        private static readonly Regex NamedPlaceholderRegex = new Regex(@"\{(\w+)(?::([^}]*))?\}", RegexOptions.Compiled);
        private static readonly Regex GenericPlaceholderRegex = new Regex(@"\{[^{}]+\}", RegexOptions.Compiled);

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
        /// Replaces named placeholders in the template with values from an object's public properties.
        /// Supports fallback values in the form <c>{key:fallback}</c>.
        /// </summary>
        /// <remarks>
        /// Returns the original template if it is null or empty.
        /// </remarks>
        /// <param name="template">The template string containing named placeholders.</param>
        /// <param name="values">An object whose property names and values are used to fill the template.</param>
        /// <returns>The filled string with placeholders replaced by property values.</returns>
        public static string Fill(this string template, object values)
        {
            if (string.IsNullOrEmpty(template)) return template;

            if (values is null)
                values = new { placeholderRegex = "" };

            var type = values.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in properties)
            {
                var val = prop.GetValue(values)?.ToString();
                dict[prop.Name] = val;
            }

            return template.Fill(dict);
        }
    }
}