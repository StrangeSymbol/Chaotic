using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfDeflection : Mugic
    {
        public SongOfDeflection(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Mipedian, 3) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Change the target of target Mugic or ability with a single target." +
                " Beauty is in the ears of the beholder - even discordant, notes sound harmonious in dire situations.";
        }
    }
}