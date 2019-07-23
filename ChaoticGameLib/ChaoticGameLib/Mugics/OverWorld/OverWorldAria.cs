using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class OverWorldAria : Mugic, ICastTarget<Creature>
    {
        public OverWorldAria(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.OverWorld, 1) { }

        public override bool CheckPlayable(Creature creature)
        {
            return creature.CreatureTribe == Tribe.OverWorld && creature.CheckHealable();
        }

        void ICastTarget<Creature>.Ability(Creature c)
        {
            c.Heal(10);
            c.ReducedFireDamage = 5;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Heal 10 Energy to target OverWorld Creature. Until the end of the turn, damage dealt by " +
                "Fire attacks to that Creature is reduced by 5. Mugicians choose host Creatures along tribe affliations.";
        }
    }
}
