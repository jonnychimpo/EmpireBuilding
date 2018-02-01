using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using EmpireBuilding.CodeFiles;
using System.Windows.Media.Imaging;

namespace EmpireBuilding.CodeFiles
{
    public class Plot : INotifyPropertyChanged
    {
        private string pvPlotGridLocation;
        public string PlotGridLocation
        {
            get { return pvPlotGridLocation; }
            set { pvPlotGridLocation = value; }
        }

        private int pvPlotTypeID;
        public int PlotTypeID
        {
            get { return pvPlotTypeID; }
            set { pvPlotTypeID = value; }
        }

        private string pvPlotName;
        public string PlotName
        {
            get { return pvPlotName; }
            set { pvPlotName = value; }
        }

        private Brush pvPlotType;
        public Brush PlotType
        {
            get { return pvPlotType; }
            set { pvPlotType = value; NotifyPropertyChanged("PlotType"); }
        }

        private string pvPlotTypeDisplay;
        public string PlotTypeDisplay
        {
            get { return pvPlotTypeDisplay; }
            set { pvPlotTypeDisplay = value; NotifyPropertyChanged("PlotTypeDisplay"); }
        }

        private string pvPlotTypeFinal;
        public string PlotTypeFinal
        {
            get { return pvPlotTypeFinal; }
            set { pvPlotTypeFinal = value; }
        }

        private int pvPlotTypeFinalID;
        public int PlotTypeFinalID
        {
            get { return pvPlotTypeFinalID; }
            set { pvPlotTypeFinalID = value; }
        }

        private int pvExplorable;
        public int Explorable
        {
            get { return pvExplorable; }
            set { pvExplorable = value; NotifyPropertyChanged("Explorable"); }
        }

        private int pvExplored;
        public int Explored
        {
            get { return pvExplored; }
            set { pvExplored = value; }
        }

        private int pvStatus;
        public int Status
        {
            get { return pvStatus; }
            set { pvStatus = value; NotifyPropertyChanged("Status"); }
        }

        private int pvPlanted;
        public int Planted
        {
            get { return pvPlanted; }
            set { pvPlanted = value; NotifyPropertyChanged("Planted"); }
        }

        private int pvFertilized;
        public int Fertilized
        {
            get { return pvFertilized; }
            set { pvFertilized = value; }
        }

        private int pvPlotFertilityRate;
        public int PlotFertilityRate
        {
            get { return pvPlotFertilityRate; }
            set { pvPlotFertilityRate = value; }
        }

        private string pvPlotGrowthColor;
        public string PlotGrowthColor
        {
            get { return pvPlotGrowthColor; }
            set { pvPlotGrowthColor = value; }
        }

        private double pvBaseGrowthRate;
        public double BaseGrowthRate
        {
            get { return pvBaseGrowthRate; }
            set { pvBaseGrowthRate = value; NotifyPropertyChanged("BaseGrowthRate"); }
        }

        private double pvPlotGrowthRate;
        public double PlotGrowthRate
        {
            get { return pvPlotGrowthRate; }
            set { pvPlotGrowthRate = value; NotifyPropertyChanged("PlotGrowthRate"); }
        }

        private int pvCurSeedType;
        public int CurSeedType
        {
            get { return pvCurSeedType; }
            set { pvCurSeedType = value; NotifyPropertyChanged("CurSeedType"); }
        }

        private int pvPlantTime;
        public int PlantTime
        {
            get { return pvPlantTime; }
            set { pvPlantTime = value; }
        }

        private string pvSeedGroup;
        public string SeedGroup
        {
            get { return pvSeedGroup; }
            set { pvSeedGroup = value; }
        }

        private string pvParentFieldName;
        public string ParentFieldName
        {
            get { return pvParentFieldName; }
            set { pvParentFieldName = value; }
        }

        private DateTime pvSeedPlantDate;
        public DateTime SeedPlantDate
        {
            get { return pvSeedPlantDate; }
            set { pvSeedPlantDate = value; }
        }

