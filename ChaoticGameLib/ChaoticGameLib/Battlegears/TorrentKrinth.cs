﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class TorrentKrinth : Battlegear
    {
        public TorrentKrinth(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, 5) { }
        public override void Equip(Creature creature)
        {
            creature.WaterDamage += this.DisciplineAmount;
            creature.WaterDamageGained += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.WaterDamage -= (byte)(creature.WaterDamage == 0 ? 0 : this.DisciplineAmount);
            creature.WaterDamageGained -= this.DisciplineAmount;
        }

        public override string Description()
        {
            return "Torrent Krinth. Battlegear. Equipped Creature gains \"Water 5 " +
                "[Water attacks made by this Creature deal an additional 5 damage.]\"";
        }
    }
}
