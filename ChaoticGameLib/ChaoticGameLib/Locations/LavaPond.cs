using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class LavaPond : Location
    {
        public LavaPond(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Speed)
        {
        }

        public override string Description()
        {
            return "Lava Pond. Location. Initiative: Speed. Card Ability: Fire attacks deal additional 5 damage. " +
                "Damage dealt by Air attacks is reduced by 5. " +
                "Magmon gains \"Fire 5\"";
        }
    }
}