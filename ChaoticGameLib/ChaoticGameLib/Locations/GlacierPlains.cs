using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class GlacierPlains : Location
    {
        public GlacierPlains(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Power)
        { 
        }

        public override string Description()
        {
            return "Glacier Plains. Location. Initiative: Power. Card Ability: UnderWorld Mugic Cards cost 1 additional " +
                "UnderWorld Mugic to play. " +
                "This frozen, remote region is as harsh and forbidding as the UnderWorld's Lava Pond.";
        }
    }
}
