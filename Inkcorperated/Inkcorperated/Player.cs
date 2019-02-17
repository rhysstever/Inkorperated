using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Inkcorperated
{
    class Player : Entity
    {
		// Fields

		private int inkLevels; 
		
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
		}

		// Methods

		public void Move(Block floor)
		{

		}
    }
}
