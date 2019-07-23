using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Klasp : Creature, ISacrificeTarget<Creature>
    {
        public Klasp(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0,
            false, 5, 0, false, false, false, 0, 0, 0, 0, 0, 10, Tribe.UnderWorld, CreatureType.Taskmaster)
        {
        }

        public override bool CheckSacrifice(bool hive)
        {
            return this.Energy > 0;
        }

        public override bool CheckSacrificeTarget(Creature creature)
        {
            return creature.Energy > 0;
        }

        public override string Description()
        {
            return "Klasp Creature - UnderWorld Taskmaster Courage: 65 Power: 90 Wisdom: 30 Speed: 65 Energy: 65 " +
                "Mugic Ability: 0 Elemental Type: Fire Creature Ability: " +
                "Recklessness 5 [When this Creature makes an attack, it deals 5 damage to itself.] " +
            "Sacrifice Klasp: Deal 10 damage to target Creature. " +
            "If you find yourself seeing eye-to-eye with Klasp, it means you previously had not.";
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            c.Energy -= this.AbilityEnergy;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetCreature; } }
    }
}
