using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class FarmPlot : Entity
    {
        /// <summary>
        /// World Energy Quality, Soil Quality, Water Volume, Average Temperature, Time Multiplier, Crop Slot Count (most plots would probably be 4, and trees would require 4 slots), Manager (each of your indebted allies could take care of a few crop plots for you)
        /// </summary>
        public readonly Dictionary<string, double> Stats = new();
        public readonly List<FarmCrop> Crops = new();
    }
}
