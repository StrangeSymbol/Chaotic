using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class EyeOfTheMaelstrom : Location
    {
        public EyeOfTheMaelstrom(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Power)
        {
        }

        public override string Description()
        {
            return "Eye Of The Maelstrom. Location. Initiative: Power. Card Ability: When Eye of the Maelstrom becomes the active " +
                "Location, both players discard 1 Mugic Card. " +
                "No wind, no sound - just madness.";
        }
    }
}