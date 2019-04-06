using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class StrongholdMorn : Location
    {
        public StrongholdMorn(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, true, LocationType.Speed)
        {
        }

        public override string Description()
        {
            return "Stronghold Morn. Location. Initiative: Speed. Card Ability: At the beginning of combat, both engaged Creatures " +
                "gain Elemental Types Fire, Water, Air, and Earth. Unique";
        }
    }
}