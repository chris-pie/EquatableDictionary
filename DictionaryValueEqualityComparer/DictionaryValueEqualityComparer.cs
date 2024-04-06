using System.Collections;

namespace DictionaryValueEqualityComparer
{
    /// <summary>
    /// Provides an <see cref="EqualityComparer{T}"/> implementation for comparing <see cref="IDictionary{TKey,TValue}"/> instances based on key and value equality comparers.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionaries to be compared.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionaries to be compared.</typeparam>
    public class DictionaryValueEqualityComparer<TKey, TValue> : EqualityComparer<IDictionary<TKey, TValue>>
    {
        private IEqualityComparer<TKey> _genericKeyComparer;
        private IEqualityComparer<TValue> _genericValueComparer;
        private IEqualityComparer _keyComparer = EqualityComparer<TKey>.Default;
        private IEqualityComparer _valueComparer = EqualityComparer<TValue>.Default;

        /// <summary>
        /// Gets or sets the equality comparer used for comparing keys.
        /// </summary>
        public IEqualityComparer KeyComparer
        {
            get => _keyComparer;
            set
            {
                _keyComparer = value;
                _genericKeyComparer = new GenericComparer<TKey>(KeyComparer);
            }
        }

        /// <summary>
        /// Gets or sets the equality comparer used for comparing values.
        /// </summary>
        public IEqualityComparer ValueComparer
        {
            get => _valueComparer;
            set
            {
                _valueComparer = value;
                _genericValueComparer = new GenericComparer<TValue>(_valueComparer);
            }
        }

        /// <summary>
        /// Determines the appropriate comparer for the specified type, based on provided comparers or default comparers.
        /// </summary>
        private static IEqualityComparer DetermineComparer(Type type,
            IDictionary<Type, IEqualityComparer>? comparers = null)
        {
            if (comparers?.TryGetValue(type, out var comparer) ?? false) return comparer;
            var isDict = Array.Exists(type.GetInterfaces(),
                i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
            if (isDict)
            {
                var types = type.GetGenericArguments();
                var innerDictComparer = typeof(DictionaryValueEqualityComparer<,>).MakeGenericType(types);
                return (IEqualityComparer)innerDictComparer.GetMethod("GetNestedComparer")!.Invoke(null,
                    [comparers])!;
            }

            var comparerType = typeof(EqualityComparer<>).MakeGenericType([type]);
            return (IEqualityComparer)comparerType.GetProperty("Default")!.GetMethod!.Invoke(null, null)!;
        }

        /// <summary>
        /// Gets a nested comparer for the specified dictionary type.
        /// </summary>
        /// <param name="comparers">Optional dictionary of custom type comparers.</param>
        /// <returns>An instance of <see cref="DictionaryValueEqualityComparer{TKey,TValue}"/> with appropriate key and value comparers.</returns>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
}