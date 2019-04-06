using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Locations
{
    public class CordacFalls : Location
    {
        public CordacFalls(Texture2D sprite, Texture2D background, Texture2D overlay)
            : base(sprite, background, overlay, LocationType.OverWorld)
        {
        }

        public override string Description()
        {
            return "Cordac Falls. Location. Initiative: Overworld. Card Ability: At the beginning of combat, all engaged Creatures " +
                "gain 5 Energy until the end of the turn. " +
                "Cordac's teardrops plunge into the abyss with nary a splash or spray...";
        }
    }
}