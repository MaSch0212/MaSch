﻿namespace MaSch.Core
{
    /// <summary>
    /// Specifies which shims should be generated. This is used by the <see cref="global::MaSch.Core.ShimsAttribute"/>.
    /// </summary>
    [global::System.Flags]
    internal enum Shims
    {
        /// <summary>
        /// No shims will be generated.
        /// </summary>
        None = 0,

        /// <summary>
        /// Generates shims for the <see href="https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#record-types">records</see> feature introduced in C# 9.0.
        /// </summary>
        Records = 1,

        /// <summary>
        /// Generates shims for the <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#index-from-end-operator-">index</see>
        /// and <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-">range</see> operators introduced in C# 8.0.
        /// </summary>
        IndexAndRange = 2,

        /// <summary>
        /// Generates shims for the <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-reference-types">nullable reference types</see> feature
        /// introduced in C# 8.0.
        /// </summary>
        NullableReferenceTypes = 4,

        /// <summary>
        /// Generates shims for the <see href="https://docs.microsoft.com/en-us/dotnet/api/system.runtime.versioning.supportedosplatformattribute?view=net-5.0">SupportedOSPlatformAttribute</see>
        /// introduced in .NET 5.0.
        /// </summary>
        OSVersioning = 8,

        /// <summary>
        /// Generates shims for the <see href="https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callerargumentexpressionattribute?view=net-6.0">CallerArgumentExpressionAttribute</see>
        /// introduced in C# 10 (available since .NET Core 3.0 and .NET 5.0).
        /// </summary>
        CallerArgumentExpression = 16,

        /// <summary>
        /// All available shims will be generated.
        /// </summary>
        All = Records | IndexAndRange | NullableReferenceTypes | OSVersioning | CallerArgumentExpression,
    }
}