using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class CascadeOfSymphony : Mugic
    {
        public CascadeOfSymphony(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.OverWorld, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Until the end of the turn, damage dealt by Air and Fire attacks to target Creature is " +
                "reduced by 5. \"There is but one true language, and that is Mugic. All else is gibberish.\" -- Donmar";
        }
    }
}
