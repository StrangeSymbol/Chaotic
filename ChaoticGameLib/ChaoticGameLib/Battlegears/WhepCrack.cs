﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class WhepCrack : Battlegear
    {
        public WhepCrack(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 15) { }
        public override void Equip(Creature creature)
        {
            creature.Power += this.DisciplineAmount;
            if (creature.CreatureTribe == Tribe.OverWorld)
                creature.FireDamage += 5;
        }
        public override void UnEquip(Creature creature)
        {
            creature.Power -= this.DisciplineAmount;
            if (creature.CreatureTribe == Tribe.OverWorld)
                creature.FireDamage -= 5;
        }

        public override string Description()
        {
            return "Whep Crack. Battlegear. Equipped Creature gains 15 Power. " +
                "If equipped Creature is UnderWorld, it gains \"Fire 5\"";
        }
    }
}