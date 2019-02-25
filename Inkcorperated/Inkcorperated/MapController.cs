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

		// Properties

		public List<Map> Level { get { return levels; } }
		public List<Block> CustomBlocks { get { return customBlocks; } }
		public Player LevelPlayer { get { return player; } }
		public Drawable Goal { get { return goal; } }
		public List<Bullet> Bullets { get { return bullets; } }

		public MapController()
        {
            levels = new List<Map>();
            customBlocks = new List<Block>();
            selectedType = BlockType.Basic;
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
            player = new Player(levels[currentLevel].PlayerStart, playerTexture);
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
            currentLevel++;
            //If there is no next level, return a failure
            if (currentLevel >= levels.Count)
                return false;
            LoadLevel(currentLevel);
            return true;
        }

        public void CheckMovement(KeyboardState previousKeyboardState)
        {
            KeyboardState currentState = Keyboard.GetState();
            //player.Move();
            if (currentState.IsKeyDown(Keys.D1))
                selectedType = BlockType.Basic;
            if (currentState.IsKeyDown(Keys.D2))
                selectedType = BlockType.Speed;
            if (currentState.IsKeyDown(Keys.D3))
                selectedType = BlockType.Bouncy;
        }

        public void CheckForRectDraw(MouseState previousMouseState)
        {
            MouseState currentState = Mouse.GetState();
            //If the player started clicking this frame
            if (previousMouseState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
            {
                customBlocks.Add(new Block(new Rectangle(RoundDownToNearestTen(currentState.X), RoundDownToNearestTen(currentState.Y), 0, 0), blockTexture, selectedType));
            }

            //if the player is clicking
            if (currentState.LeftButton == ButtonState.Pressed)
            {
                customBlocks[customBlocks.Count - 1].Width = RoundUpToNearestTen(currentState.X - customBlocks[customBlocks.Count - 1].X);
                customBlocks[customBlocks.Count - 1].Height = RoundUpToNearestTen(currentState.Y - customBlocks[customBlocks.Count - 1].Y);
            }

            //if the player stopped clicking this frame
            if (previousMouseState.LeftButton == ButtonState.Pressed && currentState.LeftButton == ButtonState.Released)
            {
                //fixes the values of the newly placed box so collision isn't a pain in the ass
                if (customBlocks[customBlocks.Count - 1].Height < 0)
                {
                    customBlocks[customBlocks.Count - 1].Y += customBlocks[customBlocks.Count - 1].Height;
                    customBlocks[customBlocks.Count - 1].Height *= -1;
                }
                if (customBlocks[customBlocks.Count - 1].Width < 0)
                {
                    customBlocks[customBlocks.Count - 1].X += customBlocks[customBlocks.Count - 1].Width;
                    customBlocks[customBlocks.Count - 1].Width *= -1;
                }
                //Removes the box if it intersects the player, goal or any other boxes
                if (customBlocks[customBlocks.Count - 1].Bounds.Intersects(player.Bounds))
                    customBlocks.RemoveAt(customBlocks.Count - 1);
                for (int i = 0; i < customBlocks.Count - 1; i++)
                {
                    if (customBlocks[customBlocks.Count - 1].Bounds.Intersects(customBlocks[i].Bounds))
                    {
                        customBlocks.RemoveAt(customBlocks.Count - 1);
                        break;
                    }
                }
                if (customBlocks.Count > 0 && levels[currentLevel].IntersectsWithExisting(customBlocks[customBlocks.Count - 1]))
                    customBlocks.RemoveAt(customBlocks.Count - 1);
            }
            //Test script for multiple levels
            if (previousMouseState.RightButton == ButtonState.Released && currentState.RightButton == ButtonState.Pressed)
            {
                Console.WriteLine("Switch");
                NextLevel();
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
        private int RoundUpToNearestTen(int i)
        {
            return Math.Sign(i) * (int)Math.Ceiling(Math.Abs(i / 10.0)) * 10;
        }

        /// <summary>
        /// Rounds downward to the nearest ten
        /// </summary>
        private int RoundDownToNearestTen(int i)
        {
            return Math.Sign(i) * (int)Math.Floor(Math.Abs(i / 10.0)) * 10;
        }
    }
}
