using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Dardemus : Creature
    {
        public Dardemus(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0,
            false, 0, 0, false, false, false,
            0, 0, 0, 0, 10, 0, 0, 0, Tribe.UnderWorld, CreatureType.Taskmaster)
        {
        }

        public override string Description()
        {
            return "Dardemus Creature - UnderWorld Taskmaster Courage: 60 Power: 75 Wisdom: 20 Speed: 65 Energy: 50 " +
                "Mugic Ability: 0 Elemental Type: Fire Creature Ability: " +
                "Intimidate Courage 10 [Opposing Creature loses 10 Courage until the end of combat.]" +
            "This UnderWorld prison guard has a simple policy that works: whip first, whip fast, whip last.";
        }

        public void Ability(Creature c)
        {
            c.Courage -= this.IntimidateCourage;
            c.CourageCombat += this.IntimidateCourage;
        }
    }
}
