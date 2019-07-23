using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfSymmetry : Mugic, ICastTargetTwo<Creature>
    {
        public SongOfSymmetry(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Danian, 1) { }

        void ICastTargetTwo<Creature>.Ability(Creature card1, Creature card2)
        {
            card1.Energy += 10;
            card1.GainedEnergyTurn += 10;
            card2.Energy -= 10;
        }

        AbilityType ICast.Type
        {
            get { return AbilityType.TargetCreatureTwo; }
        }

        public override string Description()
        {
            return base.Description() + " Target engaged Creature gains 10 Energy. Deal 10 damage to another target engaged Creature." +
                " Your strength is now mine, my weakness is now yours!";
        }
    }
}