        private DateTime pvExploreDate;
        public DateTime ExploreDate
        {
            get { return pvExploreDate; }
            set { pvExploreDate = value; }
        }

        private DateTime pvPlantCompleteDate;
        public DateTime PlantCompleteDate
        {
            get { return pvPlantCompleteDate; }
            set { pvPlantCompleteDate = value; NotifyPropertyChanged("PlantCompleteDate"); }
        }

        private DateTime pvFortitudeStartDate;
        public DateTime FortitudeStartDate
        {
            get { return pvFortitudeStartDate; }
            set { pvFortitudeStartDate = value; NotifyPropertyChanged("FortitudeStartDate"); }
        }

        private DateTime pvPlowCompleteDate;
        public DateTime PlowCompleteDate
        {
            get { return pvPlowCompleteDate; }
            set { pvPlowCompleteDate = value; }
        }

        private Brush pvPlotColor;
        public Brush PlotColor
        {
            get { return pvPlotColor; }
            set { pvPlotColor = value; NotifyPropertyChanged("PlotColor"); }
        }

        private Brush pvPlotForeColor;
        public Brush PlotForeColor
        {
            get { return pvPlotForeColor; }
            set { pvPlotForeColor = value; NotifyPropertyChanged("PlotForeColor"); }
        }

        private string pvActionTextColor;
        public string ActionTextColor
        {
            get { return pvActionTextColor; }
            set { pvActionTextColor = value; NotifyPropertyChanged("ActionTextColor"); }
        }

        private Brush pvActionColor;
        public Brush ActionColor
        {
            get { return pvActionColor; }
            set { pvActionColor = value; NotifyPropertyChanged("ActionColor"); }
        }

        private string pvActionDisplayTime;
        public string ActionDisplayTime
        {
            get { return pvActionDisplayTime; }
            set { pvActionDisplayTime = value; NotifyPropertyChanged("ActionDisplayTime"); }
        }

        private string pvActionTextBorderColor;
        public string ActionTextBorderColor
        {
            get { return pvActionTextBorderColor; }
            set { pvActionTextBorderColor = value; NotifyPropertyChanged("ActionTextBorderColor"); }
        }

        //private string pvShowExplore;
        //public string ShowExplore
        //{
        //    get { return pvShowExplore; }
        //    set { pvShowExplore = value; NotifyPropertyChanged("ShowExplore"); }
        //}

        private string pvPlotTitle;
        public string PlotTitle
        {
            get { return pvPlotTitle; }
            set { pvPlotTitle = value; NotifyPropertyChanged("PlotTitle"); }
        }

        private string pvPlotStatus;
        public string PlotStatus
        {
            get { return pvPlotStatus; }
            set { pvPlotStatus = value; NotifyPropertyChanged("PlotStatus"); }
        }

        private string pvActionTitle;
        public string ActionTitle
        {
            get { return pvActionTitle; }
            set { pvActionTitle = value; NotifyPropertyChanged("ActionTitle"); }
        }

        private int pvPlotID;
        public int PlotID
        {
            get { return pvPlotID; }
            set { pvPlotID = value; }
        }

        private int pvTileNum;
        public int TileNum
        {
            get { return pvTileNum; }
            set { pvTileNum = value; }
        }

        private int pvOwnerID;
        public int OwnerID
        {
            get { return pvOwnerID; }
            set { pvOwnerID = value; }
        }

        private string pvTimeCountDown;
        public string TimeCountDown
        {
            get { return pvTimeCountDown; }
            set { pvTimeCountDown = value; NotifyPropertyChanged("TimeCountDown"); }
        }

        private string pvHarvestCount = string.Empty;
        public string HarvestCount
        {
            get { return pvHarvestCount; }
            set { pvHarvestCount = value; NotifyPropertyChanged("HarvestCount"); }
        }

        private string pvShowPlot4Exploring = string.Empty;
        public string ShowPlot4Exploring
        {
            get { return pvShowPlot4Exploring; }
            set { pvShowPlot4Exploring = value; NotifyPropertyChanged("ShowPlot4Exploring"); }
        }

