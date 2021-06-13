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
            return true;
        }

        protected bool CheckCollisionX(float delta, byte[,] collisionMap, float y)
        {
            if (delta < 0)
            {
                if (X + delta < 0 || collisionMap[(int)(X + delta), (int)(y)] == 1)
                {
                    X = (int)X;
                    return false;
                }
            }
            else
            {
                if ((int)(X + delta + Width) >= collisionMap.GetLength(0))
                {
                    X = collisionMap.GetLength(0) - Width;
                    return false;
                }

                if (collisionMap[(int)(X + delta + Width), (int)(y)] == 1)
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
                if (Y + delta < 0 || collisionMap[(int)(x), (int)(Y + delta)] == 1)
                {
                    Y = (int)Y;
                    return false;
                }
            }
            else
            {
                if ((int)(Y + delta + Height) >= collisionMap.GetLength(1))
                {
                    Y = collisionMap.GetLength(1) - Height;
                    return false;
                }

                if (collisionMap[(int)(x), (int)(Y + delta + Height)] == 1)
                {
                    Y = (int)(Y + delta + Height) - Height;
                    return false;
                }
            }

            return true;
        }
    }
}
