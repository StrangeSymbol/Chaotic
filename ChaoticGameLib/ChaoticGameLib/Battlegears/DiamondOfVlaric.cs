using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class DiamondOfVlaric : Battlegear
    {
        public DiamondOfVlaric(Texture2D texture, Texture2D overlay) : base(texture, overlay) { }
        public override void UnEquip(Creature creature)
        {
            creature.Earth = true;
        }
        public override string Description()
        {
            return "Diamond Of Vlaric. Battlegear. Sacrifice Diamond Of Vlaric: Equipped Creature " +
                "gains Elemental Type Earth until the end of combat.";
        }
    }
}
