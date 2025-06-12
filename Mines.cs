using System;
using System.Drawing;
using System.Windows.Forms;

namespace Jahresprojekt
{
    public partial class Mines : Form
    {
        private const int BoardSize = 5; // Size of the board (5x5)
        private int mineCount; // Number of mines on the board
        private decimal wagerAmount; // Amount the player bets
        private decimal balance = 1000m; // Starting balance
        private decimal currentWin = 0m; // Current potential winnings
        private int safeClicksCount = 0; // Number of safe tiles clicked
        private int maxSafeFields = 0; // Max number of safe tiles
        private bool gameActive = false; // Whether the game is currently running

        private Button[,] tiles; // Grid of buttons representing the board
        private bool[,] mines; // Grid of booleans for mine locations

        public Mines()
        {
            InitializeComponent();
            SetupStatusLabel(); // Initialize status label
            UpdateBalanceLabel(); // Display balance
            UpdateWinLabel(); // Display current winnings

            // Style for the game panel
            pnlGameBoard.BorderStyle = BorderStyle.FixedSingle;
            pnlGameBoard.BackColor = Color.LightGray;

            // Event handlers
            btnStart.Click += BtnStart_Click;
            btnCashout.Click += BtnCashout_Click;
            btnRestart.Click += BtnRestart_Click;

            ConfigureNumericControls(); // Setup wager/mine selectors
            InitEmptyGameBoard(); // Draw initial disabled board
        }

        private void ConfigureNumericControls()
        {
            // Configure mine selector
            numMines.Minimum = 1;
            numMines.Maximum = BoardSize * BoardSize - 1;
            numMines.Value = Math.Min(numMines.Value, numMines.Maximum);

            // Configure wager selector
            numWager.Minimum = 1;
            numWager.Maximum = 1000;
            numWager.DecimalPlaces = 2;
            numWager.Value = Math.Min(10, balance);
        }

