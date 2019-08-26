using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfGeonova : Mugic, ICastTarget<Creature>
    {
        public SongOfGeonova(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Generic, 1) { }

        public override bool CheckPlayable(Creature creature)
        {
            return creature.Earth || creature.Water;
        }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            if (creature.Earth || creature.Water)
            {
                creature.Energy -= 10;
            }
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Deal 10 damage to target Creature with Elemental Type Earth or Water." +
                " Forsake the land and the land will forgive you.";
        }
    }
}