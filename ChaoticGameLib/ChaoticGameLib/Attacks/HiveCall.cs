using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class HiveCall : Attack
    {
        public HiveCall(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 0, 0, 0, 0, 0, 0, 0, false, false, false, false) { }

        public override string Description()
        {
            return base.Description() + "You may remove 1 Mugic Counter from a Creature you control. " +
            "If you do, activate Hive until the end of the turn.";
        }
    }
}
