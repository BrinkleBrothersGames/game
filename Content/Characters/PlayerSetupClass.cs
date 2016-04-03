using SurvivalGame.Content.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    // TODO - Make class level variables for things getting passed between methods in this class.
    class PlayerSetupClass
    {
        Player player;
        string inputPoints;
        string stat;
        string input;
        int amount;
        int points;


        /// <summary>
        /// To make PlayerSetup more readable.
        /// </summary>
        /// <param name="player"></param>
        public void PrintStats()
        {
            Console.WriteLine("Agility: " + this.player.stats.agility);
            Console.WriteLine("Endurance: " + this.player.stats.endurance);
            Console.WriteLine("Intelligence: " + this.player.stats.intelligence);
            Console.WriteLine("Perception: " + this.player.stats.perception);
            Console.WriteLine("Strength: " + this.player.stats.strength);

        }

        void ConfirmPlayerStats()
        {
            points = 0;
            bool choiceNotConfirmed = true;

            player.ChangePlayerStats(stat, amount);
            points -= amount;

            while (choiceNotConfirmed)
            {
                PrintStats();
                Console.WriteLine("Are you happy with these as your final stats? (y/n)");
                input = Console.ReadLine();
                Console.Clear();

                switch (input)
                {
                    case "n":
                    case "no":
                        player.ChangePlayerStats(stat, -amount);
                        points += amount;
                        choiceNotConfirmed = false;
                        break;
                    case "y":
                    case "yes":
                        choiceNotConfirmed = false;
                        break;
                    default:
                        Console.WriteLine("Invalid response.");
                        break;
                }
            }
        }


        /// <summary>
        /// Takes the player through the character creation process.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public Player PlayerSetup(Player player)
        {
            this.player = player;
            // TODO - Set points through input to allow for different diffuculty settings at a later date
            points = 5;
            
            while (points > 0)
            {
                Console.WriteLine("You have " + points + " points to distribute between your stats. Choose wisely.");
                PrintStats();
                
                stat = Console.ReadLine();
                inputPoints = Console.ReadLine();

                Console.Clear();

                bool invalidInputPoints = !(int.TryParse(inputPoints, out amount));
                bool invalidStat = (Stats.GetStat(stat, player) == -1);
                
                /// Checks if inputs are valid before conditionally changing stats, to save time if input is invalid.
                if (invalidInputPoints && invalidStat)
                {
                    Console.WriteLine("Type valid stat (all lower case), followed by number.");
                }
                  
                else if (points - amount < 0)
                {
                    Console.WriteLine("You can't give yourself points that you don't have.");
                }

                else if (Stats.GetStat(stat, this.player) + amount < 3)
                {
                    Console.WriteLine(stat + " can't go that low.");
                }

                else if (points - amount == 0)
                {
                    ConfirmPlayerStats();
                }

                else
                {
                    player.ChangePlayerStats(stat, amount);
                    points -= amount;
                }


            }
            return player;
        }
            
        }

    }

