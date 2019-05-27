using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Skartalas : Creature
    {
        public Skartalas(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2,
            false, false, false, false, false, false, Tribe.Danian, CreatureType.Controller)
        {
        }

        public override string Description()
        {
            return "Skartalas Creature - Danian Controller Courage: 55 Power: 45 Wisdom: 40 Speed: 55 Energy: 40 Mugic Ability: 2" +
                " Elemental Type: Npne Creature Ability: " +
                "Sacrifice Skartalas: Activate Hive until the end of the turn. " +
            "It's rumored that Skartalas has had secret dealings with Van Bloot and that their bitter feud is an act to hide an alliance.";
        }

        public void Ability(ref bool hive)
        {
            // Until end of turn
            hive = true;
            this.Alive = false;
        }
    }
}