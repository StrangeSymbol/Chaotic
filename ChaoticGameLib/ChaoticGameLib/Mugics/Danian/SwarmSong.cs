using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SwarmSong : Mugic, ICastTarget<Creature>
    {
        public SwarmSong(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Danian, 1) { }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            creature.Energy -= 15;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Deal 15 damage to target Creature. " +
                "The insects eclipsed the sun, turning day to night.";
        }
    }
}
