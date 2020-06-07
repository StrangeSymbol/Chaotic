using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Ekuud : Creature, IHive
    {
        public Ekuud(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, true, false,
            false, false, Tribe.Danian, CreatureType.Noble)
        {
        }

        public override string Description()
        {
            return "Ekuud Creature - Danian Noble Courage: 40 Power: 60 Wisdom: 20 Speed: 45 Energy: 50 Mugic Ability: 1" +
                " Elemental Type: Earth Creature Ability: " +
                "Hive: Ekuud gains 5 Energy for each Mandiblor you control [Hive must be activated.] " +
            "\"Fear the enemy, yes. But fear me more!\" -- Ekuud";
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
