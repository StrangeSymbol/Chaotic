using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfSurprisal : Mugic
    {
        public SongOfSurprisal(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Danian, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target Creature gains \"Range\" and \"Swift 1\" until the end of the turn. " +
                "The usefulness of Mugic in battle is but a side effect. Its true purpose remains unknown.";
        }
    }
}