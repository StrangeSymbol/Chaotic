using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Danians
{
    public class Kannen : Creature
    {
        public Kannen(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, 
            false, false, false, false, false, false, Tribe.Danian, CreatureType.Battlemaster)
        {
        }

        public override string Description()
        {
            return "Kannen Creature - Danian Battlemaster Courage: 25 Power: 20 Wisdom: 70 Speed: 25 Energy: 40 " +
                "Mugic Ability: 1 Elemental Type: None Creature Ability: " +
                "Cost 1 Mugic Counter: Activate Hive until the end of the turn." +
                "Sacrifice Kannen: Return a Mandiblor card from your discard pile to any open space. " +
            "\"My life is my tribe!\" -- Kannen";
        }

        public void Ability(ref bool hive)
        {
            // Until end of turn.
            if (this.MugicCounters >= 1)
            {
                this.MugicCounters--;
                hive = true;
            }
        }
        public void Ability()
        {
            this.Alive = false;
            // Return a Mandiblor from discard pile to any open space.
        }
    }
}