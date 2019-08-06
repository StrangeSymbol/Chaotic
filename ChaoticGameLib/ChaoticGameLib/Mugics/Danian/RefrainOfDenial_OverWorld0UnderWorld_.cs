using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class RefrainOfDenial_OverWorld0UnderWorld_ : Mugic, ICastDispel
    {
        public RefrainOfDenial_OverWorld0UnderWorld_(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Danian, 1) { }
        
        public override string Description()
        {
            return base.Description() + " Dispel target OverWorld or UnderWorld Mugic." +
                " \"The secret to using Mugic is to not try to solve its mysteries.\" -- Lore";
        }

        AbilityType ICast.Type
        {
            get { return AbilityType.DispelMugic; }
        }

        bool ICastDispel.CheckDispel(Mugic mugic)
        {
            return mugic.MugicCasting == MugicType.OverWorld || mugic.MugicCasting == MugicType.UnderWorld;
        }
    }
}