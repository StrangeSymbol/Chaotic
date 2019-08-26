using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfDeflection : Mugic, ICast
    {
        public SongOfDeflection(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Mipedian, 3) { }

        public override string Description()
        {
            return base.Description() + " Change the target of target Mugic or ability with a single target." +
                " Beauty is in the ears of the beholder - even discordant, notes sound harmonious in dire situations.";
        }

        AbilityType ICast.Type
        {
            get { return AbilityType.ChangeTarget; }
        }
    }
}