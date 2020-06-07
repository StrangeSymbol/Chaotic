using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoticGameLib.Creatures;

namespace ChaoticGameLib
{
    public abstract class Mugic : ChaoticCard
    {
        // Holds the number of mugic counters needed to activate mugic
        byte cost;

        // Holds the type of Mugic card.
        MugicType type;

        public Mugic(Texture2D sprite, Texture2D overlay, Texture2D negate, MugicType type, byte cost) : base(sprite, overlay, negate)
        {
            this.type = type;
            this.cost = cost;
        }

        public byte Cost { get { return cost; } }
        public MugicType MugicCasting { get { return type; } }

        private byte additionalCost(Creature creature, Location location)
        {
            byte addCost = 0;
            if ((location is Locations.GlacierPlains && !location.Negate && this.type == MugicType.UnderWorld) ||
                (location is Locations.WoodenPillar && !location.Negate && this.type == MugicType.OverWorld))
                addCost++;
            return addCost;
        }

        public bool CheckCanPayMugicCost(Creature creature, Location location)
        {
            if (creature.MugicCounters >= cost + additionalCost(creature, location))
            {
                if (creature is Heptadd && !creature.Negate) // Heptadd can cast mugic from any tribe
                    return true;
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

        public virtual bool CheckPlayable(Creature creature)
        {
            return true;
        }

        public virtual void PayCost(Creature creature, Location location)
        {
            creature.MugicCounters -= (byte)(cost + additionalCost(creature, location));
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
