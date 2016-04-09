using SurvivalGame.Content.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Engine.Time
{
    public class Effect
    {
        public int duration;
        Dictionary<string, int> effects;
        Player player;
        
        public Effect(int duration, Dictionary<string, int> effects, Player player)
        {
            this.duration = duration;
            this.effects = effects;
            this.player = player;
        }        

        public void DecreaseDuration(int decrement)
        {
            this.duration -= decrement;
        }

        public void ApplyEffect()
        {
            foreach(string need in effects.Keys)
            {
                player.tempStats.ChangeStats(need, effects[need]);
            }
        }

        public void RemoveEffect(Player player)
        {
            // Inverses the value of each effect, then reapplies inversed values to the player.
            foreach(string need in effects.Keys.ToList<string>())
            {
                effects[need] = effects[need] * -1;
            }

            ApplyEffect();
        }

    }
}
