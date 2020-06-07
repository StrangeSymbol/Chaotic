using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfRecovery : Mugic, ICastTarget<Creature>
    {
        public SongOfRecovery(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, MugicType.Mipedian, 1) { }
        
        public override bool CheckPlayable(Creature creature)
        {
            return creature.CheckHealable();
        }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            creature.Heal(10);
            creature.AirDamage += 5;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return base.Description() + " Heal 10 damage to target Creature. That Creature gains \"Air 5\" until the end of the turn." +
                " Tianne believes that each Mugic is but a small piece of a great opus.";
        }
    }
}