using DDaikore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Divine_Jade_Dragon_Valley
{
    public static class GameProcessor
    {
        public static Core core = new(0);
        public static Area currentArea;
        public static Character currentPlayer;
        public static int upInput, downInput, leftInput, rightInput, debugInput;
        public static Dictionary<Type, List<GameEvent>> EventsByConditionType = new();
        public static List<GameEvent> UnconditionalRunAtEndOfFrame = new();

        public static void Initialize()
        {
            ConstructTestArea();

            //currentArea = new Area();
            //currentArea.Layers = new List<TilemapLayer> {
            //    new TilemapLayer { Tiles = new int[5, 4] { { 1, 2, 1, 2 }, { 1, 2, 1, 2 }, { 1, 2, 1, 2 }, { 1, 2, 1, 2 }, { 1, 2, 1, 2 } } },
            //    new TilemapLayer { Tiles = new int[4, 4] { { 0, 0, 0, 0 }, { 2, 2, 2, 2 }, { 1, 3, 1, 2 }, { 1, 2, 1, 2 } } }
            //};

            //currentArea.CollisionMap = new byte[5, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            //var events = new List<GameEvent> { new GameEventChangeArea {
            //    TargetArea = currentArea,
            //    TargetX = 0,
            //    TargetY = 0,
            //    TriggerConditions = new ConditionCharacterInBox { WhichCharacter = ConditionContext.CharacterField.Target } } };
            //(events[0].TriggerConditions as ConditionLocationCheck).AddPoint(new System.Drawing.PointF(3, 3));
            //(events[0].TriggerConditions as ConditionLocationCheck).AddPoint(new System.Drawing.PointF(2, 2));

            //Call AddEventsToDictionary for all relevant events.

            upInput = core.RegisterInput(Keys.W);
            downInput = core.RegisterInput(Keys.S);
            leftInput = core.RegisterInput(Keys.A);
            rightInput = core.RegisterInput(Keys.D);
            debugInput = core.RegisterInput(Keys.Space);

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

        public static void ConstructTestArea()
        {
            var plots = new List<AreaPlot>
            {
                new AreaPlot //Riddler
                {
                    CollisionMap = new byte[9, 5] { { 0, 1, 1, 1, 1 }, { 0, 1, 0, 0, 1 }, { 0, 1, 0, 1, 1 }, { 0, 0, 0, 1, 1 }, { 0, 0, 0, 1, 1 }, { 0, 0, 1, 1, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } },
                    Layers = new List<TilemapLayer>{ new TilemapLayer { Tiles = new int[9, 5] { { 0, 3, 3, 3, 3 }, { 0, 3, 0, 0, 3 }, { 1, 3, 0, 3, 3 }, { 0, 0, 0, 3, 3 }, { 0, 0, 0, 3, 3 }, { 0, 0, 3, 3, 0 }, { 0, 1, 3, 0, 0 }, { 0, 1, 0, 0, 0 }, { 0, 1, 0, 0, 0 } } } },
                    ConnectionPoints = new List<Point> { new Point(0, 2), new Point(1, 1) },
                    Designation = GameConstants.PlotDesignation.Sect,
                    LocationBasedEvents = new List<GameEvent>{ new GameEventChangeArea { TargetX = 0, TargetY = 0, TriggerConditions = new ConditionCharacterInBox { WhichCharacter = ConditionContext.CharacterField.Caster } } }
                },
                new AreaPlot //Switzerland
                {
                    CollisionMap = new byte[3, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } },
                    Layers = new List<TilemapLayer>{ new TilemapLayer { Tiles = new int[3, 5] { { 0, 0, 1, 0, 0 }, { 1, 1, 1, 1, 1 }, { 0, 0, 1, 0, 0 } } } },
                    ConnectionPoints = new List<Point> { new Point(0, 1), new Point(2, 0), new Point(4, 1), new Point(2, 2) },
                    Designation = GameConstants.PlotDesignation.Hub
                },
                new AreaPlot //Lungs
                {
                    CollisionMap = new byte[5, 4] { { 1, 1, 1, 0 }, { 1, 1, 1, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } },
                    Layers = new List<TilemapLayer>{ new TilemapLayer { Tiles = new int[5, 4] { { 3, 3, 3, 0 }, { 3, 3, 3, 0 }, { 0, 0, 1, 1 }, { 2, 2, 2, 0 }, { 2, 2, 2, 0 } } } },
                    ConnectionPoints = new List<Point> { new Point(3, 2) },
                    Designation = GameConstants.PlotDesignation.Residence
                },
                new AreaPlot //Stapler
                {
                    CollisionMap = new byte[9, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } },
                    Layers = new List<TilemapLayer>{ new TilemapLayer { Tiles = new int[9, 4] { { 1, 1, 1, 1 }, { 0, 0, 0, 1 }, { 0, 0, 1, 1 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 1 }, { 0, 0, 0, 1 }, { 1, 1, 1, 1 } } } },
                    ConnectionPoints = new List<Point> { new Point(0, 0), new Point(2, 4) },
                    Designation = GameConstants.PlotDesignation.Hub
                },
                new AreaPlot //Ladder
                {
                    CollisionMap = new byte[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } },
                    Layers = new List<TilemapLayer>{ new TilemapLayer { Tiles = new int[4, 4] { { 1, 1, 1, 1 }, { 0, 1, 0, 0 }, { 0, 1, 0, 0 }, { 1, 1, 1, 1 } } } },
                    ConnectionPoints = new List<Point> { new Point(0, 0), new Point(3, 0), new Point(0, 3), new Point (3, 3) },
                    Designation = GameConstants.PlotDesignation.Hub
                },
            };
            ((plots[0].LocationBasedEvents[0] as GameEventChangeArea).TriggerConditions as ConditionCharacterInBox).AddPoints(new PointF(2.8f, 0.8f), new PointF(3.2f, 2.2f)); //Top-left and bottom-right corners

            var builder = new AreaBuilder(plots, 30, 25, Environment.TickCount);

            ChangeArea(builder.Construct());
        }

        public static void ChangeArea(Area newArea)
        {
            if (currentArea == newArea) return; //Could transition for a location change within the same area

            if (currentArea != null) RemoveEventsFromDictionary(currentArea.LocationBasedEvents);
            currentArea = newArea;
            AddEventsToDictionary(currentArea.LocationBasedEvents);
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

            if (core.GetInputState(debugInput) == InputState.JustPressed) ConstructTestArea();

            //Do any events that were triggered to run before this frame ends but not in the usual Execute call (where the condition was checked)
            while (UnconditionalRunAtEndOfFrame.Count != 0)
            {
                var e = UnconditionalRunAtEndOfFrame[0];
                if (e.LimitToOncePerFrame && e.LastExecutedOnFrame == core.frameCounter) continue;
                e.Execute(new EventContext { EndOfFrame = true });
                e.LastExecutedOnFrame = core.frameCounter;
                UnconditionalRunAtEndOfFrame.RemoveAt(0);
            }
        }

        private static void MenuLoop()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Execute all potentially applicable events (the trigger conditions will be validated first)
        /// </summary>
        public static void ExecuteEvents(Type conditionType, EventContext eventContext, ConditionContext conditionContext)
        {
            if (EventsByConditionType.TryGetValue(conditionType, out var events))
            {
                foreach (var e in events)
                {
                    if (e.LimitToOncePerFrame && e.LastExecutedOnFrame == core.frameCounter) continue;
                    if (e.TriggerConditions.Evaluate(conditionContext))
                    {
                        e.Execute(eventContext);
                        e.LastExecutedOnFrame = core.frameCounter;
                    }
                }
            }
        }

        /// <summary>
        /// Add the given events to the appropriate slots in the event dictionary. Events should be passed into this function in a flat list, as it will not traverse the event tree.
        /// </summary>
        public static void AddEventsToDictionary(IEnumerable<GameEvent> newEvents)
        {
            List<GameEvent> events;
            foreach (var e in newEvents)
            {
                var conditionTypes = (e.TriggerConditions as Condition).FlattenTree().Select(p => p.GetType()).Distinct().ToList();
                foreach (var conditionType in conditionTypes)
                {
                    if (!EventsByConditionType.TryGetValue(conditionType, out events)) EventsByConditionType.Add(conditionType, events = new List<GameEvent>());
                    events.Add(e);
                }
            }
        }

        /// <summary>
        /// Remove the given events from all the slots they should be in within the event dictionary. Events should be passed into this function in a flat list, as it will not traverse the event tree.
        /// </summary>
        public static void RemoveEventsFromDictionary(IEnumerable<GameEvent> eventsToRemove)
        {
            foreach (var e in eventsToRemove)
            {
                var conditionTypes = (e.TriggerConditions as Condition).FlattenTree().Select(p => p.GetType()).Distinct().ToList();
                foreach (var conditionType in conditionTypes)
                {
                    if (!EventsByConditionType.TryGetValue(conditionType, out var events)) continue;
                    events.Remove(e);
                }
            }
        }
    }
}
