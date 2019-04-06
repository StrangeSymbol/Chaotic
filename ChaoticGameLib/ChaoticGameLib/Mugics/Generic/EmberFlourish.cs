using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class EmberFlourish : Mugic
    {
        public EmberFlourish(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }
        public override void Ability(Creature creature)
        {
            if (creature.Air || creature.Fire)
                creature.Heal(15);
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Heal 15 damage to target Creature with Elemental Type Air or Fire." +
                " No matter how many you carry with you, a song can never weigh you down.";
        }
    }
}
