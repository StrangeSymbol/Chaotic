﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Blazier : Creature, IActivate
    {
        public Blazier(Texture2D sprite, Texture2D overlay, Texture2D negate, byte energy, byte courage,
            byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 2,
            false, false, true, false, 0, false, 0, 0, false, false, false, Tribe.OverWorld, CreatureType.Scout)
        {
        }

        public override bool CheckAbility(bool hive)
        {
            return this.MugicCounters >= 1;
        }

        void IActivate.PayCost()
        {
            this.MugicCounters--;
        }

        AbilityType IActivate.Type { get { return AbilityType.TargetLocationDeck; } }

        public override string Description()
        {
            return "Blazier Creature - Overworld Scout Courage: 35 Power: 40 Wisdom: 60 Speed: 25 Energy: 40 Mugic Ability: 2" +
                " Elemental Type: Earth Creature Ability: " +
                "One Mugic Counter: Look at the top two cards in target Location Deck. " +
                "Put one of them on top of that deck and the other on the bottom." +
            "This small, scrappy spy maps enemy territory for the OverWorld's warriors.";
        }
    }
}