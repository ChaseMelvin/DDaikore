using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class Reward : GameEvent
    {
        public readonly ItemBlueprint ItemToGrant;
        public readonly Dictionary<string, double> MaxStatsToGrant = new();
        public readonly Dictionary<string, double> StatsToGrant = new();
    }
}
