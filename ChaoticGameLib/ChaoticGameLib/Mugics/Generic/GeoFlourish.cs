using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class GeoFlourish : Mugic, ICastTarget<Creature>
    {
        public GeoFlourish(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Generic, 1) { }

        public override bool CheckPlayable(Creature creature)
        {
            return creature.CheckHealable() && (creature.Earth || creature.Water);
        }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            if (creature.Earth || creature.Water)
                creature.Heal(15);
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Heal 15 damage to target Creature with Elemental Type Earth or Water." +
                " Protect the land and the land will protect you.";
        }
    }
}
