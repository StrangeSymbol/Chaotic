using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfFocus : Mugic
    {
        public SongOfFocus(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.OverWorld, 1) { }
        public override void Ability(Creature creature)
        {
            creature.Courage += 10;
            creature.CourageTurn += 10;
            creature.Power += 10;
            creature.PowerTurn += 10;
            creature.Wisdom += 10;
            creature.WisdomTurn += 10;
            creature.Speed += 10;
            creature.SpeedTurn += 10;
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Target Creature gains 10 Courage, Power, Wisdom, and Speed until the end of the turn.";
        }
    }
}
