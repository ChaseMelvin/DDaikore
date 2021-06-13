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
    public class ConditionAdd : Condition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return Children.Sum(p => p.Evaluate(context));
        }
        public List<Condition<double>> Children = new();
    }

    public class ConditionMultiply : Condition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return Children.Select(p => p.Evaluate(context)).Aggregate(1.0, (cur, product) => cur * product);
        }
        public List<Condition<double>> Children = new();
    }

    public class ConditionDivide : Condition<double>
    {
        public double DefaultIfDividingByZero = double.MaxValue;
        public double Evaluate(ConditionContext context)
        {
            var divisor = Divisor.Evaluate(context);
            if (divisor == 0) return DefaultIfDividingByZero;
            return Dividend.Evaluate(context) / divisor;
        }

        /// <summary>
        /// The number to divide. (The numerator of the fraction.)
        /// </summary>
        public Condition<double> Dividend;
        /// <summary>
        /// The number to divide by. (The denominator of the fraction.)
        /// </summary>
        public Condition<double> Divisor;
    }

    public class ConditionLessThan : Condition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return First.Evaluate(context) < Second.Evaluate(context);
        }

        public Condition<double> First;
        public Condition<double> Second;
    }

    public class ConditionGreaterThan : Condition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return First.Evaluate(context) > Second.Evaluate(context);
        }

        public Condition<double> First;
        public Condition<double> Second;
    }

    public class ConditionEqual : Condition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return First.Evaluate(context) == Second.Evaluate(context);
        }

        public Condition<double> First;
        public Condition<double> Second;
    }

    public class ConditionNegate : Condition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return -Child.Evaluate(context);
        }

        public Condition<double> Child;
    }

    /// <summary>
    /// Get the least value
    /// </summary>
    public class ConditionMin : Condition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return Children.Min(p => p.Evaluate(context));
        }
        public List<Condition<double>> Children = new();
    }

    /// <summary>
    /// Get the greatest value
    /// </summary>
    public class ConditionMax : Condition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return Children.Max(p => p.Evaluate(context));
        }
        public List<Condition<double>> Children = new();
    }
}
