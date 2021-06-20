using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class ItemBlueprint
    {
        public ICondition<bool> SpawnConditions;
        public readonly Dictionary<string, double> StatRandomMinima = new();
        public readonly Dictionary<string, double> StatRandomMaxima = new();
        public readonly Dictionary<string, double> StatAdditionChances = new();
        //TODO: Maybe the word generator needs a full-on configuration class because some words can't be combined and many should be based on the actual stats.
        public readonly List<string> PossibleNames = new();
        public readonly List<string> PossibleFinalDescriptors = new();
        public readonly List<string> PossibleSecondaryDescriptors = new();
        public readonly List<string> PossibleTertiaryDescriptors = new();
    }
}
