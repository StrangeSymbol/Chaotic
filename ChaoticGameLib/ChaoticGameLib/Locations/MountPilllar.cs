using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class MountPillar : Location
    {
        public MountPillar(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Wisdom)
        {
        }

        public override string Description()
        {
            return "Mount Pillar. Location. Initiative: Wisdom. Card Ability: When Mount Pillar becomes the active Location, " +
                "activate Hive until end of the turn. " +
                "The Danian homeland provides vital structural support to the UnderWorld ceiling, explaining why Chaor has " +
                "resisted its demolition.";
        }
    }
}