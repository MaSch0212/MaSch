namespace MaSch.Core;

/// <summary>
/// Equatable equivilent to the <see cref="WeakReference"/> class.
/// </summary>
[SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Should already be handeled by the base class.")]
public class EquatableWeakReference : WeakReference
{
    private int _targetHashCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="EquatableWeakReference"/> class.
    /// </summary>
    /// <param name="target">The object to track or <see langword="null" />.</param>
    public EquatableWeakReference(object? target)
        : base(target)
    {
        OnTargetChanged(target);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EquatableWeakReference"/> class.
    /// </summary>
    /// <param name="target">An object to track.</param>
    /// <param name="trackResurrection">Indicates when to stop tracking the object. If <see langword="true" />, the object is tracked after finalization; if <see langword="false" />, the object is only tracked until finalization.</param>
    public EquatableWeakReference(object? target, bool trackResurrection)
        : base(target, trackResurrection)
    {
        OnTargetChanged(target);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EquatableWeakReference"/> class.
    /// </summary>
    /// <param name="info">An object that holds all the data needed to serialize or deserialize the current <see cref="T:System.WeakReference" /> object.</param>
    /// <param name="context">(Reserved) Describes the source and destination of the serialized stream specified by <paramref name="info" />.</param>
    protected EquatableWeakReference(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        OnTargetChanged(Target);
    }

    /// <inheritdoc/>
    public override object? Target
    {
        get => base.Target;
        set
        {
            base.Target = value;
            OnTargetChanged(value);
        }
    }

    /// <summary>
    /// Determines whether the two instance of type <see cref="EquatableWeakReference"/> are equal.
    /// </summary>
    /// <param name="x">The first instance.</param>
    /// <param name="y">The second instance.</param>
    /// <returns><c>true</c> when the two instances are equal; otherwise <c>false</c>.</returns>
    [SuppressMessage("Blocker Code Smell", "S3875:\"operator==\" should not be overloaded on reference types", Justification = "The caller would expect to compare the references to the target object.")]
    public static bool operator ==(EquatableWeakReference x, EquatableWeakReference y)
    {
        return Equals(x, y);
    }

    /// <summary>
    /// Determines whether the two instance of type <see cref="EquatableWeakReference"/> are not equal.
    /// </summary>
    /// <param name="x">The first instance.</param>
    /// <param name="y">The second instance.</param>
    /// <returns><c>true</c> when the two instances are not equal; otherwise <c>false</c>.</returns>
    public static bool operator !=(EquatableWeakReference x, EquatableWeakReference y)
    {
        return !Equals(x, y);
    }

    /// <inheritdoc/>
    [SuppressMessage("Minor Bug", "S2328:\"GetHashCode\" should not reference mutable fields", Justification = "Needed here")]
    public override int GetHashCode()
    {
        return _targetHashCode;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return _targetHashCode == (obj is EquatableWeakReference r ? r._targetHashCode : RuntimeHelpers.GetHashCode(obj));
    }

    /// <summary>
    /// Called when the <see cref="Target"/> property value changed.
    /// </summary>
    /// <param name="newTarget">The new target.</param>
    protected virtual void OnTargetChanged(object? newTarget)
    {
        _targetHashCode = RuntimeHelpers.GetHashCode(newTarget);
    }
}
