using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; //for Point only

namespace Divine_Jade_Dragon_Valley
{
    public class Area
    {
        public string Name;
        /// <summary>
        /// Visual tilemaps. Layers are drawn in the order they appear in this list.
        /// </summary>
        public List<TilemapLayer> Layers = new();
        /// <summary>
        /// Physical tilemaps. Each dimension of the CollisionMap should match the max respective dimension in the Layers. //TODO: Assert or otherwise enforce this.
        /// </summary>
        public byte[,] CollisionMap;
        /// <summary>
        /// Tiles where this Area can support connections to AreaPlots, in terms of whole numbers of tiles.
        /// </summary>
        public List<Point> ConnectionPoints = new();
        /// <summary>
        /// Events, relative to this area (or plot definition--in that case, copies of these will be made and returned from Stamp with appropriate final locations.)
        /// </summary>
        public List<GameEvent> LocationBasedEvents = new(); //TODO: Include 'probability' conditions, which should have an evaluate-at-time / evaluation frequency so we know to check the probability once at each time we try to use this plot, and if it fails the check, exclude the event from the placed plot

        /// <summary>
        /// As an offset from the given point (-1 or +1 on either axis but not both), get the best direction to extend the tile at the given point.
        /// </summary>
        public Point GetExtensionOfConnectionPoint(Point connectionPoint)
        {
            //Find out which direction the same or similar type of tile is in and give the point that would continue the same line of tiles but is 0 in this tilemap and collision map. Try 4 directions.
            var collisionType = CollisionMap[connectionPoint.Y, connectionPoint.X];
            var tileType = Layers.Reverse<TilemapLayer>().Select(p => p.Tiles[connectionPoint.Y, connectionPoint.X]).FirstOrDefault(p => p != 0);

            if (connectionPoint.X > 0 && connectionPoint.X < Width - 1)
            {
                //The tile on the right is empty and the tile on the left is a match to the connection point, so return "right"
                if (CollisionMap[connectionPoint.Y, connectionPoint.X - 1] == collisionType && CollisionMap[connectionPoint.Y, connectionPoint.X + 1] == 0
                    && Layers.Any(p => p.Tiles[connectionPoint.Y, connectionPoint.X - 1] == tileType) && Layers.All(p => p.Tiles[connectionPoint.Y, connectionPoint.X + 1] == 0)) return new Point(1, 0);
                //The tile on the left is empty and the tile on the right is a match to the connection point, so return "left"
                if (CollisionMap[connectionPoint.Y, connectionPoint.X + 1] == collisionType && CollisionMap[connectionPoint.Y, connectionPoint.X - 1] == 0
                    && Layers.Any(p => p.Tiles[connectionPoint.Y, connectionPoint.X + 1] == tileType) && Layers.All(p => p.Tiles[connectionPoint.Y, connectionPoint.X - 1] == 0)) return new Point(-1, 0);
            }
            if (connectionPoint.Y > 0 && connectionPoint.Y < Height - 1)
            {
                //The tile below is empty and the tile above is a match to the connection point, so return "below"
                if (CollisionMap[connectionPoint.Y - 1, connectionPoint.X] == collisionType && CollisionMap[connectionPoint.Y + 1, connectionPoint.X] == 0
                    && Layers.Any(p => p.Tiles[connectionPoint.Y - 1, connectionPoint.X] == tileType) && Layers.All(p => p.Tiles[connectionPoint.Y + 1, connectionPoint.X] == 0)) return new Point(0, 1);
                //The tile above is empty and the tile below is a match to the connection point, so return "above"
                if (CollisionMap[connectionPoint.Y + 1, connectionPoint.X] == collisionType && CollisionMap[connectionPoint.Y - 1, connectionPoint.X] == 0
                    && Layers.Any(p => p.Tiles[connectionPoint.Y + 1, connectionPoint.X] == tileType) && Layers.All(p => p.Tiles[connectionPoint.Y - 1, connectionPoint.X] == 0)) return new Point(0, -1);
            }

            //If we reach this point, there weren't 2 similar tiles in a row. We will instead just pick an adjacent empty tile.
            if (connectionPoint.Y > 0 && CollisionMap[connectionPoint.Y - 1, connectionPoint.X] == 0 && Layers.All(p => p.Tiles[connectionPoint.Y - 1, connectionPoint.X] == 0)) return new Point(0, -1);
            if (connectionPoint.Y < Height - 1 && CollisionMap[connectionPoint.Y + 1, connectionPoint.X] == 0 && Layers.All(p => p.Tiles[connectionPoint.Y + 1, connectionPoint.X] == 0)) return new Point(0, 1);

            if (connectionPoint.X > 0 && CollisionMap[connectionPoint.Y, connectionPoint.X - 1] == 0 && Layers.All(p => p.Tiles[connectionPoint.Y, connectionPoint.X - 1] == 0)) return new Point(-1, 0);
            if (connectionPoint.X < Width - 1 && CollisionMap[connectionPoint.Y, connectionPoint.X + 1] == 0 && Layers.All(p => p.Tiles[connectionPoint.Y, connectionPoint.X + 1] == 0)) return new Point(1, 0);

            //TODO: We can make the order random regardless of how many of these points are valid options, but we'd still want to do it in 2 or 3 steps (extension first, then maybe empty tile with ANY non-empty tile adjacent, then any empty tile).
            return Point.Empty; //No good option
        }

        public int Width { get => CollisionMap.GetLength(1); }
        public int Height { get => CollisionMap.GetLength(0); }
    }
}
