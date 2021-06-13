using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class ConditionAdder : Condition<double>
    {
        public double Evaluate(ConditionContext context)
        {
            return Children.Sum(p => p.Evaluate(context));
        }
        public List<Condition<double>> Children;
    }
}
