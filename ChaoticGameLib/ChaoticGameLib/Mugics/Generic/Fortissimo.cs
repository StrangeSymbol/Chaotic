﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class Fortissimo : Mugic
    {
        public Fortissimo(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }
        public override void Ability(Creature creature)
        {
            creature.Energy += 5;
            creature.Courage += 5;
            creature.CourageTurn += 5;
            creature.Power += 5;
            creature.PowerTurn += 5;
            creature.Wisdom += 5;
            creature.WisdomTurn += 5;
            creature.Speed += 5;
            creature.SpeedTurn += 5;
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target Creature gains 5 Courage, Power, Wisdom, Speed, and Energy until end of the turn." +
                " Large is good, Larger is better. But really, really big is sometimes best.";
        }
    }
}