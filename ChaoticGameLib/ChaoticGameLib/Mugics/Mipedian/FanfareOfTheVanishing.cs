using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class FanfareOfTheVanishing : Mugic
    {
        public FanfareOfTheVanishing(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Mipedian, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target Creature gains \"Invisibility: Strike 15\" until the end of the turn." +
                " When a melody becomes memory, it will never disappear.";
        }
    }
}
