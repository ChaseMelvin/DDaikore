using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public interface Condition<T>
    {
        public T Evaluate(ConditionContext context);
        public List<Condition<object>> FlattenTree();
    }
}
