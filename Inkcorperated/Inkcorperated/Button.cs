
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
        //Delegate -- needs a variable for the next thing to load
        public delegate void ClickHandler(Drawable thingToLoad);

        public Button(Rectangle bounds, Texture2D texture) : base(bounds, texture)
        {

        }

        //Mouse is within the button return true, otherwise return false
        public bool Collided()
        {
            return this.Bounds.Intersects(new Rectangle(Mouse.GetState().Position, new Point(1, 1)));
        }

        //If the mouse and the button have collided and the mouse is clicking only on this frame then return true, otherwise return false
        public bool IsClicked(MouseState currentState, MouseState prevState)
        {
            return Collided() && currentState.LeftButton != prevState.LeftButton;
        }
    }
}
