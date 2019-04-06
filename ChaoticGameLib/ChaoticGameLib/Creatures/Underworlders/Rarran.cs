using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Rarran : Creature
    {
        public Rarran(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0,
            true, 0, 0, false, false, false, Tribe.UnderWorld, CreatureType.Scout)
        {
        }

        public override string Description()
        {
            return "Rarran Creature - UnderWorld Scout Courage: 65 Power: 60 Wisdom: 30 Speed: 60 Energy: 50 Mugic Ability: 0" +
                " Elemental Type: Fire Creature Ability: " +
                "Range [This Creature may move though occupied spaces.] " +
            "A prison guard with Dardemus and Miklori, Rarran has not revealed his secret to them: he is one of Mommark's creations.";
        }
    }
}
