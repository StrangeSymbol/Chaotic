using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Rothar : Creature
    {
        public Rothar(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0,
            false, 5, 0, false, false, false, 0, 0, 0, 0, 0, 10, Tribe.UnderWorld, CreatureType.Commander)
        {
        }

        public override string Description()
        {
            return "Rothar Creature - UnderWorld Commander Courage: 75 Power: 95 Wisdom: 25 Speed: 50 Energy: 70 " +
                "Mugic Ability: 0 Elemental Type: Earth Creature Ability: " +
            "Intimidate Power 10 [Opposing Creature loses 10 Power until the end of combat.] " +
            "Recklessness 5 [When this Creature makes an attack, it deals 5 damage to itself.] " +
            "\"I don't run. I don't flinch. But I don't care, either.\" -- Rothar";
        }

        public void Ability(Creature c)
        {
            c.Power -= this.AbilityEnergy;
            c.PowerCombat += this.AbilityEnergy;
        }
    }
}
