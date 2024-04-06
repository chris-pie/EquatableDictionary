using System.Collections;

namespace EquatableDictionary;
using DictionaryValueEqualityComparer;
public class EquatableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IEquatable<EquatableDictionary<TKey, TValue>>
{
    public EquatableDictionary(IDictionary<Type, IEqualityComparer>? comparers = null)
    {
        Comparers = comparers;
    }

    public EquatableDictionary(IDictionary<TKey, TValue> dictionary, IDictionary<Type, IEqualityComparer>? comparers  = null)  : base(dictionary)
    {
        Comparers = comparers;
    }

    public EquatableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer, IDictionary<Type, IEqualityComparer>? comparers  = null) : base(dictionary, comparer)
    {
        Comparers = comparers;
    }

    public EquatableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IDictionary<Type, IEqualityComparer>? comparers  = null) : base(collection)
    {
        Comparers = comparers;
    }

    public EquatableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer, IDictionary<Type, IEqualityComparer>? comparers  = null) : base(collection, comparer)
    {
        Comparers = comparers;
    }

    public EquatableDictionary(IEqualityComparer<TKey>? comparer, IDictionary<Type, IEqualityComparer>? comparers  = null) : base(comparer)
    {
        Comparers = comparers;
    }

    public EquatableDictionary(int capacity, IDictionary<Type, IEqualityComparer>? comparers = null) : base(capacity)
    {
        Comparers = comparers;
    }

    public EquatableDictionary(int capacity, IEqualityComparer<TKey>? comparer, IDictionary<Type, IEqualityComparer>? comparers = null) : base(capacity, comparer)
    {
        Comparers = comparers;
    }

    public IDictionary<Type, IEqualityComparer>? Comparers { get; }
    public bool Equals(EquatableDictionary<TKey, TValue>? other)
    {
        return DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(Comparers).Equals(this, other);
    }
}