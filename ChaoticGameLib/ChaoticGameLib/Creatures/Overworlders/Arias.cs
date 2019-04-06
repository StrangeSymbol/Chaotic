using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Arias : Creature
    {
        public Arias(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0,
            false, false, true, false, 0, false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Warrior)
        {
        }
        public void Ability(byte numAdjacent)
        {
            this.Power += (byte)(5 * numAdjacent);
        }
        public override string Description()
        {
            return "Arias Creature - Overworld Warrior Courage: 55 Power: 65 Wisdom: 30 Speed: 55 Energy: 50 Mugic Ability: 0" +
                " Elemental Type: Earth Creature Ability: " +
                "Support Power 5 [This Creature gains 5 Power for each adjacent Overworld Creature you control.] " +
            "If you think he looks angry now, you should have been at the nose-piercing!";
        }
    }
}
