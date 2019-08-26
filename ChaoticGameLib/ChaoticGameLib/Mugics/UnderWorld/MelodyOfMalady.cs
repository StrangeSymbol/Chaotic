using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class MelodyOfMalady : Mugic, ICastTargetTwo<Creature>
    {
        public MelodyOfMalady(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.UnderWorld, 1) { }

        public override string Description()
        {
            return base.Description() + " Target engaged Creature gains 10 Energy. " +
                "Deal 5 damage to another target engaged Creature." +
                "\"A little bit more for me, a little bit less for you. Perfect!\" -- Zalver";
        }

        void ICastTargetTwo<Creature>.Ability(Creature card1, Creature card2)
        {
            card1.Energy += 10;
            card1.GainedEnergyTurn += 10;
            card2.Energy -= 5;
        }

        AbilityType ICast.Type
        {
            get { return AbilityType.TargetEngaged; }
        }
    }
}
