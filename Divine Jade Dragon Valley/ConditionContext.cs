using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class ConditionContext
    {
        public Battle CasterInBattle;
        public Dialog CurrentDialog;
        public int LastDialogChoiceIndex; //TODO: Might be better to have internal-use names for choices
        public Area CasterInArea;
        public Character Caster;
        public Character Target;
        //TODO: Could have lots of other fields. Check out how StarCraft II does it via Galaxy Editor, and do it better! Could also have priorities--e.g. you want Effect A to prevent a kill-shot by blocking damage, but Effect B blocks damage over a certain amount, so you need to check Effect B's conditions first

        #region Enums and getter methods that are like using Reflection
        public enum CharacterField
        {
            Caster, Target
        }

        public Character GetCharacter(CharacterField field)
        {
            if (field == CharacterField.Caster) return Caster;
            else if (field == CharacterField.Target) return Target;
            else throw new NotImplementedException($"Invalid {nameof(CharacterField)} value");
        }
        #endregion
    }
}
