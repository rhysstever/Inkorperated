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
			// Checks collisions between the player and each block on the screen
			foreach(Block block in controller.CustomBlocks)
			{
				if (controller.LevelPlayer.Bounds.Intersects(block.Bounds))
				{
					Rectangle intersection = Rectangle.Intersect(controller.LevelPlayer.Bounds, block.Bounds);
					// Collision is occuring on top or bottom
					if (intersection.Width > intersection.Height)
					{
						controller.LevelPlayer.YVelocity = 0;
					}
					// Collision is occuring on either the right or left
					else
					{

					}

					controller.LevelPlayer.Y++;
					controller.LevelPlayer.Falling = false;
				}
			}
			
			//Rectangle bounceCheck = new Rectangle(controller.LevelPlayer.X, controller.LevelPlayer.Y + controller.LevelPlayer.Height, controller.LevelPlayer.Width, 1);
		}
	}
}
