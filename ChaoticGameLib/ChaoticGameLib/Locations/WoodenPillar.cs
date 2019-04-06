using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class WoodenPillar : Location
    {
        public WoodenPillar(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Wisdom)
        {
        }

        public override string Description()
        {
            return "Wooden Pillar. Location. Initiative: Wisdom. Card Ability: OverWorld Mugic Cards cost 1 additional Mugic " +
                "Ability to play. " +
                "The Runic Grove takes root here, and UnderWorlders believe it grows even further down than up.";
        }
    }
}