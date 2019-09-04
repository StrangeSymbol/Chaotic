using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class MarquisDarini : Creature, IActivateTarget<Creature>
    {
        public MarquisDarini(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, true, true, false, false, 0,
            false, 0, 0, false, false, true, Tribe.Mipedian, CreatureType.Royal)
        {
        } 

        public override string Description()
        {
            return "Marquis Darini Creature - Mipedian Royal Courage: 45 Power: 40 Wisdom: 65 Speed: 40 Energy: 40 Mugic Ability: 0" +
                " Elemental Type: Fire Air Creature Ability: " +
                "Creatures you control with Invisibility deal 5 additional damage on their first attack each combat.  Unique " +
            "Proponent of peace but staunch defender of Mipedian sovereignty.";
        }

        AbilityType IActivate.Type { get { return AbilityType.TargetCreature; } }


       void IActivateTarget<Creature>.Ability(Creature card)
        {
            card.Energy -= 5;
        }

        void IActivate.PayCost()
        {
            throw new NotImplementedException();
        }
    }
}
