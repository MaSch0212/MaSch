using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Presentation
{
    /// <summary>
    /// Specifies the buttons that are displayed on an alert.
    /// </summary>
    public enum AlertButton
    {
        /// <summary>
        /// The alert displays an OK button.
        /// </summary>
        Ok = 0,

        /// <summary>
        /// The alert displays OK and Cancel buttons.
        /// </summary>
        OkCancel = 1,

        /// <summary>
        /// The alert displays Yes, No, and Cancel buttons.
        /// </summary>
        YesNoCancel = 3,

        /// <summary>
        /// The alert displays Yes and No buttons.
        /// </summary>
        YesNo = 4,
    }

    /// <summary>
    /// Specifies the icon that is displayed by a alert.
    /// </summary>
    [SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Elements has been copied from the MessageBoxImage enumeration from PresentationFramework.")]
    public enum AlertImage
    {
        /// <summary>
        /// The alert contains no symbols.
        /// </summary>
        None = 0,

        /// <summary>
        /// The alert contains a symbol consisting of a white X in a circle with a red background.
        /// </summary>
        Hand = 16,

        /// <summary>
        /// The alert contains a symbol consisting of white X in a circle with a red background.
        /// </summary>
        Stop = 16,

        /// <summary>
        /// The alert contains a symbol consisting of white X in a circle with a red background.
        /// </summary>
        Error = 16,

        /// <summary>
        /// The alert contains a symbol consisting of a question mark in a circle.
        /// The question mark message icon is no longer recommended because it does not clearly represent a specific type of message
        /// and because the phrasing of a message as a question could apply to any message type.
        /// In addition, users can confuse the question mark symbol with a help information symbol.
        /// Therefore, do not use this question mark symbol in your alerts.
        /// The system continues to support its inclusion only for backward compatibility.
        /// </summary>
        Question = 32,

        /// <summary>
        /// The alert contains a symbol consisting of an exclamation point in a triangle with a yellow background.
        /// </summary>
        Exclamation = 48,

        /// <summary>
        /// The alert contains a symbol consisting of an exclamation point in a triangle with a yellow background.
        /// </summary>
        Warning = 48,

        /// <summary>
        /// The alert contains a symbol consisting of a lowercase letter i in a circle.
        /// </summary>
        Asterisk = 64,

        /// <summary>
        /// The alert contains a symbol consisting of a lowercase letter i in a circle.
        /// </summary>
        Information = 64,
    }

    /// <summary>
    /// Specifies special display options for an alert.
    /// </summary>
    [Flags]
    public enum AlertOptions
    {
        /// <summary>
        /// No options are set.
        /// </summary>
        None = 0,

        /// <summary>
        /// The alert is displayed on the default desktop of the interactive window station.
        /// Specifies that the alert is displayed from a .NET Windows Service application in order to notify the user of an event.
        /// </summary>
        DefaultDesktopOnly = 131072,

        /// <summary>
        /// The alert text and title bar caption are right-aligned.
        /// </summary>
        RightAlign = 524288,

        /// <summary>
        /// All text, buttons, icons, and title bars are displayed right-to-left.
        /// </summary>
        RtlReading = 1048576,

        /// <summary>
        /// The alert is displayed on the currently active desktop even if a user is not logged on to the computer.
        /// Specifies that the alert is displayed from a .NET Windows Service application in order to notify the user of an event.
        /// </summary>
        ServiceNotification = 2097152,
    }

    /// <summary>
    /// Specifies which alert button that a user clicks.
    /// </summary>
    public enum AlertResult
    {
        /// <summary>
        /// The alert returns no result.
        /// </summary>
        None = 0,

        /// <summary>
        /// The result value of the alert is OK.
        /// </summary>
        Ok = 1,

        /// <summary>
        /// The result value of the alert is Cancel.
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The result value of the alert is Yes.
        /// </summary>
        Yes = 6,

        /// <summary>
        /// The result value of the alert is No.
        /// </summary>
        No = 7,
    }

    /// <summary>
    /// Specifies whether a window is minimized, maximized, or restored.
    /// </summary>
    public enum WindowVisualState
    {
        /// <summary>
        /// The window is restored.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The window is minimized.
        /// </summary>
        Minimized = 1,

        /// <summary>
        /// The window is maximized.
        /// </summary>
        Maximized = 2,
    }

    /// <summary>
    /// Specifies the type of version for an assembly.
    /// </summary>
    public enum AssemblyVersionType
    {
        /// <summary>
        /// The assembly version.
        /// </summary>
        AssemblyVersion,

        /// <summary>
        /// The assembly file version.
        /// </summary>
        AssemblyFileVersion,

        /// <summary>
        /// The assembly informational version.
        /// </summary>
        AssemblyInformationalVersion,
    }

    /// <summary>
    /// Provides extensions for the <see cref="AssemblyVersionType"/> enum.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "The file name matches the enum.")]
    public static class AssemblyVersionTypeExtensions
    {
        /// <summary>
        /// Gets the version from an <see cref="Assembly"/>.
        /// </summary>
        /// <param name="type">The version type to get.</param>
        /// <param name="assembly">The assembly to get the version from.</param>
        /// <returns>The version as <see cref="string"/> for the given <see cref="Assembly"/>.</returns>
        public static string? GetVersion(this AssemblyVersionType type, Assembly? assembly)
        {
            if (assembly == null)
                return null;
            return type switch
            {
                AssemblyVersionType.AssemblyFileVersion => assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version,
                AssemblyVersionType.AssemblyInformationalVersion => assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
                _ => assembly.GetName().Version?.ToString(),
            };
        }

        /// <summary>
        /// Gets the version from the entry <see cref="Assembly"/>.
        /// </summary>
        /// <param name="type">The version type to get.</param>
        /// <returns>The version as <see cref="string"/> for the entry <see cref="Assembly"/>.</returns>
        public static string? GetVersion(this AssemblyVersionType type)
        {
            return GetVersion(type, Assembly.GetEntryAssembly());
        }
    }
}
