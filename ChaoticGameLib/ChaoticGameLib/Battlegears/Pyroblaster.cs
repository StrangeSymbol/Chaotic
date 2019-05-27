using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class Pyroblaster : Battlegear
    {
        public Pyroblaster(Texture2D sprite, Texture2D overlay) : base(sprite, overlay) { }
        public override void UnEquip(Creature creature)
        {
            //creature.Fire = true;
        }

        public override string Description()
        {
            return "Pyroblaster. Battlegear. Sacrifice Pyroblaster: Equipped Creature gains Elemental Type Fire until the end of combat.";
        }
    }
}
