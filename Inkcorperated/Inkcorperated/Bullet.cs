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
        private Entity team;
        public Bullet(Rectangle bounds, Texture2D texture, Entity _team) : base(bounds, texture)
        {
            bulletDamage = 1;
            team = _team; //Need to figure out how to deal with team
        }
    }
}
