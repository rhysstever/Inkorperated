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

		public CollisionManager() { }

		// Methods

		public bool isColliding(Drawable objOne, Drawable objTwo)
		{
			if (objOne.Bounds.Intersects(objTwo.Bounds))
				return true;
			else
				return false;
		}

		public void Standing(Entity unit, Block platform)
		{
			if(isColliding(unit, platform))
			{
				// makes player (or enemy) not fall through platforms
			}
		}
	}
}
