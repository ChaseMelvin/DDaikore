using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class ConditionAnd : Condition, ICondition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return BoolChildren().All(p => p.Evaluate(context));
        }

        protected override Type GetAllowedChildType() => typeof(ICondition<bool>);
    }

    public class ConditionOr : Condition, ICondition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return BoolChildren().Any(p => p.Evaluate(context));
        }

        protected override Type GetAllowedChildType() => typeof(ICondition<bool>);
    }

    public class ConditionNot : Condition, ICondition<bool>
    {
        public bool Evaluate(ConditionContext context)
        {
            return !BoolChildren().First().Evaluate(context);
        }

        protected override Type GetAllowedChildType() => typeof(ICondition<bool>);
        protected override int GetMaxChildren() => 1;
    }
}
