using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfGeonova : Mugic
    {
        public SongOfGeonova(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }

        public override void Ability(Creature creature)
        {
            if (creature.Earth || creature.Water)
                creature.Energy -= 10;
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Deal 10 damage to target Creature with Elemental Type Earth or Water." +
                " Forsake the land and the land will forgive you.";
        }
    }
}