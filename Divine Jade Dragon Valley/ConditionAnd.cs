using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class ConditionAnd : Condition<bool>
    {
        public double Evaluate(ConditionContext context)
        {
            return Children.All(p => p.Evaluate(context));
        }
        public List<Condition<bool>> Children;
    }
    
    //TODO: Other operators could be... Or, Not... Multiply, Divide, LessThan, GreaterThan, Equal, Negate
}
