using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Danians
{
    class Ibiaan : Creature
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

        public void Ability(System.Collections.Generic.List<Creature> creatures, bool hive)
        {
            if (hive)
            {
                byte numMandiblor = NumMandiblorOnTeam(creatures);
                if (this.NumMandiblor != numMandiblor)
                {
                    sbyte difference = (sbyte)(numMandiblor - this.NumMandiblor);
                    this.Wisdom = (byte)(this.Wisdom + 5 * difference);
                    this.NumMandiblor = numMandiblor;
                }
            }
            else
                this.Wisdom -= (byte)(10 * this.NumMandiblor);
        }
    }
}