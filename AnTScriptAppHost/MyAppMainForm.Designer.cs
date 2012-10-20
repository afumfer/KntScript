namespace AnTScriptAppHost
{
    partial class MyAppMainForm
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
            this.buttonRunScript = new System.Windows.Forms.Button();
            this.buttonShowConsole = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textFileSourceCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonRunScript
            // 
            this.buttonRunScript.Location = new System.Drawing.Point(564, 53);
            this.buttonRunScript.Name = "buttonRunScript";
            this.buttonRunScript.Size = new System.Drawing.Size(144, 27);
            this.buttonRunScript.TabIndex = 0;
            this.buttonRunScript.Text = "Run script";
            this.buttonRunScript.UseVisualStyleBackColor = true;
            this.buttonRunScript.Click += new System.EventHandler(this.buttonRunScript_Click);
            // 
            // buttonShowConsole
            // 
            this.buttonShowConsole.Location = new System.Drawing.Point(564, 20);
            this.buttonShowConsole.Name = "buttonShowConsole";
            this.buttonShowConsole.Size = new System.Drawing.Size(144, 27);
            this.buttonShowConsole.TabIndex = 1;
            this.buttonShowConsole.Text = "Show AnTScript Console";
            this.buttonShowConsole.UseVisualStyleBackColor = true;
            this.buttonShowConsole.Click += new System.EventHandler(this.buttonShowConsole_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Script file:";
            // 
            // textFileSourceCode
            // 
            this.textFileSourceCode.Location = new System.Drawing.Point(12, 27);
            this.textFileSourceCode.Name = "textFileSourceCode";
            this.textFileSourceCode.Size = new System.Drawing.Size(458, 20);
            this.textFileSourceCode.TabIndex = 3;
            // 
            // MyAppMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 259);
            this.Controls.Add(this.textFileSourceCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonShowConsole);
            this.Controls.Add(this.buttonRunScript);
            this.Name = "MyAppMainForm";
            this.Text = "My App Main Form";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRunScript;
        private System.Windows.Forms.Button buttonShowConsole;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textFileSourceCode;
    }
}

