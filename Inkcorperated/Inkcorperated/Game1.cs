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
    public enum CharacterStates
    {
        Jump,
        Stand,
        Run
    }

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

        private CharacterStates currentCharaState;
        private GameStates currentGameState;

        private Player player; // Have to initialize
        MapController controller;
        MouseState previousMouseState;
        KeyboardState previousKeyboardState;
        private KeyboardState kbState;

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

            currentCharaState = CharacterStates.Stand;
            currentGameState = GameStates.MainMenu;

            controller = new MapController();
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

            controller.LoadLevels(Content.Load<Texture2D>("character"), Content.Load<Texture2D>("block"), null, Content.Load<Texture2D>("goal"));
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
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

            controller.CheckForRectDraw(previousMouseState);
            controller.CheckMovement(previousKeyboardState);
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            if (currentGameState == GameStates.MainMenu)
            {
                if (SingleKeyPress(Keys.Enter))
                {
                    currentGameState = GameStates.Game;
                    //ResetGame();
                }
            }

            else if (currentGameState == GameStates.PauseMenu)
            {
                if (SingleKeyPress(Keys.Escape))
                {
                    currentGameState = GameStates.Game;
                }
            }

            else if (currentGameState == GameStates.Game)
            {
                previousKeyboardState = kbState;
                kbState = Keyboard.GetState();

                if (SingleKeyPress(Keys.Escape))
                {
                    currentGameState = GameStates.PauseMenu;
                }

                player.Move();

                if(currentCharaState == CharacterStates.Jump)
                {

                }

                else if(currentCharaState == CharacterStates.Stand)
                {

                }

                else if(currentCharaState == CharacterStates.Run)
                {

                }
                
            }

            else if (currentGameState == GameStates.GameOver)
            {
                if (SingleKeyPress(Keys.Enter))
                {
                    currentGameState = GameStates.MainMenu;
                }
            }

            

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
            controller.Draw(spriteBatch);
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
            previousKeyboardState = kbState;
            kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
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
