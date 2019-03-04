using Microsoft.Xna.Framework;
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
        Game,
        GameOver
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

            controller = new MapController();
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
            controller.LoadLevels(Content.Load<Texture2D>("player_idle"), Content.Load<Texture2D>("block"), null, Content.Load<Texture2D>("goal"));
			player = controller.LevelPlayer;
            fontArial = Content.Load<SpriteFont>("fontArial");
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

            // ----- Main Menu -----
            if (currentGameState == GameStates.MainMenu)
            {
                if (SingleKeyPress(Keys.Enter))
                {
                    currentGameState = GameStates.Game;
                    controller.LoadLevel(0);
                    //ResetGame();
                }
            }

            // ----- Pause Menu -----
            else if (currentGameState == GameStates.PauseMenu)
            {
                if (SingleKeyPress(Keys.Escape))
                {
                    currentGameState = GameStates.Game;
                }
            }

            // ----- Game -----
            else if (currentGameState == GameStates.Game)
            {
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
				// Handles collisions between the player and all other collidables
				collisionManager.Colliding();
                //Handles drawing blocks
                controller.CheckForRectDraw(previousMouseState);
                //Handles switching block types
                controller.CheckBlockTypeChange(previousKeyboardState);
            }

            // ----- Game Over -----
            else if (currentGameState == GameStates.GameOver)
            {
                if (SingleKeyPress(Keys.Enter))
                {
                    currentGameState = GameStates.MainMenu;
                }
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

                    // Writes the instructions
                    spriteBatch.DrawString(
                    fontArial,
                    "Hit 'Enter' to Start",
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Hit 'Enter' to Start").X / 2), (GraphicsDevice.Viewport.Height / 4) + 50),
                    Color.White);
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

                    // Writes the instructions
                    spriteBatch.DrawString(
                    fontArial,
                    "Hit 'Esc' to Return to Game.",
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Hit 'Esc' to Return to Game").X / 2), (GraphicsDevice.Viewport.Height / 4) + 50),
                    Color.White);
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
                    "Hit 'Enter' to Return to Main Menu.",
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Hit 'Enter' to Return to Main Menu").X / 2), (GraphicsDevice.Viewport.Height / 4) + 50),
                    Color.White);
                    break;
            }
            spriteBatch.End();

			base.Draw(gameTime);
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
        
        /// <summary>
        /// Resets the data that needs to be reset to the initial state when the game restarts the level
        /// </summary>
        public void ResetGame()
        {
            // Ink levels
            // Spawned in blocks to be removed ---> MapController's LoadLevel* Method?
            // Monster positions
            // etc
        }
    }
}
