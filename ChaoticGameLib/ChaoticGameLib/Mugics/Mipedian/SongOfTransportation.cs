using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfTransportation : Mugic, ICast
    {
        public SongOfTransportation(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Mipedian, 1) { }

        public override string Description()
        {
            return base.Description() + " Turn the top card of your Location Deck face up. " +
                "This becomes the active location." + 
                " The shortest distance between two points is not always a straight line.";
        }

        AbilityType ICast.Type
        {
            get { return AbilityType.ChangeLocation; }
        }
    }
}