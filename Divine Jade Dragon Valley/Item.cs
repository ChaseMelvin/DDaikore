using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class Item : Entity
    {
        public readonly Dictionary<string, double> MaxStats = new();
        public readonly Dictionary<string, double> CurrentStats = new();
        public Character Owner;
    }
}
