using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class StormTunnel : Location
    {
        public StormTunnel(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.Power)
        {
        }

        public override string Description()
        {
            return "Storm Tunnel. Location. Initiative: Power. Card Ability: Air attacks deal an additional 5 damage " +
                "Damage dealt by Water attacks is reduced by 5. " +
                "Even those who cannot fly do so here.";
        }
    }
}