using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class AquaShield : Battlegear
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
            creature.Heal(this.DisciplineAmount);
        }

        public override string Description()
        {
            return "Aqua Shield. Battlegear. Equipped Creature gains 5 Energy. " + 
                "Sacrifice Aqua Shield: Heal 15 Energy to target Creature.";
        }
    }
}
