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

		private int inkLevels;
		private int yVelocity;
		private bool falling;
		private const int GRAVITY = 1;
		private const int SPEED = 2;

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
		
		// Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bounds">The hitbox of the player</param>
		/// <param name="texture">The visual of the player</param>
		/// <param name="fireRate">How often the player is allowed to shoot</param>
		public Player(Rectangle bounds, Texture2D texture, float fireRate = 1.0f) : base(bounds, texture, fireRate)
		{
			inkLevels = 100; // starting value of ink (can be changed for balancing)
			yVelocity = 0;
			falling = false;
		}

		// Methods
		public void Move(GameTime gameTime)
		{
			KeyboardState kbState = Keyboard.GetState();

			// Moving left or right
			if(kbState.IsKeyDown(Keys.D))
			{
				X += SPEED;
			}
			else if(kbState.IsKeyDown(Keys.A))
			{
				X -= SPEED;
			}
			
			// Falling
			// Changes the player's y-position based on the y-velocity
			// Updates y-velocity by gravity constant
			if(falling)
			{
				Y += yVelocity;
				//yVelocity = Math.Min(yVelocity + GRAVITY, 10);
				yVelocity = yVelocity + GRAVITY;
			}

			// Jumping
			// Gives player an initial y-velocity to jump into the air
			if(!falling && kbState.IsKeyDown(Keys.W))
			{
				yVelocity = -15;
				falling = true;
			}
		}
    }
}
