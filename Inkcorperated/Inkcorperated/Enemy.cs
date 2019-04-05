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

        public Enemy(int range, int health, Teams team, int direction, Rectangle bounds, Texture2D texture, float fireRate = 2.0f) 
			: base(health, team, direction, bounds, texture, fireRate)
        {
            this.range = range;
        }


    }
}
