using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ATG_Notifier.Data.Entities
{
    [Table("DbVersion")]
    public class DbVersion : INotifyPropertyChanged
    {
        [Key]
        public string Version { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
