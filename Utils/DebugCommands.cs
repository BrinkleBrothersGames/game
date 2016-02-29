using Game.Content.World;
using SurvivalGame.Content.Characters;
using SurvivalGame.Engine;
using System;

namespace SurvivalGame.Utils
{
    public class DebugCommands
    {
        public static void TryDebugAction(string action, Clock clock, Player player, Map currentLevel)
        {
            string[] splitAction = action.Split('$');
            
            if (splitAction[0].Contains("gettime"))
            {
                Console.WriteLine(clock.GetTime().ToString());
            }
            else if (splitAction[0].Contains("addtime"))
            {
                clock.AddTime(int.Parse(splitAction[1]));
                Console.WriteLine("Time updated. New time = " + clock.GetTime().ToString());
            }
            else if ((splitAction[0].Contains("settime")))
            {
                clock.SetTime(int.Parse(splitAction[1]));
                Console.WriteLine("Time set to " + clock.GetTime().ToString());
            }

        }
    }
}
