using Jahresprojekt;
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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            SQLManagement.Autoconnect(); // Automatically connect to the database
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
        private void btnPlayMines_Click(object sender, EventArgs e)
        {
            Mines minesForm = new Mines(); 
            minesForm.Show();
        }

        private void btnPlayDice_Click(object sender, EventArgs e)
        {
            Dice diceForm = new Dice(); 
            diceForm.Show();
        }

        private void btnPlayPlinko_Click(object sender, EventArgs e)
        {
            Plinko plinkoForm = new Plinko(); 
            plinkoForm.Show(); 
        }
    }
}
