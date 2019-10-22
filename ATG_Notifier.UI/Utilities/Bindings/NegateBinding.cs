using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATG_Notifier.Utilities.Bindings
{
    public class NegateBinding
    {
        string propertyName;
        object dataSource;
        string dataMember;

        public NegateBinding(string propertyName, object dataSource, string dataMember)
        {
            this.propertyName = propertyName;
            this.dataSource = dataSource;
            this.dataMember = dataMember;
        }

        public static implicit operator Binding(NegateBinding eb)
        {
            var binding = new Binding(eb.propertyName, eb.dataSource, eb.dataMember, false, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Negate);
            binding.Format += new ConvertEventHandler(Negate);
            return binding;
        }

        static void Negate(object sender, ConvertEventArgs e)
        {
            e.Value = !((bool)e.Value);
        }
    }
}
