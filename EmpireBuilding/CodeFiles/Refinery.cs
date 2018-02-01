using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    public class Refinery : INotifyPropertyChanged
    {
        private int pvMaxmG;
        public int MaxmG
        {
            get { return pvMaxmG; }
            set { pvMaxmG = value; NotifyPropertyChanged("MaxmG"); }
        }

        private int pvmG;
        public int mG
        {
            get { return pvmG; }
            set { pvmG = value; NotifyPropertyChanged("mG"); }
        }

        private string pvmGDisplay;
        public string mGDisplay
        {
            get { return pvmGDisplay; }
            set { pvmGDisplay = value; NotifyPropertyChanged("mGDisplay"); }
        }

        private string pvRefineryStatus;
        public string RefineryStatus
        {
            get { return pvRefineryStatus; }
            set { pvRefineryStatus = value; NotifyPropertyChanged("RefineryStatus"); }
        }

        private int pvUsedmG;
        public int UsedmG
        {
            get { return pvUsedmG; }
            set { pvUsedmG = value; NotifyPropertyChanged("UsedmG"); }
        }

        private int pvIsRefineryMaxed;
        public int IsRefineryMaxed
        {
            get { return pvIsRefineryMaxed; }
            set { pvIsRefineryMaxed = value; NotifyPropertyChanged("IsRefineryMaxed"); }
        }

        private int pvCropsinthisBatch;
        public int CropsinthisBatch
        {
            get { return pvCropsinthisBatch; }
            set { pvCropsinthisBatch = value; NotifyPropertyChanged("CropsinthisBatch"); }
        }

        private string pvSecsPerBatch;
        public string SecsPerBatch
        {
            get { return pvSecsPerBatch; }
            set { pvSecsPerBatch = value; NotifyPropertyChanged("SecsPerBatch"); }
        }

        private string pvSecsPerBatchLeft;
        public string SecsPerBatchLeft
        {
            get { return pvSecsPerBatchLeft; }
            set { pvSecsPerBatchLeft = value; NotifyPropertyChanged("SecsPerBatchLeft"); }
        }

        private int pvRefinedmG;
        public int RefinedmG
        {
            get { return pvRefinedmG; }
            set { pvRefinedmG = value; NotifyPropertyChanged("RefinedmG"); }
        }

        private int pvRefineRate;
        public int RefineRate
        {
            get { return pvRefineRate; }
            set { pvRefineRate = value; NotifyPropertyChanged("RefineRate"); }
        }

        private int pvCropinRefinery;
        public int CropinRefinery
        {
            get { return pvCropinRefinery; }
            set { pvCropinRefinery = value; NotifyPropertyChanged("CropinRefinery"); }
        }

        private int pvTotalCropRefined;
        public int TotalCropRefined
        {
            get { return pvTotalCropRefined; }
            set { pvTotalCropRefined = value; }
        }

        private int pvRefineRateLvl;
        public int RefineRateLvl
        {
            get { return pvRefineRateLvl; }
            set { pvRefineRateLvl = value; NotifyPropertyChanged("RefineRateLvl"); }
        }

        private int pvMaxmGLvl;
        public int MaxmGLvl
        {
            get { return pvMaxmGLvl; }
            set { pvMaxmGLvl = value; NotifyPropertyChanged("MaxmGLvl"); }
        }

        private int pvCropPermGLvl;
        public int CropPermGLvl
        {
            get { return pvCropPermGLvl; }
            set { pvCropPermGLvl = value; NotifyPropertyChanged("CropPermGLvl"); }
        }

        private string pvRefineRateFCC;
        public string RefineRateFCC // RefineRate FreeCurrency Cost
        {
            get { return pvRefineRateFCC; }
            set { pvRefineRateFCC = value; NotifyPropertyChanged("RefineRateFCC"); }
        }

        private string pvMaxmGFCC;
        public string MaxmGFCC // MaxmG FreeCurrency Cost
        {
            get { return pvMaxmGFCC; }
            set { pvMaxmGFCC = value; NotifyPropertyChanged("MaxmGFCC"); }
        }

        private string pvCropPermGFCC;
        public string CropPermGFCC // CropPermG FreeCurrency Cost
        {
            get { return pvCropPermGFCC; }
            set { pvCropPermGFCC = value; NotifyPropertyChanged("CropPermGFCC"); }
        }

        private string pvRefineRateNextLvlDisplay;
        public string RefineRateNextLvlDisplay // RefineRate Next Level Display Text
        {
            get { return pvRefineRateNextLvlDisplay; }
            set { pvRefineRateNextLvlDisplay = value; NotifyPropertyChanged("RefineRateNextLvlDisplay"); }
        }

        private string pvMaxmGNextLvlDisplay;
        public string MaxmGNextLvlDisplay // MaxmG Next Level Display Text
        {
            get { return pvMaxmGNextLvlDisplay; }
            set { pvMaxmGNextLvlDisplay = value; NotifyPropertyChanged("MaxmGNextLvlDisplay"); }
        }

        private string pvCropPermGNextLvlDisplay;
        public string CropPermGNextLvlDisplay // CropPermG Next Level Display Text
        {
            get { return pvCropPermGNextLvlDisplay; }
            set { pvCropPermGNextLvlDisplay = value; NotifyPropertyChanged("CropPermGNextLvlDisplay"); }
        }

        //private int[] pvCropRefinedArray;
        //public int[] CropRefinedArray
        //{
        //    get { return pvCropRefinedArray; }
        //    set { pvCropRefinedArray = value; }
        //}

        private int pvCropPermG;
        public int CropPermG
        {
            get { return pvCropPermG; }
            set { pvCropPermG = value; NotifyPropertyChanged("CropPermG"); }
        }

        private int pvOutputmG;
        public int OutputmG
        {
            get { return pvOutputmG; }
            set { pvOutputmG = value; NotifyPropertyChanged("OutputmG"); }
        }

        private int pvCropPerSec;
        public int CropPerSec
        {
            get { return pvCropPerSec; }
            set { pvCropPerSec = value; NotifyPropertyChanged("CropPerSec"); }
        }

        private string pvCropPerSecDisplay;
        public string CropPerSecDisplay
        {
            get { return pvCropPerSecDisplay; }
            set { pvCropPerSecDisplay = value; }
        }

        private DateTime pvRefineStartDate;
        public DateTime RefineStartDate
        {
            get { return pvRefineStartDate; }
            set { pvRefineStartDate = value; }
        }

        private DateTime pvRefineCompleteDate;
        public DateTime RefineCompleteDate
        {
            get { return pvRefineCompleteDate; }
            set { pvRefineCompleteDate = value; }
        }

        private DateTime pvLastRefineDate;
        public DateTime LastRefineDate
        {
            get { return pvLastRefineDate; }
            set { pvLastRefineDate = value; NotifyPropertyChanged("LastRefineDate"); }
        }

        private int pvRefineComplete;
        public int RefineComplete
        {
            get { return pvRefineComplete; }
            set { pvRefineComplete = value; }
        }

        private int pvLevel;
        public int Level
        {
            get { return pvLevel; }
            set { pvLevel = value; }
        }

        private string pvUpgradeBonusDesc;
        public string UpgradeBonusDesc
        {
            get { return pvUpgradeBonusDesc; }
            set { pvUpgradeBonusDesc = value; }
        }

        private int pvUpgradeTotalBonus;
        public int UpgradeTotalBonus
        {
            get { return pvUpgradeTotalBonus; }
            set { pvUpgradeTotalBonus = value; }
        }

        private string[] pvUpgradeBonusProperty;
        public string[] UpgradeBonusProperty
        {
            get { return pvUpgradeBonusProperty; }
            set { pvUpgradeBonusProperty = value; }
        }

        private int[] pvUpgradeBonusValue;
        public int[] UpgradeBonusValue
        {
            get { return pvUpgradeBonusValue; }
            set { pvUpgradeBonusValue = value; }
        }

        private int pvUpgradeBonusCost;
        public int UpgradeBonusCost
        {
            get { return pvUpgradeBonusCost; }
            set { pvUpgradeBonusCost = value; }
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
