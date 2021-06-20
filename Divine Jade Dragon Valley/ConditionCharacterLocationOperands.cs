using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Divine_Jade_Dragon_Valley
{
    public abstract class ConditionLocationCheck : Condition, ICondition<bool>
    {
        public ConditionContext.CharacterField WhichCharacter;
        protected List<PointF> Points = new();
        public bool Evaluate(ConditionContext context)
        {
            var character = context.GetCharacter(WhichCharacter);
            if (character == null)
                return false;

            return EvaluatePosition(character, context);
        }

        public void AddPoints(params PointF[] points)
        {
            foreach (var point in points)
            {
                if (Points.Count == GetMaxPoints()) throw new InvalidOperationException("The location condition has too many points.");
                Points.Add(point);
            }
        }

        public void OffsetPoints(PointF offset)
        {
            Points = Points.ConvertAll(p => new PointF(p.X + offset.X, p.Y + offset.Y));
        }

        public abstract bool EvaluatePosition(Character character, ConditionContext context);
        protected override int GetMaxChildren() => 0;
        protected abstract int GetMaxPoints();
    }

    /// <summary>
    /// The character rectangle is COMPLETELY contained within the given rectangle. Points should be the top-left and bottom-right corners of the box.
    /// </summary>
    public class ConditionCharacterInBox : ConditionLocationCheck
    {
        public override bool EvaluatePosition(Character character, ConditionContext context) //TODO: When you use this, you need some kind of condition to ensure it only triggers when THAT SPECIFIC character moves (especially if it's set to "CurrentPlayer") and only once a frame.
        {
            return character.X >= Points[0].X && character.X + character.Width <= Points[1].X
                && character.Y >= Points[0].Y && character.Y + character.Height <= Points[1].Y;
        }

        protected override int GetMaxPoints() => 2;
    }

    /// <summary>
    /// The character rectangle has at least SOME intersection with the given rectangle. Points should be the top-left and bottom-right corners of the box.
    /// </summary>
    public class ConditionCharacterTouchingBox : ConditionLocationCheck
    {
        public override bool EvaluatePosition(Character character, ConditionContext context)
        {
            return character.X + character.Width >= Points[0].X && character.X <= Points[1].X
                && character.Y + character.Height >= Points[0].Y && character.Y <= Points[1].Y;
        }

        protected override int GetMaxPoints() => 2;
    }
}
