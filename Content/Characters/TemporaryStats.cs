using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    public class TemporaryStats
    {
        Stats baseStats;

        public int tempStrength;
        public int tempEndurance;
        public int tempAgility;
        public int tempPerception;
        public int tempIntelligence;

        public TemporaryStats(Stats baseStats)
        {
            this.baseStats = baseStats;

            this.tempStrength = baseStats.strength;
            this.tempEndurance = baseStats.endurance;
            this.tempAgility = baseStats.agility;
            this.tempPerception = baseStats.perception;
            this.tempIntelligence = baseStats.intelligence;
        }

        public void UpdateTempStats(Dictionary<string, int> statChanges)
        {
            foreach(string statName in statChanges.Keys)
            {
                switch(statName)
                {
                    case ("strength"):
                        this.tempStrength += statChanges[statName];
                        break;
                    case ("endurance"):
                        this.tempEndurance += statChanges[statName];
                        break;
                    case ("agility"):
                        this.tempAgility += statChanges[statName];
                        break;
                    case ("perception"):
                        this.tempPerception += statChanges[statName];
                        break;
                    case ("intelligence"):
                        this.tempIntelligence += statChanges[statName];
                        break;
                }
            }
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
                    tempStrength += amount;
                    break;
                case "endurance":
                    tempEndurance += amount;
                    break;
                case "agility":
                    tempAgility += amount;
                    break;
                case "perception":
                    tempPerception += amount;
                    break;
                case "intelligence":
                    tempIntelligence += amount;
                    break;
                default:
                    break;
            }
        }
    }
}
