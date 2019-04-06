using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfFuturesight : Mugic
    {
        public SongOfFuturesight(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }

        public override string Description()
        {
            return base.Description() + " Look at the top two cards of target Attack or Location Deck." +
                " Put one of them on top of that deck and the other on the bottom." +
                " Take care when peering into the future. Sometimes it is best to not know what comes.";
        }
    }
}
