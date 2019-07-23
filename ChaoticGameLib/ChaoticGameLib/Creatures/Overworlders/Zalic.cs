using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Zalic : Creature, IActivateSelf
    {
        public Zalic(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2, false, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 15, Tribe.OverWorld, CreatureType.Guardian)
        {
        }

        public override string Description()
        {
            return "Zalic Creature - Overworld Guardian Courage: 45 Power: 55 Wisdom: 40 Speed: 40 Energy: 45 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Pay 1 Mugic Ability: Heal 15 Energy to Zalic. This confidant of Intress never sleeps, so Maxxor " +
            "has appointed him guardian of the Passage between the OverWorld and the UnderWorld.";
        }

        public override bool CheckAbility(bool hive)
        {
            return this.MugicCounters >= this.MugicCost && this.CheckHealable();
        }

        void IActivate.PayCost()
        {
            this.MugicCounters -= this.MugicCost;
        }

        void IActivateSelf.Ability()
        {
            Heal(this.AbilityEnergy);
        }

        AbilityType IActivate.Type { get { return AbilityType.TargetSelf; } }
    }
}
