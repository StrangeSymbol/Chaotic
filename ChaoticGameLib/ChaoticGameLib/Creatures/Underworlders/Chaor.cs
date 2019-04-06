using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Chaor : Creature
    {
        public Chaor(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 3, true, false, false, false, 0,
            false, 0, 0, false, true, true, Tribe.UnderWorld, CreatureType.ConquerorWarrior)
        {
        }

        public override string Description()
        {
            return "Chaor Creature - UnderWorld Conqueror Warrior Courage: 95 Power: 90 Wisdom: 70 Speed: 60 Energy: 70 " +
                "Mugic Ability: 3 Elemental Type: Fire Creature Ability: " +
                "Pay 3 Mugic Ability: Dispel target OverWorld Mugic or ability targeting Chaor. Chaor may not enter mixed armies. Unique " +
            "\"We'll take Kiru City and the UnderWorld will rule all of Perim!\" -- Chaor";
        }

        // TODO: Implement ability
    }
}
