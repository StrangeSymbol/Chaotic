using System;
using System.Collections.Generic;
using ChaoticGameLib.Battlegears;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib
{
    public enum Tribe : byte { OverWorld, UnderWorld, Mipedian, Danian};

    public abstract class Creature : ChaoticCard
    {
        // All the basic properties of creatures

        #region private fields
        // What elements the creature has
        bool fire; 
        bool air;
        bool earth;
        bool water;

        // holds the amount of damage a creature can take
        short energy;

        // holds the amount of gained energy a creature has.
        byte gainedEnergyTurn;

        // holds the amount of energy gained from abilities.
        byte gainedEnergy;

        // holds the number of mugicians the creature has
        byte mugicCounters;

        // the creatures discipline values
        short courage;
        short power;
        short wisdom;
        short speed;

        // Holds the amount of extra spaces the creature can move each turn
        byte swift;

        // Holds the gained extra number of moves.
        byte swiftGained;

        // Holds the amount of occupied spaces the creature can pass through
        bool range;

        // Holds the additional elemental damage 
        byte fireDamage;
        byte airDamage;
        byte earthDamage;
        byte waterDamage;

        // Holds the elemental damage gained from Battlegear.
        byte fireDamageGained;
        byte airDamageGained;
        byte earthDamageGained;
        byte waterDamageGained;

        
        // copy to store old values to restore after combat
        Creature oldCreature;

        // A boolean check to see if the creature is alive or not
        bool alive;

        // A boolean check to see if it can't enter mixed armies
        bool mixedArmies;

        // mugic cost of ability
        byte mugicCost;

        // energy amount from ability
        byte abilityEnergy;

        // Holds the battlegear attached to creature
        Battlegear battlegear;

        // restore disciplines after combat
        byte courageCombat;
        byte powerCombat;
        byte wisdomCombat;
        byte speedCombat;

        // restore disciplines end of turn
        byte courageTurn;
        byte powerTurn;
        byte wisdomTurn;
        byte speedTurn;

        // restores element values after combat.
        bool fireCombat;
        bool airCombat;
        bool earthCombat;
        bool waterCombat;

        // Holds the amount of damage the creature takes each attack it makes
        byte recklessness;

        // Holds the value of the damage taken during first attack
        byte strike;

        // Holds the value of whether it has suprise
        bool surprise;

        // Holds whether this Creature moved this turn.
        bool movedThisTurn;

        // Holds whether this Creature had first attack.
        bool firstAttack;

        // Holds the number of previous adjacent creatures.
        byte prevNumAdja;

        // The Tribe the Creature belongs to.
        Tribe tribe;

        // The Type of Creature.
        CreatureType creatureType;
        #endregion

        #region constructor
        // To initialize basics of a creature
        public Creature(Texture2D sprite, Texture2D overlay,
            short energy, short courage, short power, short wisdom, short speed,
            byte mugicCounters, bool fire, bool air, bool earth, bool water, byte swift, bool range, byte recklessness,
            byte strike, bool surprise, bool mixedArmies, bool unique, Tribe tribe, CreatureType creatureType)
            : base(sprite, overlay, unique)
        {
            this.energy = energy;
            this.courage = courage;
            this.power = power;
            this.wisdom = wisdom;
            this.speed = speed;
            this.mugicCounters = mugicCounters;
            this.fire = fire;
            this.air = air;
            this.earth = earth;
            this.water = water;
            this.swift = swift;
            this.range = range;
            this.recklessness = recklessness;
            this.strike = strike;
            this.surprise = surprise;
            this.fireDamage = 0;
            this.airDamage = 0;
            this.earthDamage = 0;
            this.waterDamage = 0;
            this.alive = true;
            this.mixedArmies = mixedArmies;
            this.tribe = tribe;
            this.battlegear = null;
            this.courageCombat = this.powerCombat = this.wisdomCombat = this.speedCombat = 0;
            this.courageTurn = this.powerTurn = this.wisdomTurn = this.speedTurn = 0;
            this.fireCombat = this.fire;
            this.airCombat = this.air;
            this.earthCombat =  this.earth;
            this.waterCombat = this.water;
            this.oldCreature = this.ShallowCopy() as Creature;
            this.gainedEnergyTurn = 0;
            this.creatureType = creatureType;
            this.firstAttack = true;
            this.prevNumAdja = 0;
            this.gainedEnergy = 0;
            this.fireDamageGained = this.airDamageGained = this.earthDamageGained = this.waterDamageGained = 0;
            this.swiftGained = 0;
        }

        public Creature(Texture2D sprite, Texture2D overlay, short energy, short courage, short power, short wisdom, short speed,
            byte mugicCounters, bool fire, bool air, bool earth, bool water,
            bool mixedArmies, bool unique, Tribe tribe, CreatureType creatureType) :
            this(sprite, overlay, energy, courage, power, wisdom, speed, mugicCounters, fire, air, earth, water,
            0, false, 0, 0, false, mixedArmies, unique, tribe, creatureType)
        {
            
        }

        public Creature(Texture2D sprite, Texture2D overlay,
            short energy, short courage, short power, short wisdom, short speed,
            byte mugicCounters, bool fire, bool air, bool earth, bool water, byte swift, bool range, byte recklessness,
            byte strike, bool surprise, bool mixedArmies, bool unique, 
            byte fireDamage, byte airDamage, byte earthDamage, byte waterDamage, Tribe tribe, CreatureType creatureType) :
            this(sprite, overlay, energy, courage, power, wisdom, speed, mugicCounters, fire, air, earth, water,
            swift, range, recklessness, strike, surprise, mixedArmies, unique, tribe, creatureType)
        {
            this.fireDamage = this.oldCreature.fireDamage = fireDamage;
            this.airDamage = this.oldCreature.airDamage = airDamage;
            this.earthDamage = this.oldCreature.earthDamage = earthDamage;
            this.waterDamage = this.oldCreature.waterDamage = waterDamage;
        }

        public Creature(Texture2D sprite, Texture2D overlay,
            short energy, short courage, short power, short wisdom, short speed,
            byte mugicCounters, bool fire, bool air, bool earth, bool water, byte swift, bool range, byte recklessness,
            byte strike, bool surprise, bool mixedArmies, bool unique, byte fireDamage, byte airDamage, byte earthDamage,
            byte waterDamage, byte mugicCost, byte abilityEnergy, Tribe tribe, CreatureType creatureType):
            this(sprite, overlay, energy, courage, power, wisdom, speed, mugicCounters,
            fire, air, earth, water, swift, range, recklessness, strike, surprise, mixedArmies, unique,
            fireDamage, airDamage, earthDamage, waterDamage, tribe, creatureType)
        {
            this.mugicCost = mugicCost;
            this.abilityEnergy = abilityEnergy;
        }
        #endregion

        #region public propertys
        public bool Fire { get { return fire; } set { fire = value; } }
        public bool Air { get { return air; } set { air = value; } }
        public bool Earth { get { return earth; } set { earth = value; } }
        public bool Water { get { return water; } set { water = value; } }

        public short Energy { get { if (energy < 0) energy = 0; else if (energy > 120) energy = 120; return energy; } set { energy = value; } }
        public byte MugicCounters { get { return mugicCounters; } set { mugicCounters = value; } }

        public short Courage { get { if (courage < 0) courage = 0; else if (courage > 120) courage = 120; return courage; } set { courage = value; } }
        public short Power { get { if (power < 0) power = 0; else if (power > 120) power = 120; return power; } set { power = value; } }
        public short Wisdom { get { if (wisdom < 0) wisdom = 0; else if (wisdom > 120) wisdom = 120; return wisdom; } set { wisdom = value; } }
        public short Speed { get { if (speed < 0) speed = 0; else if (speed > 120) speed = 120; return speed; } set { speed = value; } }

        public byte Swift { get { return swift; } set { swift = value; } }
        public byte SwiftGained { get { return swiftGained; } set { swiftGained = value; } }

        public bool Range { get { return range; } set { range = value; } }

        public Creature OldCreature { get { return oldCreature; } }

        public byte FireDamage { get { return fireDamage; } set { fireDamage = value; } }
        public byte AirDamage { get { return airDamage; } set { airDamage = value; } }
        public byte EarthDamage { get { return earthDamage; } set { { earthDamage = value; } } }
        public byte WaterDamage { get { return waterDamage; } set { waterDamage = value; } }

        public byte FireDamageGained { get { return fireDamageGained; } set { fireDamageGained = value; } }
        public byte AirDamageGained { get { return airDamageGained; } set { airDamageGained = value; } }
        public byte EarthDamageGained { get { return earthDamageGained; } set { earthDamageGained = value; } }
        public byte WaterDamageGained { get { return waterDamageGained; } set { waterDamageGained = value; } }

        public byte Recklessness { get { return recklessness; } set { recklessness = value; } }

        public bool Alive { get { return alive; } set { alive = value; } }

        public bool MixedArmies { get { return mixedArmies; } set { mixedArmies = value; } }

        public byte MugicCost { get { return mugicCost; } set { mugicCost = value; } }
        public byte AbilityEnergy { get { return abilityEnergy; } set { abilityEnergy = value; } }

        public Battlegear Battlegear { get { return battlegear; } set { battlegear = value; } }

        public byte CourageCombat { get { return courageCombat; } set { courageCombat = value; } }
        public byte PowerCombat { get { return powerCombat; } set { powerCombat = value; } }
        public byte WisdomCombat { get { return wisdomCombat; } set { wisdomCombat = value; } }
        public byte SpeedCombat { get { return speedCombat; } set { speedCombat = value; } }

        public byte CourageTurn { get { return courageTurn; } set { courageTurn = value; } }
        public byte PowerTurn { get { return powerTurn; } set { powerTurn = value; } }
        public byte WisdomTurn { get { return wisdomTurn; } set { wisdomTurn = value; } }
        public byte SpeedTurn { get { return speedTurn; } set { speedTurn = value; } }

        public bool FireCombat { get { return this.fireCombat; } set { this.fireCombat = value; } }
        public bool AirCombat { get { return this.airCombat; } set { this.airCombat = value; } }
        public bool EarthCombat { get { return this.earthCombat; } set { this.earthCombat = value; } }
        public bool WaterCombat { get { return this.waterCombat; } set { this.waterCombat = value; } }

        public byte GainedEnergyTurn { get { return gainedEnergyTurn; } set { gainedEnergyTurn = value; } }

        public byte GainedEnergy { get { return gainedEnergy; } set { gainedEnergy = value; } }

        public byte Strike { get { return strike; } set { strike = value; } }
        public bool Surprise { get { return surprise; } set { surprise = value; } }

        protected byte NumMandiblor { get; set; }

        public bool MovedThisTurn { get { return this.movedThisTurn; } set { movedThisTurn = value; } }

        public bool FirstAttack { get { return this.firstAttack; } set { firstAttack = value; } }

        protected byte PreNumAdja { get { return this.prevNumAdja; } set { this.prevNumAdja = value; } }

        public Tribe CreatureTribe { get { return tribe; } }

        public CreatureType CardType { get { return creatureType; } }
        #endregion
 
        #region Member Functions

        public void RestoreCombat()
        {
            this.courage += this.courageCombat;
            this.power += this.powerCombat;
            this.wisdom += this.wisdomCombat;
            this.speed += this.speedCombat;
            this.fire = this.fireCombat;
            this.air = this.airCombat;
            this.earth = this.earthCombat;
            this.water = this.waterCombat;
            this.courageCombat = this.powerCombat = this.wisdomCombat = this.speedCombat = 0;
            this.fireCombat = oldCreature.Fire;
            this.airCombat = oldCreature.Air;
            this.earthCombat = oldCreature.Earth;
            this.waterCombat = oldCreature.Water;
        }

        public void RestoreTurn()
        {
            this.energy = (short)(oldCreature.energy + gainedEnergy);
            this.courage += this.courageTurn;
            this.power += this.powerTurn;
            this.wisdom += this.wisdomTurn;
            this.speed += this.speedTurn;
            this.UsedAbility = false;
            this.firstAttack = true;
            this.courageTurn = this.powerTurn = this.wisdomTurn = this.speedTurn = 0;
            this.fire = oldCreature.Fire;
            this.air = oldCreature.Air;
            this.earth = oldCreature.Earth;
            this.water = oldCreature.Water;
            this.strike = oldCreature.strike;
            this.fireDamage = (byte)(oldCreature.fireDamage + fireDamageGained);
            this.airDamage = (byte)(oldCreature.airDamage + airDamageGained);
            this.earthDamage = (byte)(oldCreature.earthDamage + earthDamageGained);
            this.waterDamage = (byte)(oldCreature.waterDamage + waterDamageGained);
            this.swift = (byte)(oldCreature.swift + swiftGained);
        }

        public bool Invisibility()
        {
            if (strike > 0 || surprise)
                return true;
            else
                return false;
        }

        public void Equip(Battlegear gear)
        {
            this.battlegear = gear.ShallowCopy();
        }

        public void ActivateBattlegear()
        {
            if (battlegear != null && !this.battlegear.IsFaceUp)
            {
                this.battlegear.IsFaceUp = true;
                this.battlegear.Equip(this);
            }
        }

        public void DeactivateBattleGear()
        {
            if (battlegear != null && this.Battlegear.IsFaceUp)
            {
                this.Battlegear.IsFaceUp = false;
                this.Battlegear.UnEquip(this);
            }
        }

        public void UnEquip()
        {
            if (this.battlegear.IsFaceUp)
                this.battlegear.UnEquip(this);
            this.battlegear = null;
        }
        public void Heal(byte energy)
        {
            this.energy += energy;
            if (this.energy > this.oldCreature.energy + this.gainedEnergyTurn + this.gainedEnergy)
                this.energy = (byte)(this.oldCreature.energy + this.gainedEnergyTurn + this.gainedEnergy);
        }
        public void RemoveGainedEnergy(byte gainedAmount)
        {
            this.gainedEnergy -= gainedAmount;
            if (this.OldCreature.Energy < this.Energy)
            {
                if (this.Energy - this.OldCreature.Energy >= gainedAmount)
                    this.Energy -= gainedAmount;
                else
                    this.Energy = (byte)(this.OldCreature.Energy + this.gainedEnergy);
            }
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder(this.Name);
            builder.Append(" ");
            builder.Append(this.tribe);
            builder.Append(" ");
            builder.Append(" Creature ");
            builder.Append(this.creatureType);
            builder.Append(" ");
            builder.Append(this.energy);
            builder.Append("/");
            builder.Append(this.courage);
            builder.Append("/");
            builder.Append(this.power);
            builder.Append("/");
            builder.Append(this.wisdom);
            builder.Append("/");
            builder.Append(this.speed);
            if (this.fire)
                builder.Append(" Fire ");
            if (this.air)
                builder.Append(" Air ");
            if (this.earth)
                builder.Append(" Earth ");
            if (this.water)
                builder.Append(" Water ");

            return builder.ToString();
        }
        #endregion
    }
}