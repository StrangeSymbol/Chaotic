using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Sobtjek : Creature, ISacrificeTarget<Creature>
    {
        public Sobtjek(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 1, false, true, false, false, 0, 
            false, 0, 0, false, false, false, 0, 0, 0, 0, 1, 25, Tribe.Mipedian, CreatureType.Muge)
        {
        }

        public override string Description()
        {
            return "Sobtjek Creature - Mipedian Muge Courage: 40 Power: 25 Wisdom: 65 Speed: 40 Energy: 30 Mugic Ability: 1" +
                " Elemental Type: Air Creature Ability: " +
                "Cost 1 Mugic Counter, Sacrifice Sobtjek: Deal 25 damage to target Creature. " +
                "Few Mipedians can outlast Sobtjek in a parfkew eating contest.";
        }

        public override bool CheckSacrifice(bool hive)
        {
            return this.MugicCounters >= this.MugicCost && this.Energy > 0;
        }

        public override bool CheckSacrificeTarget(Creature creature)
        {
            return creature.Energy > 0;
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            this.MugicCounters -= this.MugicCost;
            c.Energy -= this.AbilityEnergy;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetCreature; } }
    }
}
