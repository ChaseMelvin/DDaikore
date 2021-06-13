using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class FarmCrop : Entity
    {
        /// <summary>
        /// Probably just Effective Age (ripeness) and Quality (health)
        /// </summary>
        public readonly Dictionary<string, double> Stats = new();
    }
}
