using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class SongOfAsperity : Mugic, ICastTarget<Creature>
    {
        public SongOfAsperity(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.UnderWorld, 1) { }

        public override bool CheckPlayable(Creature creature)
        {
            return creature.Fire || creature.Air;
        }

        void ICastTarget<Creature>.Ability(Creature creature)
        {
            creature.FireDamage += 5;
            creature.AirDamage += 5;
        }

        AbilityType ICast.Type { get { return AbilityType.TargetCreature; } }
        
        public override string Description()
        {
            return base.Description() + " Until the end of the turn, target Creature gains \"Fire 5\" and \"Air 5\". " +
                "When anger is all-consuming, hatred is the gateway to a hideous power.";
        }
    }
}
