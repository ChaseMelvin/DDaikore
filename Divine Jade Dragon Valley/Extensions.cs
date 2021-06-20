using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public static class Extensions
    {
        /// <summary>
        /// Fisher-Yates shuffle. (Unbiased list order randomization.)
        /// </summary>
        public static T[] Randomize<T>(this Random rnd, IEnumerable<T> items)
        {
            var asArray = items.ToArray();
            for (var i = 0; i < asArray.Length - 1; i++) {
                var j = rnd.Next(i, asArray.Length);
                var temp = asArray[i];
                asArray[i] = asArray[j];
                asArray[j] = temp;
            }
            return asArray;
        }

    }
}
