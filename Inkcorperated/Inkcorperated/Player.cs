using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Inkcorperated
{
    class Player : Entity
    {
        // Fields
        private int inkCapacity;
		private int inkLevels;
		private int yVelocity;
		private bool falling;
		private const int GRAVITY = 1;
		private const int SPEED = 2;
        private bool jumpBoost;
        private bool speedBoost;

		// Properties
		public bool Falling
		{
			get { return falling; }
			set { falling = value; } 
		}
		public int YVelocity
		{
			get { return yVelocity; }
			set { yVelocity = value; } 
		}
        public int InkLevels
        {
            get { return inkLevels; }
            set { inkLevels = value; }
        }
        public int InkCapacity
        {
            get { return inkCapacity; }
            set { inkCapacity = value; }
        }

        public bool JumpBoost { get { return jumpBoost; } set { jumpBoost = value; } }
        public bool SpeedBoost { get { return speedBoost; } set { speedBoost = value; } }
		
		// Constructor
        
		/// <param name="health">The amount of health of the player</param>
		/// <param name="team">The team of the player (Player)</param>
		/// <param name="direction">The direction the player is facing</param>
		/// <param name="bounds">The hitbox of the player</param>
		/// <param name="texture">The visual of the player</param>
		/// <param name="fireRate">How often the player is allowed to shoot</param>
		public Player(int health, Teams team, int direction, Rectangle bounds, Texture2D texture, int inkLevels, float fireRate = 1.0f) 
			: base(health, team, direction, bounds, texture, fireRate)
		{
            inkCapacity = inkLevels;
			this.inkLevels = inkLevels; // starting value of ink (can be changed for balancing)
			yVelocity = 0;
			falling = false;
            speedBoost = false;
            jumpBoost = false;
        }

        /// <summary>
        /// Allows remaking of the player without loss of the reference
        /// </summary>
        public void UpdatePlayer(Rectangle bounds, Texture2D texture, int inkLevels)
        {
            inkCapacity = inkLevels;
            this.inkLevels = inkLevels;
            yVelocity = 0;
            falling = false;
            X = bounds.X;
            Y = bounds.Y;
            Width = bounds.Width;
            Height = bounds.Height;
            this.texture = texture;
        }

		// Methods
		public void Update(GameTime gameTime, KeyboardState previousKbState)
		{
			KeyboardState kbState = Keyboard.GetState();

			// Moving left or right
			if(kbState.IsKeyDown(Keys.D))
			{
                if (speedBoost)
					X += (SPEED * 2);
                else
					X += SPEED;
				Direction = 1;
			}
			else if(kbState.IsKeyDown(Keys.A))
			{
                if (speedBoost)
					X -= (SPEED * 2);
                else
					X -= SPEED;
				Direction = -1;
			}
			
			// Falling
			// Changes the player's y-position based on the y-velocity
			// Updates y-velocity by gravity constant
			if(falling)
			{
				Y += yVelocity;
				yVelocity = yVelocity + GRAVITY;
			}

			// Jumping
			// Gives player an initial y-velocity to jump into the air
			if(!falling && kbState.IsKeyDown(Keys.W))
			{
                if (jumpBoost)
					yVelocity = -17;
                else
					yVelocity = -12;

                falling = true;
            }
			
			falling = true;
			
			// Shooting
			if (Utilities.SingleKeyPress(kbState, previousKbState, Keys.Space) && inkLevels > 0)
			{
				Fire();
				inkLevels -= 2;
			}

			base.Update(gameTime.ElapsedGameTime.Milliseconds);
		}

        //A new Draw method for the player. Flips based on if the A or D button is pressed
        public new void Draw(SpriteBatch batch, Color c)
        {
            if (Direction == -1) // facing left
                batch.Draw(texture, Bounds, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
            else
                batch.Draw(texture, Bounds, Color.White);
        }
    }
}
