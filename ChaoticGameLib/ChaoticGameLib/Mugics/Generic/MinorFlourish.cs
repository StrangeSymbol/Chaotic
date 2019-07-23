using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class MinorFlourish : Mugic, ICastTarget<Creature>
    {
        public MinorFlourish(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }

        public override bool CheckPlayable(Creature creature)
        {
            return creature.CheckHealable();
        }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            creature.Heal(10);
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Heal 10 damage to target Creature." +
                " \"Anyone can imitate a melody, but to hear it in one's head for the very first time...that is a gift.\" -- Najarin";
        }
    }
}