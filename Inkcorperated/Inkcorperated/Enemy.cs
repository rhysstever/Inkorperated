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
        private int health;

        // Properties
        public int Range { get { return range; } }
        public int Health { get { return health; } }

        // Constructor

        public Enemy(int range, int health, Rectangle bounds, Texture2D texture, float fireRate = 2.0f) : base(bounds, texture, fireRate)
        {
            this.range = range;
            this.health = health;
        }


    }
}
