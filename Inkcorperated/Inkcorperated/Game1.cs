using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        LevelSelect,
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

        Texture2D blankTexture;

        private Button<GameStates> play;
		private Button<GameStates> options;
		private Button<GameStates> unpause;
        private Button<GameStates> backToTitle;

        private Drawable levelSelectBackground;
        private List<Tuple<Button<int>, bool>> levelButtons;

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

            levelButtons = new List<Tuple<Button<int>, bool>>();
			Entity.controller = controller;
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
            blankTexture = Content.Load<Texture2D>("block");
            controller.LoadLevels(Content.Load<Texture2D>("player_idle"), blankTexture, Content.Load<Texture2D>("enemy_idle"), Content.Load<Texture2D>("goal"),
                Content.Load<Texture2D>("Ink Bar"), Content.Load<Texture2D>("Ink Fill"), Content.Load<Texture2D>("Background01"), Content.Load<Texture2D>("bullet"));
			player = controller.LevelPlayer;
            fontArial = Content.Load<SpriteFont>("fontArial");

            play = new Button<GameStates>(new Rectangle(325, 250, 150, 50), blankTexture, SwitchGameState, GameStates.LevelSelect, "Play");
			options = new Button<GameStates>(new Rectangle(325, 350, 150, 50), blankTexture, SwitchGameState, GameStates.Options, "Controls");
			unpause = new Button<GameStates>(new Rectangle(325, 250, 150, 50), blankTexture, SwitchGameState, GameStates.Game, "Unpause");
            backToTitle = new Button<GameStates>(new Rectangle(325, 325, 175, 50), blankTexture, SwitchGameState, GameStates.MainMenu, "Back to Title");

            levelSelectBackground = new Drawable(new Rectangle(GraphicsDevice.Viewport.Width / 2 - 120, GraphicsDevice.Viewport.Height / 2 - 120, 240, 240), blankTexture);

            controller.LoadLevel(0);
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
					// When clicked, switches GameState from MainMenu to the level select
                    play.IsClicked(previousMouseState);
					options.IsClicked(previousMouseState);
					break;

                case GameStates.LevelSelect:
                    foreach (Tuple<Button<int>, bool> b in levelButtons)
                    {
                        if(b.Item2)
                            b.Item1.IsClicked(previousMouseState);
                    }
					if (Utilities.SingleKeyPress(previousKeyboardState, currentKBState, Keys.Escape))
					{
						currentGameState = GameStates.MainMenu;
					}
                    break;

                case GameStates.Options:
					backToTitle.IsClicked(previousMouseState);
					break;

				case GameStates.Game:
					if (Utilities.SingleKeyPress(previousKeyboardState, currentKBState, Keys.Escape))
					{
						currentGameState = GameStates.PauseMenu;
					}

					if (player.Y > GraphicsDevice.Viewport.Height)
					{
						currentGameState = GameStates.GameOver;
					}

					// Handles player movement
					player.Update(gameTime, previousKeyboardState);
					//Handles drawing blocks
					controller.CheckForRectDraw(previousMouseState, GraphicsDevice.Viewport.Bounds);
					//Moves all of the bullets
					foreach (Bullet b in controller.Bullets)
					{
						b.X += b.Direction * 5;
					}
					// Handles collisions between the player and all other collidables
					collisionManager.Colliding();
					//Handles switching block types
					controller.CheckBlockTypeChange(previousKeyboardState);
					//Restarts the level if the player wants to
					if (Utilities.SingleKeyPress(previousKeyboardState, currentKBState, Keys.R))
						controller.ResetLevel();
					// Game Win condition
					if (collisionManager.isColliding(controller.LevelPlayer, controller.Goal))
						if (!controller.NextLevel())
							currentGameState = GameStates.GameWon;

					break;

				case GameStates.PauseMenu:
					// When clicked, "unpauses" the game
					// Changes GameState back from Pause to Game
					unpause.IsClicked(previousMouseState);
                    backToTitle.IsClicked(previousMouseState);
                    if (Utilities.SingleKeyPress(previousKeyboardState, currentKBState, Keys.Escape))
					{
						currentGameState = GameStates.Game;
					}
					break;

				case GameStates.GameOver:
					if (Utilities.SingleKeyPress(previousKeyboardState, currentKBState, Keys.Enter))
						currentGameState = GameStates.MainMenu;
					break;

				case GameStates.GameWon:
					backToTitle.IsClicked(previousMouseState);
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

                    //Draws buttons
                    play.Draw(spriteBatch, Color.Black, Color.White, fontArial);
					options.Draw(spriteBatch, Color.Black, Color.White, fontArial);
                   
                    break;
				case GameStates.Options:
					// ***Controls***
					spriteBatch.DrawString(fontArial, "Controls:", new Vector2(20, 20), Color.White);

					spriteBatch.DrawString(fontArial, "A - Move left", new Vector2(20, 80), Color.White);
					
					spriteBatch.DrawString(fontArial, "D - Move right", new Vector2(20, 115), Color.White);

					spriteBatch.DrawString(fontArial, "W - Jump", new Vector2(20, 150), Color.White);

					spriteBatch.DrawString(fontArial, "Space - Shoot", new Vector2(20, 185), Color.White);

					spriteBatch.DrawString(fontArial, "1/2/3 - Change ink color (black/blue/red)", new Vector2(20, 220), Color.White);

					spriteBatch.DrawString(fontArial, "Q/E - Cycle between ink colors", new Vector2(20, 255), Color.White);

					backToTitle.Draw(spriteBatch, Color.Black, Color.White, fontArial);
					break;

                case GameStates.LevelSelect:
                    levelSelectBackground.Draw(spriteBatch, Color.Black);
                    foreach(Tuple<Button<int>, bool> b in levelButtons)
                    {
                        b.Item1.Draw(spriteBatch, b.Item2 ? Color.Gray : Color.Red, Color.Black, fontArial);
                    }
                    break;

                case GameStates.Game:
                    controller.Draw(spriteBatch);
					// Writes the current ink level
					spriteBatch.DrawString(fontArial, 
					"Ink Level: " + controller.LevelPlayer.InkLevels,
					new Vector2(10, 10),
					Color.Black);
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

                    //Draws "Back to Title" button
                    backToTitle.Draw(spriteBatch, Color.Black, Color.White, fontArial);

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
					GraphicsDevice.Clear(Color.Gold);

					// Writes GameWon
					spriteBatch.DrawString(
					fontArial,
					"Game Won",
					new Vector2((GraphicsDevice.Viewport.Width / 2) - (fontArial.MeasureString("Game Won").X / 2), GraphicsDevice.Viewport.Height / 4),
					Color.Black);

					backToTitle.Draw(spriteBatch, Color.Black, Color.White, fontArial);
					break;
			}
            spriteBatch.End();

			base.Draw(gameTime);
		}

        private void EnterLevel(int levelID)
        {
            currentGameState = GameStates.Game;
            controller.LoadLevel(levelID);
        }

        private void SwitchGameState(GameStates state)
        {
            currentGameState = state;
            //Re-loads the level buttons
            if(currentGameState == GameStates.LevelSelect)
            {
                levelButtons.Clear();

                List<Tuple<int, bool>> mapData = controller.GetMapData();
                int x = GraphicsDevice.Viewport.Width / 2 - 105;
                int y = GraphicsDevice.Viewport.Height / 2 - 105;
                foreach (Tuple<int, bool> data in mapData)
                {
                    levelButtons.Add(new Tuple<Button<int>, bool>(new Button<int>(new Rectangle(x, y, 20, 20), blankTexture, EnterLevel, data.Item1, "" + data.Item1), data.Item2));
                    x += 30;
                    if(x > GraphicsDevice.Viewport.Width / 2 + 105)
                    {
                        x = GraphicsDevice.Viewport.Width / 2 - 105;
                        y += 30;
                    }
                }
            }
        }
    }
}
