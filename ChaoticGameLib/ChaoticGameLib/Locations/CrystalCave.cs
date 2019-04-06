using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class CrystalCave : Location
    {
        public CrystalCave(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Speed)
        {
        }

        public override string Description()
        {
            return "Crystal Cave. Location. Initiative: Speed. Card Ability: The engaged Creature with lowest Speed deals 0 " +
                "damage on its first attack. " +
                "The ancient Mugicians mined the hardest substance in all of Perim to protect their mystical melodies.";
        }
    }
}