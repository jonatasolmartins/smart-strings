// SmartStrings - String templating helpers for C#
// https://github.com/jonatasolmartins/smart-strings
//
// Copyright (c) 2025 Jonatas Olziris Martins
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

#if NET6_0_OR_GREATER
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
#endif

namespace SmartStrings
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Extension methods for configuring SmartStrings in dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds SmartStrings configuration to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Optional configuration action.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddSmartStrings(
            this IServiceCollection services, 
            Action<SmartStringsOptions>? configure = null)
        {
            services.Configure<SmartStringsOptions>(options =>
            {
                configure?.Invoke(options);
            });

            // Set global defaults from configuration
            var options = new SmartStringsOptions();
            configure?.Invoke(options);
            SmartStringExtensions.ConfigureDefaults(options);

            return services;
        }

        /// <summary>
        /// Adds SmartStrings configuration with a specific culture.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="cultureName">The culture name (e.g., "en-US", "pt-BR").</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddSmartStrings(
            this IServiceCollection services, 
            string cultureName)
        {
            return services.AddSmartStrings(options =>
            {
                options.DefaultCulture = new CultureInfo(cultureName);
            });
        }

        /// <summary>
        /// Adds SmartStrings configuration with a specific culture.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="culture">The culture to use.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddSmartStrings(
            this IServiceCollection services, 
            CultureInfo culture)
        {
            return services.AddSmartStrings(options =>
            {
                options.DefaultCulture = culture;
            });
        }
    }
#endif
}
