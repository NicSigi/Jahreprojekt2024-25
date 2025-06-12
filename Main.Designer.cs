namespace JahresprojektNeu
{
    partial class Main
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
        private Label lblTitle;
        private Label lblSubtitle;

        private void InitializeComponent()
        {
            btnPlayMines = new Button();
            btnPlayDice = new Button();
            btnPlayPlinko = new Button();
            lblTitle = new Label();
            lblSubtitle = new Label();

            SuspendLayout();

            // 
            // Main Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            BackColor = Color.FromArgb(32, 34, 37);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Name = "Main";
            Text = "Online Casino Hub";
            Load += Main_Load;

            // 
            // lblTitle
            // 
            lblTitle.Text = "🎰 ONLINE CASINO";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.Gold;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point((ClientSize.Width - 340) / 2, 40); // zentriert ca.
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblSubtitle
            // 
            lblSubtitle.Text = "Willkommen bei deinem virtuellen Glücksspielerlebnis";
            lblSubtitle.Font = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.LightGray;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point((ClientSize.Width - 400) / 2, 90); // zentriert ca.
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // btnPlayMines
            // 
            btnPlayMines.BackColor = Color.FromArgb(114, 137, 218);
            btnPlayMines.FlatAppearance.BorderSize = 0;
            btnPlayMines.FlatStyle = FlatStyle.Flat;
            btnPlayMines.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnPlayMines.ForeColor = Color.White;
            btnPlayMines.Location = new Point(100, 180);
            btnPlayMines.Name = "btnPlayMines";
            btnPlayMines.Size = new Size(180, 50);
            btnPlayMines.TabIndex = 0;
            btnPlayMines.Text = "Play Mines";
            btnPlayMines.UseVisualStyleBackColor = false;
            btnPlayMines.Click += btnPlayMines_Click;

            // 
            // btnPlayDice
            // 
            btnPlayDice.BackColor = Color.FromArgb(78, 93, 148);
            btnPlayDice.FlatAppearance.BorderSize = 0;
            btnPlayDice.FlatStyle = FlatStyle.Flat;
            btnPlayDice.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnPlayDice.ForeColor = Color.White;
            btnPlayDice.Location = new Point(310, 180);
            btnPlayDice.Name = "btnPlayDice";
            btnPlayDice.Size = new Size(180, 50);
            btnPlayDice.TabIndex = 1;
            btnPlayDice.Text = "Play Dice";
            btnPlayDice.UseVisualStyleBackColor = false;
            btnPlayDice.Click += btnPlayDice_Click;

            // 
            // btnPlayPlinko
            // 
            btnPlayPlinko.BackColor = Color.FromArgb(64, 194, 133);
            btnPlayPlinko.FlatAppearance.BorderSize = 0;
            btnPlayPlinko.FlatStyle = FlatStyle.Flat;
            btnPlayPlinko.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnPlayPlinko.ForeColor = Color.White;
            btnPlayPlinko.Location = new Point(520, 180);
            btnPlayPlinko.Name = "btnPlayPlinko";
            btnPlayPlinko.Size = new Size(180, 50);
            btnPlayPlinko.TabIndex = 2;
            btnPlayPlinko.Text = "Play Plinko";
            btnPlayPlinko.UseVisualStyleBackColor = false;
            btnPlayPlinko.Click += btnPlayPlinko_Click;

            // 
            // Add controls
            // 
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(btnPlayMines);
            Controls.Add(btnPlayDice);
            Controls.Add(btnPlayPlinko);

            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private Button btnPlayMines;
        private Button btnPlayDice;
        private Button btnPlayPlinko;
    }
}