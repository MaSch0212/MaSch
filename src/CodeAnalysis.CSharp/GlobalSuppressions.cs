// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1403:File may only contain a single namespace", Justification = "Fine here, because the extension classes should be in a different namespace. But I want to have them in the same file.", Scope = "namespace", Target = "~N:MaSch.CodeAnalysis.CSharp.SourceGeneration")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1205:Partial elements should declare access", Justification = "Accessibility is declared by ./SourceGeneration/SourceBuilder.cs", Scope = "type", Target = "~T:MaSch.CodeAnalysis.CSharp.SourceGeneration.SourceBuilder")]
