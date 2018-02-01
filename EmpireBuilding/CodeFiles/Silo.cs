using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    public class Silo
    {
        private long pvTotalSeedCount;
        public long TSC
        {
            get { return pvTotalSeedCount; }
            set { pvTotalSeedCount = value; }
        }

        private string pvSeedName;
        public string SeedName
        {
            get { return pvSeedName; }
            set { pvSeedName = value; }
        }

        private int pvSeedID;
        public int SeedID
        {
            get { return pvSeedID; }
            set { pvSeedID = value; }
        }

        private int pvSeedCount;
        public int SeedCount
        {
            get { return pvSeedCount; }
            set { pvSeedCount = value; }
        }

        private double pvSeedValue;
        public double SeedValue
        {
            get { return pvSeedValue; }
            set { pvSeedValue = value; }
        }

        private double pvSeedQuality;
        public double SeedQuality
        {
            get { return pvSeedQuality; }
            set { pvSeedQuality = value; }
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

        private string pvPlantingLevelRequired;
        public string PlantingLevelRequired
        {
            get { return pvPlantingLevelRequired; }
            set { pvPlantingLevelRequired = value; }
        }

        private string pvSeedCountinSilo;
        public string SeedCountinSilo
        {
            get { return pvSeedCountinSilo; }
            set { pvSeedCountinSilo = value; }
        }

        private string pvSeedValueFormat;
        public string SeedValueFormat
        {
            get { return pvSeedValueFormat; }
            set { pvSeedValueFormat = value; }
        }

        public string TempSiloString { get; set; }
        public string SiloSellButton { get; set; }

        public Silo(string seedname, double seedvalue, int seedcountinsilo, string seedid, string flowerimage, double seedquality)
        {
            this.SeedID = Convert.ToInt32(seedid);
            this.SeedName = seedname;
            this.SeedValue = seedvalue;
            this.SeedQuality = seedquality;
            this.FlowerImage = flowerimage; // The "Images" part is being added before being sent here.
            this.SeedWorth = (seedvalue * seedcountinsilo).ToString("C");
            this.SeedCount = seedcountinsilo;
            this.SeedCountinSilo = seedcountinsilo.ToString("N0");
            this.SeedValueFormat = seedvalue.ToString("C"); ;
            this.TempSiloString = seedname;
            this.PlantingLevelRequired = "Plant Level Required: " + seedid;
            if (seedvalue > 0) this.SiloSellButton = "Visible";
            else this.SiloSellButton = "Collapsed";
        }

        public Silo() { }

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
