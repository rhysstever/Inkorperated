namespace LevelEditor
{
    partial class window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(window));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Enemy = new System.Windows.Forms.Button();
            this.Eraser = new System.Windows.Forms.Button();
            this.Goal = new System.Windows.Forms.Button();
            this.Player = new System.Windows.Forms.Button();
            this.BouncyBlock = new System.Windows.Forms.Button();
            this.SpeedBlock = new System.Windows.Forms.Button();
            this.BasicBlock = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CurrentTile = new System.Windows.Forms.PictureBox();
            this.Save = new System.Windows.Forms.Button();
            this.Load = new System.Windows.Forms.Button();
            this.Map = new System.Windows.Forms.GroupBox();
            this.InkLimit = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InkLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Enemy);
            this.groupBox1.Controls.Add(this.Eraser);
            this.groupBox1.Controls.Add(this.Goal);
            this.groupBox1.Controls.Add(this.Player);
            this.groupBox1.Controls.Add(this.BouncyBlock);
            this.groupBox1.Controls.Add(this.SpeedBlock);
            this.groupBox1.Controls.Add(this.BasicBlock);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(120, 236);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Block Selector";
            // 
            // Enemy
            // 
            this.Enemy.BackColor = System.Drawing.Color.Transparent;
            this.Enemy.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Enemy.BackgroundImage")));
            this.Enemy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Enemy.Location = new System.Drawing.Point(62, 127);
            this.Enemy.Name = "Enemy";
            this.Enemy.Size = new System.Drawing.Size(50, 50);
            this.Enemy.TabIndex = 6;
            this.Enemy.UseVisualStyleBackColor = false;
            this.Enemy.Click += new System.EventHandler(this.ColorPicker_Click);
            // 
            // Eraser
            // 
            this.Eraser.BackColor = System.Drawing.Color.Transparent;
            this.Eraser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Eraser.BackgroundImage")));
            this.Eraser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Eraser.Location = new System.Drawing.Point(6, 180);
            this.Eraser.Name = "Eraser";
            this.Eraser.Size = new System.Drawing.Size(50, 50);
            this.Eraser.TabIndex = 5;
            this.Eraser.UseVisualStyleBackColor = false;
            this.Eraser.Click += new System.EventHandler(this.ColorPicker_Click);
            // 
            // Goal
            // 
            this.Goal.BackColor = System.Drawing.Color.Transparent;
            this.Goal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Goal.BackgroundImage")));
            this.Goal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Goal.Location = new System.Drawing.Point(6, 127);
            this.Goal.Name = "Goal";
            this.Goal.Size = new System.Drawing.Size(50, 50);
            this.Goal.TabIndex = 4;
            this.Goal.UseVisualStyleBackColor = false;
            this.Goal.Click += new System.EventHandler(this.ColorPicker_Click);
            // 
            // Player
            // 
            this.Player.BackColor = System.Drawing.Color.Transparent;
            this.Player.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Player.BackgroundImage")));
            this.Player.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Player.Location = new System.Drawing.Point(62, 75);
            this.Player.Name = "Player";
            this.Player.Size = new System.Drawing.Size(50, 50);
            this.Player.TabIndex = 3;
            this.Player.UseVisualStyleBackColor = false;
            this.Player.Click += new System.EventHandler(this.ColorPicker_Click);
            // 
            // BouncyBlock
            // 
            this.BouncyBlock.BackColor = System.Drawing.Color.Red;
            this.BouncyBlock.Location = new System.Drawing.Point(6, 75);
            this.BouncyBlock.Name = "BouncyBlock";
            this.BouncyBlock.Size = new System.Drawing.Size(50, 50);
            this.BouncyBlock.TabIndex = 2;
            this.BouncyBlock.UseVisualStyleBackColor = false;
            this.BouncyBlock.Click += new System.EventHandler(this.ColorPicker_Click);
            // 
            // SpeedBlock
            // 
            this.SpeedBlock.BackColor = System.Drawing.Color.Blue;
            this.SpeedBlock.Location = new System.Drawing.Point(62, 19);
            this.SpeedBlock.Name = "SpeedBlock";
            this.SpeedBlock.Size = new System.Drawing.Size(50, 50);
            this.SpeedBlock.TabIndex = 1;
            this.SpeedBlock.UseVisualStyleBackColor = false;
            this.SpeedBlock.Click += new System.EventHandler(this.ColorPicker_Click);
            // 
            // BasicBlock
            // 
            this.BasicBlock.BackColor = System.Drawing.Color.Black;
            this.BasicBlock.Location = new System.Drawing.Point(6, 19);
            this.BasicBlock.Name = "BasicBlock";
            this.BasicBlock.Size = new System.Drawing.Size(50, 50);
            this.BasicBlock.TabIndex = 0;
            this.BasicBlock.UseVisualStyleBackColor = false;
            this.BasicBlock.Click += new System.EventHandler(this.ColorPicker_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CurrentTile);
            this.groupBox2.Location = new System.Drawing.Point(3, 242);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(120, 120);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current Tile";
            // 
            // CurrentTile
            // 
            this.CurrentTile.BackColor = System.Drawing.Color.Black;
            this.CurrentTile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CurrentTile.Location = new System.Drawing.Point(12, 17);
            this.CurrentTile.Name = "CurrentTile";
            this.CurrentTile.Size = new System.Drawing.Size(95, 95);
            this.CurrentTile.TabIndex = 0;
            this.CurrentTile.TabStop = false;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(15, 404);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(95, 50);
            this.Save.TabIndex = 2;
            this.Save.Text = "Save File";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Load
            // 
            this.Load.Location = new System.Drawing.Point(15, 460);
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(95, 50);
            this.Load.TabIndex = 3;
            this.Load.Text = "Load File";
            this.Load.UseVisualStyleBackColor = true;
            this.Load.Click += new System.EventHandler(this.Load_Click);
            // 
            // Map
            // 
            this.Map.Location = new System.Drawing.Point(130, 0);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size(820, 510);
            this.Map.TabIndex = 4;
            this.Map.TabStop = false;
            this.Map.Text = "Map";
            // 
            // InkLimit
            // 
            this.InkLimit.Location = new System.Drawing.Point(33, 378);
            this.InkLimit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.InkLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.InkLimit.Name = "InkLimit";
            this.InkLimit.Size = new System.Drawing.Size(56, 20);
            this.InkLimit.TabIndex = 5;
            this.InkLimit.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 362);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Ink Limit";
            // 
            // window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 516);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InkLimit);
            this.Controls.Add(this.Load);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Map);
            this.Name = "window";
            this.Text = "Level Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.window_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CurrentTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InkLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BasicBlock;
        private System.Windows.Forms.Button Goal;
        private System.Windows.Forms.Button Player;
        private System.Windows.Forms.Button BouncyBlock;
        private System.Windows.Forms.Button SpeedBlock;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox CurrentTile;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Load;
        private System.Windows.Forms.GroupBox Map;
        private System.Windows.Forms.Button Eraser;
        private System.Windows.Forms.NumericUpDown InkLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Enemy;
    }
}