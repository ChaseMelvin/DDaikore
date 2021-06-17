using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class ConditionCharacterInBox : Condition<bool>
    {
        public ConditionContext.CharacterField WhichCharacter;
        public float X, Y, Width, Height;
        public bool Evaluate(ConditionContext context)
        {
            var character = context.GetCharacter(WhichCharacter);
            if (character == null)
                return false;

            return character.X >= X && character.X + character.Width <= X + Width
                && character.Y >= Y && character.Y + character.Height <= Y + Height;
        }
    }
}
