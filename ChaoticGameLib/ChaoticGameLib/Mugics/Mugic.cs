﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoticGameLib.Creatures;

namespace ChaoticGameLib
{
    public abstract class Mugic : ChaoticCard
    {
        // Holds the value of mugic counters needed to activate mugic
        byte cost;

        // Holds the type of Mugic card.
        MugicType type;

        public Mugic(Texture2D sprite, Texture2D overlay, MugicType type, byte cost) : base(sprite, overlay)
        {
            this.type = type;
            this.cost = cost;
        }

        public byte Cost { get { return cost; } }
        public MugicType MugicCasting { get { return type; } }

        public bool CheckCanPayMugicCost(Creature creature)
        {
            if (creature.MugicCounters >= cost)
            {
                switch (type)
                {
                    case MugicType.Generic:
                        return true;
                    case MugicType.OverWorld:
                        return creature.CreatureTribe == Tribe.OverWorld;
                    case MugicType.UnderWorld:
                        return creature.CreatureTribe == Tribe.UnderWorld;
                    case MugicType.Mipedian:
                        return creature.CreatureTribe == Tribe.Mipedian;
                    case MugicType.Danian:
                        return creature.CreatureTribe == Tribe.Danian;
                }
            }
            return false;
        }

        public virtual void Ability(Creature creature)
        {
            creature.MugicCounters -= cost;
        }

        public override string ToString()
        {
            return this.Name + " " + this.type + " Mugic " + this.cost;
        }

        public override string Description()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder(this.Name);
            builder.Append(" - ");
            builder.Append(this.type);
            builder.Append(" - Mugic Cost: ");
            builder.Append(this.cost);
            return builder.ToString();
        }
    }
}