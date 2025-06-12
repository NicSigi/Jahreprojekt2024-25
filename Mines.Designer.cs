namespace Jahresprojekt
{
    partial class Mines
    {
        private System.ComponentModel.IContainer components = null;

        // Alle UI-Elemente deklarieren
        private Label lblTitle;
        private Panel pnlGameBoard;
        private Label lblGuthaben;
        private Label lblWin; // HINZUGEFÜGT: lblWin deklariert
        private NumericUpDown numMines;
        private NumericUpDown numWager;
        private Button btnStart;
        private Button btnCashout;
        private Label lblMinesTitle;
        private Label lblWagerTitle;
        private Button btnRestart;

        // UI Elemente
        private Panel gamePanel;
        private Label lblBalance;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            lblTitle = new Label();
            pnlGameBoard = new Panel();
            lblGuthaben = new Label();
            lblWin = new Label();
            numMines = new NumericUpDown();
            numWager = new NumericUpDown();
            btnStart = new Button();
            btnCashout = new Button();
            lblMinesTitle = new Label();
            lblWagerTitle = new Label();
            btnRestart = new Button();
            lblStatus = new Label();

            SuspendLayout();

            // Main Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 34, 37);
            ClientSize = new Size(600, 780);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "💣 Mines - Glücksspiel";

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Gold;
            lblTitle.Location = new Point(150, 15);
            lblTitle.Text = "💣 Mines Spiel!";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // pnlGameBoard
            pnlGameBoard.BackColor = Color.FromArgb(25, 27, 30); // dunkler gemacht
            pnlGameBoard.BorderStyle = BorderStyle.FixedSingle;
            pnlGameBoard.Location = new Point(50, 70);
            pnlGameBoard.Size = new Size(500, 500);

            // lblGuthaben
            lblGuthaben.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblGuthaben.ForeColor = Color.LightGreen;
            lblGuthaben.Location = new Point(50, 580);
            lblGuthaben.Size = new Size(200, 25);
            lblGuthaben.Text = "Guthaben: 0 €";

            // lblWin
            lblWin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblWin.ForeColor = Color.Orange;
            lblWin.Location = new Point(50, 610);
            lblWin.Size = new Size(200, 25);
            lblWin.Text = "Gewinn: 0 €";

            // lblMinesTitle
            lblMinesTitle.ForeColor = Color.White;
            lblMinesTitle.Location = new Point(270, 580);
            lblMinesTitle.Size = new Size(90, 25);
            lblMinesTitle.Text = "Minen:";

            // numMines
            numMines.Location = new Point(370, 580);
            numMines.Size = new Size(60, 25);
            numMines.Minimum = 1;
            numMines.Maximum = 10;
            numMines.Value = 3;
            numMines.BackColor = Color.FromArgb(50, 52, 55);
            numMines.ForeColor = Color.White;

            // lblWagerTitle
            lblWagerTitle.ForeColor = Color.White;
            lblWagerTitle.Location = new Point(270, 610);
            lblWagerTitle.Size = new Size(90, 25);
            lblWagerTitle.Text = "Einsatz (€):";

            // numWager
            numWager.Location = new Point(370, 610);
            numWager.Size = new Size(60, 25);
            numWager.Minimum = 1;
            numWager.Maximum = 1000;
            numWager.DecimalPlaces = 2;
            numWager.Value = 10;
            numWager.BackColor = Color.FromArgb(50, 52, 55);
            numWager.ForeColor = Color.White;

            // btnStart
            btnStart.Text = "▶ Spiel starten";
            btnStart.Location = new Point(450, 580);
            btnStart.Size = new Size(100, 30);
            btnStart.BackColor = Color.FromArgb(114, 137, 218);
            btnStart.ForeColor = Color.White;
            btnStart.FlatStyle = FlatStyle.Flat;

            // btnCashout
            btnCashout.Text = "💰 Cashout";
            btnCashout.Location = new Point(450, 610);
            btnCashout.Size = new Size(100, 30);
            btnCashout.BackColor = Color.Goldenrod;
            btnCashout.ForeColor = Color.White;
            btnCashout.FlatStyle = FlatStyle.Flat;
            btnCashout.Enabled = false;

            // btnRestart
            btnRestart.BackColor = Color.FromArgb(64, 194, 133);
            btnRestart.FlatStyle = FlatStyle.Flat;
            btnRestart.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnRestart.ForeColor = Color.White;
            btnRestart.Location = new Point(230, 690); // vorher: 660
            btnRestart.Size = new Size(140, 40);
            btnRestart.Text = "🔄 Neustart";
            btnRestart.UseVisualStyleBackColor = false;

            // lblStatus – nach unten verschoben
            lblStatus.Location = new Point(140, 735); // vorher: 710 oder 715
            lblStatus.Size = new Size(320, 25);
            lblStatus.ForeColor = Color.LightGray;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            lblStatus.Text = "Bereit für ein neues Spiel!";

            // Controls hinzufügen
            Controls.Add(lblTitle);
            Controls.Add(pnlGameBoard);
            Controls.Add(lblGuthaben);
            Controls.Add(lblWin);
            Controls.Add(lblMinesTitle);
            Controls.Add(numMines);
            Controls.Add(lblWagerTitle);
            Controls.Add(numWager);
            Controls.Add(btnStart);
            Controls.Add(btnCashout);
            Controls.Add(btnRestart);
            Controls.Add(lblStatus);

            ResumeLayout(false);
        }


    }
}
