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

        [TestMethod]
        public void ArithmeticTreeTest()
        {
            var baseCondition = new ConditionLessThan();
            baseCondition.First = new ConditionLiteralDouble() { Value = .3 };
            baseCondition.Second = new ConditionMax();
            ((ConditionMax)baseCondition.Second).Children.Add(new ConditionLiteralDouble() { Value = .5 });
            ((ConditionMax)baseCondition.Second).Children.Add(new ConditionLiteralDouble() { Value = .6 });
            ((ConditionMax)baseCondition.Second).Children.Add(new ConditionLiteralDouble() { Value = .8 });
            Assert.IsTrue(baseCondition.Evaluate(null));
        }

        [TestMethod]
        public void MaxTest()
        {
            var baseCondition = new ConditionMax();
            baseCondition.Children.Add(new ConditionLiteralDouble() { Value = .6 });
            baseCondition.Children.Add(new ConditionLiteralDouble() { Value = .8 });
            baseCondition.Children.Add(new ConditionLiteralDouble() { Value = .5 });
            Assert.AreEqual(baseCondition.Evaluate(null), .8);
        }
    }
}
