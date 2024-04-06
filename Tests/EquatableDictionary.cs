using System.Collections;
using EquatableDictionary;

namespace Tests;

[TestFixture]
public class EquatableDictionaryTest
{
    [Test]
    public void EquatableDictionary_ComparerProperty()
    {
        var comparers = new Dictionary<Type, IEqualityComparer>
        {
            { typeof(int), EqualityComparer<int>.Default },
            { typeof(string), StringComparer.OrdinalIgnoreCase }
        };


        var dict1 = new EquatableDictionary<int, string>(comparers)
        {
            { 1, "one" },
            { 2, "two" }
        };

        var dict2 = new EquatableDictionary<int, string>(comparers)
        {
            { 1, "one" },
            { 2, "TWO" }
        };

        Assert.IsTrue(dict1.Equals(dict2));
        Assert.IsTrue(dict1.Equals(dict2 as object));
        Assert.That(dict2.GetHashCode(), Is.EqualTo(dict1.GetHashCode()));
    }
}