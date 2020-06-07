using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class CanonOfCasualty : Mugic, ICastTarget<Creature>
    {
        public CanonOfCasualty(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.UnderWorld, 1) { }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            creature.Energy -= 20;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Deal 20 damage to target Creature. " +
                "Even before the first three notes finish playing, OverWorlders scramble for the nearest shelter.";
        }
    }
}
