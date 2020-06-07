using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class TalismanOfTheMandiblor : Battlegear, ISacrificeReturn
    {
        public TalismanOfTheMandiblor(Texture2D sprite, Texture2D overlay, Texture2D negate) : base(sprite, overlay, negate, 15) { }
        public override void Equip(Creature creature)
        {
            creature.Speed += this.DisciplineAmount;
        }
        public override void UnEquip(Creature creature)
        {
            creature.Speed -= this.DisciplineAmount;
        }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return creatureEquipped.Energy > 0;
        }

        bool ISacrificeReturn.CheckReturnable(Creature c)
        {
            return c.CardType == CreatureType.Mandiblor || c.CardType == CreatureType.MandiblorMuge;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.ReturnCreature; } }

        public override string Description()
        {
            return "Talisman Of The Mandiblor. Battlegear. Equipped Creature gains 15 Speed. " +
                "If equipped Creature is Danian, it gains \"Sacrifice equipped Creature: " +
                "Return a Mandiblor to play in any open space.\"";
        }
    }
}
