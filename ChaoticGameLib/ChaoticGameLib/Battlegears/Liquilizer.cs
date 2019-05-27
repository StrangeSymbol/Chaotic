using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Liquilizer : Battlegear
    {
        public Liquilizer(Texture2D sprite, Texture2D overlay) : base(sprite, overlay) { }
        public override void UnEquip(Creature creature)
        {
            //creature.Water = true;
        }

        public override string Description()
        {
            return "Liquilizer. Battlegear. Sacrifice Liquilizer: Equipped Creature gains Elemental Type Water until the end of combat.";
        }
    }
}
