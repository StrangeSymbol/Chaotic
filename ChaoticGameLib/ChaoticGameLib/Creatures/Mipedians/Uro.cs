using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Mipedians
{
    public class Uro : Creature
    {
        public Uro(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, false, false, false, false, 0,
            false, 0, 5, true, false, false, Tribe.Mipedian, CreatureType.Elementalist)
        {
        }

        public override string Description()
        {
            return "Uro Creature - Mipedian Elementalist Courage: 60 Power: 60 Wisdom: 45 Speed: 60 Energy: 45 Mugic Ability: 0" +
                " Elemental Type: None Creature Ability: " +
                "Invisibility: Strike 5 [Add 5 damage to the first attack this Creature makes each combat.] " +
                "Invisibility: Surprise [This Creature wins initiative checks against Creatures without invisibility.]";
        }
    }
}
