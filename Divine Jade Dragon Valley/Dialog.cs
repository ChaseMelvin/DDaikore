using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divine_Jade_Dragon_Valley
{
    public class Dialog : GameEvent
    {
        public readonly string Text;
        public readonly List<string> Choices = new();

        public override void Execute(EventContext eventContext)
        {
            throw new NotImplementedException();
        }
    }
}
