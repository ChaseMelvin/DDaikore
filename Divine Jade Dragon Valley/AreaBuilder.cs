using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class AreaBuilder
    {
        protected List<AreaPlot> plots;
        protected Random rnd;
        protected int width;
        protected int height;
        protected Area newArea;

        public AreaBuilder(List<AreaPlot> plots, int width, int height, int seed)
        {
            this.plots = plots;
            this.width = width;
            this.height = height;
            rnd = new Random(seed);
            newArea = new Area() { CollisionMap = new byte[height, width] };
            newArea.Layers.Add(new TilemapLayer() { Tiles = new int[height, width] });
        }

        public Area Construct()
        {
            //Find a good starting place and a good starting plot to plop
            StampStartingPlot();

            //Plop the plots using the predefined ConnectionPoints
            var ploppedPlots = 0; //TODO: Rather than having tries + a desired count, we should keep plopping until we fail N times in a row. Optimally, we would also remove a connection point if nothing connects to it after we've tried to connect several things to that same point.
            const int targetPlots = 15;
            for (var plopTries = 0; plopTries < targetPlots * 12 && ploppedPlots < targetPlots; plopTries++)
            {
                if (newArea.ConnectionPoints.Count == 0) break; //Ran out of points!
                var connectToIndex = rnd.Next(0, newArea.ConnectionPoints.Count);
                var connectAtPoint = newArea.GetExtensionOfConnectionPoint(newArea.ConnectionPoints[connectToIndex]); //Gives us a unit vector in a cardinal direction
                if (connectAtPoint == Point.Empty) //No way to connect to this point. //TODO: Should really warn the map developer about this ordinarily, but it can also happen if the connection point just happens to end up at the edge of the Area.
                {
                    newArea.ConnectionPoints.RemoveAt(connectToIndex);
                    continue;
                }
                connectAtPoint.Offset(newArea.ConnectionPoints[connectToIndex]); //This gave us a unit vector before, so add the base point to it for our next usage

                //if we've plopped at least a few plots and don't have enough places to connect to, we should focus on plopping more hubs.
                var pickFrom = plots;
                if (plopTries > 2 && newArea.ConnectionPoints.Count < 3) pickFrom = pickFrom.Where(p => p.Designation == GameConstants.PlotDesignation.Hub).ToList();

                //Try every connection point for the chosen plot before we give up, in a random order. //TODO: I'd really prefer to use the GetExtensionOfConnectionPoint order (but that checks the area bounds; for plots, we need one that doesn't)
                var nextPlotIndex = rnd.Next(0, pickFrom.Count); //TODO: Weighted probability, with exponentially lower probability for plots that have been used once, but any rotated/flipped versions of that same biome should be adjusted together as a group, and symmetry shouldn't double the probability of the "same" plot
                foreach (var pointIndex in rnd.Randomize(Enumerable.Range(0, pickFrom[nextPlotIndex].ConnectionPoints.Count)))
                {
                    //Calculate the desired location for the plot (the location within newArea that should be the top-left corner of the plot when stamped)
                    if (TryStamp(connectAtPoint, pickFrom[nextPlotIndex], pointIndex, connectToIndex))
                    {
                        ploppedPlots++;
                        break;
                    }
                }
            }

            return newArea;
        }

        protected void StampStartingPlot()
        {
            var goodStarters = plots.Where(p => p.Designation.HasFlag(GameConstants.PlotDesignation.Hub)).ToList();
            if (goodStarters.Count == 0) goodStarters = plots.Where(p => p.Designation.HasFlag(GameConstants.PlotDesignation.Road)).ToList();

            var smallest = goodStarters.OrderByDescending(p => p.CollisionMap.Length).First();
            int startX, startY, starterIndex = -1, tries = 5;
            do
            {
                if (--tries > 0)
                {
                    startX = rnd.Next(0, width - smallest.Width);
                    startY = rnd.Next(0, height - smallest.Height);
                }
                else //If our random placement and selection fails several times, start picking locations based on the biggest possible dimensions, so it should be guaranteed to succeed on this try, or it'll throw an exception if it cannot succeed
                {
                    if (tries < -3) throw new InvalidOperationException("Unable to select a starting plot for the area."); //Just in case we have a bug. We don't want to get stuck in an infinite loop.

                    startX = rnd.Next(0, height - goodStarters.Max(p => p.Width));
                    startY = rnd.Next(0, height - goodStarters.Max(p => p.Height));
                }

                starterIndex = rnd.Next(0, goodStarters.Count);
            } while (!goodStarters[starterIndex].Validate(newArea, new Point(startX, startY)));
            goodStarters[starterIndex].Stamp(newArea, new Point(startX, startY));
        }

        /// <summary>
        /// Try to stamp a plot on the area we're currently building in this AreaBuilder.
        /// </summary>
        /// <returns>true on success</returns>
        protected bool TryStamp(Point connectAtPoint, AreaPlot plot, int plotConnectionPointIndex, int constructionAreaConnectionPointIndex)
        {
            var stampTopLeftPoint = new Point(connectAtPoint.X - plot.ConnectionPoints[plotConnectionPointIndex].X, connectAtPoint.Y - plot.ConnectionPoints[plotConnectionPointIndex].Y);

            //Bounds check. Right now we don't support stamping plots that would bleed outside the Area.
            if (stampTopLeftPoint.X < 0 || stampTopLeftPoint.Y < 0 || stampTopLeftPoint.X + plot.Width > newArea.Width || stampTopLeftPoint.Y + plot.Height > newArea.Height) return false;

            if (plot.Validate(newArea, stampTopLeftPoint))
            {
                plot.Stamp(newArea, stampTopLeftPoint);

                //Remove the two connection points that we just joined so we don't reuse them later.
                newArea.ConnectionPoints.RemoveAt(newArea.ConnectionPoints.Count - plot.ConnectionPoints.Count + plotConnectionPointIndex); //Since the connection points are added in order, we can remove the one we picked easily by its index
                newArea.ConnectionPoints.RemoveAt(constructionAreaConnectionPointIndex);
                return true; //Don't have to try any more ConnectionPoints of the chosen plot because this one worked.
            }
            return false;
        }
    }
}
