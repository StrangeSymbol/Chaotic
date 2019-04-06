using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Mipedians
{
    public class Ario : Creature
    {
        public Ario(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, true, false, false, 0,
            false, 0, 15, false, false, false, Tribe.Mipedian, CreatureType.RoyalWarrior)
        {
        }

        public override string Description()
        {
            return "Ario Creature - Mipedian Royal Warrior Courage: 50 Power: 55 Wisdom: 25 Speed: 55 Energy: 40 Mugic Ability: 1" +
                " Elemental Type: Air Creature Ability: " +
                "Invisibility: Strike 15 [Add 15 damage to the first attack this Creature makes each combat.] " +
            "Though not in his prime, Ario is always primed for battle.";
        }
    }
}
