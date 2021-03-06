﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class TangathToborn : Creature, IActivateSelf
    {
        public TangathToborn(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed)
            : base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, true, false, true, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 10, Tribe.OverWorld, CreatureType.StrategistWarrior) { }

        public override string Description()
        {
            return "Tangath Toborn Creature - Overworld Strategist Warrior Courage: 40 Power: 45 Wisdom: 40 Speed: 30 Energy: 30" +
                " Mugic Ability: 1 Elemental Type: Fire Earth Creature Ability: " +
                "Pay 1 Mugic Ability: Heal 10 Energy to Tangath Toborn. " +
            "\"I battle because I must. But in my heart, I know that war is not the answer.\" -- Tangath Toborn";
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
