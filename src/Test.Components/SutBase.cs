using Moq;

namespace MaSch.Test
{
    /// <summary>
    /// Base class for a sut.
    /// </summary>
    public abstract class SutBase
    {
        private MockRepository? _mocks;

        /// <summary>
        /// Gets the default mock behavior for the <see cref="Mocks"/> repository.
        /// </summary>
        public virtual MockBehavior DefaultMockBehavior { get; } = MockBehavior.Strict;

        /// <summary>
        /// Gets the <see cref="MockRepository"/> for this instance.
        /// </summary>
        public MockRepository Mocks => _mocks ??= new MockRepository(DefaultMockBehavior);
    }
}
