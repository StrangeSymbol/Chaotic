using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class DiamondOfVlaric : Battlegear, ISacrificeTarget<Creature>
    {
        public DiamondOfVlaric(Texture2D texture, Texture2D overlay, Texture2D negate) : base(texture, overlay, negate) { }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return this.IsFaceUp && !creatureEquipped.Earth;
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            c.Earth = true;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetEquipped; } }

        public override string Description()
        {
            return "Diamond Of Vlaric. Battlegear. Sacrifice Diamond Of Vlaric: Equipped Creature " +
                "gains Elemental Type Earth until the end of combat.";
        }
    }
}
