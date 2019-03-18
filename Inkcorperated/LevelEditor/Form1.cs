using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Makes the editor window with a blank map
        /// </summary>
        private void CreateMap_Click(object sender, EventArgs e)
        {
            //Makes a new editor
            window editor = new window(this);
            Hide();
            editor.ShowDialog();
        }

        /// <summary>
        /// Loads the requested window
        /// </summary>
        private void LoadMap_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "Open a level file";
            file.Filter = "Level Files|*.level";
            DialogResult result = file.ShowDialog();
            if (result == DialogResult.OK)
            {
                window editor = new window(file.FileName, this);
                editor.Text = "Level Editor - " + file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1);
                Hide();
                editor.ShowDialog();
            }
        }
    }
}
