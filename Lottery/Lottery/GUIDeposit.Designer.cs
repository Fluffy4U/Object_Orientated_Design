namespace Lottery20
{
    partial class Deposit
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
            this.textBoxDepositAmount = new System.Windows.Forms.TextBox();
            this.labelDeposit = new System.Windows.Forms.Label();
            this.buttonMakeDeposit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxDepositAmount
            // 
            this.textBoxDepositAmount.BackColor = System.Drawing.Color.White;
            this.textBoxDepositAmount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDepositAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.textBoxDepositAmount.Location = new System.Drawing.Point(118, 29);
            this.textBoxDepositAmount.Name = "textBoxDepositAmount";
            this.textBoxDepositAmount.Size = new System.Drawing.Size(226, 19);
            this.textBoxDepositAmount.TabIndex = 0;
            this.textBoxDepositAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxDepositAmount_KeyPress);
            // 
            // labelDeposit
            // 
            this.labelDeposit.AutoSize = true;
            this.labelDeposit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.labelDeposit.ForeColor = System.Drawing.Color.White;
            this.labelDeposit.Location = new System.Drawing.Point(49, 29);
            this.labelDeposit.Name = "labelDeposit";
            this.labelDeposit.Size = new System.Drawing.Size(63, 20);
            this.labelDeposit.TabIndex = 1;
            this.labelDeposit.Text = "Belopp:";
            // 
            // buttonMakeDeposit
            // 
            this.buttonMakeDeposit.Location = new System.Drawing.Point(148, 61);
            this.buttonMakeDeposit.Name = "buttonMakeDeposit";
            this.buttonMakeDeposit.Size = new System.Drawing.Size(97, 38);
            this.buttonMakeDeposit.TabIndex = 2;
            this.buttonMakeDeposit.Text = "Sätt in";
            this.buttonMakeDeposit.UseVisualStyleBackColor = true;
            this.buttonMakeDeposit.Click += new System.EventHandler(this.buttonMakeDeposit_Click);
            // 
            // Deposit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(393, 108);
            this.Controls.Add(this.buttonMakeDeposit);
            this.Controls.Add(this.labelDeposit);
            this.Controls.Add(this.textBoxDepositAmount);
            this.Name = "Deposit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Deposit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDepositAmount;
        private System.Windows.Forms.Label labelDeposit;
        private System.Windows.Forms.Button buttonMakeDeposit;
    }
}