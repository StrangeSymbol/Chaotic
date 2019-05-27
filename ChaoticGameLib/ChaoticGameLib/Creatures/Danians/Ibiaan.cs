using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Ibiaan : Creature, IHive
    {
        public Ibiaan(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0,
            false, false, true, false, false, false, Tribe.Danian, CreatureType.Mandiblor)
        {
        }

        public override string Description()
        {
            return "Ibiaan Creature - Danian Mandiblor Courage: 45 Power: 60 Wisdom: 65 Speed: 30 Energy: 50 " +
                "Mugic Ability: 0 Elemental Type: Earth Creature Ability: " +
                "Hive: Ibiaan gains 5 Wisdom for each Mandiblor you control [Hive must be activated.] " +
            "\"If Ibiaan's so smart, why is he still a Mandiblor?\" -- Hota";
        }

        public void HiveOn(byte numMandiblor)
        {
            if (this.NumMandiblor != numMandiblor)
            {
                sbyte difference = (sbyte)(numMandiblor - this.NumMandiblor);
                this.Wisdom = (byte)(this.Wisdom + 5 * difference);
                this.NumMandiblor = numMandiblor;
            }
        }

        public void HiveOff(byte numMandiblor)
        {
            this.Speed -= (byte)(5 * this.NumMandiblor);
            this.NumMandiblor = 0;
        }
    }
}