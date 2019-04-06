using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class UnderworldCity : Location
    {
        public UnderworldCity(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Speed)
        {
        }

        public override string Description()
        {
            return "Underworld City. Location. Initiative: Speed. Card Ability: When an UnderWorld Creature makes an attack, " +
                "that attack gains \"Challenge Power 15: Deal 5 damage.\" " +
                "Not known for his creativity, Chaor nevertheless won the capitol-naming contest for which he was also the judge.";
        }
    }
}