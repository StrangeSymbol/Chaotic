using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfTreachery : Mugic, ICastTarget<Creature>
    {
        public SongOfTreachery(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.UnderWorld, 1) { }

        public override bool CheckPlayable(Creature creature)
        {
            return creature.Courage > 0 || creature.Power > 0 || creature.Wisdom > 0 || creature.Speed > 0;
        }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            creature.Courage -= 15;
            creature.Power -= 15;
            creature.Wisdom -= 15;
            creature.Speed -= 15;

            creature.CourageTurn += 15;
            creature.PowerTurn += 15;
            creature.WisdomTurn += 15;
            creature.SpeedTurn += 15;
        }

       AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Target Creature loses 15 Courage, Power, Wisdom, and Speed until the end of the turn." +
                " One cannot move forward if one's always watching one's own back.";
        }
    }
}
