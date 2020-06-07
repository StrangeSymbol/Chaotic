using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class MelodyOfMirage : Mugic, ICastTarget<Attack>
    {
        public MelodyOfMirage(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Mipedian, 1) { }

        void ICastTarget<Attack>.Ability(Attack attack)
        {
            attack.DealZero = true;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetAttack; } }

        public override string Description()
        {
            return base.Description() + " Target attack deals 0 damage." +
                " Trust your ears, not your eyes.";
        }
    }
}
