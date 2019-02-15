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
        private float fireRate;
        private double timeSinceLastShot;

        public Entity(Rectangle bounds, Texture2D texture, float fireRate) : base(bounds, texture)
        {
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
        public bool Fire()
        {
            if(fireRate < timeSinceLastShot)
            {
                timeSinceLastShot = 0;
                return true;
            }
            return false;
        }

        public bool Collided(Drawable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
