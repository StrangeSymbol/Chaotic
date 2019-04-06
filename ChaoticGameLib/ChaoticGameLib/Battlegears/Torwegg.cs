using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Torwegg : Battlegear
    {
        public Torwegg(Texture2D sprite, Texture2D overlay) : base(sprite, overlay ,5) { }
        public override void Equip(Creature creature)
        {
            creature.AirDamage += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.AirDamage -= this.DisciplineAmount;
        }
        
        public override string Description()
        {
            return "Torweg. Battlegear. Equipped Creature gains \"Air 5 " +
                "[Air attacks made by this Creature deal an additional 5 damage.]\"";
        }
    }
}
