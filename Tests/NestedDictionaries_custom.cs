using System.Collections;
using DictionaryValueEqualityComparer;
namespace Tests
{
    [TestFixture]
    public class NestedDictionaryCustomTests
    {
        private DictionaryValueEqualityComparer<int, Dictionary<string, Dictionary<char, int>>> comparer;

        [SetUp]
        public void Setup()
        {
            var customComparers = new Dictionary<Type, IEqualityComparer>
            {
                { typeof(string), StringComparer.OrdinalIgnoreCase }
            };
            comparer = DictionaryValueEqualityComparer<int, Dictionary<string, Dictionary<char, int>>>.GetNestedComparer(customComparers);
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void Equals_TriplyNestedDictionaries_EqualDictionaries_ReturnsTrue()
        {
            // Arrange
            var dict1 = new Dictionary<int, Dictionary<string, Dictionary<char, int>>>
            {
                {
                    1, new Dictionary<string, Dictionary<char, int>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "abc", new Dictionary<char, int> { { 'x', 10 }, { 'y', 20 } } },
                        { "def", new Dictionary<char, int> { { 'a', 30 }, { 'b', 40 } } }
                    }
                },
                {
                    2, new Dictionary<string, Dictionary<char, int>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "ghi", new Dictionary<char, int> { { 'm', 50 }, { 'n', 60 } } }
                    }
                }
            };

            var dict2 = new Dictionary<int, Dictionary<string, Dictionary<char, int>>>
            {
                {
                    1, new Dictionary<string, Dictionary<char, int>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "aBc", new Dictionary<char, int> { { 'x', 10 }, { 'y', 20 } } },
                        { "DeF", new Dictionary<char, int> { { 'a', 30 }, { 'b', 40 } } }
                    }
                },
                {
                    2, new Dictionary<string, Dictionary<char, int>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "GHI", new Dictionary<char, int> { { 'm', 50 }, { 'n', 60 } } }
                    }
                }
            };

            bool result = comparer.Equals(dict1, dict2);

            Assert.IsTrue(result, "Equal triply nested dictionaries should return true");
        }

        [Test]
        public void Equals_TriplyNestedDictionaries_DifferentDictionaries_ReturnsFalse()
        {
            var dict1 = new Dictionary<int, Dictionary<string, Dictionary<char, int>>>
            {
                {
                    1, new Dictionary<string, Dictionary<char, int>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "aBc", new Dictionary<char, int> { { 'x', 10 }, { 'y', 20 } } },
                        { "deF", new Dictionary<char, int> { { 'a', 30 }, { 'b', 40 } } }
                    }
                },
                {
                    2, new Dictionary<string, Dictionary<char, int>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "GHI", new Dictionary<char, int> { { 'm', 50 }, { 'n', 60 } } }
                    }
                }
            };

            var dict2 = new Dictionary<int, Dictionary<string, Dictionary<char, int>>>
            {
                {
                    1, new Dictionary<string, Dictionary<char, int>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "abc", new Dictionary<char, int> { { 'x', 10 }, { 'y', 20 } } },
                        { "def", new Dictionary<char, int> { { 'a', 30 }, { 'b', 41 } } } // Different value for key 'b'
                    }
                },
                {
                    2, new Dictionary<string, Dictionary<char, int>>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "ghi", new Dictionary<char, int> { { 'm', 50 }, { 'n', 60 } } }
                    }
                }
            };

            bool result = comparer.Equals(dict1, dict2);
            Assert.IsFalse(result, "Dictionaries with different values should return false");
        }
    }
}
