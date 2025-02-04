using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    /// <summary>
    /// Probably will never be used. Stores a literal boolean as part of a condition tree.
    /// </summary>
    public class ConditionLiteralBoolean : Condition, ICondition<bool>
    {
        public bool Value;
        public bool Evaluate(ConditionContext context) => Value;
        protected override int GetMaxChildren() => 0;
    }

    public class ConditionLiteralDouble : Condition, ICondition<double>
    {
        public double Value;
        public double Evaluate(ConditionContext context) => Value;
        protected override int GetMaxChildren() => 0;
    }

    /// <summary>
    /// Get the value of a specific stat from the current condition context
    /// </summary>
    public class ConditionStatValue : Condition, ICondition<double>
    {
        /// <summary>
        /// If it's a battle or something, this could be Caster or Target or whatever
        /// </summary>
        public readonly ConditionContext.CharacterField StatsOwnerType;
        public readonly GameConstants.StatsField StatsFieldType;
        public readonly string StatName;
        public readonly double DefaultValue;
        public double Evaluate(ConditionContext context)
        {
            var character = context.GetCharacter(StatsOwnerType);
            if (!character.GetStats(StatsFieldType).TryGetValue(StatName, out var val)) return DefaultValue;
            return val;
        }

        protected override int GetMaxChildren() => 0;
    }
}
