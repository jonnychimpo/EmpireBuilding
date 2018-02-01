using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    class ViewModel : INotifyPropertyChanged
    {
        public string Title
        {
            get;
            set;
        }

        public string ImagePath
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string ButtonWords
        {
            get;
            set;
        }

        public string TBVisible
        {
            get;
            set;
        }

        public string CurrentSeedName
        {
            get;
            set;
        }

        public string CurrentSeedCountLine
        {
            get;
            set;
        }

        public string CurrentSeedWorthLine
        {
            get;
            set;
        }

        public string CurrentSeedTotal
        {
            get;
            set;
        }

        public string CurrentSeedImage
        {
            get;
            set;
        }

        public string ReqPlantingLevel
        {
            get;
            set;
        }

        private string pvSeedGrowthRate;
        public string SeedGrowthRate
        {
            get { return pvSeedGrowthRate; }
            set { pvSeedGrowthRate = value; NotifyPropertyChanged("SeedGrowthRate"); }
        }

        private string pvSeedGrowthRateMOD;
        public string SeedGrowthRateMOD
        {
            get { return pvSeedGrowthRateMOD; }
            set { pvSeedGrowthRateMOD = value; NotifyPropertyChanged("SeedGrowthRateMOD"); }
        }

        private string pvSeedValue;
        public string SeedValue
        {
            get { return pvSeedValue; }
            set { pvSeedValue = value; NotifyPropertyChanged("SeedValue"); }
        }

        private string pvSeedValueMOD;
        public string SeedValueMOD
        {
            get { return pvSeedValueMOD; }
            set { pvSeedValueMOD = value; NotifyPropertyChanged("SeedValueMOD"); }
        }

        private string pvSeedQuality;
        public string SeedQuality
        {
            get { return pvSeedQuality; }
            set { pvSeedQuality = value; NotifyPropertyChanged("SeedQuality"); }
        }

        private string pvSeedQualityMOD;
        public string SeedQualityMOD
        {
            get { return pvSeedQualityMOD; }
            set { pvSeedQualityMOD = value; NotifyPropertyChanged("SeedQualityMOD"); }
        }

        public int SeedID { get; set; }
        public string SeedIDFormatted { get; set; }
        public string SeedCount { get; set; }
        public string SeedPlantCount { get; set; }
        public string SeedHarvestCount { get; set; }

        private string pvValueForeColor;
        public string ValueForeColor
        {
            get { return pvValueForeColor; }
            set { pvValueForeColor = value; NotifyPropertyChanged("ValueForeColor"); }
        }

        private string pvPoints;
        public string Points
        {
            get { return pvPoints; }
            set { pvPoints = value; NotifyPropertyChanged("Points"); }
        }

        private string pvPointsSVMODString;
        public string PointsSVMODString
        {
            get { return pvPointsSVMODString; }
            set { pvPointsSVMODString = value; NotifyPropertyChanged("PointsSVMODString"); }
        }

        private string pvPointsSGRMODString;
        public string PointsSGRMODString
        {
            get { return pvPointsSGRMODString; }
            set { pvPointsSGRMODString = value; NotifyPropertyChanged("PointsSGRMODString"); }
        }

        private string pvPointsSQMODString;
        public string PointsSQMODString
        {
            get { return pvPointsSQMODString; }
            set { pvPointsSQMODString = value; NotifyPropertyChanged("PointsSQMODString"); }
        }

        private string pvPointsSFMODString;
        public string PointsSFMODString
        {
            get { return pvPointsSFMODString; }
            set { pvPointsSFMODString = value; NotifyPropertyChanged("PointsSFMODString"); }
        }

        private string pvPointsSVMODForeColor;
        public string PointsSVMODForeColor
        {
            get { return pvPointsSVMODForeColor; }
            set { pvPointsSVMODForeColor = value; NotifyPropertyChanged("PointsSVMODForeColor"); }
        }

        private string pvPointsSGRMODForeColor;
        public string PointsSGRMODForeColor
        {
            get { return pvPointsSGRMODForeColor; }
            set { pvPointsSGRMODForeColor = value; NotifyPropertyChanged("PointsSGRMODForeColor"); }
        }

        private string pvPointsSQMODForeColor;
        public string PointsSQMODForeColor
        {
            get { return pvPointsSQMODForeColor; }
            set { pvPointsSQMODForeColor = value; NotifyPropertyChanged("PointsSQMODForeColor"); }
        }

        private string pvPointsSFMODForeColor;
        public string PointsSFMODForeColor
        {
            get { return pvPointsSFMODForeColor; }
            set { pvPointsSFMODForeColor = value; NotifyPropertyChanged("PointsSFMODForeColor"); }
        }

        private int pvPointsSVMODNeeded;
        public int PointsSVMODNeeded
        {
            get { return pvPointsSVMODNeeded; }
            set { pvPointsSVMODNeeded = value; NotifyPropertyChanged("PointsSVMODNeeded"); }
        }

        private int pvPointsSGRMODNeeded;
        public int PointsSGRMODNeeded
        {
            get { return pvPointsSGRMODNeeded; }
            set { pvPointsSGRMODNeeded = value; NotifyPropertyChanged("PointsSGRMODNeeded"); }
        }

        private int pvPointsSQMODNeeded;
        public int PointsSQMODNeeded
        {
            get { return pvPointsSQMODNeeded; }
            set { pvPointsSQMODNeeded = value; NotifyPropertyChanged("PointsSQMODNeeded"); }
        }

        private int pvPointsSFMODNeeded;
        public int PointsSFMODNeeded
        {
            get { return pvPointsSFMODNeeded; }
            set { pvPointsSFMODNeeded = value; NotifyPropertyChanged("PointsSFMODNeeded"); }
        }

        private string pvPointsSVMODShow;
        public string PointsSVMODShow
        {
            get { return pvPointsSVMODShow; }
            set { pvPointsSVMODShow = value; NotifyPropertyChanged("PointsSVMODShow"); }
        }

        private string pvPointsSGRMODShow;
        public string PointsSGRMODShow
        {
            get { return pvPointsSGRMODShow; }
            set { pvPointsSGRMODShow = value; NotifyPropertyChanged("PointsSGRMODShow"); }
        }

        private string pvPointsSQMODShow;
        public string PointsSQMODShow
        {
            get { return pvPointsSQMODShow; }
            set { pvPointsSQMODShow = value; NotifyPropertyChanged("PointsSQMODShow"); }
        }

        private string pvPointsSFMODShow;
        public string PointsSFMODShow
        {
            get { return pvPointsSFMODShow; }
            set { pvPointsSFMODShow = value; NotifyPropertyChanged("PointsSFMODShow"); }
        }

        private string pvRarity;
        public string Rarity
        {
            get { return pvRarity; }
            set { pvRarity = value; NotifyPropertyChanged("Rarity"); }
        }

        private string pvFortitude;
        public string Fortitude
        {
            get { return pvFortitude; }
            set { pvFortitude = value; NotifyPropertyChanged("Fortitude"); }
        }

        private string pvFortitudeMOD;
        public string FortitudeMOD
        {
            get { return pvFortitudeMOD; }
            set { pvFortitudeMOD = value; NotifyPropertyChanged("FortitudeMOD"); }
        }

        private string pvRarityForeColor;
        public string RarityForeColor
        {
            get { return pvRarityForeColor; }
            set { pvRarityForeColor = value; NotifyPropertyChanged("RarityForeColor"); }
        }

        private string pvPlantXP;
        public string PlantXP
        {
            get { return pvPlantXP; }
            set { pvPlantXP = value; NotifyPropertyChanged("PlantXP"); }
        }

        private string pvPlantSize;
        public string PlantSize
        {
            get { return pvPlantSize; }
            set { pvPlantSize = value; NotifyPropertyChanged("PlantSize"); }
        }

        private string pvPlantCount;
        public string PlantCount
        {
            get { return pvPlantCount; }
            set { pvPlantCount = value; NotifyPropertyChanged("PlantCount"); }
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
