using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Inkcorperated
{
    class Bullet : Drawable
    {
        private int bulletDamage;
        public Bullet(Rectangle bounds, Texture2D texture) : base(bounds, texture)
        {
            bulletDamage = 1;
        }

        public bool Collided(Entity character)
        {
            return this.Bounds.Intersects(character.Bounds);
        }
    }
}
