/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class TrillsOfDiminution : Mugic, ICastTargetTwo<Creature>
    {
        public TrillsOfDiminution(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Mipedian, 1) { }

        public override string Description()
        {
            return base.Description() + " Target engaged Creature loses all abilities until the end of the turn." +
                " Even the simplest sounds can resonate in one's mind.";
        }

        void ICastTargetTwo<Creature>.Ability(Creature card1, Creature card2)
        {
            card1.Negate = true;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetEngaged; } }
    }
}