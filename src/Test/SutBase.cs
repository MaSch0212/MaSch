namespace MaSch.Test;

/// <summary>
/// Base class for a sut.
/// </summary>
public abstract class SutBase
{
    private MockRepository? _mocks;

    /// <summary>
    /// Initializes a new instance of the <see cref="SutBase"/> class.
    /// </summary>
    /// <param name="builderAccessor">The accessor for the builder.</param>
    protected SutBase(SutBuilderBase.Accessor builderAccessor)
    {
        BuilderAccessor = builderAccessor;
    }

    /// <summary>
    /// Gets the default mock behavior for the <see cref="Mocks"/> repository.
    /// </summary>
    public virtual MockBehavior DefaultMockBehavior { get; } = MockBehavior.Strict;

    /// <summary>
    /// Gets the <see cref="MockRepository"/> for this instance.
    /// </summary>
    public MockRepository Mocks => _mocks ??= new MockRepository(DefaultMockBehavior);

    /// <summary>
    /// Gets the accessor for the builder that built this sut.
    /// </summary>
    public SutBuilderBase.Accessor BuilderAccessor { get; }
}
