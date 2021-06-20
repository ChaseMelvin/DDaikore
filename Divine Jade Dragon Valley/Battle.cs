using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class Battle : GameEvent
    {
        public List<Character> Characters;
        public int TurnNumber;

        public override void Execute(EventContext eventContext)
        {
            throw new NotImplementedException();
        }
    }
}
