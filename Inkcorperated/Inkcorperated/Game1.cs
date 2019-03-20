﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Inkcorperated
{
	/// <summary>
	/// Jacob exists I guess
    /// Brandon was here
	/// Rhys: Is a hotdog a sandwich
	/// Sean is around
	/// </summary> 

    // Enums to hold finite states
    public enum GameStates
    {
        MainMenu,
        PauseMenu,
        Options,
        Game,
        GameOver,
        GameWon
    }
    
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
        
        private GameStates currentGameState;

        private Player player; 

        MapController controller;
		CollisionManager collisionManager;

        MouseState previousMouseState;
        KeyboardState previousKeyboardState;
        private MouseState currentMouseState;
        private KeyboardState currentKBState;

        private SpriteFont fontArial;

        private Button<GameStates> play;
		private Button<GameStates> unpause;

        public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
            IsMouseVisible = true;
            
            currentGameState = GameStates.MainMenu;

            controller = new MapController(graphics);
			collisionManager = new CollisionManager(controller);
            previousMouseState = new MouseState();
            previousKeyboardState = new KeyboardState();
            base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D blankTexture = Content.Load<Texture2D>("block");
            controller.LoadLevels(Content.Load<Texture2D>("player_idle"), blankTexture, null, Content.Load<Texture2D>("goal"),
                Content.Load<Texture2D>("Ink Bar"), Content.Load<Texture2D>("Ink Fill"), Content.Load<Texture2D>("Background01"));
			player = controller.LevelPlayer;
            fontArial = Content.Load<SpriteFont>("fontArial");

            play = new Button<GameStates>(new Rectangle(325, 250, 150, 50), blankTexture, SwitchGameState, GameStates.Game, "Play");
			unpause = new Button<GameStates>(new Rectangle(325, 250, 150, 50), blankTexture, SwitchGameState, GameStates.Game, "Unpause");

            controller.LoadLevel(0);
        }

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
        {
            currentKBState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

			switch(currentGameState)
			{
				case GameStates.MainMenu:
					// When clicked, switches GameState from MainMenu to the Game
                    play.IsClicked(previousMouseState);
					break;

				case GameStates.Options:
					
					break;

				case GameStates.Game:
					if (SingleKeyPress(Keys.Escape))
					{
						currentGameState = GameStates.PauseMenu;
					}

					if (player.Y > GraphicsDevice.Viewport.Height)
					{
						currentGameState = GameStates.GameOver;
					}

					// Handles player movement
					player.Move(gameTime);
					//Handles drawing blocks
					controller.CheckForRectDraw(previousMouseState, GraphicsDevice.Viewport.Bounds);
					//Moves all of the bullets
					foreach (Bullet b in controller.Bullets)
					{
						b.X += b.Direction;
					}
					// Handles collisions between the player and all other collidables
					collisionManager.Colliding();
					//Handles switching block types
					controller.CheckBlockTypeChange(previousKeyboardState);
					//Restarts the level if the player wants to
					if (SingleKeyPress(Keys.R))
						controller.ResetLevel();
					break;

				case GameStates.PauseMenu:
					// When clicked, "unpauses" the game
					// Changes GameState back from Pause to Game
					unpause.IsClicked(previousMouseState);
					if (SingleKeyPress(Keys.Escape))
					{
						currentGameState = GameStates.Game;
					}
					break;

				case GameStates.GameOver:
					if (SingleKeyPress(Keys.Enter))
					{
						currentGameState = GameStates.Game;
						controller.LoadLevel(0);
					}

					if(collisionManager.isColliding(controller.LevelPlayer, controller.Goal))
						if(!controller.NextLevel())
							currentGameState = GameStates.GameWon;

					break;

				case GameStates.GameWon:
					if (SingleKeyPress(Keys.Enter))
					{
						currentGameState = GameStates.MainMenu;
					}
					break;
			}
            
            previousKeyboardState = currentKBState;
            previousMouseState = currentMouseState;

            base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            switch (currentGameState)
            {
                case GameStates.MainMenu:
                    // Writes the title
                    spriteBatch.DrawString(
                    fontArial,
                    "Inkorporated",
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Inkorporated").X / 2), GraphicsDevice.Viewport.Height / 4),
                    Color.White);

                    //Draws the button
                    play.Draw(spriteBatch, Color.Black, Color.White, fontArial);

                    /* Writes the instructions
                    spriteBatch.DrawString(
                    fontArial,
                    "Hit 'Enter' to Start",
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Hit 'Enter' to Start").X / 2), (GraphicsDevice.Viewport.Height / 4) + 50),
                    Color.White);
                    */
                    break;

                case GameStates.Game:
                    controller.Draw(spriteBatch);
                    break;

                case GameStates.PauseMenu:
                    GraphicsDevice.Clear(Color.MediumPurple);

                    // Writes 'Paused'
                    spriteBatch.DrawString(
                    fontArial,
                    "Paused",
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Paused").X / 2), GraphicsDevice.Viewport.Height / 4),
                    Color.White);

					// Draws "unpause" button
					unpause.Draw(spriteBatch, Color.Black, Color.White, fontArial);

                    /* Writes the instructions
                    spriteBatch.DrawString(
                    fontArial,
                    "Hit 'Esc' to Return to Game.",
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Hit 'Esc' to Return to Game").X / 2), (GraphicsDevice.Viewport.Height / 4) + 50),
                    Color.White); */
                    break; 

                case GameStates.GameOver:
                    GraphicsDevice.Clear(Color.Black);

                    // Writes GameOver
                    spriteBatch.DrawString(
                    fontArial,
                    "Game Over",
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Game Over").X / 2), GraphicsDevice.Viewport.Height / 4),
                    Color.White);

                    // Writes the instructions
                    spriteBatch.DrawString(
                    fontArial,
                    "Hit 'Enter' to Return to Try Again.",
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Hit 'Enter' to Return to Try Again.").X / 2), (GraphicsDevice.Viewport.Height / 4) + 50),
                    Color.White);
                    break;
				case GameStates.GameWon:
					GraphicsDevice.Clear(Color.Black);

					// Writes GameWon
					spriteBatch.DrawString(
					fontArial,
					"Game Won",
					new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Game Won").X / 2), GraphicsDevice.Viewport.Height / 4),
					Color.White);

					// Writes the instructions
					spriteBatch.DrawString(
					fontArial,
					"Hit 'Enter' to Return to the Main Menu.",
					new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Hit 'Enter' to Return to the Main Menu.").X / 2), (GraphicsDevice.Viewport.Height / 4) + 50),
					Color.White);
					break;
            }
            spriteBatch.End();

			base.Draw(gameTime);
		}

        private void SwitchGameState(GameStates state)
        {
            currentGameState = state;
        }

        // Helper Methods
        /// <summary>
        /// Returns true if this is the first frame that the key was pressed
        /// False otherwise
        /// </summary>
        /// <param name="key">Represents the key to check (One of the "Keys" enum values)</param>
        public bool SingleKeyPress(Keys key)
        {
            if (currentKBState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
