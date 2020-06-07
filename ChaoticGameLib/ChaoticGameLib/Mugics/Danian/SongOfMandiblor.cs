using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfMandiblor : Mugic, ICastTargetCount<Creature>
    {
        public SongOfMandiblor(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Danian, 1) { }
        
        public override string Description()
        {
            return base.Description() + " Target Creature gains 5 Courage, Power, Wisdom, and Speed for each Danian " +
                " in play until the end of the turn. " +
                "Listen and learn the true strength of the Danian tribe -- when you face one, you contend with all.";
        }

        void ICastTargetCount<Creature>.Ability(Creature card, byte count)
        {
            byte disciplineChange = (byte)(5 * count);
            card.Courage += disciplineChange;
            card.CourageTurn += disciplineChange;
            card.Power += disciplineChange;
            card.PowerTurn += disciplineChange;
            card.Wisdom += disciplineChange;
            card.WisdomTurn += disciplineChange;
            card.Speed += disciplineChange;
            card.SpeedTurn += disciplineChange;
        }

        AbilityType ICast.Type
        {
            get { return AbilityType.TargetDanianCount; }
        }
    }
}