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
            this.separator1 = new System.Windows.Forms.MenuItem();
            this.separator2 = new System.Windows.Forms.MenuItem();
            this.ctxMenuItemExit = new System.Windows.Forms.MenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemNotificationOptions = new System.Windows.Forms.MenuItem();
            this.menuItemSeparator = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemNotificationPosition = new System.Windows.Forms.MenuItem();
            this.menuItem_Help = new System.Windows.Forms.MenuItem();
            this.menuItem_AboutNotifier = new System.Windows.Forms.MenuItem();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.panelDisplayNotifications = new System.Windows.Forms.Panel();
            this.wpfHost = new System.Windows.Forms.Integration.ElementHost();
            this.panelNotificationSortOptionsBackground = new System.Windows.Forms.Panel();
            this.panelNotificationSortOptions = new System.Windows.Forms.Panel();
            this.buttonClearList = new System.Windows.Forms.Button();
            this.menuItemPlaySound = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemTurnOnDisplay = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemDisableOnFullscreen = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemDoNotDisturb = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemNotificationPositionTopLeft = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemNotificationPositionTopRight = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemNotificationPositionBottomLeft = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.menuItemNotificationPositionBottomRight = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.ctxMenuItemPlayPopupSound = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.ctxMenuItemDoNotDisturb = new ATG_Notifier.Desktop.Utilities.Bindings.BindableMenuItem();
            this.panelBackground.SuspendLayout();
            this.panelDisplayNotifications.SuspendLayout();
            this.panelNotificationSortOptionsBackground.SuspendLayout();
            this.panelNotificationSortOptions.SuspendLayout();
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
            // separator1
            // 
            this.separator1.Index = 1;
            this.separator1.Text = "-";
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
            this.menuItemTurnOnDisplay,
            this.menuItemSeparator,
            this.menuItemDisableOnFullscreen,
            this.menuItemDoNotDisturb,
            this.menuItem1,
            this.menuItemNotificationPosition});
            this.menuItemNotificationOptions.Text = "Notifications";
            // 
            // menuItemSeparator
            // 
            this.menuItemSeparator.Index = 2;
            this.menuItemSeparator.Text = "-";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 5;
            this.menuItem1.Text = "-";
            // 
            // menuItemNotificationPosition
            // 
            this.menuItemNotificationPosition.Index = 6;
            this.menuItemNotificationPosition.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemNotificationPositionTopLeft,
            this.menuItemNotificationPositionTopRight,
            this.menuItemNotificationPositionBottomLeft,
            this.menuItemNotificationPositionBottomRight});
            this.menuItemNotificationPosition.Text = "Position";
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
            // panelBackground
            // 
            this.panelBackground.AutoScroll = true;
            this.panelBackground.BackColor = System.Drawing.Color.White;
            this.panelBackground.Controls.Add(this.panelDisplayNotifications);
            this.panelBackground.Controls.Add(this.panelNotificationSortOptionsBackground);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Margin = new System.Windows.Forms.Padding(0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(447, 153);
            this.panelBackground.TabIndex = 2;
            // 
            // panelDisplayNotifications
            // 
            this.panelDisplayNotifications.AutoScroll = true;
            this.panelDisplayNotifications.Controls.Add(this.wpfHost);
            this.panelDisplayNotifications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDisplayNotifications.Location = new System.Drawing.Point(0, 41);
            this.panelDisplayNotifications.Margin = new System.Windows.Forms.Padding(0);
            this.panelDisplayNotifications.Name = "panelDisplayNotifications";
            this.panelDisplayNotifications.Size = new System.Drawing.Size(447, 112);
            this.panelDisplayNotifications.TabIndex = 1;
            // 
            // wpfHost
            // 
            this.wpfHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wpfHost.Location = new System.Drawing.Point(0, 0);
            this.wpfHost.Margin = new System.Windows.Forms.Padding(0);
            this.wpfHost.Name = "wpfHost";
            this.wpfHost.Size = new System.Drawing.Size(447, 112);
            this.wpfHost.TabIndex = 2;
            this.wpfHost.Child = null;
            // 
            // panelNotificationSortOptionsBackground
            // 
            this.panelNotificationSortOptionsBackground.BackColor = System.Drawing.Color.LightGray;
            this.panelNotificationSortOptionsBackground.Controls.Add(this.panelNotificationSortOptions);
            this.panelNotificationSortOptionsBackground.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNotificationSortOptionsBackground.Location = new System.Drawing.Point(0, 0);
            this.panelNotificationSortOptionsBackground.Margin = new System.Windows.Forms.Padding(4);
            this.panelNotificationSortOptionsBackground.MaximumSize = new System.Drawing.Size(413, 42);
            this.panelNotificationSortOptionsBackground.Name = "panelNotificationSortOptionsBackground";
            this.panelNotificationSortOptionsBackground.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.panelNotificationSortOptionsBackground.Size = new System.Drawing.Size(413, 41);
            this.panelNotificationSortOptionsBackground.TabIndex = 0;
            // 
            // panelNotificationSortOptions
            // 
            this.panelNotificationSortOptions.BackColor = System.Drawing.Color.White;
            this.panelNotificationSortOptions.Controls.Add(this.buttonClearList);
            this.panelNotificationSortOptions.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.panelNotificationSortOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNotificationSortOptions.Location = new System.Drawing.Point(0, 0);
            this.panelNotificationSortOptions.Margin = new System.Windows.Forms.Padding(4);
            this.panelNotificationSortOptions.Name = "panelNotificationSortOptions";
            this.panelNotificationSortOptions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelNotificationSortOptions.Size = new System.Drawing.Size(413, 39);
            this.panelNotificationSortOptions.TabIndex = 1;
            // 
            // buttonClearList
            // 
            this.buttonClearList.BackColor = System.Drawing.Color.Transparent;
            this.buttonClearList.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.buttonClearList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.buttonClearList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.buttonClearList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearList.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.buttonClearList.ForeColor = System.Drawing.Color.Black;
            this.buttonClearList.Location = new System.Drawing.Point(9, 6);
            this.buttonClearList.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClearList.Name = "buttonClearList";
            this.buttonClearList.Size = new System.Drawing.Size(100, 28);
            this.buttonClearList.TabIndex = 1;
            this.buttonClearList.Text = "Clear List";
            this.buttonClearList.UseCompatibleTextRendering = true;
            this.buttonClearList.UseVisualStyleBackColor = false;
            this.buttonClearList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonClearList_MouseClick);
            this.buttonClearList.MouseEnter += new System.EventHandler(this.ButtonClearList_MouseEnter);
            this.buttonClearList.MouseLeave += new System.EventHandler(this.ButtonClearList_MouseLeave);
            // 
            // menuItemPlaySound
            // 
            this.menuItemPlaySound.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "PlayPopupSound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemPlaySound.Index = 0;
            this.menuItemPlaySound.Text = "Play Sound";
            this.menuItemPlaySound.Click += new System.EventHandler(this.OnMenuItemPlayPopupSound_Click);
            // 
            // menuItemTurnOnDisplay
            // 
            this.menuItemTurnOnDisplay.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "TurnOnDisplay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemTurnOnDisplay.Index = 1;
            this.menuItemTurnOnDisplay.Text = "Turn On Display";
            this.menuItemTurnOnDisplay.Click += new System.EventHandler(this.OnMenuItemTurnOnDisplay_Click);
            // 
            // menuItemDisableOnFullscreen
            // 
            this.menuItemDisableOnFullscreen.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "DisableOnFullscreen", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemDisableOnFullscreen.Index = 3;
            this.menuItemDisableOnFullscreen.Text = "Disable On Fullscreen";
            this.menuItemDisableOnFullscreen.Click += new System.EventHandler(this.OnMenuItemDisableOnFullscreen_Click);
            // 
            // menuItemDoNotDisturb
            // 
            this.menuItemDoNotDisturb.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "DoNotDisturb", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemDoNotDisturb.Index = 4;
            this.menuItemDoNotDisturb.Text = "Do Not Disturb";
            this.menuItemDoNotDisturb.Click += new System.EventHandler(this.OnMenuItemDoNotDisturb_Click);
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
            // ctxMenuItemPlayPopupSound
            // 
            this.ctxMenuItemPlayPopupSound.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "PlayPopupSound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ctxMenuItemPlayPopupSound.Index = 0;
            this.ctxMenuItemPlayPopupSound.Text = "Play Notification Sound";
            this.ctxMenuItemPlayPopupSound.Click += new System.EventHandler(this.OnMenuItemPlayPopupSound_Click);
            // 
            // ctxMenuItemDoNotDisturb
            // 
            this.ctxMenuItemDoNotDisturb.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.Desktop.Properties.Settings.Default, "DoNotDisturb", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ctxMenuItemDoNotDisturb.Index = 2;
            this.ctxMenuItemDoNotDisturb.Text = "Do Not Disturb";
            this.ctxMenuItemDoNotDisturb.Click += new System.EventHandler(this.OnMenuItemDoNotDisturb_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 153);
            this.Controls.Add(this.panelBackground);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(465, 100000);
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "MainWindow";
            this.panelBackground.ResumeLayout(false);
            this.panelDisplayNotifications.ResumeLayout(false);
            this.panelNotificationSortOptionsBackground.ResumeLayout(false);
            this.panelNotificationSortOptions.ResumeLayout(false);
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
        private Panel panelBackground;
        private MenuItem separator2;
        private Panel panelDisplayNotifications;
        private ElementHost wpfHost;
        private ATG_Notifier.Desktop.WPF.Controls.NotificationListbox notificationListbox1;
        private BindableMenuItem menuItemPlaySound;
        private BindableMenuItem menuItemTurnOnDisplay;
        private MenuItem menuItemSeparator;
        private BindableMenuItem menuItemDisableOnFullscreen;
        private BindableMenuItem menuItemDoNotDisturb;
        private MenuItem menuItemOptions;
        private MenuItem menuItemExit;
        private Panel panelNotificationSortOptionsBackground;
        private Panel panelNotificationSortOptions;
        private Button buttonClearList;
        private MenuItem menuItem_Help;
        private MenuItem menuItem_AboutNotifier;
        private MenuItem menuItem1;
        private MenuItem menuItemNotificationPosition;
        private BindableMenuItem menuItemNotificationPositionTopLeft;
        private BindableMenuItem menuItemNotificationPositionTopRight;
        private BindableMenuItem menuItemNotificationPositionBottomLeft;
        private BindableMenuItem menuItemNotificationPositionBottomRight;

        public Panel Panel2
        {
            get
            {
                return panelNotificationSortOptionsBackground;
            }
        }
    }
}

