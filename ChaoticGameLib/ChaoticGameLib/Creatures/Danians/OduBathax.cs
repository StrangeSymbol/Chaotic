using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class OduBathax : Creature
    {
        public OduBathax(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1,
            false, false, true, false, false, false, Tribe.Danian, CreatureType.Warrior)
        {
        }

        public override string Description()
        {
            return "Odu-Bathax Creature - Danian Warrior Courage: 45 Power: 60 Wisdom: 40 Speed: 45 Energy: 30 Mugic Ability: 1" +
                " Elemental Type: Earth Creature Ability: " +
                "Odu-Bathax gains 5 Energy for each Mandiblor you control " +
            "Odu-Bathax is the defender of the North Gate at Mount Pillar.";
        }

        public void Ability(byte numMandiblor)
        {
            if (this.NumMandiblor != numMandiblor)
            {
                this.RemoveGainedEnergy((byte)(5 * this.NumMandiblor));
                this.Energy += (byte)(5 * numMandiblor);
                this.GainedEnergy += (byte)(5 * numMandiblor);
                this.NumMandiblor = numMandiblor;
            }
        }
    }
}
