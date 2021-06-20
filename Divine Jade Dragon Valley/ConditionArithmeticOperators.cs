using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    /// <summary>
    /// Add all the child values. (For subtraction, use Negate with Add)
    /// </summary>
    public class ConditionAdd : Condition, ICondition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return DoubleChildren().Sum(p => p.Evaluate(context));
        }
    }

    public class ConditionMultiply : Condition, ICondition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return DoubleChildren().Select(p => p.Evaluate(context)).Aggregate(1.0, (cur, product) => cur * product);
        }
    }

    public class ConditionDivide : Condition, ICondition<double>
    {
        public double DefaultIfDividingByZero = double.MaxValue;
        public double Evaluate(ConditionContext context)
        {
            var divisor = DoubleChildren().First().Evaluate(context);
            if (divisor == 0) return DefaultIfDividingByZero;
            return DoubleChildren().Last().Evaluate(context) / divisor;
        }

        protected override int GetMaxChildren() => 2;
    }

    public class ConditionLessThan : Condition, ICondition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return DoubleChildren().First().Evaluate(context) < DoubleChildren().Last().Evaluate(context);
        }

        protected override int GetMaxChildren() => 2;
    }

    public class ConditionGreaterThan : Condition, ICondition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return DoubleChildren().First().Evaluate(context) > DoubleChildren().Last().Evaluate(context);
        }

        protected override int GetMaxChildren() => 2;
    }

    public class ConditionEqual : Condition, ICondition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            var value = DoubleChildren().First().Evaluate(context);
            return DoubleChildren().Skip(1).All(p => p.Evaluate(context) == value);
        }
    }

    public class ConditionNegate : Condition, ICondition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return -DoubleChildren().First().Evaluate(context);
        }

        protected override int GetMaxChildren() => 1;
    }

    /// <summary>
    /// Get the least value
    /// </summary>
    public class ConditionMin : Condition, ICondition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return DoubleChildren().Min(p => p.Evaluate(context));
        }
    }

    /// <summary>
    /// Get the greatest value
    /// </summary>
    public class ConditionMax : Condition, ICondition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return DoubleChildren().Max(p => p.Evaluate(context));
        }
    }
}
