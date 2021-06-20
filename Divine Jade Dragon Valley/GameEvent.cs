using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public abstract class GameEvent
    {
        public ICondition<bool> TriggerConditions;
        /// <summary>
        /// When one event leads to another, you could parent/child relationships instead of a Condition based on the previous event
        /// </summary>
        public GameEvent Parent;
        public readonly List<GameEvent> Children = new();
        public long LastExecutedOnFrame = -1;
        public bool LimitToOncePerFrame;

        /// <summary>
        /// Execute the event. Events are NOT allowed to cause the event list to change directly, but they can be added to the UnconditionalRunAtEndOfFrame list in GameProcessor. Check EndOfFrame in EventContext to determine whether to run or delay.
        /// </summary>
        public abstract void Execute(EventContext eventContext);

        public GameEvent ShallowCopy() => (GameEvent)MemberwiseClone();
    }
}
