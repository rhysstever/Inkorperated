using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Inkcorperated
{
    public enum Teams
    {
        Player,
        Enemy,
        Neither
    }

    class Bullet : Drawable
    {
        private int bulletDamage;
        private Teams team;
        private int direction;
        private int velocity;

        public int Direction{ get{ return direction; } }
        public Teams Team{ get { return team; } }
        public int Velocity { get { return velocity; } }

        public Bullet(Rectangle bounds, Texture2D texture, Teams _team, int direction, int velocity) : base(bounds, texture)
        {
            bulletDamage = 1;
            this.direction = direction;
            team = _team; //Need to figure out how to deal with team
            this.velocity = velocity;
        }

        public void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Draw(texture, Bounds, Color.White);
            this.Bounds = new Rectangle((int)(Bounds.X + velocity * gameTime.ElapsedGameTime.TotalSeconds), Bounds.Y, Bounds.Width, Bounds.Height);
        }
    }
}
