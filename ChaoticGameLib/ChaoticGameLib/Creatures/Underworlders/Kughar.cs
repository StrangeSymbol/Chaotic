using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Kughar : Creature
    {
        public Kughar(Texture2D sprite, Texture2D overlay, Texture2D negate, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, negate, energy, courage, power, wisdom, speed, 0, true, false, false, false, 0, 
            false, 0, 0, false, false, false, 0, 0, 0, 0, 0, 10, 0, 0, Tribe.UnderWorld, CreatureType.Taskmaster)
        {
        }

        public override string Description()
        {
            return "Kughar Creature - UnderWorld Taskmaster Courage: 65 Power: 85 Wisdom: 25 Speed: 45 Energy: 50 " +
                "Mugic Ability: 0 Elemental Type: Fire Creature Ability: " +
            "Intimidate Power 10 [Opposing Creature loses 10 Power until the end of combat.] " +
            "On bad days, Kughar is angry, furious and enraged. On good day, he's just angry and furious.";
        }
    }
}
