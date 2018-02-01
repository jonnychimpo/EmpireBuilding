using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    public class Machine : INotifyPropertyChanged
    {
        private string pvName;
        public string Name
        {
            get { return pvName; }
            set { pvName = value; }
        }

        private string pvTitle;
        public string Title
        {
            get { return pvTitle; }
            set { pvTitle = value; }
        }

        private int pvID;
        public int ID
        {
            get { return pvID; }
            set { pvID = value; }
        }

        private int pvLevel;
        public int Level
        {
            get { return pvLevel; }
            set { pvLevel = value; }
        }

        private int pvTotalEXP;
        public int TotalEXP
        {
            get { return pvTotalEXP; }
            set { pvTotalEXP = value; }
        }

        private int pvUseCount;
        public int UseCount
        {
            get { return pvUseCount; }
            set { pvUseCount = value; }
        }

        private int pvAvailable;
        public int Available
        {
            get { return pvAvailable; }
            set { pvAvailable = value; }
        }

        private int pvExecuteTime;
        public int ExecuteTime
        {
            get { return pvExecuteTime; }
            set { pvExecuteTime = value; }
        }

        private int pvPrice;
        public int Price
        {
            get { return pvPrice; }
            set { pvPrice = value; }
        }

        private string pvImage;
        public string Image
        {
            get { return pvImage; }
            set { pvImage = value; }
        }

        private int pvSpeed;
        public int Speed
        {
            get { return pvSpeed; }
            set { pvSpeed = value; }
        }

        private int pvEfficiency;
        public int Efficiency
        {
            get { return pvEfficiency; }
            set { pvEfficiency = value; }
        }

        private int pvEquipment;
        public int Equipment
        {
            get { return pvEquipment; }
            set { pvEquipment = value; }
        }

        public Machine() { }

        public Machine(string name, string title, int id, int usecount, string image, int time, int price, int level)
        {
            this.Name = name;
            this.Title = title;
            this.ID = id;
            this.UseCount = usecount;
            this.Image = image;
            this.ExecuteTime = time;
            this.Price = price;
            this.Level = level;
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
