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
    public partial class Dice : Form
    {
        // Random number generator for dice rolls
        private Random rng = new Random();

        // Game state variables
        private decimal balance = 1000.00m;     // Starting balance
        private decimal betAmount = 1.00m;      // Default bet
        private int targetNumber = 50;          // Target to roll over/under
        private bool isRollOver = true;         // True = Roll Over, False = Roll Under
        private decimal multiplier = 2.0m;      // Payout multiplier based on win chance

        // Auto Play configuration
        private bool autoPlayActive = false;    // Is auto play running
        private int autoPlayCount = 0;          // Current auto play iteration
        private int maxAutoPlays = 0;           // Maximum number of auto plays
        private decimal onWinAdjustment = 0;    // % increase/decrease after win
        private decimal onLossAdjustment = 0;   // % increase/decrease after loss
        private decimal stopOnWin = 0;          // Auto stop when reaching profit
        private decimal stopOnLoss = 0;         // Auto stop when reaching loss
        private decimal sessionProfit = 0;      // Current session profit/loss
        private decimal originalBet = 0;        // Starting bet amount before auto play
        private System.Windows.Forms.Timer autoPlayTimer = new System.Windows.Forms.Timer(); // Timer for auto play intervals

        // Game result history
        private List<GameResult> gameHistory = new List<GameResult>();
        private int maxHistoryItems = 10;       // Limit history list

        public Dice()
        {
            InitializeComponent();
            SetupControls(); // Initialize all default values and event handlers
        }

        // Triggered when bet input changes
        private void BetTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Try parsing input to decimal. If valid, store it. If not, reset to previous valid value
            if (decimal.TryParse(textBox.Text, out decimal result))
            {
                betAmount = Math.Max(0.01m, result);
            }
            else
            {
                textBox.Text = betAmount.ToString("0.00");
            }
        }

        // Half bet amount (minimum 0.01)
        private void HalfButton_Click(object sender, EventArgs e)
        {
            betAmount = Math.Max(0.01m, betAmount / 2);
            TextBox betTextBox = (TextBox)Controls.Find("txtBetAmount", true)[0];
            betTextBox.Text = betAmount.ToString("0.00");
        }

        // Double bet amount (maximum = current balance)
        private void DoubleButton_Click(object sender, EventArgs e)
        {
            betAmount = Math.Min(balance, betAmount * 2);
            TextBox betTextBox = (TextBox)Controls.Find("txtBetAmount", true)[0];
            betTextBox.Text = betAmount.ToString("0.00");
        }

        // Triggered when user changes roll mode (Over/Under)
        private void RollModeRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if (radioButton.Checked)
            {
                isRollOver = radioButton.Name == "radRollOver";
                UpdateTargetAndMultiplier(); // Update labels and payout accordingly
            }
        }

        // Triggered when user changes slider
        private void TargetTrackBar_ValueChanged(object sender, EventArgs e)
        {
            targetNumber = ((TrackBar)sender).Value;
            UpdateTargetAndMultiplier();
        }

        // Update win chance and multiplier when target number or mode changes
        private void UpdateTargetAndMultiplier()
        {
            Label targetLabel = (Label)Controls.Find("lblTargetNumber", true)[0];
            targetLabel.Text = targetNumber.ToString();

            // Calculate win chance based on selected mode
            decimal winChance = isRollOver ? 100 - targetNumber : targetNumber;

            Label winChanceLabel = (Label)Controls.Find("lblWinChance", true)[0];
            winChanceLabel.Text = winChance.ToString("0.00") + "%";

            // Basic multiplier formula considering 1% house edge
            multiplier = 99m / winChance;

            Label multiplierLabel = (Label)Controls.Find("lblMultiplier", true)[0];
            multiplierLabel.Text = multiplier.ToString("0.0000") + "×";
        }

        // Manual roll
        private void RollButton_Click(object sender, EventArgs e)
        {
            RollDice();
        }

        // Executes the game logic for rolling once
        private bool RollDice()
        {
            // Validate bet
            if (betAmount <= 0 || betAmount > balance)
            {
                MessageBox.Show("Invalid bet amount!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            balance -= betAmount; // Deduct bet

            int roll = rng.Next(1, 101); // Simulate dice roll (1-100)

            // Determine win condition based on roll mode
            bool isWin = (isRollOver && roll > targetNumber) || (!isRollOver && roll < targetNumber);

            decimal winAmount = 0;

            if (isWin)
            {
                winAmount = betAmount * multiplier;
                balance += winAmount;
                sessionProfit += winAmount - betAmount;
            }
            else
            {
                sessionProfit -= betAmount;
            }

            UpdateGameResult(roll, isWin, winAmount); // Update UI
            AddToHistory(new GameResult // Save result
            {
                Roll = roll,
                Target = targetNumber,
                IsRollOver = isRollOver,
                BetAmount = betAmount,
                WinAmount = isWin ? winAmount : 0,
                IsWin = isWin
            });

            return isWin;
        }

        // Update UI after a roll
        private void UpdateGameResult(int roll, bool isWin, decimal winAmount)
        {
            Panel resultPanel = (Panel)Controls.Find("pnlResult", true)[0];
            resultPanel.Visible = true;

            Label resultLabel = (Label)Controls.Find("lblResult", true)[0];
            resultLabel.Text = roll.ToString();
            resultLabel.ForeColor = isWin ? Color.LightGreen : Color.Tomato;

            Label resultInfoLabel = (Label)Controls.Find("lblResultInfo", true)[0];
            resultInfoLabel.Text = isWin
                ? $"You won {winAmount:C2}!"
                : $"You lost {betAmount:C2}";
            resultInfoLabel.ForeColor = isWin ? Color.LightGreen : Color.Tomato;

            // Update balance and profit labels
            Label balanceLabel = (Label)Controls.Find("lblBalance", true)[0];
            balanceLabel.Text = balance.ToString("C2");

            Label profitLabel = (Label)Controls.Find("lblProfit", true)[0];
            profitLabel.Text = sessionProfit.ToString("C2");
            profitLabel.ForeColor = sessionProfit >= 0 ? Color.LightGreen : Color.Tomato;
        }

        // Save a round result to history and refresh display
        private void AddToHistory(GameResult result)
        {
            gameHistory.Insert(0, result); // Add to front

            if (gameHistory.Count > maxHistoryItems)
            {
                gameHistory.RemoveAt(gameHistory.Count - 1); // Keep size limited
            }

            ListBox historyListBox = (ListBox)Controls.Find("lstHistory", true)[0];
            historyListBox.Items.Clear();

            foreach (GameResult item in gameHistory)
            {
                string mode = item.IsRollOver ? ">" : "<";
                string outcome = item.IsWin ? "WIN" : "LOSS";

                historyListBox.Items.Add(
                    $"Roll: {item.Roll,3} {mode} {item.Target,2} | " +
                    $"Bet: {item.BetAmount,8:F2} | {outcome} {(item.IsWin ? item.WinAmount : 0),8:F2}"
                );
            }
        }

        // Start or stop auto play
        private void AutoPlayButton_Click(object sender, EventArgs e)
        {
            Button autoPlayButton = (Button)sender;

            if (!autoPlayActive)
            {
                // Read all user settings
                NumericUpDown autoPlaysNumeric = (NumericUpDown)Controls.Find("numAutoPlays", true)[0];
                NumericUpDown onWinNumeric = (NumericUpDown)Controls.Find("numOnWin", true)[0];
                NumericUpDown onLossNumeric = (NumericUpDown)Controls.Find("numOnLoss", true)[0];
                TextBox stopWinTextBox = (TextBox)Controls.Find("txtStopWin", true)[0];
                TextBox stopLossTextBox = (TextBox)Controls.Find("txtStopLoss", true)[0];

                // Validate stop inputs
                if (!decimal.TryParse(stopWinTextBox.Text, out stopOnWin) ||
                    !decimal.TryParse(stopLossTextBox.Text, out stopOnLoss))
                {
                    MessageBox.Show("Invalid stop amounts!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Setup auto play settings
                maxAutoPlays = (int)autoPlaysNumeric.Value;
                onWinAdjustment = (decimal)onWinNumeric.Value / 100m;
                onLossAdjustment = (decimal)onLossNumeric.Value / 100m;
                originalBet = betAmount;
                autoPlayCount = 0;

                autoPlayActive = true;
                autoPlayButton.Text = "STOP AUTO PLAY";
                autoPlayTimer.Start();

                // Disable manual control
                Button rollButton = (Button)Controls.Find("btnRoll", true)[0];
                rollButton.Enabled = false;
            }
            else
            {
                StopAutoPlay();
            }
        }

        // Called every tick of the timer
        private void AutoPlayTimer_Tick(object sender, EventArgs e)
        {
            // Stop if limits are hit
            if (!autoPlayActive || autoPlayCount >= maxAutoPlays || balance < betAmount ||
                (stopOnWin > 0 && sessionProfit >= stopOnWin) ||
                (stopOnLoss > 0 && sessionProfit <= -stopOnLoss))
            {
                StopAutoPlay();
                return;
            }

            // Perform roll
            bool isWin = RollDice();
            autoPlayCount++;

            // Adjust bet for next round
            if (isWin)
                AdjustBet(onWinAdjustment);
            else
                AdjustBet(onLossAdjustment);

            // Reflect bet amount in textbox
            TextBox betTextBox = (TextBox)Controls.Find("txtBetAmount", true)[0];
            betTextBox.Text = betAmount.ToString("0.00");

            // Update button label with progress
            Button autoPlayButton = (Button)Controls.Find("btnAutoPlay", true)[0];
            autoPlayButton.Text = $"STOP AUTO PLAY ({autoPlayCount}/{maxAutoPlays})";
        }

        // Changes bet size after a win/loss
        private void AdjustBet(decimal adjustment)
        {
            if (adjustment > 0)
                betAmount = Math.Min(balance, betAmount * (1 + adjustment));
            else
                betAmount = Math.Max(0.01m, betAmount * (1 + adjustment));
        }

        // Stop auto play and reset everything
        private void StopAutoPlay()
        {
            autoPlayTimer.Stop();
            autoPlayActive = false;

            betAmount = originalBet;

            TextBox betTextBox = (TextBox)Controls.Find("txtBetAmount", true)[0];
            betTextBox.Text = betAmount.ToString("0.00");

            Button autoPlayButton = (Button)Controls.Find("btnAutoPlay", true)[0];
            autoPlayButton.Text = "START AUTO PLAY";

            Button rollButton = (Button)Controls.Find("btnRoll", true)[0];
            rollButton.Enabled = true;
        }

        private void Dice_Load(object sender, EventArgs e)
        {

        }
    }

    // Represents a result from one roll
    public class GameResult
    {
        public int Roll { get; set; }
        public int Target { get; set; }
        public bool IsRollOver { get; set; }
        public decimal BetAmount { get; set; }
        public decimal WinAmount { get; set; }
        public bool IsWin { get; set; }
    }
}
