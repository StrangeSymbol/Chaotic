using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Mallash : Creature, IHive
    {
        public Mallash(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2,
            false, false, true, false, false, false, Tribe.Danian, CreatureType.Battlemaster)
        {
        }

        public override string Description()
        {
            return "Mallash Creature - Danian Battlemaster Courage: 30 Power: 25 Wisdom: 45 Speed: 35 Energy: 30 Mugic Ability: 2" +
                " Elemental Type: Earth Creature Ability: " +
                "Hive: Mallash gains 5 Energy for each Mandiblor you control [Hive must be activated.] " +
            "\"Irresponsible? It's self-preservation.\" -- Mallash, rebuffing critics of his \"Attack first, make-sure-it's-an-enemy-later\" policy";
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