        private ImageSource pvPlotBackgroundImage;
        public ImageSource PlotBackgroundImage
        {
            get { return pvPlotBackgroundImage; }
            set { pvPlotBackgroundImage = value; NotifyPropertyChanged("PlotBackgroundImage"); }
        }

        private Brush pvPlotBorderColor;
        public Brush PlotBorderColor
        {
            get { return pvPlotBorderColor; }
            set { pvPlotBorderColor = value; NotifyPropertyChanged("PlotBorderColor"); }
        }

        private Brush pvColor2Show;
        public Brush Color2Show
        {
            get { return pvColor2Show; }
            set { pvColor2Show = value; NotifyPropertyChanged("Color2Show"); }
        }

        public Plot() { }

        public Plot(XDocument Sector2Load, int tilenum, int userid, int hometile, XDocument PlotTypeXDoc)
        {
            this.PlotID = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Attribute("PlotID").Value);
            this.PlotName = Sector2Load.Element("Sector").Element("Tile" + tilenum).Attribute("PlotName").Value;
            this.PlotGridLocation = Sector2Load.Element("Sector").Element("Tile" + tilenum).Attribute("PlotGridLocation").Value;
            //this.PlotColor = (Brush)App.Current.Resources[Sector2Load.Element("Sector").Element("Tile" + tilenum).Attribute("PlotColor").Value];
            this.TileNum = tilenum;
            this.OwnerID = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("OwnerID").Value);

