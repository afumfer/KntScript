namespace AnTScript
{
    partial class AnTScriptForm
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
            this.buttonRun = new System.Windows.Forms.Button();
            this.textFileSourceCode = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textSourceCode = new System.Windows.Forms.TextBox();
            this.textOut = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listScan = new System.Windows.Forms.ListBox();
            this.treeAST = new System.Windows.Forms.TreeView();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRun.Location = new System.Drawing.Point(921, 10);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(81, 23);
            this.buttonRun.TabIndex = 0;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // textFileSourceCode
            // 
            this.textFileSourceCode.Location = new System.Drawing.Point(12, 12);
            this.textFileSourceCode.Name = "textFileSourceCode";
            this.textFileSourceCode.Size = new System.Drawing.Size(361, 20);
            this.textFileSourceCode.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 38);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(990, 716);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(982, 690);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Code / Out";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textSourceCode);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textOut);
            this.splitContainer1.Size = new System.Drawing.Size(976, 684);
            this.splitContainer1.SplitterDistance = 502;
            this.splitContainer1.TabIndex = 0;
            // 
            // textSourceCode
            // 
            this.textSourceCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textSourceCode.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textSourceCode.Location = new System.Drawing.Point(0, 0);
            this.textSourceCode.Multiline = true;
            this.textSourceCode.Name = "textSourceCode";
            this.textSourceCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textSourceCode.Size = new System.Drawing.Size(502, 684);
            this.textSourceCode.TabIndex = 3;
            // 
            // textOut
            // 
            this.textOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textOut.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textOut.Location = new System.Drawing.Point(0, 0);
            this.textOut.Multiline = true;
            this.textOut.Name = "textOut";
            this.textOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textOut.Size = new System.Drawing.Size(470, 684);
            this.textOut.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(982, 690);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Symbols Table / AST";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listScan);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.treeAST);
            this.splitContainer2.Size = new System.Drawing.Size(976, 684);
            this.splitContainer2.SplitterDistance = 286;
            this.splitContainer2.TabIndex = 0;
            // 
            // listScan
            // 
            this.listScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listScan.FormattingEnabled = true;
            this.listScan.Location = new System.Drawing.Point(0, 0);
            this.listScan.Name = "listScan";
            this.listScan.Size = new System.Drawing.Size(286, 684);
            this.listScan.TabIndex = 10;
            // 
            // treeAST
            // 
            this.treeAST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeAST.Location = new System.Drawing.Point(0, 0);
            this.treeAST.Name = "treeAST";
            this.treeAST.Size = new System.Drawing.Size(686, 684);
            this.treeAST.TabIndex = 5;
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Location = new System.Drawing.Point(385, 10);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(96, 23);
            this.buttonLoadFile.TabIndex = 7;
            this.buttonLoadFile.Text = "Load File";
            this.buttonLoadFile.UseVisualStyleBackColor = true;
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 766);
            this.Controls.Add(this.buttonLoadFile);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.textFileSourceCode);
            this.Controls.Add(this.buttonRun);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.TextBox textFileSourceCode;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textSourceCode;
        private System.Windows.Forms.TextBox textOut;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox listScan;
        private System.Windows.Forms.TreeView treeAST;
        private System.Windows.Forms.Button buttonLoadFile;
    }
}
