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

        public Entity(int health, Teams team, int direction, Rectangle bounds, Texture2D texture, float fireRate) : base(bounds, texture)
        {
			this.health = health;
			this.fireRate = fireRate;
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

		public void Fire(Entity entity)
		{
			if(CanFire())
			{
				// Creates bullet and adds it to the 
				// Map Controller's bullet list
				// MapController.ShootBullet(new Bullet( , , entity.Team, Direction));

			}
		}

        public bool Collided(Drawable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
