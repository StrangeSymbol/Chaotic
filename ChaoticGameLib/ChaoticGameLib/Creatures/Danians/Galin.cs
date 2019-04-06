using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Danians
{
    class Galin : Creature
    {
        public Galin(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0,
            false, false, false, false, false, false, Tribe.Danian, CreatureType.Mandiblor)
        {
        }

        public override string Description()
        {
            return "Galin Creature - Danian Mandiblor Courage: 65 Power: 70 Wisdom: 40 Speed: 65 Energy: 50 " +
                "Mugic Ability: 0 Elemental Type: None Creature Ability: " +
                "Hive: Galin gains 10 Speed for each Mandiblor you control [Hive must be activated.] " +
            "Galin believes he can speak with his Danian ancestors, who have yet to tell him anything of much use.";
        }

        public void Ability(List<Creature> creatures, bool hive)
        {
            if (hive)
            {
                byte numMandiblor = NumMandiblorOnTeam(creatures);
                if (this.NumMandiblor != numMandiblor)
                {
                    sbyte difference = (sbyte)(numMandiblor - this.NumMandiblor);
                    this.Speed = (byte)(this.Speed + 10 * difference);
                    this.NumMandiblor = numMandiblor;
                }
            }
            else
                this.Speed -= (byte)(10 * this.NumMandiblor);
        }
    }
}
