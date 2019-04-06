using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Mipedians
{
    public class Vinta : Creature
    {
        public Vinta(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, true, false, false, 0,
            false, 0, 10, false, false, false, Tribe.Mipedian, CreatureType.Stalker)
        {
        }

        public override string Description()
        {
            return "Vinta Creature - Mipedian Stalker Courage: 35 Power: 60 Wisdom: 35 Speed: 40 Energy: 35 Mugic Ability: 1" +
                " Elemental Type: Air Creature Ability: " +
                "Invisibility: Strike 10 [Add 10 damage to the first attack this Creature makes each combat.] " +
                "Many trespassers have felt the breath of Vinta as they breached the Mipedim Oasis. But they have felt it too late.";
        }
    }
}
