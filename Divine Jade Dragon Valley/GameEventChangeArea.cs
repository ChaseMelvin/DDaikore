using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class GameEventChangeArea : GameEvent
    {
        public Area TargetArea;
        public float TargetX;
        public float TargetY;

        public override void Execute(EventContext eventContext)
        {
            if (eventContext.EndOfFrame)
            {
                GameProcessor.ChangeArea(TargetArea ?? GameProcessor.currentArea);
                GameProcessor.currentPlayer.X = TargetX;
                GameProcessor.currentPlayer.Y = TargetY;
            }
            else GameProcessor.UnconditionalRunAtEndOfFrame.Add(this); //Delay until the end of the frame because this event can cause the event list to change.
        }
        //TODO: Transition types such as fade out or scrolling up/down
    }
}
