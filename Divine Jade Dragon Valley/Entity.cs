using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public abstract class Entity
    {
        public string Name;
        public float X;
        public float Y;
        public float Width;
        public float Height;

        /// <summary>
        /// Move entity on X axis
        /// </summary>
        /// <param name="delta"></param>
        /// <returns>true if move was allowed, false otherwise</returns>
        public bool MoveX(float delta, byte[,] collisionMap)
        {
            if (collisionMap != null)
            {
                //Check in the direction of the character's movement in one tile
                if (!CheckCollisionX(delta, collisionMap, Y))
                {
                    return false;
                }
                if (!CheckCollisionX(delta, collisionMap, Y + Height - .0001f))
                {
                    //TODO: Check every tile size within Height
                    return false;
                }
            }

            X += delta;
            AfterMove();
            return true;
        }

        /// <summary>
        /// Move entity on Y axis
        /// </summary>
        /// <param name="delta"></param>
        /// <returns>true if move was allowed, false otherwise</returns>
        public bool MoveY(float delta, byte[,] collisionMap)
        {
            if (collisionMap != null)
            {
                //Check in the direction of the character's movement in one tile
                if (!CheckCollisionY(delta, collisionMap, X))
                {
                    return false;
                }
                if (!CheckCollisionY(delta, collisionMap, X + Width - .0001f))
                {
                    //TODO: Check every tile size within Width
                    return false;
                }
            }

            Y += delta;
            AfterMove();
            return true;
        }

        protected void AfterMove()
        {
            //Check every event that has a condition based on the location of the player
            var eventContext = new EventContext();
            var conditionContext = new ConditionContext { Caster = this as Character };
            GameProcessor.ExecuteEvents(typeof(ConditionCharacterInBox), eventContext, conditionContext);
            GameProcessor.ExecuteEvents(typeof(ConditionCharacterTouchingBox), eventContext, conditionContext);
        }

        protected bool CheckCollisionX(float delta, byte[,] collisionMap, float y)
        {
            if (delta < 0)
            {
                if (X + delta < 0 || collisionMap[(int)y, (int)(X + delta)] == 1)
                {
                    X = (int)X;
                    return false;
                }
            }
            else
            {
                if ((int)(X + delta + Width) >= collisionMap.GetLength(1))
                {
                    X = collisionMap.GetLength(1) - Width;
                    return false;
                }

                if (collisionMap[(int)y, (int)(X + delta + Width)] == 1)
                {
                    X = (int)(X + delta + Width) - Width;
                    return false;
                }
            }

            return true;
        }

        protected bool CheckCollisionY(float delta, byte[,] collisionMap, float x)
        {
            if (delta < 0)
            {
                if (Y + delta < 0 || collisionMap[(int)(Y + delta), (int)x] == 1)
                {
                    Y = (int)Y;
                    return false;
                }
            }
            else
            {
                if ((int)(Y + delta + Height) >= collisionMap.GetLength(0))
                {
                    Y = collisionMap.GetLength(0) - Height;
                    return false;
                }

                if (collisionMap[(int)(Y + delta + Height), (int)x] == 1)
                {
                    Y = (int)(Y + delta + Height) - Height;
                    return false;
                }
            }

            return true;
        }
    }
}
