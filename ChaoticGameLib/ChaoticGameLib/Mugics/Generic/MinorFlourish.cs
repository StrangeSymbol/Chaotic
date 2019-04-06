using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class MinorFlourish : Mugic
    {
        public MinorFlourish(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }
        public override void Ability(Creature creature)
        {
            creature.Heal(10);
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Heal 10 damage to target Creature." +
                " \"Anyone can imitate a melody, but to hear it in one's head for the very first time...that is a gift.\" -- Najarin";
        }
    }
}