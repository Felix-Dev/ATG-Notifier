namespace ATG_Notifier.Desktop.View
{
    partial class ToastNotificationView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public object DataContext
        {
            get => this.menuNotificationBindingSource.DataSource;
            set => this.menuNotificationBindingSource.DataSource = value;
        }

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
            this.labelTitle = new System.Windows.Forms.Label();
            this.menuNotificationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelNotificationContent = new System.Windows.Forms.Panel();
            this.ChapterNumberAndTitleTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.menuNotificationBindingSource)).BeginInit();
            this.panelNotificationContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(12, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(61, 21);
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
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI Symbol", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonCancel.Location = new System.Drawing.Point(273, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(33, 33);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "✖";
            this.buttonCancel.UseCompatibleTextRendering = true;
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnCancelButtonMouseClick);
            // 
            // panelNotificationContent
            // 
            this.panelNotificationContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(74)))), ((int)(((byte)(178)))));
            this.panelNotificationContent.Controls.Add(this.ChapterNumberAndTitleTextBox);
            this.panelNotificationContent.Controls.Add(this.buttonCancel);
            this.panelNotificationContent.Controls.Add(this.labelTitle);
            this.panelNotificationContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNotificationContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelNotificationContent.ForeColor = System.Drawing.Color.White;
            this.panelNotificationContent.Location = new System.Drawing.Point(2, 2);
            this.panelNotificationContent.Name = "panelNotificationContent";
            this.panelNotificationContent.Size = new System.Drawing.Size(306, 78);
            this.panelNotificationContent.TabIndex = 0;
            this.panelNotificationContent.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            // 
            // ChapterNumberAndTitleTextBox
            // 
            this.ChapterNumberAndTitleTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(74)))), ((int)(((byte)(178)))));
            this.ChapterNumberAndTitleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ChapterNumberAndTitleTextBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChapterNumberAndTitleTextBox.ForeColor = System.Drawing.Color.White;
            this.ChapterNumberAndTitleTextBox.Location = new System.Drawing.Point(12, 46);
            this.ChapterNumberAndTitleTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ChapterNumberAndTitleTextBox.Name = "ChapterNumberAndTitleTextBox";
            this.ChapterNumberAndTitleTextBox.ReadOnly = true;
            this.ChapterNumberAndTitleTextBox.Size = new System.Drawing.Size(88, 22);
            this.ChapterNumberAndTitleTextBox.TabIndex = 3;
            this.ChapterNumberAndTitleTextBox.TabStop = false;
            this.ChapterNumberAndTitleTextBox.Text = "Placeholder";
            this.ChapterNumberAndTitleTextBox.WordWrap = false;
            this.ChapterNumberAndTitleTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnChapterNumberAndTitleTextBoxMouseDoubleClick);
            // 
            // ToastNotificationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(107)))), ((int)(((byte)(190)))));
            this.ClientSize = new System.Drawing.Size(310, 82);
            this.Controls.Add(this.panelNotificationContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(310, 82);
            this.Name = "ToastNotificationView";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
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
        private System.Windows.Forms.TextBox ChapterNumberAndTitleTextBox;
    }
}
