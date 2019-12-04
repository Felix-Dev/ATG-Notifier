using System.ComponentModel;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.WinForms
{
    internal class BindableToolStripMenuItem : ToolStripMenuItem, IBindableComponent
    {
        public BindableToolStripMenuItem() : base()
        {
            this.BindingContext = new BindingContext();
            this.DataBindings = new ControlBindingsCollection(this);
        }

        [Browsable(false)]
        public BindingContext BindingContext { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Data")]
        public ControlBindingsCollection DataBindings { get; }
    }
}
