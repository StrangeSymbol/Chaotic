using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class PhobiaMask : Battlegear
    {
        public PhobiaMask(Texture2D sprite, Texture2D overlay) : base(sprite,overlay, 10) { }

        public override string Description()
        {
            return "Phobia Mask. Battlegear. Equipped Creature gains \"Intimidate: Courage 10\"" +
                " and \"Intimidate: Power 10\".";
        }
    }
}
