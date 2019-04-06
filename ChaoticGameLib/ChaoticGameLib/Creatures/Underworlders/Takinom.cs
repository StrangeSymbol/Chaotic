using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Takinom : Creature
    {
        public Takinom(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, false, 1,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 0, 25, Tribe.UnderWorld, CreatureType.Commander)
        {
        }

        public override string Description()
        {
            return "Takinom Creature - UnderWorld Commander Courage: 60 Power: 65 Wisdom: 20 Speed: 95 Energy: 40 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Sacrifice an UnderWorld Creature: Heal 25 Energy to Takinom. Swift 1 " +
            "Takinom unearthed a shocking secret bond with her archenemy Intress that she's wisely kept from Chaor...";
        }

        public void Ability(Creature c)
        {
            c.Alive = false;
            Heal(this.AbilityEnergy);
        }
    }
}
