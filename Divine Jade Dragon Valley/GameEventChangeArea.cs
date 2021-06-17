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
            GameProcessor.currentArea = TargetArea;
            GameProcessor.currentPlayer.X = TargetX;
            GameProcessor.currentPlayer.Y = TargetY;
        }
        //TODO: Transition types such as fade out or scrolling up/down
    }
}
