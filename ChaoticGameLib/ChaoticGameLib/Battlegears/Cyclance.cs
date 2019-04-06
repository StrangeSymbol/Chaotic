using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Cyclance : Battlegear
    {
        public Cyclance(Texture2D texture, Texture2D overlay) : base(texture, overlay) { }

        public override void UnEquip(Creature creature)
        {
            creature.Air = true;
        }

        public override string Description()
        {
            return "Cyclance. Battlegear. Sacrifice Cyclance: Equipped Creature gains Elemental Type Air until the end of combat.";
        }
    }
}
