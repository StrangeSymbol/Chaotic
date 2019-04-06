using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfAsperity : Mugic
    {
        public SongOfAsperity(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.UnderWorld, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Until the end of the turn, target Creature gains \"Fire 5\" and \"Air 5\". " +
                "When anger is all-consuming, hatred is the gateway to a hideous power.";
        }
    }
}
