using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class FearValley : Location
    {
        public FearValley(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Wisdom)
        {
        }

        public override string Description()
        {
            return "Fear Valley. Location. Initiative: Wisdom. Card Ability: At the beginning of combat, the engaged Creature with " +
                "the lowest Courage loses 10 Energy. " +
                "One night here means a lifetime of nightmares.";
        }
    }
}