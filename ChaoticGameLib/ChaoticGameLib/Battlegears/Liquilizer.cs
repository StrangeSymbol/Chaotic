using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Liquilizer : Battlegear, ISacrificeTarget<Creature>
    {
        public Liquilizer(Texture2D sprite, Texture2D overlay) : base(sprite, overlay) { }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return this.IsFaceUp && !creatureEquipped.Water;
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            c.Water = true;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetEquipped; } }

        public override string Description()
        {
            return "Liquilizer. Battlegear. Sacrifice Liquilizer: Equipped Creature gains Elemental Type Water until the end of combat.";
        }
    }
}
