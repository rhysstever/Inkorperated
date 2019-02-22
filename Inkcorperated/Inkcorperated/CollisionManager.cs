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
			
		}
	}
}
