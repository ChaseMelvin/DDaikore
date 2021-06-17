using DDaikore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Divine_Jade_Dragon_Valley
{
    public static class GameProcessor
    {
        public static Core core = new(0);
        public static Area currentArea;
        public static Character currentPlayer;
        public static int upInput, downInput, leftInput, rightInput;
        public static Dictionary<Type, List<GameEvent>> EventsByConditionType;

        public static void Initialize()
        {
            currentArea = new Area();
            currentArea.Layers = new List<TilemapLayer> {
                new TilemapLayer { Tiles = new int[5, 4] { { 1, 2, 1, 2 }, { 1, 2, 1, 2 }, { 1, 2, 1, 2 }, { 1, 2, 1, 2 }, { 1, 2, 1, 2 } } },
                new TilemapLayer { Tiles = new int[4, 4] { { 0, 0, 0, 0 }, { 2, 2, 2, 2 }, { 1, 3, 1, 2 }, { 1, 2, 1, 2 } } }
            };

            currentArea.CollisionMap = new byte[5, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            var events = new List<GameEvent> { new GameEventChangeArea {
                TargetArea = currentArea, 
                TargetX = 0, 
                TargetY = 0, 
                TriggerConditions = new ConditionCharacterInBox {WhichCharacter = ConditionContext.CharacterField.Target, X = 3, Y = 3, Height = 2, Width = 2} } };
            //TODO: Call method for putting events into EventsByConditionType
            upInput = core.RegisterInput(Keys.W);
            downInput = core.RegisterInput(Keys.S);
            leftInput = core.RegisterInput(Keys.A);
            rightInput = core.RegisterInput(Keys.D);

            currentPlayer = new Character()
            {
                Name = "Wilson",
                X = 1,
                Y = 1,
                Width = 1,
                Height = 1,
            };

            currentPlayer.CurrentStats.Add("Health", 20);

            new Thread(() =>
            {
                core.MenuLoop = MenuLoop;
                core.GameLoop = GameLoop;
                
                core.menuIndex = -1;
                core.Begin();
            }).Start();
        }

        private static void GameLoop()
        {
            if (core.GetInputState(upInput) == InputState.Held)
            {
                currentPlayer.MoveY(-.02f, currentArea.CollisionMap);
            }
            else if (core.GetInputState(downInput) == InputState.Held)
            {
                currentPlayer.MoveY(.02f, currentArea.CollisionMap);
            }

            if (core.GetInputState(leftInput) == InputState.Held)
            {
                currentPlayer.MoveX(-.02f, currentArea.CollisionMap);
            }
            else if (core.GetInputState(rightInput) == InputState.Held)
            {
                currentPlayer.MoveX(.02f, currentArea.CollisionMap);
            }
        }

        private static void MenuLoop()
        {
            //throw new NotImplementedException();
        }

        public static void ExecuteEvents(Type conditionType, EventContext eventContext, ConditionContext conditionContext)
        {
            if (EventsByConditionType.TryGetValue(conditionType, out var events))
            {
                foreach (var e in events)
                {
                    if (e.TriggerConditions.Evaluate(conditionContext))
                    {
                        e.Execute(eventContext);
                    }
                }
            }
        }

        //TODO: Create method to fill out event dictionary
    }
}
