using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class CastlePillar : Location
    {
        public CastlePillar(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, true, LocationType.Courage)
        {
        }

        public override string Description()
        {
            return "Castle Pillar. Location. Initiative: Courage. Card Ability: At the beginning of combat, the engaged Creature " +
                "with the highest Wisdom gains one mugic counter. Unique";
        }
    }
}