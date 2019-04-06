using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class BorthMajar : Creature
    {
        public BorthMajar(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0,
            false, 0, 0, false, false, false,
            0, 0, 0, 0, 0, 10, Tribe.UnderWorld, CreatureType.Commander) { }

        public override string Description()
        {
            return "Borth-Majar Creature - UnderWorld Commander Courage: 45 Power: 90 Wisdom: 85 Speed: 40 Energy: 45 " +
                "Mugic Ability: 1 Elemental Type: None Creature Ability: " +
                "Intimidate Power 10 [Opposing Creature loses 10 Power until the end of combat.] " +
            "Intimidate Wisdom 10 [Opposing Creature loses 10 Wisdom until the end of combat.] " +
            "Brains or brawn? One can't decide...and one doesn't need to.";
        }

        public void Ability(Creature c)
        {
            c.Power -= this.AbilityEnergy;
            c.PowerCombat += this.AbilityEnergy;
            c.Wisdom -= this.AbilityEnergy;
            c.WisdomCombat += this.AbilityEnergy;
        }
    }
}
