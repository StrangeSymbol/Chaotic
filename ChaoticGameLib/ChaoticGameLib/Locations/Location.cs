﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib
{
    public abstract class Location : ChaoticCard
    {
        // Holds the type of initiative to determine who goes first
        LocationType initiative;

        // Holds the active location background image.
        Texture2D background;

        // Used to signal when a location is negated.
        bool prevNegate;

        public Location(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate, LocationType initiative)
            : this(sprite, background, overlay, negate, false, initiative)
        { 
        }

        public Location(Texture2D sprite, Texture2D background, Texture2D overlay, Texture2D negate, bool unique, LocationType initiative) 
            : base(sprite, overlay, negate, unique) 
        {
            this.initiative = initiative;
            this.background = background;
            this.prevNegate = false;
        }

        public LocationType Initiative { get { return initiative; } }
        public Texture2D Background { get { return background; } }
        public bool PrevNegate { get { return prevNegate; } set { prevNegate = value; } }

        /// <summary>
        /// Determines who attacks first
        /// </summary>
        /// <param name="c1">The Players Creature who's active location it is.</param>
        /// <param name="c2">The non-active players Creature</param>
        /// <returns>Which Creature won initiative check.</returns>
        public int initiativeCheck(Creature c1, Creature c2)
        {
            if (!c1.Negate && c1.Surprise && !c2.Invisibility() &&
                !(c2.Battlegear != null && c2.Battlegear is ChaoticGameLib.Battlegears.SpectralViewer))
                return 1;
            else if (!c2.Negate && !c1.Invisibility() && c2.Surprise &&
                !(c1.Battlegear != null && c1.Battlegear is ChaoticGameLib.Battlegears.SpectralViewer))
                return -1;
            switch(initiative)
            {
                case LocationType.Courage:
                    return c1.Courage.CompareTo(c2.Courage);
                case LocationType.Power:
                    return c1.Power.CompareTo(c2.Power);
                case LocationType.Wisdom:
                    return c1.Wisdom.CompareTo(c2.Wisdom);
                case LocationType.Speed:
                    return c1.Speed.CompareTo(c2.Speed);
                case LocationType.OverWorld:
                    if (c1.CreatureTribe == Tribe.OverWorld && c2.CreatureTribe == Tribe.OverWorld)
                        return 0;
                    else if (c1.CreatureTribe == Tribe.OverWorld && c2.CreatureTribe != Tribe.OverWorld)
                        return 1;
                    else
                        return -1;
                case LocationType.UnderWorld:
                    if (c1.CreatureTribe == Tribe.UnderWorld && c2.CreatureTribe == Tribe.UnderWorld)
                        return 0;
                    else if (c1.CreatureTribe == Tribe.UnderWorld && c2.CreatureTribe != Tribe.UnderWorld)
                        return 1;
                    else
                        return -1;
                case LocationType.Mipedian:
                    if (c1.CreatureTribe == Tribe.Mipedian && c2.CreatureTribe == Tribe.Mipedian)
                        return 0;
                    else if (c1.CreatureTribe == Tribe.Mipedian && c2.CreatureTribe != Tribe.Mipedian)
                        return 1;
                    else
                        return -1;
                case LocationType.Danian:
                    if (c1.CreatureTribe == Tribe.Danian && c2.CreatureTribe == Tribe.Danian)
                        return 0;
                    else if (c1.CreatureTribe == Tribe.Danian && c2.CreatureTribe != Tribe.Danian)
                        return 1;
                    else
                        return -1;
                default:
                    throw new InitiativeCheckException("The initiative check has no type equvalent");
            }
        }

        public override string ToString()
        {
            return this.Name + " Location Initiative: " + initiative;
        }
    }
}
