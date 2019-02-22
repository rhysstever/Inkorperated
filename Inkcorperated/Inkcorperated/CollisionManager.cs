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

		// Properties

		// Constructor

		/// <summary>
		/// Creates an empty Collision Manager object
		/// to handle all collisions in the game
		/// </summary>
		public CollisionManager() { }

		// Methods

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

		public void Colliding(Entity unit, List<Block> platforms)
		{
			foreach(Block platform in platforms)
			{
				if (isColliding(unit, platform))
				{
					if(unit.Y > platform.Y) // player is above the platform, needs to be moved back up
					{
						unit.Y--;
					}
					else if(unit.Y < platform.Y) // player is below platform, needs to be moved back down
					{
						unit.Y++;
					}
				}
			}
			
		}
	}
}
