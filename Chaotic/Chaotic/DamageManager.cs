using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Chaotic
{
    class DamageManager
    {
        List<Damage> damageAmounts;

        SpriteFont font;

        public DamageManager(ContentManager content)
        {
            damageAmounts = new List<Damage>();
            font = content.Load<SpriteFont>(@"Fonts\CodedFont");
        }

        public void AddDamageAmount(short damageAmount, Vector2 position)
        {
            position += new Vector2(ChaoticEngine.kCardWidth / 2, ChaoticEngine.kCardHeight / 2);
            damageAmounts.Add(new Damage(damageAmount, position, font));
        }

        public void UpdateDamageAmounts(GameTime gameTime)
        {
            foreach (Damage damage in damageAmounts)
                damage.UpdateDamage(gameTime);

            for (int i = 0; i < damageAmounts.Count; i++)
            {
                if (!damageAmounts[i].Alive)
                {
                    damageAmounts.RemoveAt(i);
                    i--;
                }
            }
        }

        public void DrawDamageAmounts(SpriteBatch spriteBatch)
        {
            foreach (Damage damage in damageAmounts)
            {
                damage.DrawDamage(spriteBatch);
            }
        }
    }
}
