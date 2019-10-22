namespace ATG_Notifier.UI.Views
{
    partial class AboutView
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
            this.button_OK = new System.Windows.Forms.Button();
            this.label_Version = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_OK.CausesValidation = false;
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_OK.Location = new System.Drawing.Point(98, 38);
            this.button_OK.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 29);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // label_Version
            // 
            this.label_Version.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_Version.AutoSize = true;
            this.label_Version.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Version.Location = new System.Drawing.Point(64, 9);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(155, 23);
            this.label_Version.TabIndex = 0;
            this.label_Version.Text = "VersionPlaceholder";
            this.label_Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AboutView
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.CancelButton = this.button_OK;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(278, 75);
            this.Controls.Add(this.label_Version);
            this.Controls.Add(this.button_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Notifier";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label_Version;
    }
}