
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
    class Button<T1> : Drawable
    {
        public delegate void OnClick(T1 i);
        OnClick onClick;

        private T1 passValue;
        private string text;

        public Button(Rectangle bounds, Texture2D texture, OnClick function, T1 value, string text) : base(bounds, texture)
        {
            onClick = function;
            passValue = value;
            this.text = text;
        }

        public bool IsClicked(MouseState prevState)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released && prevState.LeftButton == ButtonState.Pressed && Bounds.Contains(Mouse.GetState().Position))
            {
                onClick(passValue);
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch batch, Color backgroundColor, Color textColor, SpriteFont font)
        {
            //Draws the background
            base.Draw(batch, backgroundColor);
            //Draws the text
            float x = (Bounds.X + Bounds.Width / 2.0f) - (font.MeasureString(text).X / 2.0f);
            float y = (Bounds.Y + Bounds.Height / 2.0f) - (font.MeasureString(text).Y / 2.0f);
            batch.DrawString(font, text, new Vector2(x, y), textColor);
        }
    }
}
