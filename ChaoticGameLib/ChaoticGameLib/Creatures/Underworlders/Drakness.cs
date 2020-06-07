using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Drakness : Creature, IActivateTarget<Creature>
    {
        public Drakness(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0, 
            false, 0, 0, false, false, true, 0, 0, 0, 0, 0, 0, Tribe.UnderWorld, CreatureType.Ethereal) { }

        public override string Description()
        {
            return "Drakness Creature - UnderWorld Ethereal Courage: 75 Power: 40 Wisdom: 85 Speed: 55 Energy: 45 " +
                "Mugic Ability: 0 Elemental Type: Fire Creature Ability: " +
                "If Mugic you control would deal damage, it deals an additional 5 damage. Unique " +
            "Trust not your own shadow -- for Drakness dwells there before he strikes.";
        }

        void IActivateTarget<Creature>.Ability(Creature card)
        {
            card.Energy -= 5;
        }

        AbilityType IActivate.Type
        {
            get { return AbilityType.TargetCreature; }
        }

        void IActivate.PayCost()
        {
            throw new NotImplementedException();
        }
    }
}
