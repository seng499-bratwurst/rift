namespace Rift.Tests;

[TestClass]
public sealed class Test1
{
    // Sample unit test
    [TestMethod]
    public void AddingTwoNumbers_ReturnsCorrectSum()
    {
        int a = 2;
        int b = 3;
        int sum = a + b;

        Assert.AreEqual(5, sum);
    }

}
