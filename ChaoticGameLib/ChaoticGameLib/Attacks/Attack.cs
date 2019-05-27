using System;
using ChaoticGameLib.Battlegears;
using ChaoticGameLib.Locations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib
{
    public abstract class Attack : ChaoticCard
    {
        // Holds attack cards build number
        byte buildNumber;

        // Holds the amount of damage always given
        byte baseDamage;

        // Holds the damage given by the elements
        byte fireDamage;
        byte airDamage;
        byte earthDamage;
        byte waterDamage;

        // Holds the amount of damage from a check
        byte energyAmount;

        // Holds the value of discipline to check or challenge
        byte disciplineAmount;

        // Holds the boolean value of what elements a attack has
        bool fire;
        bool air;
        bool earth;
        bool water;

        public Attack(Texture2D sprite, Texture2D overlay, 
            byte baseDamage, byte fireDamage, byte airDamage, byte earthDamage, byte waterDamage, byte buildNumber,
            byte energyAmount, byte disciplineAmount, bool fire, bool air, bool earth, bool water, bool unique=false)
            : base(sprite, overlay, unique)
        {
            this.buildNumber = buildNumber;
            this.baseDamage = baseDamage;
            this.fireDamage = fireDamage;
            this.airDamage = airDamage;
            this.earthDamage = earthDamage;
            this.waterDamage = waterDamage;
            this.energyAmount = energyAmount;
            this.disciplineAmount = disciplineAmount;
            this.fire = fire;
            this.air = air;
            this.earth = earth;
            this.water = water;
        }

        public byte BuildNumber { get { return buildNumber; } }
        public byte BaseDamage { get { return baseDamage; } }
        public byte FireDamage { get { return fireDamage; } }
        public byte AirDamage { get { return airDamage; } }
        public byte EarthDamage { get { return earthDamage; } }
        public byte WaterDamage { get { return waterDamage; } }
        protected byte DisciplineAmount { get { return disciplineAmount; } }
        protected byte EnergyAmount { get { return energyAmount; } }
    
        public virtual void Damage(Creature your, Creature enemy, Location location)
        {
            Tuple<short, short> damage = PredictedDamage(your, enemy, location);
            your.Heal((byte)damage.Item1);
            enemy.Energy += damage.Item2;

            your.FirstAttack = false;
        }

        public virtual Tuple<short, short> PotentialDamage(Creature your, Creature enemy, Location location)
        {
            short energy = enemy.Energy;
            short energy1 = 0;
            short energy2 = 0;
            energy2 -= baseDamage;

            if (your.Fire && this.fire)
                energy2 -= (byte)(fireDamage + your.FireDamage);
            if (your.Air && this.air)
                energy2 -= (byte)(airDamage + your.AirDamage);
            if (your.Earth && this.earth)
                energy2 -= (byte)(earthDamage + your.EarthDamage);
            if (your.Water && this.water)
                energy2 -= (byte)(waterDamage + your.WaterDamage);

            if (your.Battlegear is RiverlandStar && your.CreatureTribe == Tribe.OverWorld && your.Water && this.water)
                energy1 += 5;
            else if (enemy.Battlegear is StoneMail && enemy.Battlegear.IsFaceUp && enemy.Energy < energy)
                energy2 -= 5;

            //energy1 -= your.Recklessness; // TODO: Add to Burst.

            if (your.Strike > 0 && your.FirstAttack && 
                !(enemy.Battlegear.IsFaceUp && (enemy.Battlegear is SpectralViewer)))
                energy2 -= your.Strike;

            if (location is StormTunnel)
            {
                if (your.Air && this.air)
                    energy2 -= 5;
                if (your.Water && this.water)
                    energy2 += (byte)(energy2 >= 0 ? 0 : 5);
            }
            else if (location is Everrain)
            {
                if (your.Water && this.water)
                    energy2 -= 5;
                if (your.Earth && this.earth)
                    energy2 += (byte)(energy2 >= 0 ? 0 : 5);
            }
            else if (location is GloomuckSwamp)
            {
                if (your.Earth && this.earth)
                    energy2 -= 5;
                if (your.Fire && this.fire)
                    energy2 += (byte)(energy2 >= 0 ? 0 : 5);
            }
            else if (location is LavaPond)
            {
                if (your.Fire && this.fire)
                    energy2 -= 5;
                if (your.Air && this.air)
                    energy2 += (byte)(energy2 >= 0 ? 0 : 5);
            }
            else if (location is MipedimOasis && your.FirstAttack && your.CreatureTribe == Tribe.Mipedian)
                energy2 -= 10;
            else if (location is Riverlands && your.Water && this.water)
                energy1 += (byte)(your.Energy + 5 > 
                    your.OldCreature.Energy + your.GainedEnergyTurn + your.GainedEnergy ? 0 : 5);
            else if (location is UnderworldColosseum && your.FirstAttack && your.Fire)
                energy2 -= 10;
            else if (location is UnderworldCity && your.CreatureTribe == Tribe.UnderWorld)
            {
                if ((your.Power - enemy.Power) >= 15)
                    energy2 -= 5;
            }

            return new Tuple<short, short>(energy1, energy2);
        }

        public Tuple<short, short> PredictedDamage(Creature your, Creature enemy, Location location)
        {
            Tuple<short, short> damage = PotentialDamage(your, enemy, location);
            short energy1 = damage.Item1;
            short energy2 = damage.Item2;
            if (location is CrystalCave && your.FirstAttack && your.Speed < enemy.Speed)
                energy2 = 0;
            return new Tuple<short, short>(energy1, energy2);
        }

        protected byte predictedHealAmount(Creature your)
        {
            return (byte)(your.Energy + energyAmount >
                your.OldCreature.Energy + your.GainedEnergyTurn + your.GainedEnergy ? 0 : energyAmount);
        }

        public override string Description()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder(this.Name);
            builder.Append(" - Attack Build Number: ");
            builder.Append(this.buildNumber);
            builder.Append(" Base Damage: ");
            builder.Append(this.baseDamage);
            
            if (this.fireDamage > 0 || fire)
            {
                builder.Append(" Fire Damage: ");
                builder.Append(this.fireDamage);
            }
            if (this.airDamage > 0 || air)
            {
                builder.Append(" Air Damage: ");
                builder.Append(this.airDamage);
            }
            if (this.earthDamage > 0 || earth)
            {
                builder.Append(" Earth Damage: ");
                builder.Append(this.earthDamage);
            }
            if (this.waterDamage > 0 || water)
            {
                builder.Append(" Water Damage: ");
                builder.Append(this.waterDamage);
            }
            builder.Append(". ");
            return builder.ToString();
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder(this.Name);
            builder.Append(" ");
            builder.Append(this.buildNumber);
            builder.Append(" Attack ");
            builder.Append(this.baseDamage);
            builder.Append("/");
            builder.Append(this.fireDamage);
            builder.Append("/");
            builder.Append(this.airDamage);
            builder.Append("/");
            builder.Append(this.earthDamage);
            builder.Append("/");
            builder.Append(this.waterDamage);
   
            return builder.ToString();
        }
    }
}