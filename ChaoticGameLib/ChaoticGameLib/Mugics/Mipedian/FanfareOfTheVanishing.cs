using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class FanfareOfTheVanishing : Mugic, ICastTarget<Creature>
    {
        public FanfareOfTheVanishing(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Mipedian, 1) { }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            creature.Strike += 15;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Target Creature gains \"Invisibility: Strike 15\" until the end of the turn." +
                " When a melody becomes memory, it will never disappear.";
        }
    }
}
