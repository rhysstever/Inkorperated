using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Inkcorperated
{
    class Player : Entity
    {
        public Player(Rectangle bounds, Texture2D texture, float fireRate = 1.0f) : base(bounds, texture, fireRate) { }
    }
}
