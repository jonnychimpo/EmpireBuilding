using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    class Mill : INotifyPropertyChanged
    {
        private int pvSeedID;
        public int SeedID
        {
            get { return pvSeedID; }
            set { pvSeedID = value; }
        }

        private string pvSeedName;
        public string SeedName
        {
            get { return pvSeedName; }
            set { pvSeedName = value; }
        }

        private int pvSeedCount;
        public int SeedCount
        {
            get { return pvSeedCount; }
            set { pvSeedCount = value; }
        }

        private int pvCropCount;
        public int CropCount
        {
            get { return pvCropCount; }
            set { pvCropCount = value; }
        }

        private double pvSeedValue;
        public double SeedValue
        {
            get { return pvSeedValue; }
            set { pvSeedValue = value; }
        }

        private string pvFlowerImage;
        public string FlowerImage
        {
            get { return pvFlowerImage; }
            set { pvFlowerImage = value; }
        }

        private string pvSeedWorth;
        public string SeedWorth
        {
            get { return pvSeedWorth; }
            set { pvSeedWorth = value; }
        }

        private string pvMillingLevelRequired;
        public string MillingLevelRequired
        {
            get { return pvMillingLevelRequired; }
            set { pvMillingLevelRequired = value; }
        }

        private int pvHarvest2Seed;
        public int Harvest2Seed
        {
            get { return pvHarvest2Seed; }
            set { pvHarvest2Seed = value; }
        }

        private int pvHarvest2Send2Refinery;
        public int Harvest2Send2Refinery
        {
            get { return pvHarvest2Send2Refinery; }
            set { pvHarvest2Send2Refinery = value; }
        }

        private string pvCropCountFormat;
        public string CropCountFormat
        {
            get { return pvCropCountFormat; }
            set { pvCropCountFormat = value; }
        }

        public Mill(int seedid, string seedname, int seedcountinsilo, string flowerimage, int harvest2seed, int seedcount)
        {
            this.SeedID = seedid;
            this.SeedName = seedname;
            this.FlowerImage = flowerimage;
            this.CropCount = seedcountinsilo;
            this.CropCountFormat = seedcountinsilo.ToString("N0");
            this.Harvest2Seed = harvest2seed;
            this.SeedCount = seedcount;
        }

        public Mill() { }

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
