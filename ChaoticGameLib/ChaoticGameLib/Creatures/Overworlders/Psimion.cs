using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Psimion : Creature, ISupport
    {
        public Psimion(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, false, false, true, false, 0,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Strategist)
        {
        }

        public override string Description()
        {
            return "Psimion Creature - Overworld Strategist Courage: 45 Power: 30 Wisdom: 55 Speed: 40 Energy: 45 Mugic Ability: 1" +
                " Elemental Type: Earth Creature Ability: " +
                "Support Wisdom 5 [This Creature gains 5 Wisdom for each adjacent OverWorld Creature you control.] " +
            "\"Simple\" Psimion is not...though his seeming to be so has certain advantages.";
        }

        public void Ability(byte numAdjacent)
        {
            this.Wisdom -= (byte)(5 * PreNumAdja);
            this.Wisdom += (byte)(5 * numAdjacent);
            PreNumAdja = numAdjacent;
        }
    }
}
