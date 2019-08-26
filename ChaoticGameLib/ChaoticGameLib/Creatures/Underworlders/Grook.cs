using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Grook : Creature
    {
        public Grook(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 0, 10, 0, 0, Tribe.UnderWorld, CreatureType.Taskmaster)
        {
        }

        public override string Description()
        {
            return "Grook Creature - UnderWorld Taskmaster Courage: 30 Power: 100 Wisdom: 20 Speed: 60 Energy: 50 " +
                "Mugic Ability: 0 Elemental Type: Fire Creature Ability: " +
                "Intimidate Power 10 [Opposing Creature loses 10 Power until the end of combat.] " +
            "\"Mugic -- Bah! What can a Mugician do that I can't do with my fists?\" -- Grook";
        }

        public void Ability(Creature c)
        {
            c.Power -= this.IntimidatePower;
            c.PowerCombat += this.IntimidatePower;
        }
    }
}
