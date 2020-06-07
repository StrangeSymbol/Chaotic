using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Pyroblaster : Battlegear, ISacrificeTarget<Creature>
    {
        public Pyroblaster(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate) { }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return this.IsFaceUp && !creatureEquipped.Fire;
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            c.Fire = true;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetEquipped; } }

        public override string Description()
        {
            return "Pyroblaster. Battlegear. Sacrifice Pyroblaster: Equipped Creature gains Elemental Type Fire until the end of combat.";
        }
    }
}
