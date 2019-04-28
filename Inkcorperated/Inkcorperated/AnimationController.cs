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
    public enum States
    {
        FaceLeft,
        FaceRight,
        WalkLeft,
        WalkRight
    }

    class AnimationController
    {
        // Fields
        int currentFrame;
        double fps;
        double secondsPerFrame;
        double timeCounter;

        // Constructor
        public AnimationController()
        {
            currentFrame = 1;
            fps = 8.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
        }

        // Methods
        /// <summary>
		/// Updates the animation time
		/// </summary>
		/// <param name="gameTime">Game time information</param>
		public void UpdateAnimation(GameTime gameTime)
        {
            // Add to the time counter (need TOTALSECONDS here)
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame and wrap
                currentFrame++;
                if (currentFrame >= 4) currentFrame = 1;

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }
        }

        public void UpdateCharacter(States playerState)
        {
            KeyboardState kb = Keyboard.GetState();
            switch (playerState)
            {
                case States.FaceLeft:
                    if (kb.IsKeyDown(Keys.Left))
                    {
                        playerState = States.WalkLeft;
                    }

                    if (kb.IsKeyDown(Keys.Right))
                    {
                        playerState = States.WalkRight;
                    }
                    break;

                case States.FaceRight:
                    if (kb.IsKeyDown(Keys.Left))
                    {
                        playerState = States.WalkLeft;
                    }

                    if (kb.IsKeyDown(Keys.Right))
                    {
                        playerState = States.WalkRight;
                    }
                    break;

                case States.WalkLeft:
                    if (kb.IsKeyUp(Keys.Left))
                    {
                        playerState = States.FaceLeft;
                    }
                    break;

                case States.WalkRight:
                    if (kb.IsKeyUp(Keys.Right))
                    {
                        playerState = States.FaceRight;
                    }
                    break;
            }
        }
        
    }
}
