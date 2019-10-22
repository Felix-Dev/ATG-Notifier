using ATG_Notifier.Utilities.Bindings;
using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace ATG_Notifier.UI
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.ctxMenuItemPlayPopupSound = new ATG_Notifier.Utilities.Bindings.BindableMenuItem();
            this.separator1 = new System.Windows.Forms.MenuItem();
            this.ctxMenuItemDoNotDisturb = new ATG_Notifier.Utilities.Bindings.BindableMenuItem();
            this.separator2 = new System.Windows.Forms.MenuItem();
            this.ctxMenuItemExit = new System.Windows.Forms.MenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemNotificationOptions = new System.Windows.Forms.MenuItem();
            this.menuItemPlaySound = new ATG_Notifier.Utilities.Bindings.BindableMenuItem();
            this.menuItemTurnOnDisplay = new ATG_Notifier.Utilities.Bindings.BindableMenuItem();
            this.menuItemSeparator = new System.Windows.Forms.MenuItem();
            this.menuItemDisableOnFullscreen = new ATG_Notifier.Utilities.Bindings.BindableMenuItem();
            this.menuItemDoNotDisturb = new ATG_Notifier.Utilities.Bindings.BindableMenuItem();
            this.panelBackground = new System.Windows.Forms.Panel();
            this.panelDisplayNotifications = new System.Windows.Forms.Panel();
            this.wpfHost = new System.Windows.Forms.Integration.ElementHost();
            this.labelNoNotifications = new System.Windows.Forms.Label();
            this.panelNotificationSortOptionsBackground = new System.Windows.Forms.Panel();
            this.panelNotificationSortOptions = new System.Windows.Forms.Panel();
            this.buttonClearList = new System.Windows.Forms.Button();
            this.menuItem_Help = new System.Windows.Forms.MenuItem();
            this.menuItem_AboutNotifier = new System.Windows.Forms.MenuItem();
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
            // ctxMenuItemPlayPopupSound
            // 
            this.ctxMenuItemPlayPopupSound.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.UI.Properties.Settings.Default, "PlayPopupSound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
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
            this.ctxMenuItemDoNotDisturb.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.UI.Properties.Settings.Default, "DoNotDisturb", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
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
            this.menuItemTurnOnDisplay,
            this.menuItemSeparator,
            this.menuItemDisableOnFullscreen,
            this.menuItemDoNotDisturb});
            this.menuItemNotificationOptions.Text = "Notifications";
            // 
            // menuItemPlaySound
            // 
            this.menuItemPlaySound.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.UI.Properties.Settings.Default, "PlayPopupSound", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemPlaySound.Index = 0;
            this.menuItemPlaySound.Text = "Play Sound";
            this.menuItemPlaySound.Click += new System.EventHandler(this.OnMenuItemPlayPopupSound_Click);
            // 
            // menuItemTurnOnDisplay
            // 
            this.menuItemTurnOnDisplay.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.UI.Properties.Settings.Default, "TurnOnDisplay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemTurnOnDisplay.Index = 1;
            this.menuItemTurnOnDisplay.Text = "Turn On Display";
            this.menuItemTurnOnDisplay.Click += new System.EventHandler(this.OnMenuItemTurnOnDisplay_Click);
            // 
            // menuItemSeparator
            // 
            this.menuItemSeparator.Index = 2;
            this.menuItemSeparator.Text = "-";
            // 
            // menuItemDisableOnFullscreen
            // 
            this.menuItemDisableOnFullscreen.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.UI.Properties.Settings.Default, "DisableOnFullscreen", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemDisableOnFullscreen.Index = 3;
            this.menuItemDisableOnFullscreen.Text = "Disable On Fullscreen";
            this.menuItemDisableOnFullscreen.Click += new System.EventHandler(this.OnMenuItemDisableOnFullscreen_Click);
            // 
            // menuItemDoNotDisturb
            // 
            this.menuItemDoNotDisturb.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ATG_Notifier.UI.Properties.Settings.Default, "DoNotDisturb", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.menuItemDoNotDisturb.Index = 4;
            this.menuItemDoNotDisturb.Text = "Do Not Disturb";
            this.menuItemDoNotDisturb.Click += new System.EventHandler(this.OnMenuItemDoNotDisturb_Click);
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
            this.panelBackground.Size = new System.Drawing.Size(449, 198);
            this.panelBackground.TabIndex = 2;
            // 
            // panelDisplayNotifications
            // 
            this.panelDisplayNotifications.AutoScroll = true;
            this.panelDisplayNotifications.Controls.Add(this.wpfHost);
            this.panelDisplayNotifications.Controls.Add(this.labelNoNotifications);
            this.panelDisplayNotifications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDisplayNotifications.Location = new System.Drawing.Point(0, 41);
            this.panelDisplayNotifications.Margin = new System.Windows.Forms.Padding(0);
            this.panelDisplayNotifications.Name = "panelDisplayNotifications";
            this.panelDisplayNotifications.Size = new System.Drawing.Size(449, 157);
            this.panelDisplayNotifications.TabIndex = 1;
            // 
            // wpfHost
            // 
            this.wpfHost.Dock = System.Windows.Forms.DockStyle.Left;
            this.wpfHost.Location = new System.Drawing.Point(0, 0);
            this.wpfHost.Margin = new System.Windows.Forms.Padding(0);
            this.wpfHost.Name = "wpfHost";
            this.wpfHost.Size = new System.Drawing.Size(449, 157);
            this.wpfHost.TabIndex = 2;
            this.wpfHost.TabStop = false;
            this.wpfHost.Child = null;
            // 
            // labelNoNotifications
            // 
            this.labelNoNotifications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNoNotifications.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNoNotifications.Location = new System.Drawing.Point(0, 0);
            this.labelNoNotifications.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNoNotifications.Name = "labelNoNotifications";
            this.labelNoNotifications.Size = new System.Drawing.Size(449, 157);
            this.labelNoNotifications.TabIndex = 0;
            this.labelNoNotifications.Text = "No new updates";
            this.labelNoNotifications.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelNoNotifications.Visible = false;
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
            this.buttonClearList.TabStop = false;
            this.buttonClearList.Text = "Clear List";
            this.buttonClearList.UseCompatibleTextRendering = true;
            this.buttonClearList.UseVisualStyleBackColor = false;
            this.buttonClearList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ButtonClearList_MouseClick);
            this.buttonClearList.MouseEnter += new System.EventHandler(this.ButtonClearList_MouseEnter);
            this.buttonClearList.MouseLeave += new System.EventHandler(this.ButtonClearList_MouseLeave);
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 198);
            this.Controls.Add(this.panelBackground);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(465, 235);
            this.Name = "MainWindow";
            this.Text = "ATG-Notifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.panelBackground.ResumeLayout(false);
            this.panelDisplayNotifications.ResumeLayout(false);
            this.panelNotificationSortOptionsBackground.ResumeLayout(false);
            this.panelNotificationSortOptions.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private Label labelNoNotifications;
        private ElementHost wpfHost;
        private ATG_Notifier.WPF.NotificationListbox notificationListbox1;
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

        public Panel Panel2
        {
            get
            {
                return panelNotificationSortOptionsBackground;
            }
        }
    }
}

