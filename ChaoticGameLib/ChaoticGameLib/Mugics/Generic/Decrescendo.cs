using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class Decrescendo : Mugic
    {
        public Decrescendo(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }
        public override void Ability(Creature creature)
        {
            creature.Energy -= 5;
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Deal 5 damage to target Creature. Minimalism to the max.";
        }
    }
}
