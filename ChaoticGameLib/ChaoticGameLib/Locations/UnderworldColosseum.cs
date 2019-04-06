using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class UnderworldColosseum : Location
    {
        public UnderworldColosseum(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Speed)
        {
        }

        public override string Description()
        {
            return "Underworld Colosseum. Location. Initiative: Speed. Card Ability: All Creatures with Elemental Type Fire " +
                "deal an additional 10 damage on their first attack. " +
                "UnderWorlders have a simple way of settling arguments here: the one left standing is always right.";
        }
    }
}