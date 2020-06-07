using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class RavanaughRidge : Location
    {
        public RavanaughRidge(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate)
            : base(sprite, background, overlay, negate, LocationType.Power)
        {
        }

        public override string Description()
        {
            return "Ravanaugh Ridge. Location. Initiative: Power. Card Ability: At the beginning of combat, if a player controls " +
                "an engaged Creature with Elemental Type Air, that player may look at the top three cards of their Attack Deck, put " +
                "one of them on top of that deck and the others on the bottom in any order.";
        }
    }
}