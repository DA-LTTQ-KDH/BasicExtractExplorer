namespace BasicExtractExplorer
{
    partial class Processing
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
            this.labelArchiveName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.progressBarTotal = new System.Windows.Forms.ProgressBar();
            this.labelPercent = new System.Windows.Forms.Label();
            this.labelFile = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelArchiveName
            // 
            this.labelArchiveName.AutoSize = true;
            this.labelArchiveName.Location = new System.Drawing.Point(3, 18);
            this.labelArchiveName.Name = "labelArchiveName";
            this.labelArchiveName.Size = new System.Drawing.Size(100, 17);
            this.labelArchiveName.TabIndex = 0;
            this.labelArchiveName.Text = ".......................";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBarTotal);
            this.groupBox1.Controls.Add(this.labelPercent);
            this.groupBox1.Controls.Add(this.labelFile);
            this.groupBox1.Controls.Add(this.labelArchiveName);
            this.groupBox1.Location = new System.Drawing.Point(6, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(517, 107);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // progressBarTotal
            // 
            this.progressBarTotal.Location = new System.Drawing.Point(6, 70);
            this.progressBarTotal.Name = "progressBarTotal";
            this.progressBarTotal.Size = new System.Drawing.Size(505, 23);
            this.progressBarTotal.TabIndex = 8;
            // 
            // labelPercent
            // 
            this.labelPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPercent.AutoSize = true;
            this.labelPercent.Location = new System.Drawing.Point(471, 42);
            this.labelPercent.Name = "labelPercent";
            this.labelPercent.Size = new System.Drawing.Size(24, 17);
            this.labelPercent.TabIndex = 3;
            this.labelPercent.Text = "....";
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(3, 42);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(76, 17);
            this.labelFile.TabIndex = 2;
            this.labelFile.Text = ".................";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(413, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Processing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 156);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Processing";
            this.Text = "Processing";
            this.Load += new System.EventHandler(this.Processing_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelArchiveName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBarTotal;
        private System.Windows.Forms.Label labelPercent;
        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.Button button1;
    }
}