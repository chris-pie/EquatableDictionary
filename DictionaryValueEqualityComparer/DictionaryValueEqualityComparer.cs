using System.Collections;

namespace DictionaryValueEqualityComparer;

public class DictionaryValueEqualityComparer<TKey, TValue> : EqualityComparer<IDictionary<TKey, TValue>>
{
    private IEqualityComparer _keyComparer = EqualityComparer<TKey>.Default;
    private IEqualityComparer _valueComparer = EqualityComparer<TValue>.Default;

    private IEqualityComparer<TKey> _genericKeyComparer;
    private IEqualityComparer<TValue> _genericValueComparer;

    public IEqualityComparer KeyComparer
    {
        get => _keyComparer;
        set
        {
            _keyComparer = value;
            _genericKeyComparer = new GenericComparer<TKey>(KeyComparer);
        }
    }

    public IEqualityComparer ValueComparer
    {
        get => _valueComparer;
        set
        {
            _valueComparer = value;
            _genericValueComparer = new GenericComparer<TValue>(_valueComparer);
        }
    }

    private static IEqualityComparer DetermineComparer(Type type, IDictionary<Type, IEqualityComparer>? comparers = null)
    {
        if (comparers?.TryGetValue(type, out var comparer) ?? false) return comparer;
        var isDict = Array.Exists(type.GetInterfaces(),
            i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        if (isDict)
        {
            var types = type.GetGenericArguments();
            var innerDictComparer = typeof(DictionaryValueEqualityComparer<,>).MakeGenericType(types);
            return (IEqualityComparer)innerDictComparer.GetMethod("GetNestedComparer")!.Invoke(null, [comparers])!;
        }

        var comparerType = typeof(EqualityComparer<>).MakeGenericType([type]);
        return (IEqualityComparer)comparerType.GetProperty("Default")!.GetMethod!.Invoke(null, null)!;
    }

    public static DictionaryValueEqualityComparer<TKey, TValue> GetNestedComparer(
        IDictionary<Type, IEqualityComparer>? comparers = null)
    {
        var keyComparer = DetermineComparer(typeof(TKey), comparers);
        var valueComparer = DetermineComparer(typeof(TValue), comparers);

        return new DictionaryValueEqualityComparer<TKey, TValue>
        {
            KeyComparer = keyComparer,
            ValueComparer = valueComparer
        };
    }

    public override bool Equals(IDictionary<TKey, TValue>? dict1, IDictionary<TKey, TValue>? dict2)
    {
        if (dict1 is null && dict2 is null)
            return true;
        if (dict1 is null || dict2 is null)
            return false;

        if (dict1.Keys.Count != dict2.Keys.Count)
            return false;


        if (!dict1.Keys.ToHashSet(_genericKeyComparer).SetEquals(dict2.Keys))
            return false;

        return dict1.All(kvp => ValueComparer.Equals(kvp.Value, dict2[kvp.Key]));
    }

    public override int GetHashCode(IDictionary<TKey, TValue> obj)
    {
        var hashcode = new HashCode();
        foreach (var key in obj.Keys.OrderByDescending(x => KeyComparer.GetHashCode(x)))
        {
            hashcode.Add(key, _genericKeyComparer);
            hashcode.Add(obj[key], _genericValueComparer);
        }

        return hashcode.ToHashCode();
    }
}

internal class GenericComparer<T>(IEqualityComparer comparer) : IEqualityComparer<T>
{
    public bool Equals(T? x, T? y)
    {
        return comparer.Equals(x, y);
    }

    public int GetHashCode(T obj)
    {
        return comparer.GetHashCode(obj);
    }
}