using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfStasis : Mugic, ICastTarget<Creature>
    {
        public SongOfStasis(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.OverWorld, 1) { }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            creature.CannotMove = true;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }
        public override string Description()
        {
            return base.Description() + " Until the end of the turn, target Creature cannot move." +
                " A melody has the power to move you...or not.";
        }
    }
}
