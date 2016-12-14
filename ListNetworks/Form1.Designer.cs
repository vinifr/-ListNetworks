namespace ListNetworks
{
    partial class Form1
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
            this.lstBoxPrinters = new System.Windows.Forms.ListBox();
            this.btSearchNetPrinter = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstBoxPrinters
            // 
            this.lstBoxPrinters.FormattingEnabled = true;
            this.lstBoxPrinters.Location = new System.Drawing.Point(23, 57);
            this.lstBoxPrinters.Name = "lstBoxPrinters";
            this.lstBoxPrinters.Size = new System.Drawing.Size(637, 212);
            this.lstBoxPrinters.TabIndex = 0;
            // 
            // btSearchNetPrinter
            // 
            this.btSearchNetPrinter.Location = new System.Drawing.Point(197, 289);
            this.btSearchNetPrinter.Name = "btSearchNetPrinter";
            this.btSearchNetPrinter.Size = new System.Drawing.Size(101, 36);
            this.btSearchNetPrinter.TabIndex = 1;
            this.btSearchNetPrinter.Text = "Search";
            this.btSearchNetPrinter.UseVisualStyleBackColor = true;
            this.btSearchNetPrinter.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(376, 289);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 36);
            this.button2.TabIndex = 2;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 384);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btSearchNetPrinter);
            this.Controls.Add(this.lstBoxPrinters);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstBoxPrinters;
        private System.Windows.Forms.Button btSearchNetPrinter;
        private System.Windows.Forms.Button button2;
    }
}

