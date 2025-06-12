using JahresprojektNeu.Classes;
using JahresprojektNeu.Panels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JahresprojektNeu
{
    public partial class Plinko : Form
    {
        // Constants for the game board and the pins
        private const int PinRadius = 5;
        private const int BallRadius = 8;
        private const int PinSpacingX = 40;
        private const int PinSpacingY = 35;
        private const int BucketWidth = 40;
        private const int BucketHeight = 40;
        private const int StartY = 50;

        // Game variables
        private int rows = 8; // Default: 8 rows
        private RiskLevel riskLevel = RiskLevel.Medium; // Default: Medium risk
        private decimal betAmount = 1.0m; // Default bet
        private decimal balance => GameManager.Instance.Balance; // Initial balance

        // Animation
        private List<Ball> activeBalls = new List<Ball>();
        private System.Windows.Forms.Timer animationTimer = new System.Windows.Forms.Timer();
        private Random random = new Random();

        // Game settings
        private bool autoPlayEnabled = false;
        private int autoPlayCount = 0;
        private int autoPlayRemaining = 0;

        // UI elements
        private Label balanceLabel;
        private NumericUpDown betAmountInput;
        private ComboBox rowsComboBox;
        private ComboBox riskComboBox;
        private Button dropBallButton;
        private Button autoPlayButton;
        private CheckBox instantPlayCheckBox;
        private Label resultLabel;
        private Panel gamePanel;

        // Payout tables for different risk levels and rows
        private Dictionary<RiskLevel, Dictionary<int, decimal[]>> payoutTables = new Dictionary<RiskLevel, Dictionary<int, decimal[]>>();

        // Enumeration for risk levels
        public enum RiskLevel
        {
            Low,
            Medium,
            High
        }

        // Class for the ball animation
        private class Ball
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float VelocityX { get; set; }
            public float VelocityY { get; set; }
            public bool Active { get; set; } = true;
            public int TargetBucket { get; set; } = -1;
            public decimal Payout { get; set; } = 0;
        }

        public Plinko()
        {
            InitializeComponent();
            SetupPlinkoGame();
        }

        private void Plinko_Load(object sender, EventArgs e)
        {
            // InitializePayoutTables is called separately
        }

        private void InitializePayoutTables()
        {
            // Initialize dictionaries for each risk level
            payoutTables[RiskLevel.Low] = new Dictionary<int, decimal[]>();
            payoutTables[RiskLevel.Medium] = new Dictionary<int, decimal[]>();
            payoutTables[RiskLevel.High] = new Dictionary<int, decimal[]>();

            // Low risk level
            payoutTables[RiskLevel.Low][8] = new decimal[] { 5.6m, 2.1m, 1.0m, 0.7m, 0.5m, 0.7m, 1.0m, 2.1m, 5.6m };
            payoutTables[RiskLevel.Low][9] = new decimal[] { 5.6m, 2.0m, 1.4m, 1.0m, 0.7m, 0.7m, 1.0m, 1.4m, 2.0m, 5.6m };
            payoutTables[RiskLevel.Low][10] = new decimal[] { 8.9m, 3.0m, 1.4m, 1.0m, 0.7m, 0.5m, 0.7m, 1.0m, 1.4m, 3.0m, 8.9m };
            payoutTables[RiskLevel.Low][11] = new decimal[] { 8.4m, 3.0m, 1.4m, 1.0m, 0.7m, 0.5m, 0.5m, 0.7m, 1.0m, 1.4m, 3.0m, 8.4m };
            payoutTables[RiskLevel.Low][12] = new decimal[] { 10m, 3.0m, 1.4m, 1.0m, 0.7m, 0.5m, 0.5m, 0.5m, 0.7m, 1.0m, 1.4m, 3.0m, 10m };
            payoutTables[RiskLevel.Low][13] = new decimal[] { 8.1m, 4.0m, 1.4m, 1.0m, 0.7m, 0.5m, 0.5m, 0.5m, 0.5m, 0.7m, 1.0m, 1.4m, 4.0m, 8.1m };
            payoutTables[RiskLevel.Low][14] = new decimal[] { 7.1m, 4.0m, 1.4m, 1.0m, 0.7m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.7m, 1.0m, 1.4m, 4.0m, 7.1m };
            payoutTables[RiskLevel.Low][15] = new decimal[] { 15m, 4.0m, 1.4m, 1.0m, 0.7m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.7m, 1.0m, 1.4m, 4.0m, 15m };
            payoutTables[RiskLevel.Low][16] = new decimal[] { 16m, 4.0m, 1.4m, 1.0m, 0.7m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.5m, 0.7m, 1.0m, 1.4m, 4.0m, 16m };

            // Medium risk level
            payoutTables[RiskLevel.Medium][8] = new decimal[] { 13m, 3.0m, 1.2m, 0.7m, 0.4m, 0.7m, 1.2m, 3.0m, 13m };
            payoutTables[RiskLevel.Medium][9] = new decimal[] { 18m, 4.0m, 1.5m, 0.8m, 0.5m, 0.5m, 0.8m, 1.5m, 4.0m, 18m };
            payoutTables[RiskLevel.Medium][10] = new decimal[] { 22m, 6.0m, 1.5m, 0.8m, 0.5m, 0.4m, 0.5m, 0.8m, 1.5m, 6.0m, 22m };
            payoutTables[RiskLevel.Medium][11] = new decimal[] { 24m, 8.0m, 2.0m, 1.0m, 0.6m, 0.5m, 0.5m, 0.6m, 1.0m, 2.0m, 8.0m, 24m };
            payoutTables[RiskLevel.Medium][12] = new decimal[] { 33m, 11m, 4.0m, 2.0m, 0.6m, 0.4m, 0.3m, 0.4m, 0.6m, 2.0m, 4.0m, 11m, 33m };
            payoutTables[RiskLevel.Medium][13] = new decimal[] { 43m, 13m, 6.0m, 2.0m, 0.8m, 0.5m, 0.4m, 0.4m, 0.5m, 0.8m, 2.0m, 6.0m, 13m, 43m };
            payoutTables[RiskLevel.Medium][14] = new decimal[] { 58m, 16m, 5.0m, 2.5m, 1.0m, 0.6m, 0.3m, 0.2m, 0.3m, 0.6m, 1.0m, 2.5m, 5.0m, 16m, 58m };
            payoutTables[RiskLevel.Medium][15] = new decimal[] { 88m, 18m, 7.0m, 3.0m, 1.5m, 0.8m, 0.5m, 0.3m, 0.3m, 0.5m, 0.8m, 1.5m, 3.0m, 7.0m, 18m, 88m };
            payoutTables[RiskLevel.Medium][16] = new decimal[] { 110m, 41m, 10m, 4.0m, 2.0m, 1.0m, 0.5m, 0.3m, 0.3m, 0.3m, 0.5m, 1.0m, 2.0m, 4.0m, 10m, 41m, 110m };

            // High risk level
            payoutTables[RiskLevel.High][8] = new decimal[] { 29m, 5.0m, 1.0m, 0.4m, 0.2m, 0.4m, 1.0m, 5.0m, 29m };
            payoutTables[RiskLevel.High][9] = new decimal[] { 43m, 8.0m, 2.0m, 0.4m, 0.2m, 0.2m, 0.4m, 2.0m, 8.0m, 43m };
            payoutTables[RiskLevel.High][10] = new decimal[] { 76m, 10m, 3.0m, 0.8m, 0.3m, 0.2m, 0.3m, 0.8m, 3.0m, 10m, 76m };
            payoutTables[RiskLevel.High][11] = new decimal[] { 120m, 14m, 5.0m, 2.0m, 0.4m, 0.2m, 0.2m, 0.4m, 2.0m, 5.0m, 14m, 120m };
            payoutTables[RiskLevel.High][12] = new decimal[] { 170m, 24m, 8.0m, 2.0m, 0.7m, 0.2m, 0.2m, 0.2m, 0.7m, 2.0m, 8.0m, 24m, 170m };
            payoutTables[RiskLevel.High][13] = new decimal[] { 260m, 41m, 10m, 3.0m, 1.0m, 0.5m, 0.2m, 0.2m, 0.5m, 1.0m, 3.0m, 10m, 41m, 260m };
            payoutTables[RiskLevel.High][14] = new decimal[] { 420m, 56m, 15m, 4.0m, 1.5m, 0.6m, 0.3m, 0.2m, 0.3m, 0.6m, 1.5m, 4.0m, 15m, 56m, 420m };
            payoutTables[RiskLevel.High][15] = new decimal[] { 620m, 100m, 20m, 5.0m, 2.0m, 0.7m, 0.4m, 0.2m, 0.2m, 0.4m, 0.7m, 2.0m, 5.0m, 20m, 100m, 620m };
            payoutTables[RiskLevel.High][16] = new decimal[] { 1000m, 120m, 40m, 10m, 3.0m, 1.0m, 0.5m, 0.2m, 0.2m, 0.2m, 0.5m, 1.0m, 3.0m, 10m, 40m, 120m, 1000m };
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            // Calculate the game board size
            int pinsInLastRow = rows + 1;
            int boardWidth = pinsInLastRow * PinSpacingX;
            int boardHeight = rows * PinSpacingY + BucketHeight;
            int boardX = (gamePanel.Width - boardWidth) / 2;
            int topY = StartY;

            // Calculate the center of the game board (for ball drop)
            int centerX = boardX + boardWidth / 2;

            // Draw background
            using (Brush bgBrush = new SolidBrush(Color.FromArgb(20, 20, 30)))
            {
                g.FillRectangle(bgBrush, boardX - 20, topY - 20, boardWidth + 40, boardHeight + 40);
            }

            // Draw pins
            using (Brush pinBrush = new SolidBrush(Color.LightGray))
            {
                for (int row = 0; row < rows; row++)
                {
                    int pinsInThisRow = row + 1;
                    int rowStartX = centerX - (pinsInThisRow * PinSpacingX) / 2 + PinSpacingX / 2;

                    for (int pin = 0; pin < pinsInThisRow; pin++)
                    {
                        int pinX = rowStartX + pin * PinSpacingX;
                        int pinY = topY + row * PinSpacingY;
                        g.FillEllipse(pinBrush, pinX - PinRadius, pinY - PinRadius, PinRadius * 2, PinRadius * 2);
                    }
                }
            }

            // Draw buckets
            decimal[] payouts = payoutTables[riskLevel][rows];
            for (int i = 0; i < pinsInLastRow; i++)
            {
                int bucketX = boardX + i * PinSpacingX;
                int bucketY = topY + rows * PinSpacingY;

                Color bucketColor;
                if (payouts[i] < 1.0m)
                {
                    bucketColor = Color.FromArgb(220, 50, 50); // Red for loss
                }
                else if (payouts[i] < 2.0m)
                {
                    bucketColor = Color.FromArgb(220, 190, 50); // Yellow for small win
                }
                else
                {
                    bucketColor = Color.FromArgb(50, 220, 50); // Green for big win
                }

                using (Brush bucketBrush = new SolidBrush(bucketColor))
                {
                    g.FillRectangle(bucketBrush, bucketX - BucketWidth / 2, bucketY, BucketWidth, BucketHeight);
                }

                // Display payout value
                using (Font font = new Font("Arial", 8))
                using (Brush textBrush = new SolidBrush(Color.White))
                using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center })
                {
                    g.DrawString(payouts[i].ToString("0.#"), font, textBrush,
                        bucketX, bucketY + BucketHeight / 2 - 6, format);
                }
            }

            // Draw active balls
            using (Brush ballBrush = new SolidBrush(Color.Gold))
            {
                foreach (var ball in activeBalls)
                {
                    g.FillEllipse(ballBrush, ball.X - BallRadius, ball.Y - BallRadius, BallRadius * 2, BallRadius * 2);
                }
            }
        }

        private void GamePanel_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void DropBallButton_Click(object sender, EventArgs e)
        {
            if (betAmount > GameManager.Instance.Balance)
            {
                MessageBox.Show("Not enough balance for this bet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GameManager.Instance.UpdateBalance(-betAmount); // Deduct bet from GameManager balance
            UpdateBalanceDisplay(); // Update UI

            // Calculate the game board size and center
            int pinsInLastRow = rows + 1;
            int boardWidth = pinsInLastRow * PinSpacingX;
            int boardX = (gamePanel.Width - boardWidth) / 2;
            int centerX = boardX + boardWidth / 2;

            if (instantPlayCheckBox.Checked)
            {
                // Instant play without animation
                decimal payout = SimulatePlinkoResult();
                GameManager.Instance.UpdateBalance(payout); // Update GameManager balance
                UpdateBalanceDisplay(); // Update UI
            }
            else
            {
                // Animated play
                DropBallWithAnimation(centerX);
            }
        }

        private void AutoPlayButton_Click(object sender, EventArgs e)
        {
            if (autoPlayEnabled)
            {
                // Stop auto-play
                autoPlayEnabled = false;
                autoPlayRemaining = 0;
                autoPlayButton.Text = "Auto-Play (10x)";
            }
            else
            {
                // Start auto-play
                autoPlayCount = 10;
                autoPlayRemaining = autoPlayCount;
                autoPlayEnabled = true;
                autoPlayButton.Text = "Stop Auto-Play";

                // Drop the first ball
                if (activeBalls.Count == 0 && autoPlayRemaining > 0)
                {
                    DropBallButton_Click(null, null);
                }
            }
        }

        private void DropBallWithAnimation(int x)
        {
            Ball ball = new Ball
            {
                X = x,
                Y = StartY - BallRadius * 2,
                VelocityY = 1.0f
            };

            activeBalls.Add(ball);
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            bool needsRedraw = false;

            // Update all active balls
            for (int i = activeBalls.Count - 1; i >= 0; i--)
            {
                Ball ball = activeBalls[i];

                // Update physics
                ball.VelocityY += 0.2f; // Gravity
                ball.Y += ball.VelocityY;
                ball.X += ball.VelocityX;

                // Check for collisions with pins
                CheckPinCollisions(ball);

                // Check if the ball has reached a bucket
                if (ball.Y >= StartY + rows * PinSpacingY + BallRadius)
                {
                    if (ball.TargetBucket == -1)
                    {
                        // Determine which bucket the ball has fallen into
                        ball.TargetBucket = GetBucketForBall(ball);

                        // Calculate payout
                        decimal[] payouts = payoutTables[riskLevel][rows];
                        ball.Payout = payouts[ball.TargetBucket] * betAmount;

                        // Display winnings
                        if (ball.Payout > betAmount)
                        {
                            resultLabel.Text = $"Win: {ball.Payout:F2}!";
                            resultLabel.ForeColor = Color.LightGreen;
                        }
                        else if (ball.Payout == betAmount)
                        {
                            resultLabel.Text = "Tie!";
                            resultLabel.ForeColor = Color.Yellow;
                        }
                        else
                        {
                            resultLabel.Text = $"Loss: {ball.Payout - betAmount:F2}";
                            resultLabel.ForeColor = Color.Red;
                        }

                        // Update balance
                        UpdateBalanceWithPayout(ball.Payout);
                    }

                    // Stop the ball if it has fallen enough into the bucket
                    if (ball.Y >= StartY + rows * PinSpacingY + BucketHeight / 2)
                    {
                        ball.Active = false;
                        activeBalls.RemoveAt(i);

                        // Continue auto-play if enabled
                        if (autoPlayEnabled && autoPlayRemaining > 0)
                        {
                            autoPlayRemaining--;

                            if (autoPlayRemaining == 0)
                            {
                                autoPlayEnabled = false;
                                autoPlayButton.Text = "Auto-Play (10x)";
                            }
                            else if (activeBalls.Count == 0) // Wait until all balls are processed
                            {
                                DropBallButton_Click(null, null);
                            }
                        }
                    }
                }

                needsRedraw = true;
            }

            if (needsRedraw)
            {
                gamePanel.Invalidate();
            }
        }

        private void CheckPinCollisions(Ball ball)
        {
            // Calculate the game board size
            int pinsInLastRow = rows + 1;
            int boardWidth = pinsInLastRow * PinSpacingX;
            int boardX = (gamePanel.Width - boardWidth) / 2;
            int centerX = boardX + boardWidth / 2;

            // Check for collisions with pins
            for (int row = 0; row < rows; row++)
            {
                int pinsInThisRow = row + 1;
                int rowStartX = centerX - (pinsInThisRow * PinSpacingX) / 2 + PinSpacingX / 2;

                for (int pin = 0; pin < pinsInThisRow; pin++)
                {
                    int pinX = rowStartX + pin * PinSpacingX;
                    int pinY = StartY + row * PinSpacingY;

                    // Calculate distance to the pin
                    float dx = ball.X - pinX;
                    float dy = ball.Y - pinY;
                    float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                    // Collision detection
                    if (distance < PinRadius + BallRadius)
                    {
                        // Collision response
                        float overlap = (PinRadius + BallRadius) - distance;

                        // Normalized collision direction
                        float nx = dx / distance;
                        float ny = dy / distance;

                        // Push the ball back to avoid overlap
                        ball.X += nx * overlap * 0.5f;
                        ball.Y += ny * overlap * 0.5f;

                        // Reflect velocity
                        float dotProduct = ball.VelocityX * nx + ball.VelocityY * ny;

                        // Bounce velocity
                        ball.VelocityX = ball.VelocityX - 2 * dotProduct * nx;
                        ball.VelocityY = ball.VelocityY - 2 * dotProduct * ny;

                        // Dampen the bounce
                        ball.VelocityX *= 0.8f;
                        ball.VelocityY *= 0.8f;

                        // Add some randomness (to create more variance)
                        ball.VelocityX += (float)(random.NextDouble() - 0.5) * 0.4f;
                    }
                }
            }

            // Check for collisions with the edges of the game board
            int leftBoundary = boardX;
            int rightBoundary = boardX + boardWidth;

            if (ball.X < leftBoundary + BallRadius)
            {
                ball.X = leftBoundary + BallRadius;
                ball.VelocityX = -ball.VelocityX * 0.8f;
            }
            else if (ball.X > rightBoundary - BallRadius)
            {
                ball.X = rightBoundary - BallRadius;
                ball.VelocityX = -ball.VelocityX * 0.8f;
            }
        }

        private int GetBucketForBall(Ball ball)
        {
            // Calculate the game board size
            int pinsInLastRow = rows + 1;
            int boardWidth = pinsInLastRow * PinSpacingX;
            int boardX = (gamePanel.Width - boardWidth) / 2;

            // Determine which bucket the ball lands in
            int bucketIndex = (int)Math.Floor((ball.X - boardX + BucketWidth / 2) / PinSpacingX);

            // Ensure the index is within a valid range
            bucketIndex = Math.Max(0, Math.Min(pinsInLastRow - 1, bucketIndex));

            return bucketIndex;
        }

        private decimal SimulatePlinkoResult()
        {
            // This method simulates the outcome of a Plinko game without animation
            // Based on probabilities

            // Simulate a binomial distribution
            int steps = rows;
            int position = 0;

            for (int i = 0; i < steps; i++)
            {
                if (random.NextDouble() < 0.5)
                    position++; // Move right
            }

            // Display result
            decimal[] payouts = payoutTables[riskLevel][rows];
            decimal payout = payouts[position] * betAmount;

            // Display winnings
            if (payout > betAmount)
            {
                resultLabel.Text = $"Win: {payout:F2}!";
                resultLabel.ForeColor = Color.LightGreen;
            }
            else if (payout == betAmount)
            {
                resultLabel.Text = "Tie!";
                resultLabel.ForeColor = Color.Yellow;
            }
            else
            {
                resultLabel.Text = $"Loss: {payout - betAmount:F2}";
                resultLabel.ForeColor = Color.Red;
            }

            return payout;
        }

        private void UpdateBalanceWithPayout(decimal payout)
        {
            GameManager.Instance.UpdateBalance(payout);
            UpdateBalanceDisplay();
        }

        private void UpdateBalanceDisplay()
        {
            balanceLabel.Text = $"Balance: {GameManager.Instance.Balance:F2}";
            if (GameManager.Instance.Balance < betAmount)
            {
                dropBallButton.Enabled = false;
                autoPlayButton.Enabled = false;
            }
            else
            {
                dropBallButton.Enabled = true;
                autoPlayButton.Enabled = true;
            }
        }

        private void SetupPlinkoGame()
        {
            // Form settings
            this.Text = "Plinko";
            this.Size = new Size(800, 700);
            this.BackColor = Color.FromArgb(25, 25, 35);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);

            // Initialize payout tables
            InitializePayoutTables();

            // Game panel
            gamePanel = new DoubleBufferedPanel();
            gamePanel.Dock = DockStyle.Fill;
            gamePanel.Paint += GamePanel_Paint;
            gamePanel.MouseClick += GamePanel_MouseClick;
            this.Controls.Add(gamePanel);

            // Control area
            Panel controlPanel = new Panel();
            controlPanel.Dock = DockStyle.Top;
            controlPanel.Height = 100;
            controlPanel.BackColor = Color.FromArgb(35, 35, 45);
            controlPanel.Padding = new Padding(10);
            this.Controls.Add(controlPanel);

            // Balance display
            balanceLabel = new Label();
            balanceLabel.Text = $"Balance: {balance:F2}";
            balanceLabel.AutoSize = true;
            balanceLabel.Location = new Point(10, 15);
            balanceLabel.ForeColor = Color.LightGreen;
            controlPanel.Controls.Add(balanceLabel);

            // Bet setting
            Label betLabel = new Label();
            betLabel.Text = "Bet:";
            betLabel.AutoSize = true;
            betLabel.Location = new Point(10, 45);
            controlPanel.Controls.Add(betLabel);

            betAmountInput = new NumericUpDown();
            betAmountInput.Minimum = 0.1m;
            betAmountInput.Maximum = 100m;
            betAmountInput.DecimalPlaces = 2;
            betAmountInput.Increment = 0.1m;
            betAmountInput.Value = betAmount;
            betAmountInput.Location = new Point(80, 43);
            betAmountInput.Width = 80;
            betAmountInput.ValueChanged += (s, e) => betAmount = betAmountInput.Value;
            controlPanel.Controls.Add(betAmountInput);

            // Rows selection
            Label rowsLabel = new Label();
            rowsLabel.Text = "Rows:";
            rowsLabel.AutoSize = true;
            rowsLabel.Location = new Point(200, 15);
            controlPanel.Controls.Add(rowsLabel);

            rowsComboBox = new ComboBox();
            for (int i = 8; i <= 16; i++)
            {
                rowsComboBox.Items.Add(i);
            }
            rowsComboBox.SelectedIndex = 0; // Default: 8 rows
            rowsComboBox.Location = new Point(270, 12);
            rowsComboBox.Width = 80;
            rowsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            rowsComboBox.SelectedIndexChanged += (s, e) =>
            {
                rows = (int)rowsComboBox.SelectedItem;
                gamePanel.Invalidate();
            };
            controlPanel.Controls.Add(rowsComboBox);

            // Risk selection
            Label riskLabel = new Label();
            riskLabel.Text = "Risk:";
            riskLabel.AutoSize = true;
            riskLabel.Location = new Point(200, 45);
            controlPanel.Controls.Add(riskLabel);

            riskComboBox = new ComboBox();
            riskComboBox.Items.Add("Low");
            riskComboBox.Items.Add("Medium");
            riskComboBox.Items.Add("High");
            riskComboBox.SelectedIndex = 1; // Default: Medium risk
            riskComboBox.Location = new Point(270, 42);
            riskComboBox.Width = 80;
            riskComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            riskComboBox.SelectedIndexChanged += (s, e) =>
            {
                switch (riskComboBox.SelectedIndex)
                {
                    case 0: riskLevel = RiskLevel.Low; break;
                    case 1: riskLevel = RiskLevel.Medium; break;
                    case 2: riskLevel = RiskLevel.High; break;
                }
                gamePanel.Invalidate();
            };
            controlPanel.Controls.Add(riskComboBox);

            // "Drop Ball" button
            dropBallButton = new Button();
            dropBallButton.Text = "Drop Ball";
            dropBallButton.Size = new Size(150, 30);
            dropBallButton.Location = new Point(400, 15);
            dropBallButton.BackColor = Color.FromArgb(70, 130, 180);
            dropBallButton.FlatStyle = FlatStyle.Flat;
            dropBallButton.FlatAppearance.BorderSize = 0;
            dropBallButton.Click += DropBallButton_Click;
            controlPanel.Controls.Add(dropBallButton);

            // Auto-play button
            autoPlayButton = new Button();
            autoPlayButton.Text = "Auto-Play (10x)";
            autoPlayButton.Size = new Size(150, 30);
            autoPlayButton.Location = new Point(400, 55);
            autoPlayButton.BackColor = Color.FromArgb(70, 130, 180);
            autoPlayButton.FlatStyle = FlatStyle.Flat;
            autoPlayButton.FlatAppearance.BorderSize = 0;
            autoPlayButton.Click += AutoPlayButton_Click;
            controlPanel.Controls.Add(autoPlayButton);

            // Instant play checkbox
            instantPlayCheckBox = new CheckBox();
            instantPlayCheckBox.Text = "Instant Play";
            instantPlayCheckBox.AutoSize = true;
            instantPlayCheckBox.Location = new Point(600, 20);
            instantPlayCheckBox.ForeColor = Color.White;
            controlPanel.Controls.Add(instantPlayCheckBox);

            // Result label
            resultLabel = new Label();
            resultLabel.Text = "";
            resultLabel.AutoSize = true;
            resultLabel.Location = new Point(600, 55);
            resultLabel.ForeColor = Color.White;
            controlPanel.Controls.Add(resultLabel);

            // Set up animation timer
            animationTimer.Interval = 16; // About 60 FPS
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();

            // Hotkeys
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Space)
                {
                    DropBallButton_Click(null, null);
                }
            };
        }
    }
}