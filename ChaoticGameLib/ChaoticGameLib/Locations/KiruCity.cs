using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class KiruCity : Location
    {
        public KiruCity(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Wisdom)
        {
        }

        public override string Description()
        {
            return "Kiru City. Location. Initiative: Wisdom. Card Ability: OverWorld Creatures gain 10 Energy. " +
                "The majestic capital of the OverWorld is named in the memory of one of the greatest leaders";
        }
    }
}