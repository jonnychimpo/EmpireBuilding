using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    public class Granary : INotifyPropertyChanged
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

        private int pvSeedGrowthRate;
        public int SeedGrowthRate
        {
            get { return pvSeedGrowthRate; }
            set { pvSeedGrowthRate = value; }
        }

        private int pvSeedsPlanted;
        public int SeedsPlanted
        {
            get { return pvSeedsPlanted; }
            set { pvSeedsPlanted = value; }
        }

        private int pvSeedPlantBonus;
        public int SeedPlantBonus
        {
            get { return pvSeedPlantBonus; }
            set { pvSeedPlantBonus = value; }
        }

        private int pvSeedPoints;
        public int SeedPoints
        {
            get { return pvSeedPoints; }
            set { pvSeedPoints = value; }
        }

        private string pvSeedCountText;
        public string SeedCountText
        {
            get { return pvSeedCountText; }
            set { pvSeedCountText = value; }
        }

        private double pvSeedValue;
        public double SeedValue
        {
            get { return pvSeedValue; }
            set { pvSeedValue = value; }
        }

        private string pvSeedImage;
        public string SeedImage
        {
            get { return pvSeedImage; }
            set { pvSeedImage = value; }
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

        private string pvS2PSelectedVisible;
        public string S2PSelectedVisible
        {
            get { return pvS2PSelectedVisible; }
            set { pvS2PSelectedVisible = value; NotifyPropertyChanged("S2PSelectedVisible"); }
        }

        private string pvS2PSelectedBackColor;
        public string S2PSelectedBackColor
        {
            get { return pvS2PSelectedBackColor; }
            set { pvS2PSelectedBackColor = value; NotifyPropertyChanged("S2PSelectedBackColor"); }
        }

        private string pvS2PSelectedForeColor;
        public string S2PSelectedForeColor
        {
            get { return pvS2PSelectedForeColor; }
            set { pvS2PSelectedForeColor = value; NotifyPropertyChanged("S2PSelectedForeColor"); }
        }

        private string pvGranaryPointsAvailable;
        public string GranaryPointsAvailable
        {
            get { return pvGranaryPointsAvailable; }
            set { pvGranaryPointsAvailable = value; NotifyPropertyChanged("GranaryPointsAvailable"); }
        }

        private double pvSeedOpacity;
        public double SeedOpacity
        {
            get { return pvSeedOpacity; }
            set { pvSeedOpacity = value; }
        }

        private string pvSelectedColor;
        public string SelectedColor
        {
            get { return pvSelectedColor; }
            set { pvSelectedColor = value; NotifyPropertyChanged("SelectedColor"); }
        }

        public Granary(string seedname, int seedcount, string seedimage, string visible, string plotcolor, string backcolor, int plantlvl, int seedid)
        {
            this.SeedName = seedname;
            this.SeedCount = seedcount;
            this.SeedImage = seedimage;
            this.S2PSelectedVisible = visible;
            this.S2PSelectedForeColor = plotcolor;
            this.S2PSelectedBackColor = backcolor;
            this.PlantingLevelRequired = plantlvl.ToString();
            this.SeedID = seedid;

            if (seedname == "??????")
            {
                this.SeedCountText = string.Empty;
                this.SeedOpacity = 1.0;
            }
            else if (seedcount == 0) this.SeedOpacity = 0.5;
            else
            {
                this.SeedOpacity = 1.0;
                this.SeedCountText = "Count: " + SeedCount;
            }
        }

        public Granary(string seedname, int seedcount, string seedimage, string pointsavail, int seedid, int seedpoints)
        {
            this.SeedName = seedname;
            this.SeedCount = seedcount;
            this.SeedImage = seedimage;
            this.GranaryPointsAvailable = pointsavail;
            this.SeedID = seedid;
            this.SeedPoints = seedpoints;
        }

        public Granary() { }

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
