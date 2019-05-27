using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Wamma : Creature, IHive
    {
        public Wamma(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0,
            true, false, false, false, false, false, Tribe.Danian, CreatureType.Mandiblor)
        {
        }

        public override string Description()
        {
            return "Wamma Creature - Danian Mandiblor Courage: 40 Power: 55 Wisdom: 30 Speed: 25 Energy: 50 Mugic Ability: 0" +
                " Elemental Type: Fire Creature Ability: " +
                "Hive: Wamma gains 5 Energy for each Mandiblor you control [Hive must be activated.] " +
            "Most Mandiblor are dilgent, dedicated warriors. Wamma is not like most Mandiblors.";
        }

        public void HiveOn(byte numMandiblor)
        {
            if (this.NumMandiblor != numMandiblor)
            {
                this.RemoveGainedEnergy((byte)(5 * this.NumMandiblor));
                this.Energy += (byte)(5 * numMandiblor);
                this.GainedEnergy += (byte)(5 * numMandiblor);
                this.NumMandiblor = numMandiblor;
            }
        }

        public void HiveOff(byte numMandiblor)
        {
            this.RemoveGainedEnergy((byte)(5 * this.NumMandiblor));
            this.NumMandiblor = 0;
        }
    }
}
