using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Najarin : Creature
    {
        public Najarin(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 2, false, false, false, false, 0,
            false, 0, 0, false, false, true, Tribe.OverWorld, CreatureType.StrategistMuge)
        {
        }

        public override string Description()
        {
            return "Najarin Creature - Overworld Strategist Muge Courage: 60 Power: 30 Wisdom: 90 Speed: 35 Energy: 30 Mugic Ability: 2" +
                " Elemental Type: None Creature Ability: " +
                "Pay 2 Mugic Ability: Return target Mugic Card from your discard pile to your hand. Unique " +
            "\"The search for the Cothica is the means to an end. But finding it may mean the end,\" -- Najarin";
        }
    }
}
