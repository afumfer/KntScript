namespace KntScriptAppHost
{
    partial class Test
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
            this.buttonRunScriptFile = new System.Windows.Forms.Button();
            this.buttonShowConsole = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRunScript
            // 
            this.buttonRunScript.Location = new System.Drawing.Point(12, 24);
            this.buttonRunScript.Name = "buttonRunScript";
            this.buttonRunScript.Size = new System.Drawing.Size(123, 28);
            this.buttonRunScript.TabIndex = 0;
            this.buttonRunScript.Text = "Run script";
            this.buttonRunScript.UseVisualStyleBackColor = true;
            this.buttonRunScript.Click += new System.EventHandler(this.buttonRunScript_Click);
            // 
            // buttonRunScriptFile
            // 
            this.buttonRunScriptFile.Location = new System.Drawing.Point(12, 58);
            this.buttonRunScriptFile.Name = "buttonRunScriptFile";
            this.buttonRunScriptFile.Size = new System.Drawing.Size(123, 25);
            this.buttonRunScriptFile.TabIndex = 1;
            this.buttonRunScriptFile.Text = "Run script file";
            this.buttonRunScriptFile.UseVisualStyleBackColor = true;
            this.buttonRunScriptFile.Click += new System.EventHandler(this.buttonRunScriptFile_Click);
            // 
            // buttonShowConsole
            // 
            this.buttonShowConsole.Location = new System.Drawing.Point(12, 89);
            this.buttonShowConsole.Name = "buttonShowConsole";
            this.buttonShowConsole.Size = new System.Drawing.Size(123, 29);
            this.buttonShowConsole.TabIndex = 2;
            this.buttonShowConsole.Text = "Show console";
            this.buttonShowConsole.UseVisualStyleBackColor = true;
            this.buttonShowConsole.Click += new System.EventHandler(this.buttonShowConsole_Click);
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 302);
            this.Controls.Add(this.buttonShowConsole);
            this.Controls.Add(this.buttonRunScriptFile);
            this.Controls.Add(this.buttonRunScript);
            this.Name = "Test";
            this.Text = "KntScript test ";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRunScript;
        private System.Windows.Forms.Button buttonRunScriptFile;
        private System.Windows.Forms.Button buttonShowConsole;
    }
}