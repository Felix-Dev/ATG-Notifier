using ATG_Notifier.Desktop.Views.ToastNotification;
using ATG_Notifier.Desktop.Utilities.Bindings;
using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace ATG_Notifier.Desktop.Views
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.ctxMenuItemPlayPopupSound = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.separator1 = new System.Windows.Forms.MenuItem();
            this.ctxMenuItemDoNotDisturb = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.separator2 = new System.Windows.Forms.MenuItem();
            this.ctxMenuItemExit = new System.Windows.Forms.MenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemNotificationOptions = new System.Windows.Forms.MenuItem();
            this.menuItemPlaySound = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemSeparator = new System.Windows.Forms.MenuItem();
            this.menuItemDisableOnFullscreen = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemDoNotDisturb = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemNotificationPosition = new System.Windows.Forms.MenuItem();
            this.menuItemNotificationPositionTopLeft = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemNotificationPositionTopRight = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemNotificationPositionBottomLeft = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemNotificationPositionBottomRight = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItem_Help = new System.Windows.Forms.MenuItem();
            this.menuItem_AboutNotifier = new System.Windows.Forms.MenuItem();
            this.wpfHost = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenu = this.contextMenu;
            this.notifyIcon.Text = "ATG-Notifier";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotificationIcon_MouseDoubleClick);
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ctxMenuItemPlayPopupSound,
            this.separator1,
            this.ctxMenuItemDoNotDisturb,
            this.separator2,
            this.ctxMenuItemExit});
            // 
            // ctxMenuItemPlayPopupSound
            // 
            this.ctxMenuItemPlayPopupSound.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "PlayPopupSound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ctxMenuItemPlayPopupSound.Index = 0;
            this.ctxMenuItemPlayPopupSound.Text = "Play Notification Sound";
            this.ctxMenuItemPlayPopupSound.Click += new System.EventHandler(this.OnMenuItemPlayPopupSound_Click);
            // 
            // separator1
            // 
            this.separator1.Index = 1;
            this.separator1.Text = "-";
            // 
            // ctxMenuItemDoNotDisturb
            // 
            this.ctxMenuItemDoNotDisturb.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "DoNotDisturb", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ctxMenuItemDoNotDisturb.Index = 2;
            this.ctxMenuItemDoNotDisturb.Text = "Do Not Disturb";
            this.ctxMenuItemDoNotDisturb.Click += new System.EventHandler(this.OnMenuItemDoNotDisturb_Click);
            // 
            // separator2
            // 
            this.separator2.Index = 3;
            this.separator2.Text = "-";
            // 
            // ctxMenuItemExit
            // 
            this.ctxMenuItemExit.Index = 4;
            this.ctxMenuItemExit.Text = "Exit";
            this.ctxMenuItemExit.Click += new System.EventHandler(this.OnMenuItemExit_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOptions,
            this.menuItemNotificationOptions,
            this.menuItem_Help});
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.Index = 0;
            this.menuItemOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemExit});
            this.menuItemOptions.Text = "Options";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 0;
            this.menuItemExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.OnMenuItemExit_Click);
            // 
            // menuItemNotificationOptions
            // 
            this.menuItemNotificationOptions.Index = 1;
            this.menuItemNotificationOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPlaySound,
            this.menuItemSeparator,
            this.menuItemDisableOnFullscreen,
            this.menuItemDoNotDisturb,
            this.menuItem1,
            this.menuItemNotificationPosition});
            this.menuItemNotificationOptions.Text = "Notifications";
            // 
            // menuItemPlaySound
            // 
            this.menuItemPlaySound.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "PlayPopupSound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemPlaySound.Index = 0;
            this.menuItemPlaySound.Text = "Play Sound";
            this.menuItemPlaySound.Click += new System.EventHandler(this.OnMenuItemPlayPopupSound_Click);
            // 
            // menuItemSeparator
            // 
            this.menuItemSeparator.Index = 1;
            this.menuItemSeparator.Text = "-";
            // 
            // menuItemDisableOnFullscreen
            // 
            this.menuItemDisableOnFullscreen.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "DisableOnFullscreen", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemDisableOnFullscreen.Index = 2;
            this.menuItemDisableOnFullscreen.Text = "Disable On Fullscreen";
            this.menuItemDisableOnFullscreen.Click += new System.EventHandler(this.OnMenuItemDisableOnFullscreen_Click);
            // 
            // menuItemDoNotDisturb
            // 
            this.menuItemDoNotDisturb.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "DoNotDisturb", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemDoNotDisturb.Index = 3;
            this.menuItemDoNotDisturb.Text = "Do Not Disturb";
            this.menuItemDoNotDisturb.Click += new System.EventHandler(this.OnMenuItemDoNotDisturb_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 4;
            this.menuItem1.Text = "-";
            // 
            // menuItemNotificationPosition
            // 
            this.menuItemNotificationPosition.Index = 5;
            this.menuItemNotificationPosition.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNotificationPositionTopLeft,
            this.menuItemNotificationPositionTopRight,
            this.menuItemNotificationPositionBottomLeft,
            this.menuItemNotificationPositionBottomRight});
            this.menuItemNotificationPosition.Text = "Position";
            // 
            // menuItemNotificationPositionTopLeft
            // 
            this.menuItemNotificationPositionTopLeft.Index = 0;
            this.menuItemNotificationPositionTopLeft.RadioCheck = true;
            this.menuItemNotificationPositionTopLeft.Text = "Top left";
            this.menuItemNotificationPositionTopLeft.Click += new System.EventHandler(this.OnMenuItemNotificationPositionTopLeft_Click);
            // 
            // menuItemNotificationPositionTopRight
            // 
            this.menuItemNotificationPositionTopRight.Index = 1;
            this.menuItemNotificationPositionTopRight.RadioCheck = true;
            this.menuItemNotificationPositionTopRight.Text = "Top right";
            this.menuItemNotificationPositionTopRight.Click += new System.EventHandler(this.OnMenuItemNotificationPositionTopRight_Click);
            // 
            // menuItemNotificationPositionBottomLeft
            // 
            this.menuItemNotificationPositionBottomLeft.Index = 2;
            this.menuItemNotificationPositionBottomLeft.RadioCheck = true;
            this.menuItemNotificationPositionBottomLeft.Text = "Bottom left";
            this.menuItemNotificationPositionBottomLeft.Click += new System.EventHandler(this.OnMenuItemNotificationPositionBottomLeft_Click);
            // 
            // menuItemNotificationPositionBottomRight
            // 
            this.menuItemNotificationPositionBottomRight.Index = 3;
            this.menuItemNotificationPositionBottomRight.RadioCheck = true;
            this.menuItemNotificationPositionBottomRight.Text = "Bottom right";
            this.menuItemNotificationPositionBottomRight.Click += new System.EventHandler(this.OnMenuItemNotificationPositionBottomRight_Click);
            // 
            // menuItem_Help
            // 
            this.menuItem_Help.Index = 2;
            this.menuItem_Help.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_AboutNotifier});
            this.menuItem_Help.Text = "Help";
            // 
            // menuItem_AboutNotifier
            // 
            this.menuItem_AboutNotifier.Index = 0;
            this.menuItem_AboutNotifier.Text = "About Notifier";
            this.menuItem_AboutNotifier.Click += new System.EventHandler(this.OnMenuItem_AboutNotifier_Click);
            // 
            // wpfHost
            // 
            this.wpfHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpfHost.Location = new System.Drawing.Point(0, 0);
            this.wpfHost.Margin = new System.Windows.Forms.Padding(0);
            this.wpfHost.Name = "wpfHost";
            this.wpfHost.Size = new System.Drawing.Size(364, 211);
            this.wpfHost.TabIndex = 1;
            this.wpfHost.Child = null;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(364, 211);
            this.Controls.Add(this.wpfHost);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(380, 81257);
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(250, 250);
            this.Name = "MainWindow";
            this.ResumeLayout(false);

        }

        private void Binding_Format(object sender, ConvertEventArgs e)
        {
            foreach (MenuItem menuItem in this.menuItemNotificationPosition.MenuItems)
            {
                menuItem.Checked = false;
            }

            switch (e.Value)
            {
                case DisplayPosition.TopLeft:
                    this.menuItemNotificationPositionTopLeft.Checked = true;
                    e.Value = true;
                    break;
                case DisplayPosition.TopRight:
                    this.menuItemNotificationPositionTopRight.Checked = true;
                    e.Value = false;
                    break;
                case DisplayPosition.BottomLeft:
                    this.menuItemNotificationPositionBottomLeft.Checked = true;
                    e.Value = false;
                    break;
                case DisplayPosition.BottomRight:
                    this.menuItemNotificationPositionBottomRight.Checked = true;
                    e.Value = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private NotifyIcon notifyIcon;
        private ContextMenu contextMenu;
        private MenuItem ctxMenuItemExit;
        private BindableMenuItem ctxMenuItemDoNotDisturb;
        private BindableMenuItem ctxMenuItemPlayPopupSound;
        private MenuItem separator1;

        private MainMenu mainMenu;
        private MenuItem menuItemNotificationOptions;
        private MenuItem separator2;
        private ElementHost wpfHost;
        private BindableMenuItem menuItemPlaySound;
        private MenuItem menuItemSeparator;
        private BindableMenuItem menuItemDisableOnFullscreen;
        private BindableMenuItem menuItemDoNotDisturb;
        private MenuItem menuItemOptions;
        private MenuItem menuItemExit;
        private MenuItem menuItem_Help;
        private MenuItem menuItem_AboutNotifier;
        private MenuItem menuItem1;
        private MenuItem menuItemNotificationPosition;
        private BindableMenuItem menuItemNotificationPositionTopLeft;
        private BindableMenuItem menuItemNotificationPositionTopRight;
        private BindableMenuItem menuItemNotificationPositionBottomLeft;
        private BindableMenuItem menuItemNotificationPositionBottomRight;
    }
}

