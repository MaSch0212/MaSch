#pragma warning disable SA1649 // File name should match first type name

namespace MaSch.Core
{
    /// <summary>
    /// Specifies how a string should be checked for emptiness.
    /// </summary>
    public enum StringNullMode
    {
        /// <summary>
        /// The string is checked if it is null.
        /// </summary>
        IsNull,

        /// <summary>
        /// The string is checked if it is null or an empty string.
        /// </summary>
        IsNullOrEmpty,

        /// <summary>
        /// The string is checked it it is null, an empty string or only contains whitespace characters.
        /// </summary>
        IsNullOrWhitespace,
    }

    /// <summary>
    /// Defines actions that can be taken on a service inside an <see cref="IServiceContext"/>.
    /// </summary>
    public enum ServiceAction
    {
        /// <summary>
        /// No action has been taken.
        /// </summary>
        None,

        /// <summary>
        /// The service has been added.
        /// </summary>
        Added,

        /// <summary>
        /// The service has been replaced/changed.
        /// </summary>
        Changed,

        /// <summary>
        /// The service has been removed.
        /// </summary>
        Removed,
    }
}
