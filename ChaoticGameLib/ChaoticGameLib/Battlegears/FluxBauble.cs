using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class FluxBauble : Battlegear
    {
        public FluxBauble(Texture2D sprite, Texture2D overlay) : base(sprite, overlay) { }

        public override string Description()
        {
            return "Flux Bauble. Battlegear. At the beginning of combat, look at the top two " +
                "cards of your Location Deck. Put one of them on top of that deck and the other on the bottom.";
        }
    }
}
