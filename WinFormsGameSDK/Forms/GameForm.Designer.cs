namespace WinFormsGameSDK.Forms
{
    partial class GameForm
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
            this.components = new System.ComponentModel.Container();
            this.timerInvalidate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerInvalidate
            // 
            this.timerInvalidate.Enabled = true;
            this.timerInvalidate.Interval = 10;
            this.timerInvalidate.Tick += new System.EventHandler(this.timerInvalidate_Tick);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(964, 513);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ForeColor = System.Drawing.Color.Lime;
            this.KeyPreview = true;
            this.Name = "GameForm";
            this.Text = "Game Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerInvalidate;
    }
}