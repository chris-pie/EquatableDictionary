using DictionaryValueEqualityComparer;
namespace Tests
{
    [TestFixture]
    public class NestedDictionaryDefaultTests
    {
        private DictionaryValueEqualityComparer<int, Dictionary<string, Dictionary<char, int>>> comparer;

        [SetUp]
        public void Setup()
        {
            comparer = DictionaryValueEqualityComparer<int, Dictionary<string, Dictionary<char, int>>>.GetNestedComparer(null);
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void Equals_TriplyNestedDictionaries_EqualDictionaries_ReturnsTrue()
        {
            var dict1 = new Dictionary<int, Dictionary<string, Dictionary<char, int>>>
            {
                {
                    1, new Dictionary<string, Dictionary<char, int>>
                    {
                        { "abc", new Dictionary<char, int> { { 'x', 10 }, { 'y', 20 } } },
                        { "def", new Dictionary<char, int> { { 'a', 30 }, { 'b', 40 } } }
                    }
                },
                {
                    2, new Dictionary<string, Dictionary<char, int>>
                    {
                        { "ghi", new Dictionary<char, int> { { 'm', 50 }, { 'n', 60 } } }
                    }
                }
            };

            var dict2 = new Dictionary<int, Dictionary<string, Dictionary<char, int>>>
            {
                {
                    1, new Dictionary<string, Dictionary<char, int>>
                    {
                        { "abc", new Dictionary<char, int> { { 'x', 10 }, { 'y', 20 } } },
                        { "def", new Dictionary<char, int> { { 'a', 30 }, { 'b', 40 } } }
                    }
                },
                {
                    2, new Dictionary<string, Dictionary<char, int>>
                    {
                        { "ghi", new Dictionary<char, int> { { 'm', 50 }, { 'n', 60 } } }
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
                    1, new Dictionary<string, Dictionary<char, int>>
                    {
                        { "abc", new Dictionary<char, int> { { 'x', 10 }, { 'y', 20 } } },
                        { "def", new Dictionary<char, int> { { 'a', 30 }, { 'b', 40 } } }
                    }
                },
                {
                    2, new Dictionary<string, Dictionary<char, int>>
                    {
                        { "ghi", new Dictionary<char, int> { { 'm', 50 }, { 'n', 60 } } }
                    }
                }
            };

            var dict2 = new Dictionary<int, Dictionary<string, Dictionary<char, int>>>
            {
                {
                    1, new Dictionary<string, Dictionary<char, int>>
                    {
                        { "abc", new Dictionary<char, int> { { 'x', 10 }, { 'y', 20 } } },
                        { "def", new Dictionary<char, int> { { 'a', 30 }, { 'b', 41 } } } // Different value for key 'b'
                    }
                },
                {
                    2, new Dictionary<string, Dictionary<char, int>>
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
