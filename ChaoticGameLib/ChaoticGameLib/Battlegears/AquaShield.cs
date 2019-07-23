using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class AquaShield : Battlegear, ISacrificeTarget<Creature>
    {
        public AquaShield(Texture2D texture, Texture2D overlay) : base(texture, overlay, 15) { }
        public override void Equip(Creature creature)
        {
            creature.GainedEnergy += 5;
            creature.Energy += 5;
        }

        public override void UnEquip(Creature creature)
        {
            creature.RemoveGainedEnergy(5);
        }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return this.IsFaceUp;
        }

        public override bool CheckSacrificeTarget(Creature target)
        {
            return target.CheckHealable();
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            c.Heal(this.DisciplineAmount);
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetCreature; } }

        public override string Description()
        {
            return "Aqua Shield. Battlegear. Equipped Creature gains 5 Energy. " + 
                "Sacrifice Aqua Shield: Heal 15 Energy to target Creature.";
        }
    }
}
