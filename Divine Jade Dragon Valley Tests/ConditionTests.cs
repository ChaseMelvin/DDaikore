using Microsoft.VisualStudio.TestTools.UnitTesting;
using Divine_Jade_Dragon_Valley;

namespace Divine_Jade_Dragon_Valley_Tests
{
    [TestClass]
    public class ConditionTests
    {
        [TestMethod]
        public void TestSmallTree()
        {
            var baseCondition = new ConditionAnd();
            baseCondition.Children.Add(new ConditionLiteralBoolean() { Value = true });
            baseCondition.Children.Add(new ConditionNot() { Child = new ConditionLiteralBoolean() { Value = false } });
            Assert.IsTrue(baseCondition.Evaluate(null));
        }
    }
}
