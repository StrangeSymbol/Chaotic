using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class SpectralViewer : Battlegear
    {
        public SpectralViewer(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate) { }

        public override string Description()
        {
            return "Spectral Viewer. Battlegear. Equipped Creature is immune to all Invisibility abilities.";
        }
    }
}
