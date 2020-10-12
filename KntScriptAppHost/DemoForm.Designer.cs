namespace KntScriptAppHost
{
    partial class DemoForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupSamples = new System.Windows.Forms.GroupBox();
            this.buttonRunSample = new System.Windows.Forms.Button();
            this.buttonShowSample = new System.Windows.Forms.Button();
            this.listSamples = new System.Windows.Forms.ListBox();
            this.statusInfo = new System.Windows.Forms.StatusStrip();
            this.buttonInteract = new System.Windows.Forms.Button();
            this.groupSamples.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(305, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Run simple script (embedded code)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonRunScript_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 86);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(305, 29);
            this.button3.TabIndex = 1;
            this.button3.Text = "Show console";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.buttonShowConsole_Click);
            // 
            // groupSamples
            // 
            this.groupSamples.Controls.Add(this.buttonRunSample);
            this.groupSamples.Controls.Add(this.buttonShowSample);
            this.groupSamples.Controls.Add(this.listSamples);
            this.groupSamples.Location = new System.Drawing.Point(12, 145);
            this.groupSamples.Name = "groupSamples";
            this.groupSamples.Size = new System.Drawing.Size(425, 276);
            this.groupSamples.TabIndex = 3;
            this.groupSamples.TabStop = false;
            this.groupSamples.Text = "Samples";
            // 
            // buttonRunSample
            // 
            this.buttonRunSample.Location = new System.Drawing.Point(262, 19);
            this.buttonRunSample.Name = "buttonRunSample";
            this.buttonRunSample.Size = new System.Drawing.Size(148, 30);
            this.buttonRunSample.TabIndex = 3;
            this.buttonRunSample.Text = "Run sample";
            this.buttonRunSample.UseVisualStyleBackColor = true;
            this.buttonRunSample.Click += new System.EventHandler(this.buttonRunSample_Click);
            // 
            // buttonShowSample
            // 
            this.buttonShowSample.Location = new System.Drawing.Point(262, 55);
            this.buttonShowSample.Name = "buttonShowSample";
            this.buttonShowSample.Size = new System.Drawing.Size(148, 31);
            this.buttonShowSample.TabIndex = 4;
            this.buttonShowSample.Text = "Show sample in console";
            this.buttonShowSample.UseVisualStyleBackColor = true;
            this.buttonShowSample.Click += new System.EventHandler(this.buttonShowSample_Click);
            // 
            // listSamples
            // 
            this.listSamples.FormattingEnabled = true;
            this.listSamples.ItemHeight = 15;
            this.listSamples.Location = new System.Drawing.Point(13, 19);
            this.listSamples.Name = "listSamples";
            this.listSamples.Size = new System.Drawing.Size(230, 244);
            this.listSamples.TabIndex = 2;
            this.listSamples.SelectedIndexChanged += new System.EventHandler(this.listSamples_SelectedIndexChanged);
            // 
            // statusInfo
            // 
            this.statusInfo.Location = new System.Drawing.Point(0, 511);
            this.statusInfo.Name = "statusInfo";
            this.statusInfo.Size = new System.Drawing.Size(450, 22);
            this.statusInfo.TabIndex = 4;
            this.statusInfo.Text = "...";
            // 
            // buttonInteract
            // 
            this.buttonInteract.Location = new System.Drawing.Point(12, 52);
            this.buttonInteract.Name = "buttonInteract";
            this.buttonInteract.Size = new System.Drawing.Size(305, 28);
            this.buttonInteract.TabIndex = 5;
            this.buttonInteract.Text = "Interacting with kntscript (embedded code)";
            this.buttonInteract.UseVisualStyleBackColor = true;
            this.buttonInteract.Click += new System.EventHandler(this.buttonInteract_Click);
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 452);
            this.Controls.Add(this.buttonInteract);
            this.Controls.Add(this.statusInfo);
            this.Controls.Add(this.groupSamples);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DemoForm";
            this.Text = "KntScript demo";
            this.Load += new System.EventHandler(this.DemoForm_Load);
            this.groupSamples.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupSamples;
        private System.Windows.Forms.Button buttonRunSample;
        private System.Windows.Forms.Button buttonShowSample;
        private System.Windows.Forms.ListBox listSamples;
        private System.Windows.Forms.StatusStrip statusInfo;
        private System.Windows.Forms.Button buttonInteract;
    }
}