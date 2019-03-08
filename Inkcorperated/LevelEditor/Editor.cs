using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LevelEditor
{
    public enum BlockTypes
    {
        Empty,
        Basic,
        Speed,
        Bouncy,
        Player,
        Goal
    }

    public partial class window : Form
    {
        PictureBox[,] visualMap = new PictureBox[40, 24];
        int width;
        int height;

        Image player;
        Image goal;

        Tuple<int, int> playerPos = new Tuple<int, int>(-1, -1);
        Tuple<int, int> goalPos = new Tuple<int, int>(-1, -1);

        BlockTypes editType = BlockTypes.Basic;

        bool changed = false;

        Form1 creator;

        public window(Form1 mainForm, int x = 40, int y = 24)
        {
            width = x;
            height = y;
            creator = mainForm;
            InitializeComponent();
            player = Player.BackgroundImage;
            goal = Goal.BackgroundImage;
            GenerateTiles();
        }

        public window(string path, Form1 mainForm)
        {
            width = 40;
            height = 24;
            creator = mainForm;
            InitializeComponent();
            player = Player.BackgroundImage;
            goal = Goal.BackgroundImage;
            LoadLevel(path);
        }

        /// <summary>
        /// Makes all of the display tiles
        /// </summary>
        private void GenerateTiles()
        {
            SuspendLayout();
            foreach (PictureBox p in visualMap)
            {
                if(p != null)
                    p.Dispose();
            }
            visualMap = new PictureBox[40, 24];
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    visualMap[x, y] = new PictureBox();
                    visualMap[x, y].BackColor = Color.Transparent;
                    visualMap[x, y].BackgroundImageLayout = ImageLayout.Stretch;
                    visualMap[x, y].Name = x + ", " + y;
                    visualMap[x, y].Location = new Point(140 + 20 * x, 20 + 20 * y);
                    visualMap[x, y].Size = new Size(20, 20);
                    visualMap[x, y].Click += Tile_Click;
                    Controls.Add(visualMap[x, y]);
                    visualMap[x, y].BringToFront();
                }
            }
            ResumeLayout();
        }

        /// <summary>
        /// Saves the map to a .level file
        /// </summary>
        /// <param name="path">Where to save the .level to</param>
        public void SaveLevel(string path)
        {
            Stream outStream = File.OpenWrite(path);
            BinaryWriter file = new BinaryWriter(outStream);
            file.Flush();
            file.Write((int)InkLimit.Value);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (visualMap[x, y].BackColor == Color.Black)
                        file.Write(1);
                    else if (visualMap[x, y].BackColor == Color.Blue)
                        file.Write(2);
                    else if (visualMap[x, y].BackColor == Color.Red)
                        file.Write(3);
                    else
                    {
                        if (visualMap[x, y].BackgroundImage == null)
                            file.Write(0);
                        else if (visualMap[x, y].BackgroundImage == player)
                            file.Write(4);
                        else if (visualMap[x, y].BackgroundImage == goal)
                            file.Write(5);
                    }
                }
            }
            file.Close();
            ActiveForm.Text = "Level Editor - " + path.Substring(path.LastIndexOf('\\') + 1);
            MessageBox.Show("File Saved Successfully.", "File Saved", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Loads the requested level
        /// </summary>
        /// <param name="path">Path of the level to load</param>
        public void LoadLevel(string path)
        {
            Stream inStream = File.OpenRead(path);
            BinaryReader file = new BinaryReader(inStream);
            InkLimit.Value = file.ReadInt32();
            GenerateTiles();
            int value;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    value = file.ReadInt32();
                    Console.Write(value);
                    if (value == 1)
                        visualMap[x, y].BackColor = Color.Black;
                    else if (value == 2)
                        visualMap[x, y].BackColor = Color.Blue;
                    else if (value == 3)
                        visualMap[x, y].BackColor = Color.Red;
                    else if (value == 4)
                    {
                        visualMap[x, y].BackgroundImage = player;
                        playerPos = new Tuple<int, int>(x, y);
                    }
                    else if (value == 5)
                    {
                        visualMap[x, y].BackgroundImage = goal;
                        goalPos = new Tuple<int, int>(x, y);
                    }
                }
            }
            file.Close();
            changed = false;
            ActiveForm.Text = "Level Editor - " + path.Substring(path.LastIndexOf('\\') + 1);
            MessageBox.Show("File Loaded Successfully.", "File Loaded", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Changes what color is selected to place
        /// </summary>
        private void ColorPicker_Click(object sender, EventArgs e)
        {
            CurrentTile.BackgroundImage = null;
            CurrentTile.BackColor = Color.Transparent;
            if (((Button)sender).Name == "BasicBlock")
            {
                editType = BlockTypes.Basic;
                CurrentTile.BackColor = Color.Black;
            }
            else if (((Button)sender).Name == "SpeedBlock")
            {
                editType = BlockTypes.Speed;
                CurrentTile.BackColor = Color.Blue;
            }
            else if (((Button)sender).Name == "BouncyBlock")
            {
                editType = BlockTypes.Bouncy;
                CurrentTile.BackColor = Color.Red;
            }
            else if (((Button)sender).Name == "Player")
            {
                editType = BlockTypes.Player;
                CurrentTile.BackgroundImage = player;
            }
            else if (((Button)sender).Name == "Goal")
            {
                editType = BlockTypes.Goal;
                CurrentTile.BackgroundImage = goal;
            }
            else if (((Button)sender).Name == "Eraser")
            {
                editType = BlockTypes.Empty;
            }
        }

        /// <summary>
        /// Changes what the clicked tile displays
        /// </summary>
        private void Tile_Click(object sender, EventArgs e)
        {
            if (playerPos.ToString().Equals("(" + ((PictureBox)sender).Name + ")"))
                playerPos = new Tuple<int, int>(-1, -1);
            if (goalPos.ToString().Equals("(" + ((PictureBox)sender).Name + ")"))
                goalPos = new Tuple<int, int>(-1, -1);
            ((PictureBox)sender).BackgroundImage = null;
            ((PictureBox)sender).BackColor = Color.Transparent;
            switch (editType)
            {
                case BlockTypes.Basic:
                    ((PictureBox)sender).BackColor = Color.Black;
                    break;
                case BlockTypes.Speed:
                    ((PictureBox)sender).BackColor = Color.Blue;
                    break;
                case BlockTypes.Bouncy:
                    ((PictureBox)sender).BackColor = Color.Red;
                    break;
                case BlockTypes.Player:
                    ((PictureBox)sender).BackgroundImage = player;
                    if (playerPos.Item1 != -1 && playerPos.Item2 != -1)
                        visualMap[playerPos.Item1, playerPos.Item2].BackgroundImage = null;
                    playerPos = new Tuple<int, int>(int.Parse(((PictureBox)sender).Name.Substring(0, ((PictureBox)sender).Name.IndexOf(','))), int.Parse(((PictureBox)sender).Name.Substring(((PictureBox)sender).Name.IndexOf(' ') + 1)));
                    //Console.WriteLine("Drawing player");
                    break;
                case BlockTypes.Goal:
                    ((PictureBox)sender).BackgroundImage = goal;
                    if (goalPos.Item1 != -1 && goalPos.Item2 != -1)
                        visualMap[goalPos.Item1, goalPos.Item2].BackgroundImage = null;
                    goalPos = new Tuple<int, int>(int.Parse(((PictureBox)sender).Name.Substring(0, ((PictureBox)sender).Name.IndexOf(','))), int.Parse(((PictureBox)sender).Name.Substring(((PictureBox)sender).Name.IndexOf(' ') + 1)));
                    break;
            }
            if(!changed)
                ActiveForm.Text += " *";
            changed = true;
        }

        /// <summary>
        /// Finds where to save the level to
        /// </summary>
        private void Save_Click(object sender, EventArgs e)
        {
            if (playerPos.Item1 == -1 && playerPos.Item2 == -1)
            {
                MessageBox.Show("You cannot save a level without a player in it.", "Error", MessageBoxButtons.OK);
                return;
            }
            if (goalPos.Item1 == -1 && goalPos.Item2 == -1)
            {
                MessageBox.Show("You cannot save a level without a goal in it.", "Error", MessageBoxButtons.OK);
                return;
            }
            SaveFileDialog file = new SaveFileDialog();
            file.Title = "Choose where to save to";
            file.Filter = "Level Files|*.level";
            DialogResult result = file.ShowDialog();
            if (result == DialogResult.OK)
            {
                SaveLevel(file.FileName);
                if(changed)
                    ActiveForm.Text = ActiveForm.Text.Substring(0, ActiveForm.Text.Count() - 2);
                changed = false;
            }
        }

        /// <summary>
        /// Finds the .level to be loaded
        /// </summary>
        private void Load_Click(object sender, EventArgs e)
        {
            if (changed) {
                DialogResult check = MessageBox.Show("You have unsaved changes, are you sure you want to change files?", "Unsaved changes", MessageBoxButtons.YesNo);
                if (check == DialogResult.No)
                    return;
            }
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "Open a level file";
            file.Filter = "Level Files|*.level";
            DialogResult result = file.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadLevel(file.FileName);
            }
        }

        /// <summary>
        /// Intercepts the closing of the window if there are unsaved changes
        /// </summary>
        private void window_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changed)
            {
                DialogResult result = MessageBox.Show("You have unsaved changes, are you sure you want to exit?", "Unsaved changes", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    e.Cancel = true;
            }
            if (e.Cancel == false)
                creator.Show();
        }
    }
}
