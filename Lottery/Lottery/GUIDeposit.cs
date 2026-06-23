using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery20
{
    public partial class Deposit : Form
    {
        private Player currentPlayer;
        private Label label;
        public Deposit(Player currentPlayer, Label label)
        {
            InitializeComponent();
            this.currentPlayer = currentPlayer;
            this.label = label;
        }

        private void buttonMakeDeposit_Click(object sender, EventArgs e)
        {
            int depositAmount = int.Parse(textBoxDepositAmount.Text);
            currentPlayer.Balance += depositAmount;
            label.Text = (currentPlayer.Balance.ToString() + " sek"); 
            this.Close();
        }

        private void textBoxDepositAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
