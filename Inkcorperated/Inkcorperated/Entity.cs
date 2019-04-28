using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Inkcorperated
{
    class Entity : Drawable
    {
		public static MapController controller;

		private int health;
		private Teams team;
		private int direction;
		private float fireRate;
        private double timeSinceLastShot;

		// Properties

		public int Health
		{
			get { return health; }
			set { health = value; }
		}
		public Teams Team { get { return team; } }
		public int Direction
		{
			get { return direction; }
			set { direction = value; }
		}
        public Texture2D Texture { get { return texture; } }
        public float FireRate { get { return fireRate; } }

        public Entity(int health, Teams team, int direction, Rectangle bounds, Texture2D texture, float fireRate) 
			: base(bounds, texture)
        {
			this.health = health;
			this.fireRate = fireRate;
			this.team = team;
			this.direction = direction;
            timeSinceLastShot = 0;
        }

        /// <summary>
        /// To be called every frame, updates the time since last shot
        /// </summary>
        public void Update(double timeSinceLastFrame)
        {
            timeSinceLastShot += timeSinceLastFrame;
        }

        /// <summary>
        /// Checks to see if the entity can fire
        /// Resets the timeSinceLast fire if so
        /// </summary>
        /// <returns>If the entity can fire</returns>
        public bool CanFire()
        {
            if(fireRate < timeSinceLastShot)
            {
                timeSinceLastShot = 0;
                return true;
            }
            return false;
        }

		public void Fire()
		{
			if(CanFire())
			{
				int x = 0;
				int y = (Height / 2) + Y - 10;

				if(Direction == 1)
					x = X + Width + 5;
				else if(Direction == -1)
					x = X - 16;

				// Sends info to make a bullet in the Map Controller class
				controller.ShootBullet(new Rectangle(x, y, 16, 16), Team, Direction);
			}
		}

        public bool Collided(Drawable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
