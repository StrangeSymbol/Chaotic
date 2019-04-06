using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Creatures
{
    public class Xield : Creature
    {
        public Xield(Texture2D sprite, Texture2D overlay, 
            byte energy, byte courage, byte power, byte wisdom, byte speed) :
            base(sprite, overlay, energy, courage, power, wisdom, speed, 0, false, false, false, false, 0,
            false, 0, 0, false, false, false, 0, 0, 0, 0, 0, 10, Tribe.UnderWorld, CreatureType.Elementalist)
        {
        }

        public override string Description()
        {
            return "Xield Creature - UnderWorld Elementalist Courage: 90 Power: 40 Wisdom: 35 Speed: 15 Energy: 20 Mugic Ability: 0" +
                " Elemental Type: None Creature Ability: Deal 10 damage to Xield: " +
                "Target Creature gains 10 Energy until the end of the turn. This ability may only be used once per turn " +
            "When the arrows shattered against his back, Xield knew he had found his calling.";
        }

        public void Ability(Creature c)
        {
            if (!this.UsedAbility)
            {
                this.UsedAbility = true;
                this.Energy -= this.AbilityEnergy;
                c.Energy += this.AbilityEnergy;
            }
        }
    }
}
