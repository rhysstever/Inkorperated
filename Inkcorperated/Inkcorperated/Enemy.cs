using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Inkcorperated
{
    class Enemy : Entity
    {
        // Fields
        private int range;

        // Properties
        public int Range { get { return range; } }

        // Constructor

        public Enemy(int range, int health, int direction, Rectangle bounds, Texture2D texture, float fireRate = 1000.0f) 
			: base(health, Teams.Enemy, direction, bounds, texture, fireRate)
        {
            this.range = range;
        }

        //A new Draw method for the player. Flips based on the direction the enemy is facing
        public new void Draw(SpriteBatch batch, Color c)
        {
			if(Health > 0)
			{
				if (Direction == -1) // facing left
					batch.Draw(texture, Bounds, null, c, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
				else
					batch.Draw(texture, Bounds, c);
			}
        }
    }
}
