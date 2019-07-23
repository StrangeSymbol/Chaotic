using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class NexusFuse : Battlegear, ISacrificeTarget<Creature>
    {
        public NexusFuse(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 5) { }
        public override void Equip(Creature creature)
        {
            creature.Energy += this.DisciplineAmount;
            creature.GainedEnergy += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.RemoveGainedEnergy(this.DisciplineAmount);
        }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return this.IsFaceUp;
        }

        public override bool CheckSacrificeTarget(Creature target)
        {
            return target.Energy > 0;
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            c.Energy -= 15;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return "Nexus Fuse. Battlegear. Equipped Creature gains 5 Energy. " +
                "Sacrifice Nexus Fuse: Deal 15 damage to target Creature.";
        }
    }
}
