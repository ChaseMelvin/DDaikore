using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public abstract class GameEvent
    {
        public readonly Condition<bool> TriggerConditions;
        /// <summary>
        /// When one event leads to another, you could parent/child relationships instead of a Condition based on the previous event
        /// </summary>
        public readonly GameEvent Parent;
        public readonly List<GameEvent> Children = new();
    }
}
