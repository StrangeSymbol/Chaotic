﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures.Mipedians
{
    public class Ubliqun : Creature
    {
        public Ubliqun(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2, false, false, false, false, 0,
            false, 0, 0, false, false, false, Tribe.Mipedian, CreatureType.Stalker)
        {
        }

        public override string Description()
        {
            return "Ubliqun Creature - Mipedian Stalker Courage: 35 Power: 45 Wisdom: 60 Speed: 60 Energy: 35 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Cost 1 Mugic Counter: Until the end of the turn, Ubliqun can move to any space on the board. " +
                "When this unseen warrior takes to the air, look out. But you'll see nothing...until it is too late!";
        }
    }
}