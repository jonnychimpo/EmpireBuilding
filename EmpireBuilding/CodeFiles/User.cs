using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmpireBuilding.CodeFiles
{
    class User : INotifyPropertyChanged
    {
        private string pvUserName;
        public string UserName
        {
            get { return pvUserName; }
            set { pvUserName = value; }
        }

        private int pvUserID;
        public int UserID
        {
            get { return pvUserID; }
            set { pvUserID = value; }
        }

        private double pvFreeCurrency;
        public double FreeCurrency
        {
            get { return pvFreeCurrency; }
            set { pvFreeCurrency = value; NotifyPropertyChanged("FreeCurrency"); }
        }

        private string pvFreeCurrencyText;
        public string FreeCurrencyText
        {
            get { return pvFreeCurrencyText; }
            set { pvFreeCurrencyText = value; }
        }

        private double pvPaidCurrency;
        public double PaidCurrency
        {
            get { return pvPaidCurrency; }
            set { pvPaidCurrency = value; }
        }

        private double pvMoneySpent;
        public double MoneySpent
        {
            get { return pvMoneySpent; }
            set { pvMoneySpent = value; }
        }

        private double pvExplorePrice;
        public double ExplorePrice
        {
            get { return pvExplorePrice; }
            set { pvExplorePrice = value; }
        }

        private int pvCurrentAction;
        public int CurrentAction
        {
            get { return pvCurrentAction; }
            set { pvCurrentAction = value; }
        }

        private int pvExploreTime;
        public int ExploreTime
        {
            get { return pvExploreTime; }
            set { pvExploreTime = value; }
        }

        private int pvBioFuelUsed;
        public int BioFuelUsed
        {
            get { return pvBioFuelUsed; }
            set { pvBioFuelUsed = value; }
        }

        private int pvTimeInGame;
        public int TimeInGame
        {
            get { return pvTimeInGame; }
            set { pvTimeInGame = value; }
        }

        private string pvGlobalBaseLocation;
        public string GlobalBaseLocation
        {
            get { return pvGlobalBaseLocation; }
            set { pvGlobalBaseLocation = value; }
        }

        private int pvHaggleTime;
        public int HaggleTime
        {
            get { return pvHaggleTime; }
            set { pvHaggleTime = value; }
        }

        private int pvMillTime;
        public int MillTime
        {
            get { return pvMillTime; }
            set { pvMillTime = value; }
        }

        private double pvPlantPrice;
        public double PlantPrice
        {
            get { return pvPlantPrice; }
            set { pvPlantPrice = value; }
        }

        private double pvHagglePrice;
        public double HagglePrice
        {
            get { return pvHagglePrice; }
            set { pvHagglePrice = value; }
        }

        private int pvHarvestEXP;
        public int HarvestEXP
        {
            get { return pvHarvestEXP; }
            set { pvHarvestEXP = value; }
        }

        private int pvPlantEXP;
        public int PlantEXP
        {
            get { return pvPlantEXP; }
            set { pvPlantEXP = value; }
        }

        private int pvExploreEXP;
        public int ExploreEXP
        {
            get { return pvExploreEXP; }
            set { pvExploreEXP = value; }
        }

        private int pvHaggleEXP;
        public int HaggleEXP
        {
            get { return pvHaggleEXP; }
            set { pvHaggleEXP = value; }
        }

        private int pvPlowEXP;
        public int PlowEXP
        {
            get { return pvPlowEXP; }
            set { pvPlowEXP = value; }
        }

        private int pvMillEXP;
        public int MillEXP
        {
            get { return pvMillEXP; }
            set { pvMillEXP = value; }
        }

        private int pvHarvestLevel;
        public int HarvestLevel
        {
            get { return pvHarvestLevel; }
            set { pvHarvestLevel = value; }
        }

        private int pvPlantLevel;
        public int PlantLevel
        {
            get { return pvPlantLevel; }
            set { pvPlantLevel = value; }
        }

        private int pvExploreLevel;
        public int ExploreLevel
        {
            get { return pvExploreLevel; }
            set { pvExploreLevel = value; }
        }

        private int pvHaggleLevel;
        public int HaggleLevel
        {
            get { return pvHaggleLevel; }
            set { pvHaggleLevel = value; }
        }

        private int pvPlowLevel;
        public int PlowLevel
        {
            get { return pvPlowLevel; }
            set { pvPlowLevel = value; }
        }

        private int pvMillLevel;
        public int MillLevel
        {
            get { return pvMillLevel; }
            set { pvMillLevel = value; }
        }

        private DateTime pvMillCompleteDate;
        public DateTime MillCompleteDate
        {
            get { return pvMillCompleteDate; }
            set { pvMillCompleteDate = value; }
        }

        private int pvIsMillProcessing;
        public int IsMillProcessing
        {
            get { return pvIsMillProcessing; }
            set { pvIsMillProcessing = value; }
        }

        private int pvMillCurrentSeed;
        public int MillCurrentSeed
        {
            get { return pvMillCurrentSeed; }
            set { pvMillCurrentSeed = value; }
        }

        private int pvMillSeedProcessingCount;
        public int MillSeedProcessingCount
        {
            get { return pvMillSeedProcessingCount; }
            set { pvMillSeedProcessingCount = value; }
        }

        private int pvCurrentExploredPlots;
        public int CurrentExploredPlots
        {
            get { return pvCurrentExploredPlots; }
            set { pvCurrentExploredPlots = value; }
        }

        private int pvPlotsPlowed;
        public int PlotsPlowed
        {
            get { return pvPlotsPlowed; }
            set { pvPlotsPlowed = value; }
        }

        private int pvPlotsPlanted;
        public int PlotsPlanted
        {
            get { return pvPlotsPlanted; }
            set { pvPlotsPlanted = value; }
        }

        private int pvPlotsHarvested;
        public int PlotsHarvested
        {
            get { return pvPlotsHarvested; }
            set { pvPlotsHarvested = value; }
        }

        private int pvSelectedAction;
        public int SelectedAction
        {
            get { return pvSelectedAction; }
            set { pvSelectedAction = value; }
        }

        private int pvHomeTile;
        public int HomeTile
        {
            get { return pvHomeTile; }
            set { pvHomeTile = value; }
        }

        private int pvHomeSector;
        public int HomeSector
        {
            get { return pvHomeSector; }
            set { pvHomeSector = value; }
        }

        private int pvHomeTileNumber;
        public int HomeTileNumber
        {
            get { return pvHomeTileNumber; }
            set { pvHomeTileNumber = value; }
        }

        private string pvDeviceID;
        public string DeviceID
        {
            get { return pvDeviceID; }
            set { pvDeviceID = value; }
        }
        public User DefaultUser(string userxml)
        {
            User tempUser = new User();
            TextReader tr = new StringReader(userxml);
            XDocument xdoc = XDocument.Load(tr);

            tempUser.UserID = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("ID").Value);
            tempUser.UserName = xdoc.Element("UserInfo").Element("User").Element("Name").Value;
            tempUser.FreeCurrency = Convert.ToDouble(xdoc.Element("UserInfo").Element("User").Element("FreeCurrency").Value);
            tempUser.PaidCurrency = Convert.ToDouble(xdoc.Element("UserInfo").Element("User").Element("PaidCurrency").Value);
            tempUser.CurrentExploredPlots = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("CurrentExploredPlots").Value);
            tempUser.MoneySpent = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("MoneySpent").Value);
            tempUser.BioFuelUsed = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("BioFuelUsed").Value);
            tempUser.TimeInGame = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("TimeInGame").Value);
            tempUser.MillEXP = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("MillEXP").Value);
            tempUser.MillTime = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("MillTime").Value);
            tempUser.MillLevel = 1;
            tempUser.IsMillProcessing = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("MillProcessing").Value);
            string tempMCS = xdoc.Element("UserInfo").Element("User").Element("MillCurrentSeed").Value;
            if (tempMCS != "") tempUser.MillCurrentSeed = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("MillCurrentSeed").Value);
            string tempMCD = xdoc.Element("UserInfo").Element("User").Element("MillCompleteDate").Value;
            if (tempMCD != "") tempUser.MillCompleteDate = Convert.ToDateTime(xdoc.Element("UserInfo").Element("User").Element("MillCompleteDate").Value);
            tempUser.MillSeedProcessingCount = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("MillSeedProcessingCount").Value);
            tempUser.GlobalBaseLocation = xdoc.Element("UserInfo").Element("User").Element("GlobalBaseLocation").Value;
            tempUser.HomeTile = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("HomeTile").Value);
            tempUser.HomeSector = Convert.ToInt32(xdoc.Element("UserInfo").Element("User").Element("HomeSector").Value);

            return tempUser;
        }

        public void SetPrice(int skill)
        {
            // Skill
            // 1 - Harvest
            // 2 - Plant
            // 3 - Explore
            double defaultExplorePrice = 10000;

            //if (skill == 1) this.HarvestLevel = i - 1;
            //if (skill == 2) this.PlantLevel = i - 1;
            if (skill == 3) this.ExplorePrice = defaultExplorePrice * this.CurrentExploredPlots;
        }

        public long GetNextLevelXP(int skill, int CurLvl)
        {
            long tempNextLevelXP = 0;

            // Skill
            // 1 - Harvest
            // 2 - Plant
            // 3 - Explore
            // 4 - Plow
            // 5 - Mill

            double levelmultiplier = 1.25;
            long[] arrLevels = new long[100];
            arrLevels[0] = 0;
            int tLevel1 = 60;
            int tLevel2 = 400;

            if (skill == 1)
            {
                arrLevels[1] = 1000;
                arrLevels[2] = 100000;
            }
            else if (skill == 2)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 3)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 4)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 5)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }

            for (int i = 3; i < 100; i++)
            {
                arrLevels[i] = Convert.ToInt64(arrLevels[i - 1] + (arrLevels[i - 1] - arrLevels[i - 2]) * levelmultiplier);
            }

            tempNextLevelXP = arrLevels[CurLvl];

            return tempNextLevelXP;
        }


        public void GetLevel(int skill, int curEXP)
        {
            // Skill
            // 1 - Harvest
            // 2 - Plant
            // 3 - Explore
            // 4 - Plow
            // 5 - Mill

            double levelmultiplier = 1.25;
            long[] arrLevels = new long[100];
            arrLevels[0] = 0;
            int tLevel1 = 60;
            int tLevel2 = 400;

            if (skill == 1)
            {
                arrLevels[1] = 1000;
                arrLevels[2] = 100000;
            }
            else if (skill == 2)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 3)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 4)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }
            else if (skill == 5)
            {
                arrLevels[1] = tLevel1;
                arrLevels[2] = tLevel2;
            }

            for (int i = 3; i < 100; i++)
            {
                arrLevels[i] = Convert.ToInt64(arrLevels[i - 1] + (arrLevels[i - 1] - arrLevels[i - 2]) * levelmultiplier);
            }

            for (int i = 0; i < 100; i++)
            {
                if (curEXP < arrLevels[i])
                {
                    if (skill == 1) this.HarvestLevel = i;
                    if (skill == 2) this.PlantLevel = i;
                    if (skill == 3) this.ExploreLevel = i;
                    if (skill == 4) this.PlowLevel = i;
                    if (skill == 5) this.MillLevel = i;
                    i = 100;
                }
            }
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
