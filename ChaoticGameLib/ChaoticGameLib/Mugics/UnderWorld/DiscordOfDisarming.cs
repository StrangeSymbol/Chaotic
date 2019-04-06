using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class DiscordOfDisarming : Mugic
    {
        public DiscordOfDisarming(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.UnderWorld, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Destroy target Battlegear. " +
                "As his Windstrider decomposes into shrapnel, Bodal calculates that it will take " + 
                "56.4 seconds before he splats on the ground.";
        }
    }
}
