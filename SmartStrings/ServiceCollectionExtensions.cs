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
using Microsoft.Extensions.Configuration;
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

        /// <summary>
        /// Adds SmartStrings configuration from a configuration section.
        /// Supports binding from appsettings.json.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configurationSection">The configuration section containing SmartStrings options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddSmartStrings(
            this IServiceCollection services,
            IConfigurationSection configurationSection)
        {
            services.Configure<SmartStringsOptions>(options =>
            {
                configurationSection.Bind(options);
            });

            // Apply configuration to global defaults
            var options = new SmartStringsOptions();
            configurationSection.Bind(options);
            SmartStringExtensions.ConfigureDefaults(options);

            return services;
        }

        /// <summary>
        /// Adds SmartStrings configuration from a configuration section with additional configuration.
        /// Supports binding from appsettings.json with override options.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configurationSection">The configuration section containing SmartStrings options.</param>
        /// <param name="configure">Additional configuration action to override or supplement configuration values.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddSmartStrings(
            this IServiceCollection services,
            IConfigurationSection configurationSection,
            Action<SmartStringsOptions> configure)
        {
            services.Configure<SmartStringsOptions>(options =>
            {
                configurationSection.Bind(options);
                configure(options);
            });

            // Apply combined configuration to global defaults
            var options = new SmartStringsOptions();
            configurationSection.Bind(options);
            configure(options);
            SmartStringExtensions.ConfigureDefaults(options);

            return services;
        }
    }
#endif
}
