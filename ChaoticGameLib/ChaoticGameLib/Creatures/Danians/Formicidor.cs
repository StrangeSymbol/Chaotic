using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Formicidor : Creature, IHive
    {
        public Formicidor(Texture2D sprite, Texture2D overlay, Texture2D negate,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false,
            true, false, false, false, Tribe.Danian, CreatureType.SquadleaderScout)
        {
        }

        public override string Description()
        {
            return "Formicidor Creature - Danian Squadleader Scout Courage: 60 Power: 40 Wisdom: 35 Speed: 40 Energy: 40 " +
                "Mugic Ability: 1 Elemental Type: Earth Creature Ability: " +
                "Hive: Formicidor gains 10 Courage for each Mandiblor you control [Hive must be activated.] " +
            "Because he dares to roam the UnderWorld, Formicidor knows secrets even some UnderWorlders do not.";
        }

        public void HiveOn(byte numMandiblor)
        {
            if (this.NumMandiblor != numMandiblor)
            {
                sbyte difference = (sbyte)(numMandiblor - this.NumMandiblor);
                this.Courage = (byte)(this.Courage + 10 * difference);
                this.NumMandiblor = numMandiblor;
            }
        }

        public void HiveOff(byte numMandiblor)
        {
            this.Courage -= (byte)(10 * this.NumMandiblor);
            this.NumMandiblor = 0;
        }
    }
}

