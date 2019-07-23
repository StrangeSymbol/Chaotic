using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Kannen : Creature, IActivateChange, ISacrificeReturn
    {
        public Kannen(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, 
            false, false, false, false, false, false, Tribe.Danian, CreatureType.Battlemaster)
        {
        }

        public override string Description()
        {
            return "Kannen Creature - Danian Battlemaster Courage: 25 Power: 20 Wisdom: 70 Speed: 25 Energy: 40 " +
                "Mugic Ability: 1 Elemental Type: None Creature Ability: " +
                "Cost 1 Mugic Counter: Activate Hive until the end of the turn." +
                "Sacrifice Kannen: Return a Mandiblor card from your discard pile to any open space. " +
            "\"My life is my tribe!\" -- Kannen";
        }

        public override bool CheckAbility(bool hive)
        {
            return this.MugicCounters >= 1 && !hive;
        }

        public override bool CheckSacrifice(bool sameOwner)
        {
            return this.Energy > 0;
        }

        void IActivate.PayCost()
        {
            this.MugicCounters--;
        }

        void IActivateChange.Ability(ref bool hive)
        {
            hive = true;
        }

        bool ISacrificeReturn.CheckReturnable(Creature c)
        {
            return c.CardType == CreatureType.Mandiblor || c.CardType == CreatureType.MandiblorMuge;
        }

        AbilityType IActivate.Type { get { return AbilityType.TargetSelfChange; } }
        AbilityType ISacrifice.Type { get { return AbilityType.ReturnCreature; } }
    }
}