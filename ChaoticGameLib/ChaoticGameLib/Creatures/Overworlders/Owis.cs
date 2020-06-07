using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Owis : Creature, IActivateTarget<Creature>
    {
        public Owis(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, false, false, true, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 5, Tribe.OverWorld, CreatureType.Guardian)
        {
        }

        public override string Description()
        {
            return "Owis Creature - Overworld Guardian Courage: 65 Power: 25 Wisdom: 55 Speed: 30 Energy: 45 Mugic Ability: 1" +
                " Elemental Type: Water Creature Ability: " +
                "Pay 1 Mugic Ability: Heal 5 Energy to target Creature. Pay 1 Mugic Ability: Heal 10 Energy to Owis." +
            "UnderWorlders have made countless attempts to breach Cordac Falls. Owis has repelled all but one...the one that haunts him.";
        }

        public override bool CheckAbility(bool hive)
        {
            return this.MugicCounters >= this.MugicCost;
        }

        public override bool CheckAbilityTarget(Creature creature, bool sameOwner)
        {
            return creature.CheckHealable();
        }

        void IActivate.PayCost()
        {
            this.MugicCounters -= this.MugicCost;
        }

        void IActivateTarget<Creature>.Ability(Creature c)
        {
            if (this != c)
                c.Heal(this.AbilityEnergy);
            else
                c.Heal(10);
        }

        AbilityType IActivate.Type { get { return AbilityType.TargetCreature; } }
    }
}
