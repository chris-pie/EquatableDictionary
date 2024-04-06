using DictionaryValueEqualityComparer;
namespace Tests;

[TestFixture]
public class SimpleDictionaryTests
{
    private DictionaryValueEqualityComparer<int, string> comparer;


    [SetUp]
    public void Setup()
    {
        comparer = DictionaryValueEqualityComparer<int, string>.GetNestedComparer(null);
    }

    [TearDown]
    public void Teardown()
    {
    }

    [Test]
    public void Equals_EqualDictionaries_ReturnsTrue()
    {
        var dict1 = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
        var dict2 = new Dictionary<int, string> { { 2, "two" }, { 1, "one" } };
        bool result = comparer.Equals(dict1, dict2);
        Assert.IsTrue(result, "Equal dictionaries should return true");
    }

    [Test]
    public void Equals_DifferentDictionaries_ReturnsFalse()
    {
        var dict1 = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
        var dict3 = new Dictionary<int, string> { { 1, "one" }, { 2, "three" } };
        bool result = comparer.Equals(dict1, dict3);
        Assert.IsFalse(result, "Dictionaries with different values should return false");
    }

    [Test]
    public void Equals_DifferentLengthDictionaries_ReturnsFalse()
    {
        var dict1 = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
        var dict3 = new Dictionary<int, string> { { 1, "one" } };
        bool result = comparer.Equals(dict1, dict3);
        Assert.IsFalse(result, "Dictionaries with different values should return false");
    }    
    [Test]
    public void Equals_DifferentKeysDictionaries_ReturnsFalse()
    {
        var dict1 = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
        var dict3 = new Dictionary<int, string> { { 1, "one" }, { 3, "two" }};
        bool result = comparer.Equals(dict1, dict3);
        Assert.IsFalse(result, "Dictionaries with different values should return false");
    }
    [Test]
    public void Equals_Nulls()
    {
        var dict1 = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
        bool resultNulls = comparer.Equals(null, null);
        bool resultFirstNull = comparer.Equals(null, dict1);
        bool resultSecondNull = comparer.Equals(dict1, null);

        Assert.Multiple(() =>
        {
            Assert.IsTrue(resultNulls, "Two null objects should return true");
            Assert.IsFalse(resultFirstNull, "One null object should return false");
            Assert.IsFalse(resultSecondNull, "One null object should return false");
        });
    }

}
