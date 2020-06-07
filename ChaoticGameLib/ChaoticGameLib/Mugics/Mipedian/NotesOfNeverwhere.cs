/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class NotesOfNeverwhere : Mugic, ICastTarget<Location>
    {
        public NotesOfNeverwhere(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Mipedian, 1) { }

        public override string Description()
        {
            return base.Description() + " Target Location loses all abilities until the end of the turn." +
                " \"If you have all that you need, you are always home.\" -- From an ancient Mugician text";
        }

        AbilityType ICast.Type { get { return AbilityType.TargetLocation; } }

        void ICastTarget<Location>.Ability(Location card)
        {
            card.Negate = true;
        }
    }
}