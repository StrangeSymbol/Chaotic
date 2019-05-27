using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Junda : Creature
    {
        public Junda(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1,
            false, false, false, false, false, false, Tribe.Danian, CreatureType.SquadLeader)
        {
        }

        public override string Description()
        {
            return "Junda Creature - Danian Squadleader Courage: 35 Power: 55 Wisdom: 50 Speed: 40 Energy: 50 " +
                "Mugic Ability: 1 Elemental Type: None Creature Ability: " +
                "Cost 1 Mugic Counter: Activate Hive until the end of the turn." +
            "Every Mandiblor aspires to be a Squadleader. So, even while Junda faces the enemy, he's also watching his back.";
        }

        public void Ability(ref bool hive)
        {
            // Until end of turn.
            if (this.MugicCounters >= 1)
            {
                this.MugicCounters--;
                hive = true;
            }
        }
    }
}