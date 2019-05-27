using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Kebna : Creature, IHive
    {
        public Kebna(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, 
            true, false, false, false, false, false, Tribe.Danian, CreatureType.Battlemaster)
        {
        }

        public override string Description()
        {
            return "Kebna Creature - Danian Battlemaster Courage: 65 Power: 30 Wisdom: 55 Speed: 35 Energy: 45 Mugic Ability: 1" +
                " Elemental Type: Fire Creature Ability: " +
                "Hive: Kebna gains 5 Energy for each Mandiblor you control [Hive must be activated.] " +
            "There's fine line between bravery and insanity. Kebna crosses that line daily.";
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
