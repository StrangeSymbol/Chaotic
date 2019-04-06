using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class OrbOfForesight : Battlegear
    {
        public OrbOfForesight(Texture2D sprite, Texture2D overlay) : base(sprite, overlay) { }

        public override string Description()
        {
            return "Orb Of Foresight. Battlegear. At the beginning of combat, look at the top three " +
                "cards of your Attack Deck. Put one of them on top of that deck and the other two on the bottom in any order.";
        }
    }
}
