using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Ulfhedinn : Creature, ISupport
    {
        public Ulfhedinn(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0,
            false, false, false, false, 0, false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Warrior)
        {
        }
        public void Ability(byte numAdjacent)
        {
            this.Power -= (byte)(5 * PreNumAdja);
            this.Power += (byte)(5 * numAdjacent);
            PreNumAdja = numAdjacent;
        }
        public override string Description()
        {
            return "Ulfhedinn Creature - Overworld Warrior Courage: 70 Power: 80 Wisdom: 45 Speed: 50 Energy: 50 Mugic Ability: 0" +
                " Elemental Type: None Creature Ability: " +
                "Support Power 5 [This Creature gains 5 Power for each adjacent Overworld Creature you control.]";
        }
    }
}
