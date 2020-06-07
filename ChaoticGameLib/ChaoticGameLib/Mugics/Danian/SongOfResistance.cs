using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfResistance : Mugic, ICastTarget<Creature>
    {
        public SongOfResistance(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Danian, 2) { }
        
        public override string Description()
        {
            return base.Description() + " All damage dealt to this Creature by attacks is reduced by 5 until the end of the turn." +
                " Even the proudest Danian would nevery deny a chance to harden his shell.";
        }

        void ICastTarget<Creature>.Ability(Creature card)
        {
            card.ReducedDamage = 5;
        }

        AbilityType ICast.Type
        {
            get { return AbilityType.TargetSelf; }
        }
    }
}