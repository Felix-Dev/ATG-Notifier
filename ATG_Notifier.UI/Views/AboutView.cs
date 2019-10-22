using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATG_Notifier.UI.Views
{
    public partial class AboutView : Form
    {
        public AboutView()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            this.label_Version.Text = $"Version {FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion}";

            this.label_Version.Left = (this.Width - this.label_Version.Width) / 2;
            this.button_OK.Left = (this.Width - this.button_OK.Width) / 2;
        }
    }
}
