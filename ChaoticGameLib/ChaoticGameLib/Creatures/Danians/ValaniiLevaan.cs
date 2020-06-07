using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class ValaniiLevaan : Creature, IHive
    {
        public ValaniiLevaan(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1,
            false, false, true, false, false, false, Tribe.Danian, CreatureType.Noble)
        {
        }

        public override string Description()
        {
            return "Valanii Levaan Creature - Danian Noble Courage: 50 Power: 50 Wisdom: 15 Speed: 25 Energy: 50 " +
                "Mugic Ability: 1 Elemental Type: Earth Creature Ability: " +
                "Hive: Vlanii Levaan gains 10 Power for each Mandiblor you control [Hive must be activated.] " +
            "When his Squadleaders don't execute orders, Valanii Levaan orders executions.";
        }

        public void HiveOn(byte numMandiblor)
        {
            if (this.NumMandiblor != numMandiblor)
            {
                sbyte difference = (sbyte)(numMandiblor - this.NumMandiblor);
                this.Power = (byte)(this.Power + 10 * difference);
                this.NumMandiblor = numMandiblor;
            }
        }

        public void HiveOff(byte numMandiblor)
        {
            this.Power -= (byte)(10 * this.NumMandiblor);
            this.NumMandiblor = 0;
        }
    }
}

