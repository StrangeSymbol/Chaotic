using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class ForestOfLife : Location
    {
        public ForestOfLife(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Wisdom)
        {
        }

        public override string Description()
        {
            return "Forest Of Life. Location. Initiative: Wisdom. Card Ability: At the beginning of combat, the engaged Creature with " +
                "the highest Power gains 5 Energy.(If both Creatures have the same Power, there is no effect). " +
                "A place of perpetuity: nothing is born or perishes within.";
        }
    }
}