        private void InitEmptyGameBoard()
        {
            pnlGameBoard.Controls.Clear();
            tiles = new Button[BoardSize, BoardSize];

            int tileSize = Math.Min(pnlGameBoard.Width, pnlGameBoard.Height) / BoardSize;

            // Create a grid of disabled tiles
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Button tile = new Button
                    {
                        Size = new Size(tileSize - 2, tileSize - 2),
                        Location = new Point(col * tileSize + 1, row * tileSize + 1),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        Tag = new Point(row, col),
                        Enabled = false
                    };
                    tile.FlatAppearance.BorderSize = 1;
                    pnlGameBoard.Controls.Add(tile);
                    tiles[row, col] = tile;
                }
            }
        }

        private void UpdateBalanceLabel()
        {
            // Update balance display
            lblGuthaben.Text = $"Guthaben: {balance:F2} €";
        }

        private void UpdateWinLabel()
        {
            // Update win display
            lblWin.Text = $"Gewinn: {currentWin:F2} €";
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                mineCount = (int)numMines.Value;
                wagerAmount = numWager.Value;

                // Check if player has enough balance
                if (wagerAmount > balance)
                {
                    MessageBox.Show("Not enough balance!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                balance -= wagerAmount; // Deduct bet
                UpdateBalanceLabel();

                InitGameBoard(); // Enable tiles
                PlaceMines(); // Randomly place mines

                maxSafeFields = BoardSize * BoardSize - mineCount;
                safeClicksCount = 0;
                currentWin = 0m;
                gameActive = true;
                btnCashout.Enabled = true;
                btnStart.Enabled = false;
                UpdateWinLabel();

                lblStatus.Text = "Game started! Click on a tile to reveal.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitGameBoard()
        {
            pnlGameBoard.Controls.Clear();
            tiles = new Button[BoardSize, BoardSize];

            int tileSize = Math.Min(pnlGameBoard.Width, pnlGameBoard.Height) / BoardSize;

            // Create interactive game board
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Button tile = new Button
                    {
                        Size = new Size(tileSize - 2, tileSize - 2),
                        Location = new Point(col * tileSize + 1, row * tileSize + 1),
                        BackColor = Color.LightGray,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        Tag = new Point(row, col),
                        Enabled = true
                    };
                    tile.FlatAppearance.BorderSize = 1;
                    tile.Click += Tile_Click;

                    pnlGameBoard.Controls.Add(tile);
                    tiles[row, col] = tile;
                }
            }

            pnlGameBoard.Refresh();
        }

        private void PlaceMines()
        {
            mines = new bool[BoardSize, BoardSize];
            Random rand = new Random();
            int placed = 0;

            // Randomly place mines on the board
            while (placed < mineCount)
            {
                int r = rand.Next(BoardSize);
                int c = rand.Next(BoardSize);
                if (!mines[r, c])
                {
                    mines[r, c] = true;
                    placed++;
                }
            }

            // Debug check to ensure correct number of mines
            int totalMines = 0;
            for (int r = 0; r < BoardSize; r++)
                for (int c = 0; c < BoardSize; c++)
                    if (mines[r, c]) totalMines++;

            if (totalMines != mineCount)
                throw new Exception($"Mine placement error. Expected: {mineCount}, Actual: {totalMines}");
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            if (!gameActive) return;

            Button tile = (Button)sender;
            if (!tile.Enabled) return;

            Point pos = (Point)tile.Tag;
            int row = pos.X;
            int col = pos.Y;

            // Player clicked on a mine
            if (mines[row, col])
            {
                tile.BackColor = Color.Red;
                tile.Text = "💣";
                EndGame(false);
            }
            else
            {
                // Safe tile clicked
                tile.BackColor = Color.ForestGreen;
                tile.Text = "✓";
                tile.Enabled = false;
                safeClicksCount++;

                decimal multiplier = CalculateMultiplier();
                currentWin = Math.Round(wagerAmount * multiplier, 2);
                UpdateWinLabel();

                lblStatus.Text = $"Safe tile revealed! Potential win: {currentWin:F2} €";

                // All safe tiles revealed
                if (safeClicksCount == maxSafeFields)
                {
                    EndGame(true, "Maximum win! All safe tiles revealed.");
                }
            }
        }

        private decimal CalculateMultiplier()
        {
            // Multiplier formula increases exponentially per safe tile
            double baseValue = 0.55;
            double growthRate = 1.15;
            double multiplier = baseValue * Math.Pow(growthRate, safeClicksCount);

            // Higher mine count increases multiplier slightly
            if (mineCount > 5)
            {
                multiplier *= (1 + ((mineCount - 5) * 0.05));
            }

            // Cap and adjust multiplier for house edge
            multiplier = Math.Min(multiplier, 1000.0) * 0.97;
            return (decimal)Math.Round(multiplier, 2);
        }

        private void BtnCashout_Click(object sender, EventArgs e)
        {
            // Cashout current winnings
            if (gameActive && currentWin > 0)
            {
                EndGame(true, "Cashout successful!");
            }
        }

        private void EndGame(bool won, string message = "")
        {
            gameActive = false;
            btnCashout.Enabled = false;
            btnStart.Enabled = true;

            if (won)
            {
                balance += currentWin;
                UpdateBalanceLabel();
                MessageBox.Show($"{message}\nWinnings: {currentWin:F2} €", "Win", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = $"Game ended. Win: {currentWin:F2} €";
            }
            else
            {
                currentWin = 0m;
                UpdateWinLabel();
                MessageBox.Show($"Game over! You lost {wagerAmount:F2} €.", "Lost", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "Game lost! Start a new game.";
            }

            RevealMines(); // Show all mines
        }

        private void RevealMines()
        {
            // Reveal all mines and disable board
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (mines[row, col] && tiles[row, col].Text != "💣")
                    {
                        tiles[row, col].Text = "💣";
                        tiles[row, col].BackColor = Color.Red;
                    }
                    tiles[row, col].Enabled = false;
                }
            }
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            // Reset game state
            safeClicksCount = 0;
            currentWin = 0m;
            gameActive = false;
            btnCashout.Enabled = false;
            btnStart.Enabled = true;

            UpdateWinLabel();
            InitEmptyGameBoard();
            lblStatus.Text = "Ready for a new game!";
        }

        private Label lblStatus;

        private void SetupStatusLabel()
        {
            // Set properties for the status label
            if (this.lblStatus != null)
            {
                this.lblStatus.Location = new Point(140, 710);
                this.lblStatus.Size = new Size(320, 25);
                this.lblStatus.ForeColor = Color.White;
                this.lblStatus.TextAlign = ContentAlignment.MiddleCenter;
                this.lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                this.lblStatus.Text = "Ready for a new game!";
            }
        }
    }
}
