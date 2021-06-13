using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class ConditionAnd : Condition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return Children.All(p => p.Evaluate(context));
        }
        public List<Condition<bool>> Children = new();
    }

    public class ConditionOr : Condition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return Children.Any(p => p.Evaluate(context));
        }
        public List<Condition<bool>> Children = new();
    }

    public class ConditionNot : Condition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return !Child.Evaluate(context);
        }
        public Condition<bool> Child;
    }
}
