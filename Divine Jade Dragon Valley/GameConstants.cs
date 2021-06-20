using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public static class GameConstants
    {
        /// <summary>
        /// X and Y dimensions in pixels of our standard tiles
        /// </summary>
        public const int TILE_SIZE = 64;

        /// <summary>
        /// Characters and items and such can have current stats and max stats, defined by this enum. We could potentially add Min, Initial, a separate BuffedMax, and possibly other types if needed.
        /// </summary>
        public enum StatsField
        {
            Current, Max
        }

        /// <summary>
        /// Information about how a plot is meant to be used. Several can apply to one plot.
        /// </summary>
        [Flags]
        public enum PlotDesignation
        {
            Hub, Road, Bazaar, Sect, Residence, Ruins, Decoration,
            Common, Uncommon, Rare //Might not want these; probably want actual spawn probabilities instead.
        }
    }
}
