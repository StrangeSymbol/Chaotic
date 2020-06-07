using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class RunicGrove : Location
    {
        public RunicGrove(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Courage)
        {
        }

        public override string Description()
        {
            return "Runic Grove. Location. Initiative: Courage. Card Ability: Only Generic Mugic may be played. " +
                "Secret symbols or a simple secret? Many have been tantalized by what remains indecipherable to this day.";
        }
    }
}