namespace ATG_Notifier.Desktop.View
{
    partial class ToastNotificationView
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public object DataContext
        {
            get => this.menuNotificationBindingSource.DataSource;
            set => this.menuNotificationBindingSource.DataSource = value;
        }

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelTitle = new System.Windows.Forms.Label();
            this.menuNotificationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelNotificationContent = new System.Windows.Forms.Panel();
            this.TextBox_Chapter = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.menuNotificationBindingSource)).BeginInit();
            this.panelNotificationContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(16, 20);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(74, 28);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Debug";
            this.labelTitle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            // 
            // menuNotificationBindingSource
            // 
            this.menuNotificationBindingSource.DataSource = typeof(ATG_Notifier.ViewModels.ViewModels.ChapterProfileViewModel);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(9)))), ((int)(((byte)(74)))), ((int)(((byte)(178)))));
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(49)))), ((int)(((byte)(109)))), ((int)(((byte)(210)))));
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI Symbol", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonCancel.Location = new System.Drawing.Point(364, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(44, 41);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "❌";
            this.buttonCancel.UseCompatibleTextRendering = true;
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnCancelButtonMouseClick);
            // 
            // panelNotificationContent
            // 
            this.panelNotificationContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(74)))), ((int)(((byte)(178)))));
            this.panelNotificationContent.Controls.Add(this.TextBox_Chapter);
            this.panelNotificationContent.Controls.Add(this.buttonCancel);
            this.panelNotificationContent.Controls.Add(this.labelTitle);
            this.panelNotificationContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNotificationContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelNotificationContent.ForeColor = System.Drawing.Color.White;
            this.panelNotificationContent.Location = new System.Drawing.Point(3, 2);
            this.panelNotificationContent.Margin = new System.Windows.Forms.Padding(4);
            this.panelNotificationContent.Name = "panelNotificationContent";
            this.panelNotificationContent.Size = new System.Drawing.Size(407, 97);
            this.panelNotificationContent.TabIndex = 0;
            this.panelNotificationContent.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            // 
            // TextBox_Chapter
            // 
            this.TextBox_Chapter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(74)))), ((int)(((byte)(178)))));
            this.TextBox_Chapter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Chapter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox_Chapter.ForeColor = System.Drawing.Color.White;
            this.TextBox_Chapter.Location = new System.Drawing.Point(16, 57);
            this.TextBox_Chapter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TextBox_Chapter.Name = "TextBox_Chapter";
            this.TextBox_Chapter.ReadOnly = true;
            this.TextBox_Chapter.Size = new System.Drawing.Size(383, 27);
            this.TextBox_Chapter.TabIndex = 3;
            this.TextBox_Chapter.TabStop = false;
            this.TextBox_Chapter.Text = "Placeholder";
            this.TextBox_Chapter.WordWrap = false;
            this.TextBox_Chapter.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TextBox_Chapter_MouseDoubleClick);
            // 
            // ToastNotificationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(107)))), ((int)(((byte)(190)))));
            this.ClientSize = new System.Drawing.Size(413, 101);
            this.Controls.Add(this.panelNotificationContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(413, 101);
            this.Name = "ToastNotificationView";
            this.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.menuNotificationBindingSource)).EndInit();
            this.panelNotificationContent.ResumeLayout(false);
            this.panelNotificationContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelNotificationContent;

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.BindingSource menuNotificationBindingSource;
        private System.Windows.Forms.TextBox TextBox_Chapter;
    }
}
