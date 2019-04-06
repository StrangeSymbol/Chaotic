using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib
{
    public abstract class ChaoticCard : Sprite, IComparable<ChaoticCard>
    {
        // The name of the card.
        string name;

        // Determines if creature used an ability.
        bool usedAbility;

        // Determines if the card is unique (only aloud one in deck).
        bool unique;

        // The direction for card to move in.
        Vector2 direction;

        // The amount of time till movement is finished.
        double time;

        // How fast the card moves.
        const float speed = 1000f;

        // The Width of Chaotic Card.
        const int kWidth = 59;

        // The Height of Chaotic Card.
        const int kHeight = 82;

        // The percent of completion of the transition.
        private int percent;

        // Determines whether the card transition starts face down and ends face up.
        private bool isFromFaceDownToFaceUp = true;

        // Determines whether the card is face down.
        private bool isFaceDown;

        // Controls what is visibly seen in the transition.
        Rectangle destination;

        public ChaoticCard(Texture2D sprite, Texture2D overlay)
            : base (sprite, Vector2.Zero, overlay)
        {
            this.unique = false;
            this.usedAbility = false;
            this.name = getNameFromType();
        }

        public ChaoticCard(Texture2D sprite, Texture2D overlay, bool unique)
            : this(sprite, overlay)
        {
            this.unique = unique;
        }

        // Gets the name of card from resource name.
        private string getNameFromType()
        {
            string name = this.GetType().Name;
            for (int i = 1; i < name.Length; i++)
            {
                if (name[i] <= 'Z' && 'A' <= name[i])
                {
                    name = name.Insert(i, " ");
                    i++;
                }
                else if (name[i] == '_')
                {
                    int j = name.LastIndexOf('_');
                    name = name.Replace('_', ' ').Insert(i + 1, "(").Insert(j + 1, ")");
                    if (name.IndexOf('0') != -1)
                        name = name.Replace('0', '/');
                    break;
                }
            }
            return name;
        }

        public bool UsedAbility { get { return usedAbility; } set { usedAbility = value; } }
        public bool Unique { get { return unique; } }
        public string Name { get { return name; } }
        public bool IsFromFaceDownToFaceUp { get { return IsFromFaceDownToFaceUp; } set { isFromFaceDownToFaceUp = value; } }
        public Rectangle Destination { set { destination = value; } }
        new public Rectangle CollisionRectangle { get { return new Rectangle((int)position.X, (int)position.Y, kWidth, kHeight); } }
        public double Time { get { return time; } }
        public bool IsMoving { get; set; }
        public bool GotToDestination { get; set; }

        public void CourseToCard(Vector2 p2)
        {
            Vector2 p1 = new Vector2(this.position.X + kWidth / 2, this.position.Y + kHeight / 2);
            direction = Vector2.Normalize(new Vector2(p2.X + kWidth / 2 - p1.X, p2.Y + kHeight / 2 - p1.Y));
            double distance = Vector2.Distance(p1, new Vector2(p2.X + kWidth / 2, p2.Y + kHeight / 2));
            time = distance / speed * 1000;
        }

        private double distanceToDestination(Vector2 position, Vector2 p2)
        {
            Vector2 p1 = new Vector2(position.X + kWidth / 2, position.Y + kHeight / 2);
            return Vector2.Distance(p1, new Vector2(p2.X + kWidth / 2, p2.Y + kHeight / 2));
        }

        public void Move(GameTime gameTime)
        {
            position += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds) * direction;
        }

        public void Move(GameTime gameTime, Vector2 initial, Vector2 final)
        {
            position += (float)(speed * gameTime.ElapsedGameTime.TotalSeconds) * direction;
            double distanceFromInitToFinal = distanceToDestination(initial, final);
            double distanceFromPosToDest = distanceToDestination(position, final);
            // Calculate the completion percent of the animation
            percent = 100 - (int)(distanceFromPosToDest / distanceFromInitToFinal * 100);

            if (percent >= 100)
            {
                percent = 0;
            }

            int currentPercent;
            if (percent < 50)
            {
                // On the first half of the animation the component is
                // on its initial size
                currentPercent = percent;
                isFaceDown = isFromFaceDownToFaceUp;
            }
            else
            {
                // On the second half of the animation the component
                // is flipped
                currentPercent = 100 - percent;
                isFaceDown = !isFromFaceDownToFaceUp;
            }
            // Shrink and widen the component to look like it is flipping
            destination =
                new Rectangle((int)(this.position.X + kWidth * currentPercent / 100),
                    (int)this.position.Y, (int)(kWidth - kWidth * currentPercent / 100 * 2), kHeight);
        }

        public void Draw(SpriteBatch spriteBatch, bool isPlayer1)
        {
            SpriteEffects spriteEffect;
            if (isPlayer1)
                spriteEffect = SpriteEffects.None;
            else
                spriteEffect = SpriteEffects.FlipVertically;
            spriteBatch.Draw(this.sprite, CollisionRectangle, null, Color.White, 0f, Vector2.Zero, spriteEffect, 0.9f);
            if (IsCovered)
                spriteBatch.Draw(this.overlay, CollisionRectangle, null, Color.White, 0f, Vector2.Zero, spriteEffect, 0.85f);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, bool isPlayer1)
        {
            SpriteEffects spriteEffect;
            if (isPlayer1)
                spriteEffect = SpriteEffects.None;
            else
                spriteEffect = SpriteEffects.FlipVertically;
            if (isFaceDown)
                spriteBatch.Draw(sprite, destination, null, Color.White, 0f, Vector2.Zero, spriteEffect, 0.9f);
            else
                spriteBatch.Draw(texture, destination, null, Color.White, 0f, Vector2.Zero, spriteEffect, 0.9f);
        }

        public ChaoticCard ShallowCopy()
        {
            return this.MemberwiseClone() as ChaoticCard;
        }

        public ChaoticCard Copy(Vector2 position)
        {
            ChaoticCard card = this.ShallowCopy();
            card.Position = position;
            return card;
        }

        // The description of the card.
        public abstract string Description();

        public int CompareTo(ChaoticCard other)
        {
            return this.name.CompareTo(other.Name);
        }
    }
}
