using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class ConditionStatValue : Condition<double>
    {
        /// <summary>
        /// If it's a battle or something, this could be Caster or Target or whatever
        /// </summary>
        public readonly ContextInfo.CharacterField StatOwnerType;
        public readonly string StatName;
        public readonly double DefaultValue;
        public double Evaluate(ConditionContext context)
        {
            var character = context.GetCharacter(StatOwnerType);
            if (!character.Stats.TryGetValue(StatName, out var val)) return DefaultValue;
            return val;
        }
    }
}
