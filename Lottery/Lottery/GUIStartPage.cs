using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Numerics;
using System.Diagnostics.Metrics;


namespace Lottery20
{
    public partial class StartPage : Form
    {
        FiveLottery fiveLottery;
        LotteryDrawFive currentLottery;
        Player currentPlayer;

        public StartPage()
        {
            this.fiveLottery = new FiveLottery();
            this.currentLottery = fiveLottery.lotteryDrawFive;
            fiveLottery.Jackpot = 10000000;
            InitializeComponent();
            labelCurrentJackpot.Text = $"{fiveLottery.Jackpot.ToString()} sek";
            panelStartPage.Visible = true;
            panelInputLoggedIn.Visible = false;
            panelPlayedRows.Visible = false;

        }
        #region skitsaker
        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUsername.ForeColor != Color.DarkGray && !string.IsNullOrWhiteSpace(textBoxUsername.Text))
            {
                textBoxUsername.ForeColor = Color.White;
            }
        }
        private void textBoxUsername_Enter(object sender, EventArgs e)
        {
            if (textBoxUsername.Text == "Personnummer")
            {
                textBoxUsername.Text = string.Empty;
                textBoxUsername.ForeColor = Color.White;
            }
        }
        private void textBoxUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUsername.Text))
            {
                textBoxUsername.ForeColor = Color.DarkGray;
                textBoxUsername.Text = "Personnummer";
            }
        }
        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPassword.ForeColor != Color.DarkGray && !string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                textBoxPassword.ForeColor = Color.White;
            }
        }
        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "Lösenord")
            {
                textBoxPassword.Text = string.Empty;
                textBoxPassword.ForeColor = Color.White;
                textBoxPassword.PasswordChar = '*';
            }
        }
        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                textBoxPassword.ForeColor = Color.DarkGray;
                textBoxPassword.PasswordChar = '\0';
                textBoxPassword.Text = "Lösenord";
            }
        }
        private void textBoxCheckIfInt(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            if (textBoxUsername.Text.Length != 12 || textBoxUsername.Text == "Personnummer")
            {
                MessageBox.Show("Personnumret ska innehålla 12 siffror", "Felaktigt personnummer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (textBoxPassword.Text == "Lösenord")
            {
                MessageBox.Show("Lösenordet kan inte vara tomt", "Felaktigt lösenord", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string pnr = textBoxUsername.Text;
                string inputPassword = textBoxPassword.Text;
                string savedPassword = null;

                if (fiveLottery.Players.ContainsKey(pnr))
                {
                    if (fiveLottery.Players.TryGetValue(pnr, out var player))
                    {
                        savedPassword = player.Password;
                        labelBalance.Text = $"{player.Balance.ToString()} sek";

                        //Player testplayer = fiveLottery.Players[pnr];
                        //savedPassword = testplayer.Password;
                        if (savedPassword == inputPassword)
                        {
                            currentPlayer = player;
                            currentPlayer.currentLottery = fiveLottery;
                            panelInputLoggedIn.Visible = true;
                            panelStartPage.Visible = false;
                            panelNewRow.Visible = false;
                            panelPlayedRows.Visible = false;
                            panelResults.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Felaktigt lösenord", "Fel lösenord", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Personnummer finns inte registrerat", "Inte registrerad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            panelStartPage.Visible = true;
            panelInputLoggedIn.Visible = false;
        }
        #endregion
        private void läggSpelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelNewRow.Visible = true;
            panelPlayedRows.Visible = false;
            panelResults.Visible = false;
        }
        private void minaRaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelNewRow.Visible = false;
            panelPlayedRows.Visible = true;
            panelResults.Visible = false;
            string numbers;
            int counter = 1;
            Button button;

            fiveLottery.Tickets.TryGetValue(currentPlayer.Pnr, out var tickets);
            if (tickets != null && tickets.Count() != 0)
            {
                foreach (FiveTicket ticket in tickets)
                {
                    textBoxPlayedRow1.Font = new System.Drawing.Font(textBoxPlayedRow1.Font.FontFamily, 9);
                    numbers = string.Join("    -    ", ticket.ticketNumbers);
                    TextBox textBox = this.Controls.Find($"textBoxPlayedRow{counter}", true).FirstOrDefault() as TextBox;
                    button = this.Controls.Find($"buttonRemovePlayedRow{counter}", true).FirstOrDefault() as Button;
                    textBox.Text = numbers;
                    textBox.ForeColor = Color.Black;
                    button.Text = "Prenumerering";
                    button.BackColor = Color.White;
                    button.Enabled = true;
                    if (ticket.Subscriber)
                    {
                        textBox.BackColor = Color.Green;
                    }
                    else
                    {
                        textBox.BackColor = Color.White;
                    }
                    counter++;
                }
            }
            else
            {
                textBoxPlayedRow1.Text = "Du har inga rader spelade";
                textBoxPlayedRow1.ForeColor = Color.White;
                textBoxPlayedRow1.BackColor = Color.FromArgb(20, 30, 40);
                textBoxPlayedRow1.Font = new System.Drawing.Font(textBoxPlayedRow1.Font.FontFamily, 12);
                buttonRemovePlayedRow1.Text = "";
                buttonRemovePlayedRow1.BackColor = Color.FromArgb(20, 30, 40);
                buttonRemovePlayedRow1.Enabled = false;
                for (int i = 2; i <= 17; i++)
                {
                    TextBox textBox = this.Controls.Find($"textBoxPlayedRow{i}", true).FirstOrDefault() as TextBox;
                    button = this.Controls.Find($"buttonRemovePlayedRow{i}", true).FirstOrDefault() as Button;
                    textBox.Text = "";
                    textBox.BackColor = Color.FromArgb(20, 30, 40);
                    textBox.ForeColor = Color.FromArgb(20, 30, 40);
                    button.Text = "";
                    button.BackColor = Color.FromArgb(20, 30, 40);
                    button.Enabled = false;
                }
            }
        }
        private void visaSenasteDragningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelNewRow.Visible = false;
            panelPlayedRows.Visible = false;
            panelResults.Visible = true;
        }
        private void ToggleButtonColor(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton.BackColor == Color.White)
            {
                clickedButton.BackColor = Color.Green;
            }
            else
            {
                clickedButton.BackColor = Color.White;
            }
        }
        private void buttonPirkoBoy(object sender, EventArgs e)
        {

            PirkoBoy pirkoBoyForm = new PirkoBoy();
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1250;
            timer.Tick += (s, args) =>
            {
                pirkoBoyForm.Close();
                timer.Stop();
            };

            pirkoBoyForm.Show();
            timer.Start();


            int[] drawnNumbers = currentLottery.DrawNumber(35, true);

            ComboBox[] comboBoxes = new ComboBox[5];

            for (int i = 1; i <= 5; i++)
            {
                comboBoxes[i - 1] = this.Controls.Find($"comboBox{i}Ticket", true).FirstOrDefault() as ComboBox;
            }

            for (int i = 0; i < 5; i++)
            {
                if (comboBoxes[i] != null)
                {
                    comboBoxes[i].Text = drawnNumbers[i].ToString();
                }
            }
        }
        private void buttonClearRow(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int rowNumber = int.Parse(clickedButton.Name.Replace("buttonRemoveRow", ""));
            int totalRows = 17;

            for (int row = rowNumber; row < totalRows; row++)
            {
                for (int i = 1; i <= 5; i++)
                {

                    TextBox currentTextBox = this.Controls.Find($"textBox{i}Row{row}", true).FirstOrDefault() as TextBox;
                    TextBox nextTextBox = this.Controls.Find($"textBox{i}Row{row + 1}", true).FirstOrDefault() as TextBox;

                    if (currentTextBox != null)
                    {

                        if (nextTextBox != null)
                        {
                            currentTextBox.Text = nextTextBox.Text;
                            currentTextBox.BackColor = nextTextBox.BackColor;
                        }
                        else
                        {

                            currentTextBox.Text = "";
                            currentTextBox.BackColor = Color.White;
                        }
                    }
                }
            }
        }
        private void buttonAddRow_Click(object sender, EventArgs e)
        {
            bool allComboBoxesHaveValues = true;
            for (int box = 1; box <= 5; box++)
            {
                ComboBox comboBox = this.Controls.Find("comboBox" + box + "Ticket", true).FirstOrDefault() as ComboBox;
                if (comboBox == null || string.IsNullOrEmpty(comboBox.Text))
                {
                    allComboBoxesHaveValues = false;
                    break;
                }
            }
            if (allComboBoxesHaveValues)
            {
                for (int row = 1; row <= 17; row++)
                {
                    TextBox firstTextBox = this.Controls.Find("textBox1Row" + row, true).FirstOrDefault() as TextBox;
                    if (firstTextBox != null && string.IsNullOrEmpty(firstTextBox.Text))
                    {
                        for (int box = 1; box <= 5; box++)
                        {
                            ComboBox comboBox = this.Controls.Find("comboBox" + box + "Ticket", true).FirstOrDefault() as ComboBox;
                            TextBox rowTextBox = this.Controls.Find("textBox" + box + "Row" + row, true).FirstOrDefault() as TextBox;
                            rowTextBox.Text = comboBox.Text;
                            if (buttonSubscribe.BackColor == Color.Green)
                            {
                                rowTextBox.BackColor = Color.Green;
                            }
                            comboBox.SelectedIndex = -1;
                        }
                        break;
                    }
                }
            }
        }
        private void buttonDeposit_Click(object sender, EventArgs e)
        {
            Deposit deposit = new Deposit(currentPlayer, labelBalance);
            deposit.Show();
        }
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string pnr = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            bool registered = true;

            if (textBoxUsername.Text.Length != 12 || textBoxUsername.Text == "Personnummer")
            {
                MessageBox.Show("Personnumret ska innehålla 12 siffror", "Felaktigt personnummer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (textBoxPassword.Text == "Lösenord")
            {
                MessageBox.Show("Lösenordet kan inte vara tomt", "Felaktigt lösenord", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (fiveLottery.Players != null)
                {
                    if (fiveLottery.Players.ContainsKey(pnr))
                    {
                        MessageBox.Show("Personnummer redan registrerat.", "Redan registrerad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        currentPlayer = fiveLottery.createPlayer(fiveLottery, pnr, registered, password, 0);
                        MessageBox.Show("Konto registrerat", "Registrerad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    currentPlayer = fiveLottery.createPlayer(fiveLottery, pnr, registered, password, 0);
                    MessageBox.Show("Konto registrerat", "Registrerad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void buttonPay_Click(object sender, EventArgs e)
        {
            int rowNumber = 1;
            double amountToPay = 0;
            while (true) 
            {
                TextBox firstTextBox = this.Controls.Find($"textBox1Row{rowNumber}", true).FirstOrDefault() as TextBox;

                if (firstTextBox == null || string.IsNullOrEmpty(firstTextBox.Text))
                {
                    break;
                }

                bool subscriber = true;
                int[] ticketNumbers = new int[5];

                for (int j = 1; j <= 5; j++)
                {
                    TextBox rowTextBox = this.Controls.Find($"textBox{j}Row{rowNumber}", true).FirstOrDefault() as TextBox;
                    ticketNumbers[j - 1] = int.Parse(rowTextBox.Text);

                    if (rowTextBox.BackColor == Color.White)
                    {
                        subscriber = false; 
                    }

                    rowTextBox.Text = "";
                    rowTextBox.BackColor = Color.White;
                }

                amountToPay += currentPlayer.CreateTicket(subscriber, ticketNumbers);
                rowNumber++;
            }
            if (amountToPay <= currentPlayer.Balance)
            {
                Pay pay = new Pay(currentPlayer, amountToPay, labelBalance, labelCurrentJackpot, fiveLottery);
                pay.Show();
            }
            else
            {
                MessageBox.Show("Inte tillräckligt med dollars på kontot", "Too poor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void Simulera(object sender, EventArgs e)
        {
            string name = textBoxUsername.Text;
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            string numberString = clickedItem.Text;
            string digits = numberString.Split(' ')[0];
            int number = int.Parse(digits);
            while (number > 0)
            {
                fiveLottery.DoDraw();
                number--;
            }
            UpdatePlayerResult();
            fiveLottery.RemoveNonSubTicket();
            fiveLottery.RemoveGuests();
            fiveLottery.TakeSubscriberPayment();
            labelBalance.Text = $"{currentPlayer.Balance.ToString()} sek";
            labelCurrentJackpot.Text = $"{fiveLottery.Jackpot.ToString()} sek";
        }
        private void buttonGuestLogin_Click(object sender, EventArgs e)
        {
            string pnr = textBoxUsername.Text;
            string password = null;
            bool registered = false;

            if (textBoxUsername.Text.Length != 12 || textBoxUsername.Text == "Personnummer")
            {
                MessageBox.Show("Personnumret ska innehålla 12 siffror", "Felaktigt personnummer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (fiveLottery.Players != null)
                {
                    if (fiveLottery.Players.ContainsKey(pnr))
                    {
                        MessageBox.Show("Personnummer registrerat, logga in istället.", "Redan registrerad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        currentPlayer = fiveLottery.Players[pnr];
                        panelInputLoggedIn.Visible = true;
                        panelStartPage.Visible = false;
                        panelNewRow.Visible = false;
                        panelPlayedRows.Visible = false;
                        panelResults.Visible = false;
                    }
                }
            }
        }
        private void comboBox1Ticket_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox changedComboBox = sender as ComboBox;

            List<string> selectedValues = new List<string>();

            for (int i = 1; i <= 5; i++)
            {
                ComboBox comboBox = this.Controls.Find($"comboBox{i}Ticket", true).FirstOrDefault() as ComboBox;
                if (comboBox != null && comboBox != changedComboBox && !string.IsNullOrEmpty(comboBox.Text))
                {
                    selectedValues.Add(comboBox.Text);
                }
            }

            if (selectedValues.Contains(changedComboBox.Text))
            {
                changedComboBox.SelectedIndex = -1;
            }
        }
        private void ChangeSubscriptionStatus(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string strRow = button.Name;
            int row = int.Parse(strRow.Replace("buttonRemovePlayedRow", ""));
            fiveLottery.Tickets.TryGetValue(currentPlayer.Pnr, out var tickets);
            TextBox textBox = this.Controls.Find($"textBoxPlayedRow{row}", true).FirstOrDefault() as TextBox;
            if (textBox.BackColor == Color.White)
            {
                tickets[row - 1].Subscriber = true;
                textBox.BackColor = Color.Green;
            }
            else
            {
                tickets[row - 1].Subscriber = false;
                textBox.BackColor = Color.White;
            }
        }
        private void UpdatePlayerResult()
        {
            fiveLottery.Tickets.TryGetValue(currentPlayer.Pnr, out var tickets);
            if (tickets.Count() != 0)
            {
                if(tickets[0].drawnNumbers != null)
                {
                    Array.Sort(tickets[0].drawnNumbers);
                    labelLastDraw.Text = string.Join("    -    ", tickets[0].drawnNumbers);

                    int bestRow = 0;
                    int currentRow;
                    int ticketIndex = 0;
                    int counter = 0;

                    foreach (Ticket ticket in tickets)
                    {

                        if (ticket.drawnNumbers != null)
                        {
                            currentRow = ticket.drawnNumbers.Intersect(ticket.ticketNumbers).Count();
                            if (currentRow > bestRow)
                            {
                                bestRow = currentRow;
                                ticketIndex = counter;
                            }
                        }
                        counter++;
                    }
                    Array.Sort(tickets[ticketIndex].ticketNumbers);
                    labelCorrect.Text = string.Join("    -    ", tickets[ticketIndex].ticketNumbers);
                    labelCorrectInt.Text = bestRow.ToString();
                }
                
            }
        }
    }
}
