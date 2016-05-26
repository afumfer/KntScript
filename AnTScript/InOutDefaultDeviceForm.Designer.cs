namespace AnTScript
{
    partial class InOutDefaultDeviceForm
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
            this.textOut = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textOut
            // 
            this.textOut.BackColor = System.Drawing.Color.Black;
            this.textOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textOut.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textOut.ForeColor = System.Drawing.Color.White;
            this.textOut.Location = new System.Drawing.Point(0, 0);
            this.textOut.Multiline = true;
            this.textOut.Name = "textOut";
            this.textOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textOut.Size = new System.Drawing.Size(570, 487);
            this.textOut.TabIndex = 7;
            // 
            // InOutDefaultDeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(570, 487);
            this.Controls.Add(this.textOut);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Name = "InOutDefaultDeviceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AnTScript - Out";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InOutDefaultDeviceForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textOut;
    }
}