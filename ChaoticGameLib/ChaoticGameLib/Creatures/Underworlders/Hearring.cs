using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Hearring : Creature, IActivate
    {
        public Hearring(Texture2D sprite, Texture2D overlay,
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 1, false, false, false, false, 0,
            false, 0, 0, false, false, false, Tribe.UnderWorld, CreatureType.Scout) { }

        public override bool CheckAbility(bool hive)
        {
            return this.MugicCounters >= this.MugicCost;
        }

        void IActivate.PayCost()
        {
            this.MugicCounters -= this.MugicCost;
        }

        AbilityType IActivate.Type { get { return AbilityType.TargetLocationDeck; } }

        public override string Description()
        {
            return "H'earring Creature - UnderWorld Scout Courage: 70 Power: 30 Wisdom: 55 Speed: 45 Energy: 50 Mugic Ability: 1" +
                " Elemental Type: None Creature Ability: " +
                "Pay 1 Mugic Ability: Look at top three cards in target Location Deck and put them back in any order " +
            "Perhaps the most effective of the UnderWorld spies, H'earring mainly puts his supersonic skills to work for himself.";
        }
    }
}
