using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Krekk : Creature
    {
        public Krekk(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0,
            true, 0, 0, false, false, false, 0, 0, 0, 0, 0, 10, Tribe.UnderWorld, CreatureType.CommanderScout)
        {
        }

        public override string Description()
        {
            return "Krekk Creature - UnderWorld Commander Scout Courage: 10 Power: 85 Wisdom: 20 Speed: 60 Energy: 40 " +
                "Mugic Ability: 0 Elemental Type: Fire Creature Ability: " +
                "Intimidate Power 10 [Opposing Creature loses 10 Power until the end of combat.] " +
            "Range [This Creature may move though occupied spaces.] " +
            "\"Fear is a great motivator. Just look at Krekk.\" -- Lord Van Bloot";
        }

        public void Ability(Creature c)
        {
            c.Power -= this.AbilityEnergy;
            c.PowerCombat += this.AbilityEnergy;
        }
    }
}
