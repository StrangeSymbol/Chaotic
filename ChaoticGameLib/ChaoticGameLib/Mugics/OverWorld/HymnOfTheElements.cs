using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class HymnOfTheElements : Mugic, ICast
    {
        public HymnOfTheElements(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.OverWorld, 1) { }

        public override bool CheckPlayable(Creature creature)
        {
            return !(creature.Fire && creature.Air && creature.Earth && creature.Water);
        }

        AbilityType ICast.Type { get { return AbilityType.TargetSelectElemental; } }

        public override string Description()
        {
            return base.Description() + " Target Creature gains the Elemental Type of your choice until the end of the turn. " +
                "All elements stem from nature - as does the Cothica itself.";
        }
    }
}
