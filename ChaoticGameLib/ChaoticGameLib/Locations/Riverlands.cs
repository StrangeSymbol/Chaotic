using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class Riverlands : Location
    {
        public Riverlands(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Wisdom)
        {
        }

        public override string Description()
        {
            return "Riverlands. Location. Initiative: Wisdom. Card Ability: If a Creature deals Water damage, Riverlands " +
                "heals 5 Energy to that Creature. " +
                "No one has yet to reach the source of the rushing waters, Could the Cothica be where it all begins?";
        }
    }
}