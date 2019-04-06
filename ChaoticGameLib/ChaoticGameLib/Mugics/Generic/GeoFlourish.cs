using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class GeoFlourish : Mugic 
    {
        public GeoFlourish(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }
        public override void Ability(Creature creature)
        {
            if (creature.Earth || creature.Water)
                creature.Heal(15);
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Heal 15 damage to target Creature with Elemental Type Earth or Water." +
                " Protect the land and the land will protect you.";
        }
    }
}
