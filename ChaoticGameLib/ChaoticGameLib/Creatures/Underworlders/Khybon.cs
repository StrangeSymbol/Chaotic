using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Khybon : Creature
    {
        public Khybon(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 10, 0, 10, 0, Tribe.UnderWorld, CreatureType.Taskmaster)
        {
        }

        public override string Description()
        {
            return "Khybon Creature - UnderWorld Taskmaster Courage: 75 Power: 25 Wisdom: 85 Speed: 20 Energy: 40 " +
                "Mugic Ability: 1 Elemental Type: None Creature Ability: " +
                "Intimidate Courage 10 [Opposing Creature loses 10 Courage until the end of combat.] " +
            "Intimidate Wisdom 10 [Opposing Creature loses 10 Wisdom until the end of combat.] " +
            "\"Yeah, I could build that.\" -- Khybon";
        }

        public void Ability(Creature c)
        {
            c.Courage -= this.IntimidateCourage;
            c.CourageCombat += this.IntimidateCourage;
            c.Wisdom -= this.IntimidateWisdom;
            c.WisdomCombat += this.IntimidateWisdom;
        }
    }
}
