using Lottery20;
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
    public partial class Pay : Form
    {
        private Player currentPlayer;
        private double price;
        private Label balanceLabel;
        private Label jackpotlabel;
        private FiveLottery fiveLottery;

        public Pay(Player currentPlayer, double price, Label balanceLabel, Label jackpotlabel, FiveLottery fiveLottery)
        {
            InitializeComponent();
            this.currentPlayer = currentPlayer;
            this.price = price;
            labelPayAmount.Text = $"{price.ToString()} sek";
            this.balanceLabel = balanceLabel;
            this.jackpotlabel = jackpotlabel;
            this.fiveLottery = fiveLottery;
        }
        private void buttonPay_Click(object sender, EventArgs e)
        {
            currentPlayer.PayTicket(price);
            MessageBox.Show("Betalningen genomförd", "You rich son of a bitch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            balanceLabel.Text = $"{currentPlayer.Balance.ToString()} sek";
            jackpotlabel.Text = $"{fiveLottery.Jackpot.ToString()} sek";
            this.Close();
        }
    }
}
