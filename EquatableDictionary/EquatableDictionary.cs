using System.Collections;
using DictionaryValueEqualityComparer;

namespace EquatableDictionary;

public class EquatableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IEquatable<EquatableDictionary<TKey, TValue>>
    where TKey : notnull
{
    public EquatableDictionary(IDictionary<Type, IEqualityComparer>? comparers = null)
    {
        ValueComparer = DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(comparers);
    }

    public EquatableDictionary(IDictionary<TKey, TValue> dictionary,
        IDictionary<Type, IEqualityComparer>? comparers = null) : base(dictionary)
    {
        ValueComparer = DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(comparers);
    }

    public EquatableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer,
        IDictionary<Type, IEqualityComparer>? comparers = null) : base(dictionary, comparer)
    {
        ValueComparer = DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(comparers);
    }

    public EquatableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection,
        IDictionary<Type, IEqualityComparer>? comparers = null) : base(collection)
    {
        ValueComparer = DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(comparers);
    }

    public EquatableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer,
        IDictionary<Type, IEqualityComparer>? comparers = null) : base(collection, comparer)
    {
        ValueComparer = DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(comparers);
    }

    public EquatableDictionary(IEqualityComparer<TKey>? comparer,
        IDictionary<Type, IEqualityComparer>? comparers = null) : base(comparer)
    {
        ValueComparer = DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(comparers);
    }

    public EquatableDictionary(int capacity, IDictionary<Type, IEqualityComparer>? comparers = null) : base(capacity)
    {
        ValueComparer = DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(comparers);
    }

    public EquatableDictionary(int capacity, IEqualityComparer<TKey>? comparer,
        IDictionary<Type, IEqualityComparer>? comparers = null) : base(capacity, comparer)
    {
        ValueComparer = DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(comparers);
    }

    private DictionaryValueEqualityComparer<TKey, TValue> ValueComparer { get; }

    public bool Equals(EquatableDictionary<TKey, TValue>? other)
    {
        return ValueComparer.Equals(this, other);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as EquatableDictionary<TKey, TValue>);
    }

    public override int GetHashCode()
    {
        return ValueComparer.GetHashCode(this);
    }
}