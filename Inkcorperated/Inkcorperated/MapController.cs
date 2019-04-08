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
		List<Enemy> enemies;
        Drawable goal;
		static List<Bullet> bullets;
        int currentLevel;
        Texture2D playerTexture;
        Texture2D blockTexture;
        Texture2D enemyTexture;
        Texture2D background;
		Texture2D bulletTexture;
        Drawable inkContainer;
        Drawable inkFill;
        BlockType selectedType;
        GraphicsDeviceManager graphics;

        bool invalidDrawCheck;

		// Properties
		
		public List<Block> CustomBlocks { get { return customBlocks; } }
		public Player LevelPlayer { get { return player; } }
		public List<Enemy> Enemies { get { return enemies; } }
		public Drawable Goal { get { return goal; } }
		public List<Bullet> Bullets { get { return bullets; } }
        public bool DrawingBlock { get { return Mouse.GetState().LeftButton == ButtonState.Pressed && !invalidDrawCheck; } }
		public GraphicsDeviceManager Graphics { get { return graphics; } }

		public MapController(GraphicsDeviceManager graphics)
        {
            levels = new List<Map>();
            customBlocks = new List<Block>();
            selectedType = BlockType.Basic;
            player = new Player(1, Teams.Player, 1, new Rectangle(), null, 0);
			enemies = new List<Enemy>();
            bullets = new List<Bullet>();
            invalidDrawCheck = false;
            this.graphics = graphics;
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
        public void LoadLevels(Texture2D playerTexture, Texture2D blockTexture, Texture2D enemyTexture, Texture2D goalTexture,
            Texture2D inkContainerTexture, Texture2D inkFillTexture, Texture2D bgPic, Texture2D bulletPic)
        {
            this.playerTexture = playerTexture;
            this.blockTexture = blockTexture;
            this.enemyTexture = enemyTexture;
            background = bgPic;
			bulletTexture = bulletPic;
            inkContainer = new Drawable(new Rectangle(400, 30, 2, 2), inkContainerTexture);
            inkFill = new Drawable(new Rectangle(20, 20, 10, 50), inkFillTexture);
            //Sets up the goal to have the goal texture
            goal = new Drawable(new Rectangle(), goalTexture);
            for(int i = 1; i < i + 1; i++)
            {
                if(File.Exists("../../../../Content/Level" + i + ".level")) {
                    Stream inStream = File.OpenRead("../../../../Content/Level" + i + ".level");
                    BinaryReader file = new BinaryReader(inStream);
                    Map newMap = new Map(file.ReadInt32());
                    int value;
                    for (int y = 0; y < 24; y++)
                    {
                        for (int x = 0; x < 40; x++)
                        {
                            value = file.ReadInt32();
                            if (value == 1)
                                newMap.AddBlock(new Block(new Rectangle(x * 20, y * 20, 20, 20), blockTexture, BlockType.Basic));
                            else if (value == 2)
                                newMap.AddBlock(new Block(new Rectangle(x * 20, y * 20, 20, 20), blockTexture, BlockType.Speed));
                            else if (value == 3)
                                newMap.AddBlock(new Block(new Rectangle(x * 20, y * 20, 20, 20), blockTexture, BlockType.Bouncy));
                            else if (value == 4)
                                newMap.PlayerStart = new Rectangle(x * 20, y * 20, 20, 20);
                            else if (value == 5)
                                newMap.Goal = new Rectangle(x * 20, y * 20, 20, 20);
                        }
                    }
                    newMap.Unlocked = file.ReadBoolean();
                    levels.Add(newMap);
                    file.Close();
                }
                else
                {
                    break;
                }
            }
            levels[0].Unlocked = true;
        }

        public void LoadLevel(int level)
        {
            currentLevel = level;
            //Clears all of the player-drawn blocks
            customBlocks.Clear();
            selectedType = BlockType.Basic;
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

        public List<Tuple<int, bool>> GetMapData()
        {
            List<Tuple<int, bool>> mapData = new List<Tuple<int, bool>>();
            for(int i = 0; i < levels.Count; i++)
            {
                mapData.Add(new Tuple<int, bool>(i, levels[i].Unlocked));
            }
            Console.WriteLine(mapData.Count);
            return mapData;
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
            levels[currentLevel].Unlocked = true;
            LoadLevel(currentLevel);
            return true;
        }

        public void ShootBullet(Rectangle bounds, Teams team, int direction)
        {
			bullets.Add(new Bullet(bounds, bulletTexture, team, direction));
        }

        public void CheckBlockTypeChange(KeyboardState previousKeyboardState)
        {
            KeyboardState currentState = Keyboard.GetState();
            
            if (currentState.IsKeyDown(Keys.D1) || (Utilities.SingleKeyPress(previousKeyboardState, currentState, Keys.Q) && selectedType == BlockType.Speed) 
				|| (Utilities.SingleKeyPress(previousKeyboardState, currentState, Keys.E) && selectedType == BlockType.Bouncy))
                selectedType = BlockType.Basic;
            else if (currentState.IsKeyDown(Keys.D2) || (Utilities.SingleKeyPress(previousKeyboardState, currentState, Keys.Q) && selectedType == BlockType.Bouncy) 
				|| (Utilities.SingleKeyPress(previousKeyboardState, currentState, Keys.E) && selectedType == BlockType.Basic))
                selectedType = BlockType.Speed;
            else if (currentState.IsKeyDown(Keys.D3) || (Utilities.SingleKeyPress(previousKeyboardState, currentState, Keys.Q) && selectedType == BlockType.Basic) 
				|| (Utilities.SingleKeyPress(previousKeyboardState, currentState, Keys.E) && selectedType == BlockType.Speed))
                selectedType = BlockType.Bouncy;
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
                customBlocks.Add(new Block(new Rectangle(Utilities.RoundDownToNearestTwenty(currentState.X), 
					Utilities.RoundDownToNearestTwenty(currentState.Y), 0, 0), blockTexture, selectedType));
            }

            //if the player is clicking
            if (DrawingBlock)
            {
                customBlocks[customBlocks.Count - 1].Width = Utilities.RoundUpToNearestTwenty(currentState.X - customBlocks[customBlocks.Count - 1].X);
                customBlocks[customBlocks.Count - 1].Height = Utilities.RoundUpToNearestTwenty(currentState.Y - customBlocks[customBlocks.Count - 1].Y);
                if(currentState.RightButton == ButtonState.Pressed){
                    invalidDrawCheck = true;
                    customBlocks.RemoveAt(customBlocks.Count - 1);
                }
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
            batch.Draw(background, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
            levels[currentLevel].Draw(batch);
            goal.Draw(batch, Color.White);
            player.Draw(batch, Color.White);

            /* Ink level bar not functional yet
            switch (selectedType)
            {
                case BlockType.Basic:
                    inkFill.Draw(batch, Color.Black);
                    inkContainer.Draw(batch, Color.White);
                    break;
                case BlockType.Speed:
                    inkFill.Draw(batch, Color.Blue);
                    inkContainer.Draw(batch, Color.White);
                    break;
                case BlockType.Bouncy:
                    inkFill.Draw(batch, Color.Red);
                    inkContainer.Draw(batch, Color.White);
                    break;
                default:
                    inkFill.Draw(batch, Color.Black);
                    inkContainer.Draw(batch, Color.White);
                    break;
            }*/

            foreach(Bullet b in bullets)
            {
                b.Draw(batch, b.Team == Teams.Player ? Color.Black : Color.Red);
            }

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
                Block fixedBox = new Block(new Rectangle(customBlocks[customBlocks.Count - 1].X, customBlocks[customBlocks.Count - 1].Y,
					customBlocks[customBlocks.Count - 1].Width, customBlocks[customBlocks.Count - 1].Height),
					blockTexture, customBlocks[customBlocks.Count - 1].Type);
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
    }
}
