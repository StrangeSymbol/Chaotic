using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class TrillsOfDiminution : Mugic
    {
        public TrillsOfDiminution(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Mipedian, 1) { }

        public override string Description()
        {
            return base.Description() + " Target engaged Creature loses all abilities until the end of the turn." +
                " Even the simplest sounds can resonate in one's mind.";
        }
    }
}