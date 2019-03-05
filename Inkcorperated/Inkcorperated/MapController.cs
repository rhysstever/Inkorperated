using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Inkcorperated
{
    class MapController
    {
        List<Map> levels;
        List<Block> customBlocks;
        Player player;
        Drawable goal;
		List<Bullet> bullets;
        int currentLevel;
        Texture2D playerTexture;
        Texture2D blockTexture;
        Texture2D enemyTexture;
        BlockType selectedType;

        bool invalidDrawCheck;

		// Properties
		
		public List<Block> CustomBlocks { get { return customBlocks; } }
		public Player LevelPlayer { get { return player; } }
		public Drawable Goal { get { return goal; } }
		public List<Bullet> Bullets { get { return bullets; } }
        public bool DrawingBlock { get { return Mouse.GetState().LeftButton == ButtonState.Pressed && !invalidDrawCheck; } }

		public MapController()
        {
            levels = new List<Map>();
            customBlocks = new List<Block>();
            selectedType = BlockType.Basic;
            player = new Player(new Rectangle(), null, 0);
            invalidDrawCheck = false;
        }

        /// <summary>
        /// Loads all levels in from an external file that exists at: *\Inkcorperated\Inkcorperated\bin\Windows\x86\Debug\Levels.txt
        /// File Formatting (all listed values are integers):
        /// amountOfLevels
        /// playerStartX playerStartY playerWidth playerHeight                                      |
        /// inkLimit                                                                                |
        /// goalX goalY goalWidth goalHeight                                                        |
        /// amtOfBlocks                                                                             |--- Repeat for amtOfLevels
        /// blockX blockY blockWidth blockHeight                |--repeat for amtOfBlocks           |
        /// blockType                                           |                                   |
        /// amtOfEnemies                                        Should be 0 until enemies are made  |
        /// enemyX enemyY enemyWidth enemyHeight                --repeat for amtOfEnemies           |
        /// </summary>
        public void LoadLevels(Texture2D playerTexture, Texture2D blockTexture, Texture2D enemyTexture, Texture2D goalTexture)
        {
            this.playerTexture = playerTexture;
            this.blockTexture = blockTexture;
            this.enemyTexture = enemyTexture;
            //Sets up the goal to have the goal texture
            goal = new Drawable(new Rectangle(), goalTexture);

            StreamReader reader = new StreamReader("../../../../Content/Levels.txt");
            int amtOfLevels = int.Parse(reader.ReadLine());
            //For the amount of levels that need to be loaded
            for (int i = 0; i < amtOfLevels; i++)
            {
                Map newMap = new Map(ParseQuad(reader.ReadLine()), int.Parse(reader.ReadLine()), ParseQuad(reader.ReadLine()));
                int amtOfBlocks = int.Parse(reader.ReadLine());
                //Loads all of the blocks in this level
                for (int x = 0; x < amtOfBlocks; x++)
                {
                    newMap.AddBlock(new Block(ParseQuad(reader.ReadLine()), blockTexture, (BlockType)int.Parse(reader.ReadLine())));
                }

                int amtOfEnemies = int.Parse(reader.ReadLine());
                //Loads all of the enemies in this level
                for (int x = 0; x < amtOfEnemies; x++)
                {
                    newMap.AddEnemy(new Enemy(ParseQuad(reader.ReadLine()), blockTexture));
                }

                //Adds the level to the main list
                levels.Add(newMap);
            }
            reader.Close();
        }

        public void LoadLevel(int level)
        {
            currentLevel = level;
            //Clears all of the player-drawn blocks
            customBlocks.Clear();
            //Updates the player position and size
            player.UpdatePlayer(levels[currentLevel].PlayerStart, playerTexture, levels[currentLevel].InkLimit);
            //Changes the position and size of the goal to match the new level's
            goal.X = levels[currentLevel].Goal.X;
            goal.Y = levels[currentLevel].Goal.Y;
            goal.Width = levels[currentLevel].Goal.Width;
            goal.Height = levels[currentLevel].Goal.Height;
        }

		public Map GetCurrentMap()
		{
			return levels[currentLevel];
		}

        public void ResetLevel()
        {
            LoadLevel(currentLevel);
        }

        public bool NextLevel()
        {
            //If there is no next level, return a failure
            if (currentLevel + 1 >= levels.Count)
                return false;
            currentLevel++;
            LoadLevel(currentLevel);
            return true;
        }

        public void CheckBlockTypeChange(KeyboardState previousKeyboardState)
        {
            KeyboardState currentState = Keyboard.GetState();
            
            if (currentState.IsKeyDown(Keys.D1) || (SingleKeyPress(previousKeyboardState, currentState, Keys.Q) && selectedType == BlockType.Speed) || (SingleKeyPress(previousKeyboardState, currentState, Keys.E) && selectedType == BlockType.Bouncy))
                selectedType = BlockType.Basic;
            else if (currentState.IsKeyDown(Keys.D2) || (SingleKeyPress(previousKeyboardState, currentState, Keys.Q) && selectedType == BlockType.Bouncy) || (SingleKeyPress(previousKeyboardState, currentState, Keys.E) && selectedType == BlockType.Basic))
                selectedType = BlockType.Speed;
            else if (currentState.IsKeyDown(Keys.D3) || (SingleKeyPress(previousKeyboardState, currentState, Keys.Q) && selectedType == BlockType.Basic) || (SingleKeyPress(previousKeyboardState, currentState, Keys.E) && selectedType == BlockType.Speed))
                selectedType = BlockType.Bouncy;
        }

        /// <summary>
        /// Returns true if this is the first frame that the key was pressed
        /// False otherwise
        /// </summary>
        /// <param name="key">Represents the key to check (One of the "Keys" enum values)</param>
        public bool SingleKeyPress(KeyboardState previous, KeyboardState current, Keys key)
        {
            return current.IsKeyDown(key) && previous.IsKeyUp(key);
        }

        public void CheckForRectDraw(MouseState previousMouseState, Rectangle window)
        {
            MouseState currentState = Mouse.GetState();
            //If the player started clicking this frame
            if (previousMouseState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
            {
                if (!window.Contains(currentState.Position))
                {
                    invalidDrawCheck = true;
                    return;
                }
                customBlocks.Add(new Block(new Rectangle(RoundDownToNearestTwenty(currentState.X), RoundDownToNearestTwenty(currentState.Y), 0, 0), blockTexture, selectedType));
            }

            //if the player is clicking
            if (DrawingBlock)
            {
                customBlocks[customBlocks.Count - 1].Width = RoundUpToNearestTwenty(currentState.X - customBlocks[customBlocks.Count - 1].X);
                customBlocks[customBlocks.Count - 1].Height = RoundUpToNearestTwenty(currentState.Y - customBlocks[customBlocks.Count - 1].Y);
            }

            //if the player stopped clicking this frame
            if (previousMouseState.LeftButton == ButtonState.Pressed && currentState.LeftButton == ButtonState.Released)
            {
                if (invalidDrawCheck)
                {
                    invalidDrawCheck = false;
                    return;
                }
                //fixes the values of the newly placed box so collision isn't a pain in the ass
                if (customBlocks[customBlocks.Count - 1].Width < 0)
                {
                    customBlocks[customBlocks.Count - 1].X += customBlocks[customBlocks.Count - 1].Width;
                    customBlocks[customBlocks.Count - 1].Width *= -1;
                }
                if (customBlocks[customBlocks.Count - 1].Height < 0)
                {
                    customBlocks[customBlocks.Count - 1].Y += customBlocks[customBlocks.Count - 1].Height;
                    customBlocks[customBlocks.Count - 1].Height *= -1;
                }
                bool removed = false;

                //Removes the box if it intersects the player, goal or any other boxes
                if (customBlocks[customBlocks.Count - 1].Bounds.Intersects(player.Bounds))
                {
                    customBlocks.RemoveAt(customBlocks.Count - 1);
                    removed = true;
                }
                for (int i = 0; i < customBlocks.Count - 1; i++)
                {
                    if (customBlocks[customBlocks.Count - 1].Bounds.Intersects(customBlocks[i].Bounds))
                    {
                        customBlocks.RemoveAt(customBlocks.Count - 1);
                        removed = true;
                        break;
                    }
                }
                if (customBlocks.Count > 0 && levels[currentLevel].IntersectsWithExisting(customBlocks[customBlocks.Count - 1]))
                {
                    customBlocks.RemoveAt(customBlocks.Count - 1);
                    removed = true;
                }

                Console.WriteLine(removed);

                if (!removed)
                {
                    if(player.InkLevels >= customBlocks[customBlocks.Count - 1].GetInkCost())
                        player.InkLevels -= customBlocks[customBlocks.Count - 1].GetInkCost();
                    else
                        customBlocks.RemoveAt(customBlocks.Count - 1);
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            levels[currentLevel].Draw(batch);
            goal.Draw(batch, Color.White);
            player.Draw(batch, Color.White);

            if (customBlocks.Count > 0)
            {
                //draws each of the player-drawn blocks
                for (int i = 0; i < customBlocks.Count - 1; i++)
                {
                    switch (customBlocks[i].Type)
                    {
                        case BlockType.Basic:
                            customBlocks[i].Draw(batch, Color.Black);
                            break;
                        case BlockType.Speed:
                            customBlocks[i].Draw(batch, Color.Blue);
                            break;
                        case BlockType.Bouncy:
                            customBlocks[i].Draw(batch, Color.Red);
                            break;
                    }
                }

                //Fixes the values of the one the player is currently drawing so it draws correctly
                Block fixedBox = new Block(new Rectangle(customBlocks[customBlocks.Count - 1].X, customBlocks[customBlocks.Count - 1].Y, customBlocks[customBlocks.Count - 1].Width, customBlocks[customBlocks.Count - 1].Height), blockTexture, customBlocks[customBlocks.Count - 1].Type);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !invalidDrawCheck)
                {
                    if (fixedBox.Height < 0)
                    {
                        fixedBox.Y += fixedBox.Height;
                        fixedBox.Height *= -1;
                    }
                    if (fixedBox.Width < 0)
                    {
                        fixedBox.X += fixedBox.Width;
                        fixedBox.Width *= -1;
                    }
                    switch (fixedBox.Type)
                    {
                        case BlockType.Basic:
                            fixedBox.Draw(batch, player.InkLevels >= customBlocks[customBlocks.Count - 1].GetInkCost() ? Color.Black : new Color(0, 0, 0, 63));
                            break;
                        case BlockType.Speed:
                            fixedBox.Draw(batch, player.InkLevels >= customBlocks[customBlocks.Count - 1].GetInkCost() ? Color.Blue : new Color(0, 0, 255, 63));
                            break;
                        case BlockType.Bouncy:
                            fixedBox.Draw(batch, player.InkLevels >= customBlocks[customBlocks.Count - 1].GetInkCost() ? Color.Red : new Color(255, 0, 0, 63));
                            break;
                    }
                }
                else
                {
                    switch (fixedBox.Type)
                    {
                        case BlockType.Basic:
                            fixedBox.Draw(batch, Color.Black);
                            break;
                        case BlockType.Speed:
                            fixedBox.Draw(batch, Color.Blue);
                            break;
                        case BlockType.Bouncy:
                            fixedBox.Draw(batch, Color.Red);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Parses a string in the form "number1 number2 number3 number4" into a rectangle with the corresponding values
        /// </summary>
        private Rectangle ParseQuad(string n)
        {
            string[] split = n.Split(' ');
            return new Rectangle(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]));
        }

        /// <summary>
        /// Rounds upwards to the nearest ten
        /// </summary>
        private int RoundUpToNearestTwenty(int i)
        {
            return Math.Sign(i) * (int)Math.Ceiling(Math.Abs(i / 20.0)) * 20;
        }

        /// <summary>
        /// Rounds downward to the nearest ten
        /// </summary>
        private int RoundDownToNearestTwenty(int i)
        {
            return Math.Sign(i) * (int)Math.Floor(Math.Abs(i / 20.0)) * 20;
        }
    }
}
