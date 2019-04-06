using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Attacks
{
    public class Incinerase : Attack
    {
        public Incinerase(Texture2D sprite, Texture2D overlay)
            : base(sprite, overlay, 0, 10, 0, 0, 0, 0, 0, 0, true, false, false, false) { }
        public override void Damage(Creature your, Creature enemy)
        {
            base.Damage(your, enemy);
            your.Fire = your.FireCombat = false;
        }

        public override string Description()
        {
            return base.Description() + "Fire: Your engaged Creature loses Elemental Type Fire until end of the turn.";
        }
    }
}
