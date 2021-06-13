using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Divine_Jade_Dragon_Valley.GameConstants;

namespace Divine_Jade_Dragon_Valley
{
    public class Character : Entity
    {
        public readonly Dictionary<string, double> MaxStats = new();
        public readonly Dictionary<string, double> CurrentStats = new();
        public readonly List<Item> Items = new();

        #region Getter methods that are like using Reflection
        public Dictionary<string, double> GetStats(StatsField field)
        {
            if (field == StatsField.Current) return CurrentStats;
            else if (field == StatsField.Max) return MaxStats;
            else throw new NotImplementedException($"Invalid {nameof(StatsField)} value");
        }
        #endregion
    }
}
