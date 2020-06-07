using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfFury : Mugic, ICastTargetTwo<Creature>
    {
        public SongOfFury(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.UnderWorld, 1) { }

        void ICastTargetTwo<Creature>.Ability(Creature creature1, Creature creature2)
        {
            creature1.Energy -= 10;
            creature2.FireDamage += 5;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreatureTwo; } }

        public override string Description()
        {
            return base.Description() + " Deal 10 damage to target Creature. " +
                "Until the end of the turn, another target Creature gains \"Fire 5\". " +
                "\"My legs or my Mugician? There is no choice. Cut them off!\" -- Miklon";
        }
    }
}
