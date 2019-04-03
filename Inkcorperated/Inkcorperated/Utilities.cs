using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inkcorperated
{
	class Utilities
	{
		/// <summary>
		/// Returns true if this is the first frame that the key was pressed
		/// False otherwise
		/// </summary>
		/// <param name="key">Represents the key to check (One of the "Keys" enum values)</param>
		public static bool SingleKeyPress(KeyboardState previous, KeyboardState current, Keys key)
		{
			return current.IsKeyDown(key) && previous.IsKeyUp(key);
		}

		/// <summary>
		/// Rounds upwards to the nearest ten
		/// </summary>
		public static int RoundUpToNearestTwenty(int i)
		{
			return Math.Sign(i) * (int)Math.Ceiling(Math.Abs(i / 20.0)) * 20;
		}

		/// <summary>
		/// Rounds downward to the nearest ten
		/// </summary>
		public static int RoundDownToNearestTwenty(int i)
		{
			return Math.Sign(i) * (int)Math.Floor(Math.Abs(i / 20.0)) * 20;
		}
	}
}
