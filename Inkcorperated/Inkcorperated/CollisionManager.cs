using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inkcorperated
{
	class CollisionManager
	{
		// Fields
		MapController controller;
		Map currentMap;

		// Properties
		
		// Constructor

		public CollisionManager(MapController controller)
		{
			this.controller = controller;
		}

		// Methods
		
		public void GetCurrentMap()
		{
			currentMap = controller.GetCurrentMap();
		}

		/// <summary>
		/// Checks if the two Drawable objects are colliding
		/// </summary>
		/// <param name="objOne">The first object</param>
		/// <param name="objTwo">The second object</param>
		/// <returns>Returns if they are colliding</returns>
		public bool isColliding(Drawable objOne, Drawable objTwo)
		{
			if (objOne.Bounds.Intersects(objTwo.Bounds))
				return true;
			else
				return false;
		}

		public void Colliding()
		{
			controller.LevelPlayer.Falling = false;

			Rectangle bounceCheck = new Rectangle(controller.LevelPlayer.X, controller.LevelPlayer.Y + controller.LevelPlayer.Height, controller.LevelPlayer.Width, 1);

			// Checks collisions between the player and each block on the screen
			foreach (Block block in controller.CustomBlocks)
			{
				if (controller.LevelPlayer.Bounds.Intersects(block.Bounds))
				{
					Rectangle intersection = Rectangle.Intersect(controller.LevelPlayer.Bounds, block.Bounds);

					// Collision is occuring on top or bottom
					if (intersection.Width > intersection.Height)
					{
						// Moves the player up or down away from the block
						controller.LevelPlayer.Y -= intersection.Height * Math.Sign(block.Bounds.Y - controller.LevelPlayer.Y);
						controller.LevelPlayer.YVelocity = 0;
					}
					// Collision is occuring on either the right or left
					else
					{
						// Moves the player left or right away from the block
						controller.LevelPlayer.X -= intersection.Width * Math.Sign(block.Bounds.X - controller.LevelPlayer.X);
					}
				}
				else if(block.Bounds.Intersects(bounceCheck) && controller.LevelPlayer.YVelocity >= 0)
				{
					controller.LevelPlayer.Falling = false;
					controller.LevelPlayer.YVelocity = 0;
				}
			}
			
			
		}
	}
}
