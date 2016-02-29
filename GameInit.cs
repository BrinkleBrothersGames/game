using Game.Content.World;
using SurvivalGame.Content.Characters;
using SurvivalGame.Engine;
using SurvivalGame.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    // TODO - Is this really the gameInit? Refactor?
    // TODO - Aim to do just rendering and i/o in this class
    public class GameInit
    {
        private Map currentLevel;
        private Player player;
        private Clock clock;
        bool run;

        /// <summary>
        /// Game Init constructor class.
        /// </summary>
        /// <param name="currentLevel">The currently loaded Map</param>
        /// <param name="player">The player character</param>
        public GameInit(Map currentLevel, Player player, Clock clock)
        {
            this.currentLevel = currentLevel;
            this.player = player;
            this.clock = clock;
        }

        // TODO - Should probably go in a player control class or something
        // TODO - logic is terrible - should call updatePlayerPosition only once
        public void doPlayerAction(string action)
        {
            // Initialised and assigned here to account for player not taking specific action
            int[] playerCoords = player.getPlayerCoords();
            int timeTaken = 0;

            switch (action)
            {
                case ("w"):
                    playerCoords[0] -= 1;
                    timeTaken = 10;
                    break;
                case ("s"):
                    playerCoords[0] += 1;
                    timeTaken = 10;
                    break;
                case ("a"):
                    playerCoords[1] -= 1;
                    timeTaken = 10;
                    break;
                case ("d"):
                    playerCoords[1] += 1;
                    timeTaken = 10;
                    break;
                case ("exit"):
                    run = false;
                    break;
                default:
                    // TryDebugAction for testing only. Should be removed in release versions.
                    DebugCommands.TryDebugAction(action, clock, player, currentLevel);

                    // Do nothing
                    return;
            }

            player.updatePlayerPosition(currentLevel, player, playerCoords);
            clock.AddTime(timeTaken);

        }

        static void Main()
        {
            // TODO - the GameInit initialize class should really do all this
            Map currentLevel = new Map();
            Player player = new Player();
            Clock clock = new Clock();
            GameInit game = new GameInit(currentLevel, player, clock);
            game.run = true;
            string input;

            player.updatePlayerPosition(currentLevel, player, new int[] { 5, 5 });

            while (game.run)
            {
                // Render changes to map
                // TODO - shouldn't have to pass 'currentLevel' twice
                currentLevel.RenderMap(currentLevel);

                // Wait for player to take action
                input = Console.ReadLine();

                // TODO - Need to refactor this and map.render to be somewhere sensible.
                // Clear the console to output results of new action
                Console.Clear();

                game.doPlayerAction(input);
                
            }
        }

// TODO - MUCH LATER - For collisions, consider adding 'blockMovement' bool to our place vector.
// TODO - DURING REFACTOR - Hide everything in get/set methods. 

    }
}