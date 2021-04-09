using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaSch.Test.Assertion
{
    /// <summary>
    /// A collection of helper classes to test various conditions within unit tests.
    /// If the condition being tested is not met, an <see cref="AssertInconclusiveException"/> is thrown.
    /// </summary>
    /// <seealso cref="MaSch.Test.Assertion.AssertBase" />
    public class InconclusiveAssert : AssertBase
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="InconclusiveAssert"/>.
        /// </summary>
        public static InconclusiveAssert Instance { get; } = new InconclusiveAssert();

        /// <inheritdoc/>
        protected override string? AssertNamePrefix { get; } = "Assert.Inc";

        private InconclusiveAssert()
        {
        }

        /// <inheritdoc/>
        protected override void HandleFailedAssertion(string message)
        {
            throw new AssertInconclusiveException("[Inconclusive] " + message);
        }
    }
}
