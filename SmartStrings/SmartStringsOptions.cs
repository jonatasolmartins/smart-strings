// SmartStrings - String templating helpers for C#
// https://github.com/jonatasolmartins/smart-strings
//
// Copyright (c) 2025 Jonatas Olziris Martins
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Globalization;

namespace SmartStrings
{
    /// <summary>
    /// Configuration options for SmartStrings library.
    /// </summary>
    public class SmartStringsOptions
    {
        /// <summary>
        /// Gets or sets the default culture to use for formatting.
        /// If null, will use Thread.CurrentCulture when InheritThreadCulture is true,
        /// otherwise falls back to CultureInfo.InvariantCulture.
        /// </summary>
        public CultureInfo? DefaultCulture { get; set; } = null;

        /// <summary>
        /// Gets or sets whether to inherit culture from Thread.CurrentCulture.
        /// When true (default), respects ASP.NET Core request localization.
        /// When false, uses DefaultCulture or InvariantCulture.
        /// </summary>
        public bool InheritThreadCulture { get; set; } = true;
    }
}
