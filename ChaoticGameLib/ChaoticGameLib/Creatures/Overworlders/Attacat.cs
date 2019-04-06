using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Attacat : Creature
    {
        public Attacat(Texture2D sprite, Texture2D overlay, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0,
            false, false, false, false, 1, false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.GuardianWarrior)
        {
        }
        public override string Description()
        {
            return "Attacat Creature - Overworld Guardian Warrior Courage: 30 Power: 65 Wisdom: 60 Speed: 105 Energy: 50 Mugic Ability: 0" +
                " Elemental Type: None Creature Ability: " +
                "Swift 1 [This Creature may move one additional space.] " +
            "\"A ferocious warrior - when he finally gets up the nerve to battle.\"--Intress";
        }
    }
}
