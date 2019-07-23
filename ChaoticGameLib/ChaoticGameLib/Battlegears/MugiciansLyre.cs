using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Battlegears
{
    public class MugiciansLyre : Battlegear, ISacrificeTarget<Creature>
    {
        public MugiciansLyre(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, 1) { }

        public override bool CheckSacrifice(Creature creatureEquipped)
        {
            return this.IsFaceUp;
        }

        void ISacrificeTarget<Creature>.Ability(Creature c)
        {
            c.MugicCounters += this.DisciplineAmount;
        }

        AbilityType ISacrifice.Type { get { return AbilityType.TargetEquipped; } }

        public override string Description()
        {
            return "Mugician's Lyre. Battlegear. Sacrifice Mugician's Lyre: Add 1 Mugic Counter to this Creature.";
        }
    }
}
