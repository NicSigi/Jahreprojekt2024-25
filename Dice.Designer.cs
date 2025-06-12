namespace JahresprojektNeu
{
    partial class Dice
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
            this.SuspendLayout();
            // 
            // Dice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Dice";
            this.Text = "Dice";
            this.Load += new System.EventHandler(this.Dice_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private void SetupControls()
        {
            // Initialize timer
            autoPlayTimer = new System.Windows.Forms.Timer();
            autoPlayTimer.Interval = 500;
            autoPlayTimer.Tick += AutoPlayTimer_Tick;

            // Main form setup
            this.Text = "Dice Game";
            this.Size = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(32, 34, 37)
            };
            this.Controls.Add(mainPanel);

            // Balance display
            Label balanceLabel = new Label
            {
                Text = "Balance:",
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            mainPanel.Controls.Add(balanceLabel);

            Label balanceValueLabel = new Label
            {
                Name = "lblBalance",
                Text = balance.ToString("C2"),
                ForeColor = Color.LightGreen,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(100, 20),
                AutoSize = true
            };
            mainPanel.Controls.Add(balanceValueLabel);

            // Profit/Loss display
            Label profitLabel = new Label
            {
                Text = "Session Profit:",
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(250, 20),
                AutoSize = true
            };
            mainPanel.Controls.Add(profitLabel);

            Label profitValueLabel = new Label
            {
                Name = "lblProfit",
                Text = sessionProfit.ToString("C2"),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(380, 20),
                AutoSize = true
            };
            mainPanel.Controls.Add(profitValueLabel);

            // Bet amount controls
            Label betLabel = new Label
            {
                Text = "Bet Amount:",
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                Location = new Point(20, 60),
                AutoSize = true
            };
            mainPanel.Controls.Add(betLabel);

            TextBox betTextBox = new TextBox
            {
                Name = "txtBetAmount",
                Text = betAmount.ToString("0.00"),
                Location = new Point(130, 60),
                Width = 100,
                BackColor = Color.FromArgb(50, 52, 55),
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                TextAlign = HorizontalAlignment.Right
            };
            mainPanel.Controls.Add(betTextBox);
            betTextBox.TextChanged += BetTextBox_TextChanged;

            // Half and Double buttons
            Button halfButton = new Button
            {
                Text = "½",
                Location = new Point(240, 60),
                Size = new Size(40, 25),
                BackColor = Color.FromArgb(60, 62, 65),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            mainPanel.Controls.Add(halfButton);
            halfButton.Click += HalfButton_Click;

            Button doubleButton = new Button
            {
                Text = "2×",
                Location = new Point(290, 60),
                Size = new Size(40, 25),
                BackColor = Color.FromArgb(60, 62, 65),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            mainPanel.Controls.Add(doubleButton);
            doubleButton.Click += DoubleButton_Click;

            // Roll mode selection
            GroupBox rollModeGroup = new GroupBox
            {
                Text = "Roll Mode",
                ForeColor = Color.White,
                Font = new Font("Arial", 10),
                Location = new Point(20, 100),
                Size = new Size(310, 60),
                BackColor = Color.FromArgb(40, 42, 45)
            };
            mainPanel.Controls.Add(rollModeGroup);

            RadioButton rollOverRadio = new RadioButton
            {
                Name = "radRollOver",
                Text = "Roll Over",
                Checked = isRollOver,
                ForeColor = Color.White,
                Location = new Point(20, 25),
                AutoSize = true,
                BackColor = Color.FromArgb(40, 42, 45)
            };
            rollModeGroup.Controls.Add(rollOverRadio);
            rollOverRadio.CheckedChanged += RollModeRadio_CheckedChanged;

            RadioButton rollUnderRadio = new RadioButton
            {
                Name = "radRollUnder",
                Text = "Roll Under",
                Checked = !isRollOver,
                ForeColor = Color.White,
                Location = new Point(150, 25),
                AutoSize = true,
                BackColor = Color.FromArgb(40, 42, 45)
            };
            rollModeGroup.Controls.Add(rollUnderRadio);
            rollUnderRadio.CheckedChanged += RollModeRadio_CheckedChanged;

            // Target number slider
            Label targetNumberLabel = new Label
            {
                Text = "Target Number:",
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                Location = new Point(20, 175),
                AutoSize = true
            };
            mainPanel.Controls.Add(targetNumberLabel);

            Label targetNumberValueLabel = new Label
            {
                Name = "lblTargetNumber",
                Text = targetNumber.ToString(),
                ForeColor = Color.Yellow,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(150, 175),
                AutoSize = true
            };
            mainPanel.Controls.Add(targetNumberValueLabel);

            TrackBar targetTrackBar = new TrackBar
            {
                Name = "trkTarget",
                Minimum = 1,
                Maximum = 99,
                Value = targetNumber,
                Location = new Point(20, 200),
                Size = new Size(310, 45),
                BackColor = Color.FromArgb(32, 34, 37)
            };
            mainPanel.Controls.Add(targetTrackBar);
            targetTrackBar.ValueChanged += TargetTrackBar_ValueChanged;

            // Win chance and multiplier display
            Label winChanceLabel = new Label
            {
                Text = "Win Chance:",
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                Location = new Point(20, 250),
                AutoSize = true
            };
            mainPanel.Controls.Add(winChanceLabel);

            Label winChanceValueLabel = new Label
            {
                Name = "lblWinChance",
                Text = "50%",
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                Location = new Point(130, 250),
                AutoSize = true
            };
            mainPanel.Controls.Add(winChanceValueLabel);

            Label multiplierLabel = new Label
            {
                Text = "Multiplier:",
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                Location = new Point(200, 250),
                AutoSize = true
            };
            mainPanel.Controls.Add(multiplierLabel);

            Label multiplierValueLabel = new Label
            {
                Name = "lblMultiplier",
                Text = "2.00×",
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                Location = new Point(280, 250),
                AutoSize = true
            };
            mainPanel.Controls.Add(multiplierValueLabel);

            // Roll button
            Button rollButton = new Button
            {
                Name = "btnRoll",
                Text = "ROLL DICE",
                Location = new Point(20, 290),
                Size = new Size(310, 40),
                BackColor = Color.FromArgb(114, 137, 218),
                ForeColor = Color.White,
                Font = new Font("Arial", 14, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            mainPanel.Controls.Add(rollButton);
            rollButton.Click += RollButton_Click;

            // Result display
            Panel resultPanel = new Panel
            {
                Name = "pnlResult",
                Location = new Point(20, 350),
                Size = new Size(310, 100),
                BackColor = Color.FromArgb(40, 42, 45),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };
            mainPanel.Controls.Add(resultPanel);

            Label resultLabel = new Label
            {
                Name = "lblResult",
                Text = "",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 10),
                Size = new Size(310, 40),
                BackColor = Color.Transparent
            };
            resultPanel.Controls.Add(resultLabel);

            Label resultInfoLabel = new Label
            {
                Name = "lblResultInfo",
                Text = "",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 60),
                Size = new Size(310, 30),
                BackColor = Color.Transparent
            };
            resultPanel.Controls.Add(resultInfoLabel);

            // Auto Play Panel
            GroupBox autoPlayGroup = new GroupBox
            {
                Text = "Auto Play",
                ForeColor = Color.White,
                Font = new Font("Arial", 10),
                Location = new Point(350, 60),
                Size = new Size(400, 320),
                BackColor = Color.FromArgb(40, 42, 45)
            };
            mainPanel.Controls.Add(autoPlayGroup);

            // Number of Plays
            Label autoPlaysLabel = new Label
            {
                Text = "Number of Plays:",
                ForeColor = Color.White,
                Location = new Point(15, 30),
                AutoSize = true
            };
            autoPlayGroup.Controls.Add(autoPlaysLabel);

            NumericUpDown autoPlaysNumeric = new NumericUpDown
            {
                Name = "numAutoPlays",
                Minimum = 1,
                Maximum = 1000,
                Value = 10,
                Location = new Point(200, 30),
                Size = new Size(80, 20),
                BackColor = Color.FromArgb(50, 52, 55),
                ForeColor = Color.White
            };
            autoPlayGroup.Controls.Add(autoPlaysNumeric);

            // On Win Adjustment
            Label onWinLabel = new Label
            {
                Text = "On Win Adjustment (%):",
                ForeColor = Color.White,
                Location = new Point(15, 70),
                AutoSize = true
            };
            autoPlayGroup.Controls.Add(onWinLabel);

            NumericUpDown onWinNumeric = new NumericUpDown
            {
                Name = "numOnWin",
                Minimum = -100,
                Maximum = 100,
                Value = 0,
                Location = new Point(200, 70),
                Size = new Size(80, 20),
                BackColor = Color.FromArgb(50, 52, 55),
                ForeColor = Color.White
            };
            autoPlayGroup.Controls.Add(onWinNumeric);

            // On Loss Adjustment
            Label onLossLabel = new Label
            {
                Text = "On Loss Adjustment (%):",
                ForeColor = Color.White,
                Location = new Point(15, 110),
                AutoSize = true
            };
            autoPlayGroup.Controls.Add(onLossLabel);

            NumericUpDown onLossNumeric = new NumericUpDown
            {
                Name = "numOnLoss",
                Minimum = -100,
                Maximum = 100,
                Value = 0,
                Location = new Point(200, 110),
                Size = new Size(80, 20),
                BackColor = Color.FromArgb(50, 52, 55),
                ForeColor = Color.White
            };
            autoPlayGroup.Controls.Add(onLossNumeric);

            // Stop on Win
            Label stopWinLabel = new Label
            {
                Text = "Stop on Win Amount:",
                ForeColor = Color.White,
                Location = new Point(15, 150),
                AutoSize = true
            };
            autoPlayGroup.Controls.Add(stopWinLabel);

            TextBox stopWinTextBox = new TextBox
            {
                Name = "txtStopWin",
                Text = "0.00",
                Location = new Point(200, 150),
                Size = new Size(80, 20),
                BackColor = Color.FromArgb(50, 52, 55),
                ForeColor = Color.White,
                TextAlign = HorizontalAlignment.Right
            };
            autoPlayGroup.Controls.Add(stopWinTextBox);

            // Stop on Loss
            Label stopLossLabel = new Label
            {
                Text = "Stop on Loss Amount:",
                ForeColor = Color.White,
                Location = new Point(15, 190),
                AutoSize = true
            };
            autoPlayGroup.Controls.Add(stopLossLabel);

            TextBox stopLossTextBox = new TextBox
            {
                Name = "txtStopLoss",
                Text = "0.00",
                Location = new Point(200, 190),
                Size = new Size(80, 20),
                BackColor = Color.FromArgb(50, 52, 55),
                ForeColor = Color.White,
                TextAlign = HorizontalAlignment.Right
            };
            autoPlayGroup.Controls.Add(stopLossTextBox);

            // Auto play button
            Button autoPlayButton = new Button
            {
                Name = "btnAutoPlay",
                Text = "START AUTO PLAY",
                Location = new Point(15, 240),
                Size = new Size(370, 40),
                BackColor = Color.FromArgb(114, 137, 218),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            autoPlayGroup.Controls.Add(autoPlayButton);
            autoPlayButton.Click += AutoPlayButton_Click;

            // Game History Panel
            GroupBox historyGroup = new GroupBox
            {
                Text = "Game History",
                ForeColor = Color.White,
                Font = new Font("Arial", 10),
                Location = new Point(350, 390),
                Size = new Size(400, 160),
                BackColor = Color.FromArgb(40, 42, 45)
            };
            mainPanel.Controls.Add(historyGroup);

            ListBox historyListBox = new ListBox
            {
                Name = "lstHistory",
                BackColor = Color.FromArgb(30, 32, 35),
                ForeColor = Color.White,
                Font = new Font("Consolas", 9),
                Location = new Point(15, 30),
                Size = new Size(370, 115),
                BorderStyle = BorderStyle.None,
                FormattingEnabled = true,
                IntegralHeight = false
            };
            historyGroup.Controls.Add(historyListBox);

            // Set up auto play timer
            // Timer already initialized at the top of the method

            // Update display
            UpdateTargetAndMultiplier();
        }

    }
}