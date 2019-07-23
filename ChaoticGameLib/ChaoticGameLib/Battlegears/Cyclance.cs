using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Cyclance : Battlegear, ISacrificeTarget<Creature>
    {
        public Cyclance(Texture2D texture, Texture2D overlay) : base(texture, overlay) { }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return this.IsFaceUp && !creatureEquipped.Air;
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            c.Air = true;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetEquipped; } }

        public override string Description()
        {
            return "Cyclance. Battlegear. Sacrifice Cyclance: Equipped Creature gains Elemental Type Air until the end of combat.";
        }
    }
}
