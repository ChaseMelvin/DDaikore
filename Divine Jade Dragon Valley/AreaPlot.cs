using System.Collections.Generic;
using System.Linq;
using System.Drawing; //for Point only

namespace Divine_Jade_Dragon_Valley
{
    /// <summary>
    /// A manually-created plot of structure and/or terrain and/or roads and/or events, etc. All tiles of an AreaPlot must be contiguous.
    /// </summary>
    public class AreaPlot : Area
    {
        public GameConstants.PlotDesignation Designation;
        protected Point[] EdgeTiles;

        //TODO: Probabilities, allowed biomes, biome generation (biomes could be a random pre-drawn polygon with random rotation and stretch; any ConnectionPoint within the biome bounds would filter down to only plots that are allowed in that biome)

        //TODO: A GenerateRotations method (if it doesn't destroy the visuals--or if it would, we could still have such a method, but it'd have to switch the tiles out for appropriate similar ones to match the rotation)

        //TODO: A Trim method for map editor use, minimizing the dimensions of the tilemaps.

        /// <summary>
        /// Identify all the tiles that are adjacent to air in a cardinal direction, setting EdgeTiles along the way. Only checks visual tiles, under the assumption that CollisionMap is never nonzero where there's no visual tile. //TODO: Assert or enforce that condition (in a map editor).
        /// </summary>
        protected void FindEdgeTiles()
        {
            var edgeTiles = new List<Point>();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    //If any adjacent tile is empty (in the 4 cardinal directions), this is an edge.
                    //Note: this expects that all layers have the same dimensions. //TODO: Assert or enforce.
                    if (Layers.All(p => p.Tiles[y, x] != 0) && //The tile you're looking at ISN'T empty
                        (x == 0 || y == 0 || x == Width - 1 || y == Height - 1 || //Along the edges of the plot
                        Layers.All(p => p.Tiles[y, x - 1] == 0) || Layers.All(p => p.Tiles[y - 1, x] == 0) || Layers.All(p => p.Tiles[y, x + 1] == 0) || Layers.All(p => p.Tiles[y + 1, x] == 0))) //NOT along the edges of the plot, but next to a blank tile
                    {
                        edgeTiles.Add(new Point(x, y));
                    }
                }
            }

            EdgeTiles = edgeTiles.ToArray(); //It could just be a List permanently, but whatever.
        }

        /// <summary>
        /// Ensure that this tilemap can be stamped at the specified location atop the given Area without changing any existing nonzero tiles in the Area.
        /// </summary>
        /// <returns>true if valid, false otherwise</returns>
        public bool Validate(Area applyTo, Point applyAt)
        {
            //Lazy-evaluate: find the coordinates of all the edge tiles of this AreaPlot and keep them in memory (in 'EdgeTiles') if we haven't already determined them.
            if (EdgeTiles == null) FindEdgeTiles();

            var applicableWidth = applyTo.Width - applyAt.X;
            var applicableHeight = applyTo.Height - applyAt.Y;
            if (applicableWidth < Width || applicableHeight < Height) return false; //TODO: What shall we do when we try to plop this plot partway off the map? That would probably look good for some structures (and perhaps only some rotations thereof) but not others.
            //TODO: If we want to be able to plop it partway off the map, instead of the above return statement, we would want to fliter out the edge tiles beyond applicableWidth or applicableHeight.

            //Check every edge tile in this plot against the tiles in the under-construction area. If the under-construction area has any nonzero tiles in that intersection, we can't place this plot there.
            foreach (var layer in applyTo.Layers)
            {
                foreach (var tile in EdgeTiles)
                {
                    if (layer.Tiles[tile.Y + applyAt.Y, tile.X + applyAt.X] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Apply this plot to the given Area and get the GameEvents that should be associated with the area (with their coordinates corrected).
        /// Should only be called after checking that Validate returns true.
        /// </summary>
        public void Stamp(Area applyTo, Point applyAt)
        {
            //Loop through this.Layers and this.CollisionMap. Any NONZERO tile in this.Layers or in this.CollisionMap should be overwritten in the given applyTo Area.
            for (var layerIndex = 0; layerIndex < Layers.Count; layerIndex++)
            {
                //Ensure the Area we're stamping on has enough layers.
                if (applyTo.Layers.Count < layerIndex) applyTo.Layers.Add(new TilemapLayer() { Tiles = new int[applyTo.Height, applyTo.Width] });

                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        if (Layers[layerIndex].Tiles[y, x] != 0)
                        {
                            if (applyTo.Layers.Any(p => p.Tiles[y + applyAt.Y, x + applyAt.X] != 0)) System.Diagnostics.Debugger.Break(); //To catch a bug; it's happened ONCE that I saw.
                            applyTo.Layers[layerIndex].Tiles[y + applyAt.Y, x + applyAt.X] = Layers[layerIndex].Tiles[y, x];
                        }
                    }
                }
            }

            //Do the same for the CollisionMap. (This repetition is a reason that we might want to change the CollisionMap to just be stored as Layers[0] instead.)
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (CollisionMap[y, x] != 0) applyTo.CollisionMap[y + applyAt.Y, x + applyAt.X] = CollisionMap[y, x];
                }
            }

            //Copy the ConnectionPoints to the Area
            foreach (var point in ConnectionPoints)
            {
                var pointClone = new Point(point.X, point.Y);
                pointClone.Offset(applyAt);
                applyTo.ConnectionPoints.Add(pointClone);
            }

            //Clone the LocationBasedEvents and update the clones' condition trees to reflect the adjusted coordinates. Those events should all be listed at the top level, not just left in their tree structure (that's only useful for checking if they should trigger).
            //TODO: This assumes no location conditions are in OTHER Areas. One event triggered by this area could theoretically have a child event in another Area (in fact, it's kind of likely), so we might have to include the Area as part of every location check object (it'd be hard to process if we were to include it as a separate Condition, though).
            foreach (var e in LocationBasedEvents.Select(p => p.ShallowCopy()))
            {
                //Do a semi-deep copy of the trigger condition tree (cloning only as far as the nodes we want to modify)
                e.TriggerConditions = (e.TriggerConditions as Condition).Clone((e.TriggerConditions as Condition).LocationConditions()) as ICondition<bool>;

                //Now update the location nodes
                (e.TriggerConditions as Condition).LocationConditions().ToList().ForEach(p => p.OffsetPoints(applyAt));

                applyTo.LocationBasedEvents.Add(e);
            }
        }

    }
}
