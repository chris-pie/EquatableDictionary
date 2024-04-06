using System.Collections;
using DictionaryValueEqualityComparer;

namespace DictionaryValueEquals;

public static class DictionaryExtensions
{
    public static bool ValueEquals<TKey, TValue>(this Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2,
        IDictionary<Type, IEqualityComparer>? comparers = null) where TKey : notnull
    {
        return DictionaryValueEqualityComparer<TKey, TValue>.GetNestedComparer(comparers).Equals(dict1, dict2);
    }
}