using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib
{
    public abstract class Battlegear : ChaoticCard
    {
        // amount of discipine change
        byte disciplineAmount;

        // whether the battlegear is face-up.
        bool isFaceUp;

        // Whether to reveal battlegear at beginning of game.
        bool revealAtBeginning;

        // Holds whether this battlegear is negated.
        bool negate;

        public Battlegear(Texture2D texture, Texture2D overlay)
            : this(texture, overlay, 0)
        {
        }

        public Battlegear(Texture2D texture, Texture2D overlay, byte disciplineAmount)
            : base(texture, overlay)
        {
            this.disciplineAmount = disciplineAmount;
        }

        public Battlegear(Texture2D texture, Texture2D overlay, byte disciplineAmount, bool reveal)
            : this(texture, overlay, disciplineAmount)
        {
            this.revealAtBeginning = reveal;
        }

        protected byte DisciplineAmount { get { return disciplineAmount; } set { disciplineAmount = value; } }
        public bool IsFaceUp { get { return this.isFaceUp; } set { this.isFaceUp = value; } }
        public bool RevealAtBeginning { get { return this.revealAtBeginning; } }

        public bool Negate { get { return this.negate; } set { negate = value; } }

        new public Battlegear ShallowCopy()
        {
            return this.MemberwiseClone() as Battlegear;
        }
        public virtual void Equip(Creature creature)
        {
            // ERROR: If not implemented means no equip effect.
        }
        public virtual void UnEquip(Creature creature)
        {
            // ERROR: If not implemented means no unequip effect.
        }

        public virtual bool CheckSacrifice(Creature creatureEquipped)
        {
            return false;
        }

        public virtual bool CheckSacrificeTarget(Creature target)
        {
            return true;
        }

        public override string ToString()
        {
            return this.Name + " Battlegear.";
        } 
    }
}
