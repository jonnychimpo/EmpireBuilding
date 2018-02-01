using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    public class GameUI : INotifyPropertyChanged
    {

        private string pvPlotCanvasVisible;
        public string PlotCanvasVisible
        {
            get { return pvPlotCanvasVisible; }
            set { pvPlotCanvasVisible = value; NotifyPropertyChanged("PlotCanvasVisible"); }
        }

        private string pvSectorDisplayName;
        public string SectorDisplayName
        {
            get { return pvSectorDisplayName; }
            set { pvSectorDisplayName = value; NotifyPropertyChanged("SectorDisplayName"); }
        }

        public GameUI() { }

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
