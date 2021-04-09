using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MaSch.Test.Assertion
{
    /// <summary>
    /// A collection of helper classes to test various conditions within unit tests.
    /// If the condition being tested is not met, an <see cref="AssertFailedException"/> is thrown.
    /// </summary>
    /// <seealso cref="MaSch.Test.Assertion.AssertBase" />
    public class Assert : AssertBase
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="Assert"/>.
        /// </summary>
        public static Assert Instance { get; } = new Assert();

        /// <summary>
        /// Gets the singleton instance of the original MSTest <see cref="Microsoft.VisualStudio.TestTools.UnitTesting.Assert"/> functionality.
        /// </summary>
        public MSAssert That => MSAssert.That;

        /// <summary>
        /// Gets an assert object that instead of throwing <see cref="AssertFailedException"/>s uses <see cref="AssertInconclusiveException"/>s instead.
        /// </summary>
        public InconclusiveAssert Inc => InconclusiveAssert.Instance;

        /// <inheritdoc/>
        protected override string? AssertNamePrefix { get; } = "Assert";

        private Assert()
        {
        }

        /// <inheritdoc/>
        protected override void HandleFailedAssertion(string message)
        {
            throw new AssertFailedException(message);
        }
    }
}
