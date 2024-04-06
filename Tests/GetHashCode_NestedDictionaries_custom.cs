using System.Collections;
using DictionaryValueEqualityComparer;

namespace Tests;

[TestFixture]
public class NestedDictionaryHashcodeCustomTests
{
    [SetUp]
    public void Setup()
    {
        var customComparers = new Dictionary<Type, IEqualityComparer>
        {
            { typeof(string), StringComparer.OrdinalIgnoreCase }
        };
        comparer =
            DictionaryValueEqualityComparer<int, Dictionary<string, Dictionary<char, int>>>.GetNestedComparer(
                customComparers);
    }

    [TearDown]
    public void Teardown()
    {
    }

    private DictionaryValueEqualityComparer<int, Dictionary<string, Dictionary<char, int>>> comparer;

    [Test]
    public void GetHashCode_TriplyNestedDictionaries_EqualDictionaries_Equal()
    {
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


        Assert.That(comparer.GetHashCode(dict1), Is.EqualTo(comparer.GetHashCode(dict2)));
    }

    [Test]
    public void GetHashCode_TriplyNestedDictionaries_DifferentDictionaries_NotEqual()
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

        Assert.That(comparer.GetHashCode(dict1), Is.Not.EqualTo(comparer.GetHashCode(dict2)));
    }
}