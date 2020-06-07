using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Velreth : Creature
    {
        public Velreth(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed)
            : base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, false, false, 1,
            false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Warrior)
        {
        }

        public override string Description()
        {
            return "Velreth Creature - Overworld Warrior Courage: 55 Power: 80 Wisdom: 40 Speed: 25 Energy: 45 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Swift 1 [This Creature may move one additional space.] " +
            "Velreth is easy to find: just follow the path of destruction.";
        }
    }
}
