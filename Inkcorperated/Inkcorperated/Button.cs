
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Inkcorperated
{
    class Button : Drawable
    {
        public Button(Rectangle bounds, Texture2D texture) : base(bounds, texture)
        {

        }

        //Mouse is within the button
        public bool Collided()
        {
            return this.Bounds.Intersects(new Rectangle(Mouse.GetState().Position, new Point(1, 1)));
        }
    }
}
