namespace MaSch.Core;

/// <summary>
/// Storage object that stores extension data for other objects.
/// </summary>
public class ObjectExtensionDataStorage
{
    private readonly Dictionary<object, Dictionary<string, object?>> _dataStore = new(new ObjectHashComparer());

    /// <summary>
    /// Gets the stored data from the specified object.
    /// </summary>
    /// <param name="dataObject">The object to get data from.</param>
    /// <returns>A dictionary containing all extension data stored for the <paramref name="dataObject"/>.</returns>
    public IDictionary<string, object?> this[object dataObject]
    {
        get
        {
            _ = Guard.NotNull(dataObject);
            if (_dataStore.TryGetValue(dataObject, out var data))
                return data;
            data = new();
            _dataStore.Add(new EquatableWeakReference(dataObject), data);
            return data;
        }
    }

    [ExcludeFromCodeCoverage]
    private sealed class ObjectHashComparer : IEqualityComparer<object>
    {
        public new bool Equals(object? x, object? y)
        {
            if (x is EquatableWeakReference r1)
                return r1.Equals(y);
            else if (y is EquatableWeakReference r2)
                return r2.Equals(x);
            else
                return object.Equals(x, y);
        }

        public int GetHashCode([DisallowNull] object obj)
        {
            return obj is EquatableWeakReference r ? r.GetHashCode() : obj.GetInitialHashCode();
        }
    }
}
