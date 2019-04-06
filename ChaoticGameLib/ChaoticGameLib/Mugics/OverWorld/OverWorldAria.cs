using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class OverWorldAria : Mugic
    {
        public OverWorldAria(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.OverWorld, 1) { }
        public override void Ability(Creature creature)
        {
            // TODO:
            base.Ability(creature);
        }
        public override string Description()
        {
            return base.Description() + " Heal 10 Energy to target OverWorld Creature. Until the end of the turn, damage dealt by " +
                "Fire attacks to that Creature is reduced by 5. Mugicians choose host Creatures along tribe affliations.";
        }
    }
}
