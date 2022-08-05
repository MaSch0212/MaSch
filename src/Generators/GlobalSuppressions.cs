// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0161:Convert to file-scoped namespace", Justification = "file-scoped namedspaces should not be used, because the user of the generator might not use C# 10.", Scope = "namespace", Target = "~N:MaSch.Core")]
