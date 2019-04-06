using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class SpectralViewer : Battlegear
    {
        public SpectralViewer(Texture2D sprite, Texture2D overlay) : base(sprite, overlay) { }

        public override string Description()
        {
            return "Spectral Viewer. Battlegear. Equipped Creature is immune to all Invisibility abilities.";
        }
    }
}
