namespace WinFormTest
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
            if (disposing && (components != null)) {
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
            this.winFormTagsEditor1 = new WFTE.WinFormTagsEditor();
            this.SuspendLayout();
            // 
            // winFormTagsEditor1
            // 
            this.winFormTagsEditor1.Location = new System.Drawing.Point(226, 118);
            this.winFormTagsEditor1.Margin = new System.Windows.Forms.Padding(4);
            this.winFormTagsEditor1.Name = "winFormTagsEditor1";
            this.winFormTagsEditor1.Size = new System.Drawing.Size(150, 150);
            this.winFormTagsEditor1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 426);
            this.Controls.Add(this.winFormTagsEditor1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private WFTE.WinFormTagsEditor winFormTagsEditor1;
    }
}

