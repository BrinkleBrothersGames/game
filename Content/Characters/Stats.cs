using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    public class Stats
    {
        // TODO - make these private. We want to use TemporaryStats, and it's a good way to avoid accidentally calling the wrong ones
        public int strength;
        public int endurance;
        public int agility;
        public int perception;
        public int intelligence;

        /// <summary>
        /// Creates stats at default 5.
        /// </summary>
        public Stats()
        {
            this.strength= 5;
            this.endurance = 5;
            this.agility = 5;
            this.perception = 5;
            this.intelligence = 5;
        }

        /// <summary>
        /// Creates stats profile with inputted values
        /// </summary>
        /// <param name="strength"></param>
        /// <param name="endurance"></param>
        /// <param name="agility"></param>
        /// <param name="perception"></param>
        /// <param name="intelligence"></param>
        public Stats(int strength, int endurance, int agility, int perception, int intelligence)
        {
            this.strength = strength;
            this.endurance = endurance;
            this.agility = agility;
            this.perception = perception;
            this.intelligence = intelligence;

        }

        /// <summary>
        /// Changes single stat by inputted amount
        /// </summary>
        /// <param name="stat">The stat to be updated</param>
        /// <param name="amount">Positive or negative value</param>
        public void ChangeStats(string stat, int amount)
        {
            switch (stat)
            {
                case "strength":
                    strength += amount;
                    break;
                case "endurance":
                    endurance += amount;
                    break;
                case "agility":
                    agility += amount;
                    break;
                case "perception":
                    perception += amount;
                    break;
                case "intelligence":
                    intelligence += amount;
                    break;
                default:
                    break;
            }
        }

        // TODO - Change this to return -1 if stat not found
        /// <summary>
        /// Returns the value of a player's stat. If typed stat doesn't exist, returns 0.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int GetStat(string stat, Player player)
        {
            int value = -1;
            
            switch (stat)
            {
                case "strength":
                    value = player.stats.strength;
                    break;
                case "endurance":
                    value = player.stats.endurance;
                    break;
                case "agility":
                    value = player.stats.agility;
                    break;
                case "perception":
                    value = player.stats.perception;
                    break;
                case "intelligence":
                    value = player.stats.intelligence;
                    break;
                default:
                    break;
            }

            return value;
        }
    }
}
