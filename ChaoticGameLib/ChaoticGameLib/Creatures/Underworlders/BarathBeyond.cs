using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class BarathBeyond : Creature
    {
        public BarathBeyond(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, true, false, false, false, 0,
            false, 5, 0, false, false, false,
            5, 0, 0, 0, 0, 10, 0, 0, Tribe.UnderWorld, CreatureType.EtherealWarrior){}

        public override string Description()
        {
            return "Barath Beyond Creature - UnderWorld Ethereal Warrior Courage: 40 Power: 85 Wisdom: 15 Speed: 65 Energy: 60 " +
                "Mugic Ability: 1 Elemental Type: Fire Creature Ability: " +
                "Fire 5 [Fire attacks made by this Creature deal an additional 5 damage.] " +
            "Intimidate Power 10 [Opposing Creature loses 10 Power until the end of combat.] " +
            "Recklessness 5 [When this Creature makes an attack, it deals 5 damage to itself.]";
        }
    }
}
