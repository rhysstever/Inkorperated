using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Inkcorperated
{
    class Drawable
    {
        private Rectangle bounds;
        private Texture2D texture;

        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }
        }

        public int X
        {
            get
            {
                return bounds.X;
            }

            set
            {
                bounds.X = value;
            }
        }

        public int Y
        {
            get
            {
                return bounds.Y;
            }

            set
            {
                bounds.Y = value;
            }
        }

        public int Width
        {
            get
            {
                return bounds.Width;
            }

            set
            {
                bounds.Width = value;
            }
        }

        public int Height
        {
            get
            {
                return bounds.Height;
            }

            set
            {
                bounds.Height = value;
            }
        }

        public Drawable(Rectangle bounds, Texture2D texture)
        {
            this.bounds = bounds;
            this.texture = texture;
        }

        /// <summary>
        /// Draws this object to the spritebatch with the specified color
        /// </summary>
        public void Draw(SpriteBatch batch, Color c)
        {
            batch.Draw(texture, bounds, c);
        }
    }
}
