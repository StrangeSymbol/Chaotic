using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Malvadine : Creature
    {
        public Malvadine(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, false, false, false, false, 0,
            false, 0, 10, true, false, false, Tribe.Mipedian, CreatureType.EliteWarrior)
        {
        }

        public override string Description()
        {
            return "Malvadine Creature - Mipedian Elite Warrior Courage: 50 Power: 75 Wisdom: 40 Speed: 55 Energy: 50 Mugic Ability: 0" +
                " Elemental Type: None Creature Ability: " +
                "Invisibility: Strike 10 [Add 10 damage to the first attack this Creature makes each combat.] " +
                "Invisibility: Surprise [This Creature wins initiative checks against Creatures without invisibility.]";
        }

    }
}
