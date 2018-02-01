using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EmpireBuilding.CodeFiles
{
    public class Update : INotifyPropertyChanged
    {
        private string pvMessage;
        public string Message
        {
            get { return pvMessage; }
            set { pvMessage = value; }
        }

        private Brush pvMessageForeground;
        public Brush MessageForeground
        {
            get { return pvMessageForeground; }
            set { pvMessageForeground = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
