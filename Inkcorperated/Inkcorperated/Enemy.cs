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
        public Enemy(Rectangle bounds, Texture2D texture, float fireRate = 2.0f) : base(bounds, texture, fireRate) { }
    }
}
