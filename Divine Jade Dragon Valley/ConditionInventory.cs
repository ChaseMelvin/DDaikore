using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class ConditionInventory : Condition, ICondition<double>
    {
        public readonly ConditionContext.CharacterField ItemOwnerType;
        public readonly string ItemName; //TODO: Could have various checks built into one class, like the item type (e.g. "Ring" or "Universe Bag") or tier (which would be a stat) or whatever
        public double Evaluate(ConditionContext context)
        {
            var character = context.GetCharacter(ItemOwnerType);
            return character.Items.Count(p => p.Name == ItemName);
        }

        protected override int GetMaxChildren() => 0;
    }

    //TODO: Other condition operands could include lots of things like "character within polygon" and "character is on the ground"
}
