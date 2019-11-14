using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Utilities.Bindings
{
    public class BindableMenuItem : MenuItem, IBindableComponent
    {
        #region IBindableComponent Members

        private BindingContext bindingContext;
        private ControlBindingsCollection dataBindings;

        [Browsable(false)]
        public BindingContext BindingContext
        {
            get => bindingContext;
            set => bindingContext = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Data")]
        public ControlBindingsCollection DataBindings => dataBindings;

        #endregion

        public BindableMenuItem() : base()
        {
            bindingContext = new BindingContext();
            dataBindings = new ControlBindingsCollection(this);
        }        
    }
}