            this.Status = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("Status").Value);
            this.Planted = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("Planted").Value);
            this.PlantCompleteDate = Convert.ToDateTime(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlantCompleteDate").Value);
            this.HarvestCount = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("HarvestCount").Value;
            this.Explored = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("Explored").Value);
            if (OwnerID == 0) {
                // Check if the Tile directly south of this Tile is Owned by this Player.
                if (tilenum + 10 < 100) {
                    int TempOwnerID = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + (tilenum + 10)).Element("OwnerID").Value);
                    int TempStatus = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + (tilenum + 10)).Element("Status").Value);
                    if ((TempOwnerID == userid) && (this.Explored == 0) && (TempStatus != 3) && (TempStatus != 1)) { this.Explorable = 1; }
                };
                // Check if the Tile directly south of this Tile is Owned by this Player   
                if (tilenum - 10 > 0) {
                    int TempOwnerID = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + (tilenum - 10)).Element("OwnerID").Value);
                    int TempStatus = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + (tilenum - 10)).Element("Status").Value);
                    if ((TempOwnerID == userid) && (this.Explored == 0) && (TempStatus != 3) && (TempStatus != 1)) { this.Explorable = 1; }
                };
                // Check if the Tile directly East of this Tile is Owned by this Player   
                if (tilenum + 1 < 100) {
                    int TempOwnerID = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + (tilenum + 1)).Element("OwnerID").Value);
                    int TempStatus = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + (tilenum + 1)).Element("Status").Value);
                    int CurrentTileRoundDown = Convert.ToInt32(Math.Floor(Convert.ToDouble(tilenum / 10)));
                    int NextTileRoundDown = Convert.ToInt32(Math.Floor(Convert.ToDouble((tilenum+1) / 10)));
                    if (((TempOwnerID == userid) && (CurrentTileRoundDown == NextTileRoundDown)) && (this.Explored == 0) && (TempStatus != 3) && (TempStatus != 1)) { this.Explorable = 1; }
                };
                    // Check if the Tile directly West of this Tile is Owned by this Player   
                if (tilenum - 1 > 0) {
                    int TempOwnerID = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + (tilenum - 1)).Element("OwnerID").Value);
                    int TempStatus = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + (tilenum - 1)).Element("Status").Value);
                    int CurrentTileRoundDown = Convert.ToInt32(Math.Floor(Convert.ToDouble(tilenum / 10)));
                    int NextTileRoundDown = Convert.ToInt32(Math.Floor(Convert.ToDouble((tilenum - 1) / 10)));
                    if (((TempOwnerID == userid) && (CurrentTileRoundDown == NextTileRoundDown)) && (this.Explored == 0) && (TempStatus != 3) && (TempStatus != 1)) { this.Explorable = 1; }
                };
            }
            
            //int TemporaryStatus = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("Status").Value);
            //if (TemporaryStatus > 0) { this.Explorable = 0; }

            //this.Explorable = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("Explorable").Value);
            this.ExploreDate = Convert.ToDateTime(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("ExploreDate").Value);
            this.TimeCountDown = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("TimeCountDown").Value;
            this.PlotTitle = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotTitle").Value;
            this.ActionTitle = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("ActionTitle").Value;
            this.PlotType = (Brush)App.Current.Resources[Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotType").Value];
            this.PlotTypeDisplay = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotType").Value;
            this.PlotTypeFinal = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotTypeFinal").Value;
            this.PlotTypeFinalID = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotTypeFinalID").Value);
            string tempSPD = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("SeedPlantDate").Value;
            if (tempSPD != "") { this.SeedPlantDate = Convert.ToDateTime(tempSPD); }
            this.SeedGrowthRate = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("SeedGrowthRate").Value);
            this.PlotFertilityRate = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotFertilityRate").Value);
            this.PlotGrowthRate = Convert.ToDouble(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotGrowthRate").Value);
            string tempPGRM = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotGrowthRateModified").Value;
            if (tempPGRM == "") { this.PlotGrowthRateModified = 0.0; } else { this.PlotGrowthRateModified = Convert.ToDouble(tempPGRM); }
            //this.PlotGrowthRateModified = Convert.ToDouble(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotGrowthRateModified").Value);
            this.PlantTime = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlantTime").Value);
            this.CurSeedType = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("CurSeedType").Value);
            this.SeedID = CurSeedType;
            this.PlotTypeID = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotTypeID").Value);
            this.MaxSeedCount = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("MaxSeedCount").Value);
            this.CurrentSeedCount = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("CurrentSeedCount").Value);
            this.Acres = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("Acres").Value);
            this.MaxPlantTime = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("MaxPlantTime").Value);
            this.PlowCompleteDate = Convert.ToDateTime(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlowCompleteDate").Value);
            //int plotbonus, 
            this.ExploredAcres = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("ExploredAcres").Value);
            this.FlowerImage = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("FlowerImage").Value;
            this.PlotGrowthColor = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotGrowthColor").Value;
            this.CurrentSeedVolume = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("CurrentSeedVolume").Value);
            this.SeedVolumeIsVisible = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("SeedVolumeIsVisible").Value;
            this.MaxSeedCountSmall = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("MaxSeedCountSmall").Value;
            this.PlotExploresLeft = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("PlotExploresLeft").Value;
            this.TotalAcresText = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("TotalAcresText").Value;
            this.SeedsPerSecond = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("SeedsPerSecond").Value;
            this.FertilityText = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("FertilityText").Value;
            this.ActionIsVisible = Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("ActionIsVisible").Value;
            this.WasPlotMaxed = Convert.ToInt32(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("WasPlotMaxed").Value);
            this.FortitudeStartDate = Convert.ToDateTime(Sector2Load.Element("Sector").Element("Tile" + tilenum).Element("FortitudeStartDate").Value);
            this.FOWisVisible = "Visible";
            this.PlotBorderColor = (Brush)App.Current.Resources["MainGreen"];
            Uri CurrentURI = new Uri("", UriKind.Relative);
            ImageSource imgsrc1 = new BitmapImage(CurrentURI);
            this.PlotBackgroundImage = imgsrc1;
            this.PlotForeColor = new SolidColorBrush(Colors.White);

            if (Explorable == 1 || OwnerID == userid)
            {
                ShowPlot4Exploring = "100";
                if (Explored == 1 || OwnerID == userid)
                {
                    this.PlotColor = this.PlotType;
                    this.ExploreVisible = "Collapsed";
                    this.pvActionDisplayTime = this.MaxSeedCountSmall;
                }
                else
                {
                    this.PlotTitle = "????";
                    this.PlotColor = (Brush)App.Current.Resources["Explorable"];
                    this.PlotTypeDisplay = "????";
                    this.ExploreVisible = "Visible";
                }
            }
            else
            {
                if (this.OwnerID != userid && this.OwnerID != 0)
                {
                    this.PlotColor = new SolidColorBrush(Colors.Purple);
                    this.PlotTitle = "Enemy";
                    this.Status = 0;
                    this.ActionDisplayTime = "";
                    this.Explorable = 0;
                    CurrentURI = new Uri("Images/skull_small2.png", UriKind.Relative);
                    ImageSource Enemyimgsrc1 = new BitmapImage(CurrentURI);
                    this.PlotBackgroundImage = Enemyimgsrc1;
                }
                else
                {
                    ShowPlot4Exploring = "50";
                    //ShowPlot4Exploring = "0.01";
                    this.PlotTypeDisplay = "????";
                    CurrentURI = new Uri("Images/fogofexploring.png", UriKind.Relative);
                    ImageSource Enemyimgsrc1 = new BitmapImage(CurrentURI);
                    this.PlotBackgroundImage = Enemyimgsrc1;
                }
            }

            string tempPC = ""; string tempPBC = ""; string tempADT = ""; string tempBGImage = "";
            if (this.Status == 1) { tempPC = "ExploreBorder"; tempPBC = "ExploreBlue"; tempADT = "--:--:--"; tempBGImage = "Images/explorer_small.png"; }
            if (this.Status == 2) { tempPC = "PlantYellow"; tempPBC = "PlantBorder"; tempADT = "--:--:--"; tempBGImage = "Images/Planting.jpg"; }
            if (this.Status == 3) { tempPC = "ExploreBorder"; tempPBC = "ExploreBlue"; tempADT = "Done"; tempBGImage = "Images/explorer_small.png"; }
            if (this.Status == 4) { tempPC = "PlantYellow"; tempPBC = "PlantBorder"; tempADT = "Done"; tempBGImage = "Images/Planting.jpg"; }
            if (this.Status == 5) { tempPC = "PlowRed"; tempPBC = "PlowBorder"; tempADT = "--:--:--"; tempBGImage = "Images/Plowing.jpg"; }
            if (this.Status == 6) { tempPC = "PlowRed"; tempPBC = "PlowBorder"; tempADT = "Done"; tempBGImage = "Images/Plowing.jpg"; }

            if (this.Status > 0 && this.Status < 7)
            {
                //this.PlotColor = (Brush)App.Current.Resources[tempPC];
                this.ActionColor = (Brush)App.Current.Resources[tempPC];
                this.PlotBorderColor = (Brush)App.Current.Resources[tempPBC];
                this.ActionDisplayTime = tempADT;
                this.PlotTitle = "";
                this.PlotStatus = "Loading";
                Uri BGImage = new Uri(tempBGImage, UriKind.Relative);
                ImageSource BGImageSrc = new BitmapImage(BGImage);
                this.PlotBackgroundImage = BGImageSrc;
            }

            this.PlowVisible = "Collapsed";
            this.PlantVisible = "Collapsed";
            this.HarvestVisible = "Collapsed";

            if (this.Planted == 1) 
            { 
                this.PlotStatus = "Loading";
                Uri FlowerURI = new Uri(this.FlowerImage, UriKind.Relative);
                ImageSource Flowerimgsrc1 = new BitmapImage(FlowerURI);
                this.PlotBackgroundImage = Flowerimgsrc1;
                this.ActionColor = new SolidColorBrush(Colors.Green);
            }

            if (PlotID == hometile) {
                CurrentURI = new Uri("Images/home_small.png", UriKind.Relative);
                ImageSource HTimgsrc1 = new BitmapImage(CurrentURI);
                this.PlotBackgroundImage = HTimgsrc1;
                this.PlotColor = new SolidColorBrush(Colors.Orange); 
                this.PlotTitle = "Home";
                this.Status = 0;
                this.ActionDisplayTime = "";
                this.Explored = 1;
            }

            if (CurrentURI.OriginalString == "" && this.Explored == 1)
            {
                string PlotTypeImage = "Images/" + PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + this.PlotTypeFinalID).Element("PlotImage").Value;
                //if (PlotTypeImage == "") { PlotTypeImage = ""; }
                CurrentURI = new Uri(PlotTypeImage, UriKind.Relative);
                ImageSource PTimgsrc1 = new BitmapImage(CurrentURI);
                this.PlotBackgroundImage = PTimgsrc1;
            }
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

        private double pvSeedValue;
        public double SeedValue
        {
            get { return pvSeedValue; }
            set { pvSeedValue = value; }
        }

        private int pvSeedGrowthRate;
        public int SeedGrowthRate
        {
            get { return pvSeedGrowthRate; }
            set { pvSeedGrowthRate = value; }
        }

        private int pvPlotTypeBonus;
        public int PlotTypeBonus
        {
            get { return pvPlotTypeBonus; }
            set { pvPlotTypeBonus = value; }
        }

        private int pvMaxSeedCount;
        public int MaxSeedCount
        {
            get { return pvMaxSeedCount; }
            set { pvMaxSeedCount = value; }
        }

        private string pvMaxSeedCountSmall;
        public string MaxSeedCountSmall
        {
            get { return pvMaxSeedCountSmall; }
            set { pvMaxSeedCountSmall = value; NotifyPropertyChanged("MaxSeedCountSmall"); }
        }

        private int pvCurrentSeedCount;
        public int CurrentSeedCount
        {
            get { return pvCurrentSeedCount; }
            set { pvCurrentSeedCount = value; }
        }

        private string pvPlotExploresLeft;
        public string PlotExploresLeft
        {
            get { return pvPlotExploresLeft; }
            set { pvPlotExploresLeft = value; NotifyPropertyChanged("PlotExploresLeft"); }
        }

        private int pvAutoHarvestTotalTime;
        public int AutoHarvestTotalTime
        {
            get { return pvAutoHarvestTotalTime; }
            set { pvAutoHarvestTotalTime = value; NotifyPropertyChanged("AutoHarvestTotalTime"); }
        }

        private string pvAutoHarvestActionDate;
        public string AutoHarvestActionDate
        {
            get { return pvAutoHarvestActionDate; }
            set { pvAutoHarvestActionDate = value; }
        }

        private int pvAcres;
        public int Acres
        {
            get { return pvAcres; }
            set { pvAcres = value; }
        }

        private int pvExploredAcres;
        public int ExploredAcres
        {
            get { return pvExploredAcres; }
            set { pvExploredAcres = value; NotifyPropertyChanged("ExploredAcres"); }
        }

        private int pvMaxPlantTime;
        public int MaxPlantTime
        {
            get { return pvMaxPlantTime; }
            set { pvMaxPlantTime = value; }
        }

        //private Bonus[] pvPlotBonus;
        //public Bonus[] PlotBonus
        //{
        //    get { return pvPlotBonus; }
        //    set { pvPlotBonus = value; NotifyPropertyChanged("PlotBonus"); }
        //}

        private string pvTotalAcresText;
        public string TotalAcresText
        {
            get { return pvTotalAcresText; }
            set { pvTotalAcresText = value; NotifyPropertyChanged("TotalAcresText"); }
        }

        private string pvSeedsPerSecond;
        public string SeedsPerSecond
        {
            get { return pvSeedsPerSecond; }
            set { pvSeedsPerSecond = value; NotifyPropertyChanged("SeedsPerSecond"); }
        }

        private string pvFertilityText;
        public string FertilityText
        {
            get { return pvFertilityText; }
            set { pvFertilityText = value; NotifyPropertyChanged("FertilityText"); }
        }

        private string pvFlowerImage;
        public string FlowerImage
        {
            get { return pvFlowerImage; }
            set { pvFlowerImage = value; NotifyPropertyChanged("FlowerImage"); }
        }

        private int pvCurrentSeedVolume;
        public int CurrentSeedVolume
        {
            get { return pvCurrentSeedVolume; }
            set { pvCurrentSeedVolume = value; NotifyPropertyChanged("CurrentSeedVolume"); }
        }

        private string pvSeedVolumeIsVisible;
        public string SeedVolumeIsVisible
        {
            get { return pvSeedVolumeIsVisible; }
            set { pvSeedVolumeIsVisible = value; NotifyPropertyChanged("SeedVolumeIsVisible"); }
        }

        private string pvActionIsVisible;
        public string ActionIsVisible
        {
            get { return pvActionIsVisible; }
            set { pvActionIsVisible = value; NotifyPropertyChanged("ActionIsVisible"); }
        }

        private string pvFOWisVisible;
        public string FOWisVisible
        {
            get { return pvFOWisVisible; }
            set { pvFOWisVisible = value; NotifyPropertyChanged("FOWisVisible"); }
        }

        private double pvPlotGrowthRateModified;
        public double PlotGrowthRateModified
        {
            get { return pvPlotGrowthRateModified; }
            set { pvPlotGrowthRateModified = value; }
        }

        private int pvIsPlotMaxed;
        public int IsPlotMaxed
        {
            get { return pvIsPlotMaxed; }
            set { pvIsPlotMaxed = value; NotifyPropertyChanged("IsPlotMaxed"); }
        }

        private int pvWasPlotMaxed;
        public int WasPlotMaxed
        {
            get { return pvWasPlotMaxed; }
            set { pvWasPlotMaxed = value; NotifyPropertyChanged("WasPlotMaxed"); }
        }

        private int pvIsPlotSelected;
        public int IsPlotSelected
        {
            get { return pvIsPlotSelected; }
            set { pvIsPlotSelected = value; NotifyPropertyChanged("IsPlotSelected"); }
        }

        private int pvIsFortitudeActive;
        public int IsFortitudeActive
        {
            get { return pvIsFortitudeActive; }
            set { pvIsFortitudeActive = value; NotifyPropertyChanged("IsFortitudeActive"); }
        }

        private long pvDecayTotal;
        public long DecayTotal
        {
            get { return pvDecayTotal; }
            set { pvDecayTotal = value; NotifyPropertyChanged("DecayTotal"); }
        }

        private string pvExploreVisible;
        public string ExploreVisible
        {
            get { return pvExploreVisible; }
            set { pvExploreVisible = value; NotifyPropertyChanged("ExploreVisible"); }
        }

        private string pvHarvestVisible;
        public string HarvestVisible
        {
            get { return pvHarvestVisible; }
            set { pvHarvestVisible = value; NotifyPropertyChanged("HarvestVisible"); }
        }

        private string pvPlantVisible;
        public string PlantVisible
        {
            get { return pvPlantVisible; }
            set { pvPlantVisible = value; NotifyPropertyChanged("PlantVisible"); }
        }

        private string pvPlowVisible;
        public string PlowVisible
        {
            get { return pvPlowVisible; }
            set { pvPlowVisible = value; NotifyPropertyChanged("PlowVisible"); }
        }

        private string pvShowAction;
        public string ShowAction
        {
            get { return pvShowAction; }
            set { pvShowAction = value; NotifyPropertyChanged("ShowAction"); }
        }

        private string pvTheExplorePrice;
        public string TheExplorePrice
        {
            get { return pvTheExplorePrice; }
            set { pvTheExplorePrice = value; NotifyPropertyChanged("TheExplorePrice"); }
        }

        private string pvTheExploreTime;
        public string TheExploreTime
        {
            get { return pvTheExploreTime; }
            set { pvTheExploreTime = value; NotifyPropertyChanged("TheExploreTime"); }
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
