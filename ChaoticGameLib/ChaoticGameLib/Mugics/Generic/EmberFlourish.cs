using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class EmberFlourish : Mugic, ICastTarget<Creature>
    {
        public EmberFlourish(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Generic, 1) { }
       
        public override bool CheckPlayable(Creature creature)
        {
            return creature.CheckHealable() && (creature.Air || creature.Fire);
        }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            if (creature.Air || creature.Fire)
                creature.Heal(15);
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Heal 15 damage to target Creature with Elemental Type Air or Fire." +
                " No matter how many you carry with you, a song can never weigh you down.";
        }
    }
}
