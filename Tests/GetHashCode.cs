using DictionaryValueEqualityComparer;

namespace Tests;

[TestFixture]
public class HashCodeTests
{
    [SetUp]
    public void Setup()
    {
        comparer = DictionaryValueEqualityComparer<int, string>.GetNestedComparer();
    }

    [TearDown]
    public void Teardown()
    {
    }

    private DictionaryValueEqualityComparer<int, string> comparer;

    [Test]
    public void GetHashCode_EqualDictionaries_Equal()
    {
        var dict1 = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
        var dict2 = new Dictionary<int, string> { { 2, "two" }, { 1, "one" } };
        Assert.That(comparer.GetHashCode(dict2), Is.EqualTo(comparer.GetHashCode(dict1)));
    }

    [Test]
    public void GetHashCode_DifferentDictionaries_NotEqual()
    {
        var dict1 = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
        var dict2 = new Dictionary<int, string> { { 2, "two" }, { 3, "one" } };
        Assert.That(comparer.GetHashCode(dict2), Is.Not.EqualTo(comparer.GetHashCode(dict1)));
    }
}