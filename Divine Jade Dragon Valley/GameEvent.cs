using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public abstract class GameEvent
    {
        public Condition<bool> TriggerConditions;
        /// <summary>
        /// When one event leads to another, you could parent/child relationships instead of a Condition based on the previous event
        /// </summary>
        public GameEvent Parent;
        public readonly List<GameEvent> Children = new();

        public abstract void Execute(EventContext eventContext);
    }
}
