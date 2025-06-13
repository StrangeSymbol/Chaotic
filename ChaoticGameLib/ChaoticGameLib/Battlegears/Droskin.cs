using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Droskin : Battlegear, ISacrificeTarget<Attack>
    {
        public Droskin(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate) { }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return this.IsFaceUp;
        }

        void ISacrificeTarget<Attack>.Ability(Attack c)
        {
            c.Negate = true;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetAttack; } }

        public override string Description()
        {
            return "Droskin. Battlegear. Sacrifice Droskin: Negate target attack. " + 
                "Play this ability only while equipped Creature is engaged. " +
                "The Droskin is rumored to be able to absorb even the most devasting attacks.";
        }
    }
}
