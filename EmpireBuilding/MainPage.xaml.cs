using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EmpireBuilding.Resources;
using EmpireBuilding.CodeFiles;
using System.IO;
using System.Xml.Linq;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.ComponentModel;

namespace EmpireBuilding
{
    public partial class MainPage : PhoneApplicationPage
    {
        int PerfCount = 0;
        int PerfCount2 = 0;
        int PerfCount3 = 0;
        int BatchCounter = 0;
        int WhichCheck = 0;
        TimeSpan PerfTimeSpan;
        DateTime PerfStartTime;
        DateTime PerfEndTime;
        string NewGame = "0";
        int ResumeGame = 0;
        int TheOption = 0;
        int IsSeedSettingsCheckDone = 0;
        int IsMachineSettingsCheckDone = 0;
        int IsPlotTypeSettingsCheckDone = 0;
        int IsGranarySettingsCheckDone = 0;
        int IsSiloSettingsCheckDone = 0;
        int IsMillSettingsCheckDone = 0;
        int IsRefinerySettingsCheckDone = 0;
        int IsUpgradeSettingsCheckDone = 0;
        int IsLoadingMapDone = 0;
        int PlotDataOpen = 0;
        int GarageDataOpen = 0;
        int RefineryOpen = 0;
        int GranaryOpen = 0;
        int SiloDataOpen = 0;
        int MillOpen = 0;
        int MenuOpen = 0;
        int UserDataOpen = 0;
        int MessageOpen = 0;
        int BasePlowSeedFindPCT = 90;
        int BaseExploreSeedFindPCT = 90;
        int FirstLBUpdate = 0;
        int tLeaderBoardTick = 0;
        User TheUser = new User();
        string userxml = string.Empty;
        string seedxml = string.Empty;
        string plottypexml = string.Empty;
        string sectorxml = string.Empty;
        string fullmapxml = string.Empty;
        string refineryxml = string.Empty;
        string machineryxml = string.Empty;
        string granaryxml = string.Empty;
        string siloxml = string.Empty;
        string millxml = string.Empty;
        string upgradexml = string.Empty;
        XDocument UserXDoc = new XDocument();
        XDocument SectorXDoc = new XDocument();
        XDocument SeedsXDoc = new XDocument();
        XDocument MachinesXDoc = new XDocument();
        XDocument PlotTypeXDoc = new XDocument();
        XDocument RefineryXDoc = new XDocument();
        XDocument GranaryXDoc = new XDocument();
        XDocument SiloXDoc = new XDocument();
        XDocument MillXDoc = new XDocument();
        XDocument UpgradeXDoc = new XDocument();
        Plot[] SectorPlots = new Plot[100];
        Plot SelectedPlot = new Plot();
        Plot PreviousPlot = new Plot();
        DispatcherTimer dt = new DispatcherTimer();
        Silo tSilo = new Silo();
        Seed CurrentSeed = new Seed();
        Machine tDrill = new Machine();
        Machine tPlow = new Machine();
        Machine tExplorer = new Machine();
        Machine YourHarvestor = new Machine();
        GameUI CurrentGameUI = new GameUI();
        Refinery tRefinery = new Refinery();
        TimeSpan TSInterval;
        TimeSpan MillInterval;
        TimeSpan RefineryBatchInterval;
        Random MyRandomNumber = new Random();
        Granary SelectedGranarySeed = new Granary();
        Granary PreviousSelectedGranarySeed = new Granary();
        Granary SelectedS2P = new Granary();
        Silo SelectedSiloFlower = new Silo();
        Mill SelectedMillSeed = new Mill();
        Border PreviousSelectedButton = new Border(); // Holds the previous selected button object.
        List<string> AvailableSeedsList = new List<string>();
        string[] AvailableSeedsName = new string[2000];
        int[] AvailableSeedsID = new int[2000];
        List<Granary> tlistboxSeed2Plant = new List<Granary>();
        List<Message> MessageList = new List<Message>();
        Message CurrentMessage = new Message();
        // ============================================== //
        // Need to Add to Online DB                       //
        int PointsperSGRMOD = 3;
        int PointsperSVMOD = 2;
        int PointsperSQMOD = 1;
        int PointsperSFMOD = 1;
        //int HarvestNeeded4Points = 1000;
        double SeedValueBonus = 0.05;
        double SeedQualityBonus = 0.03;
        int SeedGrowthRateBonus = 1;
        int SeedFortitudeBonus = 30;
        int BaseRefineTime = 30;
        int tRefineTime = 0;
        int BaseSeedCount2Add = 300;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs NavArgs)
        {
            if (NavigationContext.QueryString.ContainsKey("NewGame")) { NewGame = NavigationContext.QueryString["NewGame"]; }
            if (NavigationContext.QueryString.ContainsKey("Resume")) { ResumeGame = Convert.ToInt32(NavigationContext.QueryString["Resume"]); }

            // Load the User from XML File
            userxml = helper.CheckFiles("User.xml", 1, 0, "");
            TextReader tr = new StringReader(userxml);
            UserXDoc = XDocument.Load(tr);
            TheUser = TheUser.DefaultUser(userxml);
            textBlockCurrentFC.DataContext = TheUser;

            canvasPlot.Visibility = System.Windows.Visibility.Collapsed;
            //CurrentGameUI.PlotCanvasVisible = "Collapsed";
            
            //LayoutRoot.DataContext = CurrentGameUI;

            if (ResumeGame == 1)
            {
                rectShade.Visibility = System.Windows.Visibility.Visible;
                RadProgressIndicator.IsRunning = true;
                RadProgressIndicator.Content = "Loading Game";
                SectorXDoc = helper.LoadSectorXDoc(TheUser.HomeSector);

                CheckSeedSettings();
                CheckMachineSettings();
                CheckPlotTypeSettings();
                CheckGranarySettings();
                CheckSiloSettings();
                CheckMillSettings();
                CheckRefinerySettings();
                CheckUpgradeSettings();
                LoadSector(SectorXDoc);
            }

            if (NewGame == "1")
            {
                rectShade.Visibility = System.Windows.Visibility.Visible;
                RadProgressIndicator.IsRunning = true;
                RadProgressIndicator.Content = "Building Empire";
                TheOption = 6;
                string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&UserID={1}", TheOption, TheUser.UserID);

                WebClient wcBuildEmpire = new WebClient();
                wcBuildEmpire.DownloadStringAsync(new Uri(newURL));
                wcBuildEmpire.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcBuildEmpire_DownloadStringCompleted);
            }
        }

        private void wcBuildEmpire_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;
                TextReader tr = new StringReader(s);
                XDocument xdoc = XDocument.Load(tr);

                if (s != string.Empty)
                {
                    int DefaultUserCurrency = 500000;
                    int DefaultPaidCurrency = 0;
                    int DefaultCurExPlots = 0;

                    // Reset User Stats
                    userxml = helper.CreateNewUser(userxml, "1.00", TheUser.UserName, TheUser.UserID, DefaultUserCurrency, DefaultPaidCurrency, DefaultCurExPlots, 0, 0, 0);
                    
                    TextReader trUser = new StringReader(userxml);
                    XDocument InitialUserXDoc = XDocument.Load(trUser);
                    InitialUserXDoc.Element("UserInfo").Element("User").SetElementValue("HomeTile", xdoc.Element("NewEmpire").Element("HomeTile").Value);
                    InitialUserXDoc.Element("UserInfo").Element("User").SetElementValue("HomeSector", xdoc.Element("NewEmpire").Element("SectorID").Value);
                    userxml = helper.SaveUpdatedXML(InitialUserXDoc, "User.xml");
                    UserXDoc = InitialUserXDoc;
                    TheUser = TheUser.DefaultUser(userxml);
                    textBlockCurrentFC.DataContext = TheUser;

                    // Reset XML Data to Default
                    IsolatedStorageFile ISOstore = IsolatedStorageFile.GetUserStoreForApplication();
                    if ((ISOstore.FileExists("Machine.xml") == true)) { ISOstore.DeleteFile("Machine.xml"); }
                    if ((ISOstore.FileExists("Granary.xml") == true)) { ISOstore.DeleteFile("Granary.xml"); }
                    if ((ISOstore.FileExists("Seeds.xml") == true)) { ISOstore.DeleteFile("Seeds.xml"); }
                    if ((ISOstore.FileExists("Refinery.xml") == true)) { ISOstore.DeleteFile("Refinery.xml"); }
                    if ((ISOstore.FileExists("Upgrade.xml") == true)) { ISOstore.DeleteFile("Upgrade.xml"); }
                    if ((ISOstore.FileExists("Silo.xml") == true)) { ISOstore.DeleteFile("Silo.xml"); }
                    if ((ISOstore.FileExists("Mill.xml") == true)) { ISOstore.DeleteFile("Mill.xml"); }
                    if ((ISOstore.FileExists("PlotTypes.xml") == true)) { ISOstore.DeleteFile("PlotTypes.xml"); }

                    CheckSeedSettings();
                    CheckMachineSettings();
                    CheckPlotTypeSettings();
                    CheckGranarySettings();
                    CheckSiloSettings();
                    CheckMillSettings();
                    CheckRefinerySettings();
                    CheckUpgradeSettings();

                    //MessageBox.Show("HomeTile:" + TheUser.HomeTile + " Sector" + xdoc.Element("NewEmpire").Element("SectorID").Value);
                    int Sector2Load = Convert.ToInt32(xdoc.Element("NewEmpire").Element("SectorID").Value);

                    helper.ManageFullMapXML(1, "");
                    fullmapxml = helper.CheckFiles("FullMap.xml", 1, 0, "");

                    //RadProgressIndicator.IsRunning = true;
                    RadProgressIndicator.Content = "Loading Map";
                    TheOption = 7;
                    string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&SectorID={1}", TheOption, Sector2Load);

                    WebClient wcLoadMap = new WebClient();
                    wcLoadMap.DownloadStringAsync(new Uri(newURL));
                    wcLoadMap.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcLoadMap_DownloadStringCompleted);
                }
            }
            catch
            {
                MessageBox.Show("You failed to connect.");
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                //buttonRetry.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void wcLoadMap_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;
                

                if (s != string.Empty)
                {
                    TextReader tr = new StringReader(s);
                    XDocument xdoc = XDocument.Load(tr);
                    helper.ManageFullMapXML(2, s);
                    fullmapxml = helper.CheckFiles("FullMap.xml", 1, 0, "");

                    SectorXDoc = helper.LoadSectorXDoc(TheUser.HomeSector);

                    LoadSector(SectorXDoc);

                    LoadingDone();
                    //RadProgressIndicator.IsRunning = false;
                    
                }
            }
            catch
            {
                MessageBox.Show("Load Map Failed!");
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                //buttonRetry.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void CheckSeedSettings()
        {
            seedxml = helper.CheckFiles("Seeds.xml", 1, 0, "");
            TextReader tr = new StringReader(seedxml);
            SeedsXDoc = XDocument.Load(tr);
            string seedXMLVersion = SeedsXDoc.Element("AllSeeds").Attribute("SeedVersion").Value;

            TheOption = 8;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&SeedVersion={1}&UserID={2}", TheOption, seedXMLVersion, TheUser.UserID);

            WebClient wcCheckSeed = new WebClient();
            wcCheckSeed.DownloadStringAsync(new Uri(newURL));
            wcCheckSeed.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcCheckSeed_DownloadStringCompleted);
        }

        private void wcCheckSeed_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;

                if (s == "1")
                {
                    // Seed XML is up to date
                    IsSeedSettingsCheckDone = 1;
                    
                }
                else
                {
                    // Need to save the new Seed XML to the device.
                    helper.CheckFiles("Seeds.xml", 2, 0, s); // Update the XML file on Phone
                    seedxml = helper.CheckFiles("Seeds.xml", 1, 0, "");
                    TextReader tr = new StringReader(seedxml);
                    SeedsXDoc = XDocument.Load(tr);
                    IsSeedSettingsCheckDone = 1;
                }

                CreateSeedList(seedxml);
                WhichCheck = 1;
                LoadingDone();
            }
            catch
            {
                MessageBox.Show("Load Seed Settings Failed!");
                IsSeedSettingsCheckDone = 0;
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                //buttonRetry.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void CreateSeedList(string tempseedxml)
        {
            TextReader tr = new StringReader(tempseedxml);
            XDocument tSeedsXDoc = XDocument.Load(tr);
            int BonusEntryCounter = 0;
            string tSeedName = string.Empty;
            int tSeedID = 0;
            AvailableSeedsList = new List<string>();

            int tTotalSeedCount = Convert.ToInt32(tSeedsXDoc.Element("AllSeeds").Attribute("TotalSeedCount").Value);
            for (int i = 1; i < (tTotalSeedCount + 1); i++)
            {
                int tBonusEntries = Convert.ToInt32(tSeedsXDoc.Element("AllSeeds").Element("Seed" + i).Element("BonusEntries").Value);
                tSeedName = tSeedsXDoc.Element("AllSeeds").Element("Seed" + i).Attribute("Name").Value;
                tSeedID = Convert.ToInt32(tSeedsXDoc.Element("AllSeeds").Element("Seed" + i).Attribute("SeedID").Value);
                for (int j = 0; j < tBonusEntries; j++)
                {
                    AvailableSeedsList.Add(tSeedName);
                    AvailableSeedsName[BonusEntryCounter] = tSeedName;
                    AvailableSeedsID[BonusEntryCounter] = tSeedID;
                    BonusEntryCounter++;
                }
            }
        }

        private void CheckMachineSettings()
        {
            machineryxml = helper.CheckFiles("Machine.xml", 1, 0, "");
            TextReader tr = new StringReader(machineryxml);
            MachinesXDoc = XDocument.Load(tr);
            string machineXMLVersion = MachinesXDoc.Element("Machinery").Attribute("MachineVersion").Value;

            TheOption = 9;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&MachineVersion={1}&UserID={2}", TheOption, machineXMLVersion, TheUser.UserID);

            WebClient wcCheckMachine = new WebClient();
            wcCheckMachine.DownloadStringAsync(new Uri(newURL));
            wcCheckMachine.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcCheckMachine_DownloadStringCompleted);
        }

        private void wcCheckMachine_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;

                if (s == "1")
                {
                    // Machine XML is up to date
                    IsMachineSettingsCheckDone = 1;
                }
                else
                {
                    // Need to save the new Seed XML to the device.
                    helper.CheckFiles("Machine.xml", 2, 0, s); // Update the XML file on Phone
                    machineryxml = helper.CheckFiles("Machine.xml", 1, 0, "");
                    TextReader tr = new StringReader(machineryxml);
                    MachinesXDoc = XDocument.Load(tr);
                    IsMachineSettingsCheckDone = 1;
                }

                YourHarvestor = helper.LoadMachine(MachinesXDoc, 1);
                tDrill = helper.LoadMachine(MachinesXDoc, 2);
                tExplorer = helper.LoadMachine(MachinesXDoc, 3);
                tPlow = helper.LoadMachine(MachinesXDoc, 4);
                WhichCheck = 2;
                LoadingDone();
            }
            catch
            {
                MessageBox.Show("Load Machine Settings Failed!");
                IsMachineSettingsCheckDone = 0;
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
        }

        private void CheckPlotTypeSettings()
        {
            plottypexml = helper.CheckFiles("PlotTypes.xml", 1, 0, "");
            TextReader tr = new StringReader(plottypexml);
            PlotTypeXDoc = XDocument.Load(tr);
            string plottypeXMLVersion = PlotTypeXDoc.Element("AllPlotTypes").Attribute("PlotTypeVersion").Value;

            TheOption = 10;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&PlotTypeVersion={1}&UserID={2}", TheOption, plottypeXMLVersion, TheUser.UserID);

            WebClient wcCheckPlotType = new WebClient();
            wcCheckPlotType.DownloadStringAsync(new Uri(newURL));
            wcCheckPlotType.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcCheckPlotType_DownloadStringCompleted);
        }

        private void wcCheckPlotType_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;

                if (s == "1")
                {
                    // PlotType XML is up to date
                    IsPlotTypeSettingsCheckDone = 1;
                }
                else
                {
                    // Need to save the new Seed XML to the device.
                    helper.CheckFiles("PlotTypes.xml", 2, 0, s); // Update the XML file on Phone
                    plottypexml = helper.CheckFiles("PlotTypes.xml", 1, 0, "");
                    TextReader tr = new StringReader(plottypexml);
                    PlotTypeXDoc = XDocument.Load(tr);
                    IsPlotTypeSettingsCheckDone = 1;
                }
                WhichCheck = 3;
                LoadingDone();
            }
            catch
            {
                MessageBox.Show("Load PlotType Settings Failed!");
                IsPlotTypeSettingsCheckDone = 0;
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                //buttonRetry.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void CheckGranarySettings()
        {
            granaryxml = helper.CheckFiles("Granary.xml", 1, 0, "");
            TextReader tr = new StringReader(granaryxml);
            GranaryXDoc = XDocument.Load(tr);
            string granaryXMLVersion = GranaryXDoc.Element("Granary").Attribute("GranaryVersion").Value;

            TheOption = 11;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&GranaryVersion={1}&UserID={2}", TheOption, granaryXMLVersion, TheUser.UserID);

            WebClient wcCheckGranary = new WebClient();
            wcCheckGranary.DownloadStringAsync(new Uri(newURL));
            wcCheckGranary.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcCheckGranary_DownloadStringCompleted);
        }

        private void wcCheckGranary_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;

                if (s == "1")
                {
                    // Granary XML is up to date
                    IsGranarySettingsCheckDone = 1;

                }
                else
                {
                    // Need to save the new Seed XML to the device.
                    helper.CheckFiles("Granary.xml", 2, 0, s); // Update the XML file on Phone
                    granaryxml = helper.CheckFiles("Granary.xml", 1, 0, "");
                    TextReader tr = new StringReader(granaryxml);
                    GranaryXDoc = XDocument.Load(tr);
                    IsGranarySettingsCheckDone = 1;
                }
                WhichCheck = 4;
                LoadingDone();
            }
            catch
            {
                MessageBox.Show("Load Granary Settings Failed!");
                IsGranarySettingsCheckDone = 0;
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
        }

        private void CheckSiloSettings()
        {
            siloxml = helper.CheckFiles("Silo.xml", 1, 0, "");
            TextReader tr = new StringReader(siloxml);
            SiloXDoc = XDocument.Load(tr);
            string siloXMLVersion = SiloXDoc.Element("SeedSilo").Attribute("SiloVersion").Value;

            TheOption = 12;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&SiloVersion={1}&UserID={2}", TheOption, siloXMLVersion, TheUser.UserID);

            WebClient wcCheckSilo = new WebClient();
            wcCheckSilo.DownloadStringAsync(new Uri(newURL));
            wcCheckSilo.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcCheckSilo_DownloadStringCompleted);
        }

        private void wcCheckSilo_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;

                if (s == "1")
                {
                    // Silo XML is up to date
                    IsSiloSettingsCheckDone = 1;
                }
                else
                {
                    // Need to save the new Seed XML to the device.
                    helper.CheckFiles("Silo.xml", 2, 0, s); // Update the XML file on Phone
                    siloxml = helper.CheckFiles("Silo.xml", 1, 0, "");
                    TextReader tr = new StringReader(siloxml);
                    SiloXDoc = XDocument.Load(tr);
                    IsSiloSettingsCheckDone = 1;
                }
                WhichCheck = 5;
                LoadingDone();
            }
            catch
            {
                MessageBox.Show("Load Silo Settings Failed!");
                IsSiloSettingsCheckDone = 0;
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
        }

        private void CheckMillSettings()
        {
            millxml = helper.CheckFiles("Mill.xml", 1, 0, "");
            TextReader tr = new StringReader(millxml);
            MillXDoc = XDocument.Load(tr);
            string millXMLVersion = MillXDoc.Element("Mill").Attribute("MillVersion").Value;

            TheOption = 13;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&MillVersion={1}&UserID={2}", TheOption, millXMLVersion, TheUser.UserID);

            WebClient wcCheckMill = new WebClient();
            wcCheckMill.DownloadStringAsync(new Uri(newURL));
            wcCheckMill.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcCheckMill_DownloadStringCompleted);
        }

        private void wcCheckMill_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;

                if (s == "1")
                {
                    // Mill XML is up to date
                    IsMillSettingsCheckDone = 1;
                }
                else
                {
                    // Need to save the new Seed XML to the device.
                    helper.CheckFiles("Mill.xml", 2, 0, s); // Update the XML file on Phone
                    millxml = helper.CheckFiles("Mill.xml", 1, 0, "");
                    TextReader tr = new StringReader(millxml);
                    MillXDoc = XDocument.Load(tr);
                    IsMillSettingsCheckDone = 1;
                }
                WhichCheck = 6;
                LoadingDone();
            }
            catch
            {
                MessageBox.Show("Load Mill Settings Failed!");
                IsMillSettingsCheckDone = 0;
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
        }

        private void CheckRefinerySettings()
        {
            refineryxml = helper.CheckFiles("Refinery.xml", 1, 0, "");
            TextReader tr = new StringReader(refineryxml);
            RefineryXDoc = XDocument.Load(tr);
            string refineryXMLVersion = RefineryXDoc.Element("Refinery").Attribute("RefineryVersion").Value;

            TheOption = 14;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&RefineryVersion={1}&UserID={2}", TheOption, refineryXMLVersion, TheUser.UserID);

            WebClient wcCheckRefinery = new WebClient();
            wcCheckRefinery.DownloadStringAsync(new Uri(newURL));
            wcCheckRefinery.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcCheckRefinery_DownloadStringCompleted);
        }

        private void wcCheckRefinery_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;

                if (s == "1")
                {
                    // Refinery XML is up to date
                    IsRefinerySettingsCheckDone = 1;
                }
                else
                {
                    // Need to save the new Refinery XML to the device.
                    helper.CheckFiles("Refinery.xml", 2, 0, s); // Update the XML file on Phone
                    refineryxml = helper.CheckFiles("Refinery.xml", 1, 0, "");
                    TextReader tr = new StringReader(refineryxml);
                    RefineryXDoc = XDocument.Load(tr);
                    IsRefinerySettingsCheckDone = 1;
                }
                WhichCheck = 7;
                LoadingDone();
            }
            catch
            {
                MessageBox.Show("Load Refinery Settings Failed!");
                IsRefinerySettingsCheckDone = 0;
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
        }

        private void CheckUpgradeSettings()
        {
            upgradexml = helper.CheckFiles("Upgrade.xml", 1, 0, "");
            TextReader tr = new StringReader(upgradexml);
            UpgradeXDoc = XDocument.Load(tr);
            string upgradeXMLVersion = UpgradeXDoc.Element("AllUpgrades").Attribute("UpgradeVersion").Value;

            TheOption = 15;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&UpgradeVersion={1}&UserID={2}", TheOption, upgradeXMLVersion, TheUser.UserID);

            WebClient wcCheckUpgrade = new WebClient();
            wcCheckUpgrade.DownloadStringAsync(new Uri(newURL));
            wcCheckUpgrade.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcCheckUpgrade_DownloadStringCompleted);
        }

        private void wcCheckUpgrade_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;

                if (s == "1")
                {
                    // Refinery XML is up to date
                    IsUpgradeSettingsCheckDone = 1;
                }
                else
                {
                    // Need to save the new Seed XML to the device.
                    helper.CheckFiles("Upgrade.xml", 2, 0, s); // Update the XML file on Phone
                    upgradexml = helper.CheckFiles("Upgrade.xml", 1, 0, "");
                    TextReader tr = new StringReader(upgradexml);
                    UpgradeXDoc = XDocument.Load(tr);
                    IsUpgradeSettingsCheckDone = 1;
                }
                WhichCheck = 8;
                LoadingDone();
            }
            catch
            {
                MessageBox.Show("Load Upgrade Settings Failed!");
                IsUpgradeSettingsCheckDone = 0;
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
            }
        }

        private void LoadSector(XDocument tempXSector)
        {
            //List<Tile> BoardTiles = new List<Tile>();
            int GridX = 0;
            int GridY = 0;
            int Counter = 0;
            int SectorStartTile = Convert.ToInt32(SectorXDoc.Element("Sector").Attribute("SectorStartID").Value);
            TheUser.HomeTileNumber = TheUser.HomeTile - SectorStartTile;

            for (int i = 0; i < 100; i++)
            {
                PerfStartTime = DateTime.Now;

                Canvas BlankCanvas = new Canvas();
                BlankCanvas.Margin = new Thickness(0);
                BlankCanvas.Tap += Tile_Tap;
                //Binding Test1 = new Binding();
                //Test1.Source = "Tile" + i;
                //BindingOperations.SetBinding(BlankCanvas, Canvas.NameProperty, Test1);

                Border BlankBorder = new Border();
                BlankBorder.BorderThickness = new Thickness(1);
                BlankBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                BlankBorder.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                BlankBorder.Height = 60;
                BlankBorder.Width = 60;

                Image BackgroundImage = new Image();
                BackgroundImage.Height = 58;
                BackgroundImage.Width = 58;
                BackgroundImage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                BackgroundImage.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                //BackgroundImage.Opacity = 0.50;
                
                //BlankBorder.BorderBrush = (Brush)App.Current.Resources["Unknown"];

                TextBlock BlankTextBlock = new TextBlock();
                BlankTextBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                BlankTextBlock.Height = 20;
                BlankTextBlock.Width = 60;
                BlankTextBlock.TextWrapping = TextWrapping.Wrap;
                BlankTextBlock.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                BlankTextBlock.TextAlignment = TextAlignment.Center;
                BlankTextBlock.FontSize = 14;
                BlankTextBlock.FontFamily = new FontFamily("/EmpireBuilding;component/Fonts/Fonts.zip#Buxton Sketch");

                //TextBlock BlankTextBlock2 = new TextBlock();
                //BlankTextBlock2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                //BlankTextBlock2.Height = 26;
                //BlankTextBlock2.Width = 33;
                //BlankTextBlock2.TextWrapping = TextWrapping.Wrap;
                //BlankTextBlock2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                //BlankTextBlock2.TextAlignment = TextAlignment.Center;
                //BlankTextBlock2.FontSize = 13.333;

                TextBlock BlankTextBlock3 = new TextBlock();
                BlankTextBlock3.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                BlankTextBlock3.Height = 37;
                BlankTextBlock3.Width = 60;
                BlankTextBlock3.TextWrapping = TextWrapping.Wrap;
                BlankTextBlock3.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                BlankTextBlock3.TextAlignment = TextAlignment.Center;
                BlankTextBlock3.FontSize = 18.667;
                BlankTextBlock3.Name = "TileTB" + i;

                //<Canvas HorizontalAlignment="Left" Height="60" Margin="0" VerticalAlignment="Top" Width="60" Background="Black" Opacity="0.01">
                //Canvas BlankCanvas2 = new Canvas();
                //BlankCanvas2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                //BlankCanvas2.Height = 60;
                //BlankCanvas2.Width = 60;
                //BlankCanvas2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                //BlankCanvas2.Margin = new Thickness(0);
                //BlankCanvas2.Background = new SolidColorBrush(Colors.Black);
                //BlankCanvas2.Opacity = 0.01;

                // Action Canvas //
                Canvas ActionCanvas = new Canvas();
                ActionCanvas.Height = 60;
                ActionCanvas.Width = 60;
                ActionCanvas.Margin = new Thickness(0);
                //ActionCanvas.Background = (Brush)App.Current.Resources["ExploreBlue"];
                //ActionCanvas.Opacity = 0.50;

                Rectangle BlankRectangle = new Rectangle();
                BlankRectangle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                BlankRectangle.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                BlankRectangle.Height = 58;
                BlankRectangle.Width = 58;
                //BlankRectangle.Stroke = new SolidColorBrush(Colors.Red);
                BlankRectangle.Opacity = 0.75;
                BlankRectangle.Fill = (Brush)App.Current.Resources["ExploreBlue"];
                //BlankRectangle.Visibility = System.Windows.Visibility.Collapsed;

                TextBlock TimeTextBlock = new TextBlock();
                TimeTextBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                TimeTextBlock.Height = 26;
                TimeTextBlock.Width = 56;
                TimeTextBlock.TextWrapping = TextWrapping.Wrap;
                TimeTextBlock.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                TimeTextBlock.TextAlignment = TextAlignment.Right;
                TimeTextBlock.FontSize = 13.333;
                TimeTextBlock.Text = "00:00:00";

                BlankCanvas.Children.Add(BlankBorder);
                
                //BlankCanvas.Children.Add(BlankTextBlock2);
                BlankCanvas.Children.Add(BlankTextBlock3);
                //BlankCanvas.Children.Add(BlankRectangle);

                ActionCanvas.Children.Add(BlankRectangle);
                ActionCanvas.Children.Add(TimeTextBlock);
                Canvas.SetTop(TimeTextBlock, 38);
                Canvas.SetLeft(TimeTextBlock, 0);
                Canvas.SetTop(BlankRectangle, 2);
                Canvas.SetLeft(BlankRectangle, 2);
                BlankCanvas.Children.Add(BackgroundImage);
                BlankCanvas.Children.Add(ActionCanvas);
                BlankCanvas.Children.Add(BlankTextBlock);
                //Canvas.SetZIndex(BackgroundImage, 0);
                //Canvas.SetZIndex(ActionCanvas, 1);

                //Canvas.SetTop(BlankRectangle, 1);
                //Canvas.SetLeft(BlankRectangle, 1);
                Canvas.SetTop(ActionCanvas, 0);
                Canvas.SetLeft(ActionCanvas, 0);
                Canvas.SetTop(BackgroundImage, 2);
                Canvas.SetLeft(BackgroundImage, 2);
                Canvas.SetTop(BlankBorder, 1);
                Canvas.SetLeft(BlankBorder, 1);
                Canvas.SetTop(BlankTextBlock, 3);
                Canvas.SetLeft(BlankTextBlock, 3);
                //Canvas.SetTop(BlankTextBlock2, 38);
                //Canvas.SetLeft(BlankTextBlock2, 28);
                Canvas.SetTop(BlankTextBlock3, 0);
                Canvas.SetLeft(BlankTextBlock3, 0);

                PerfEndTime = DateTime.Now;
                PerfTimeSpan = PerfEndTime - PerfStartTime;
                PerfCount += PerfTimeSpan.Milliseconds;

                PerfStartTime = DateTime.Now;

                SectorPlots[i] = new Plot(SectorXDoc, i, TheUser.UserID, TheUser.HomeTile, PlotTypeXDoc);
                
                if (TheUser.HomeTile == SectorPlots[i].PlotID)
                {
                    TheUser.GlobalBaseLocation = SectorPlots[i].PlotGridLocation;
                }

                BlankCanvas.DataContext = SectorPlots[i];

                canvasGameBoard.Children.Add(BlankCanvas);

                PerfEndTime = DateTime.Now;
                PerfStartTime = DateTime.Now;
                PerfTimeSpan = PerfEndTime - PerfStartTime;
                PerfCount2 += PerfTimeSpan.Milliseconds;

                // Populate Blank Top Level Canvas
                Binding myBinding = new Binding();
                myBinding.Path = new PropertyPath("TileNum");
                myBinding.Source = SectorPlots[i];
                myBinding.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(BlankCanvas, Canvas.TagProperty, myBinding);

                // Populate Blank Top Level Canvas
                Binding myBinding2 = new Binding();
                myBinding2.Path = new PropertyPath("ShowPlot4Exploring");
                myBinding2.Source = SectorPlots[i];
                myBinding2.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(BlankCanvas, Canvas.OpacityProperty, myBinding2);

                Binding myBindingTB1 = new Binding();
                myBindingTB1.Path = new PropertyPath("PlotStatus");
                myBindingTB1.Source = SectorPlots[i];
                myBindingTB1.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(BlankTextBlock, TextBlock.TextProperty, myBindingTB1);

                Binding myBindingTBFC1 = new Binding();
                myBindingTBFC1.Path = new PropertyPath("PlotForeColor");
                myBindingTBFC1.Source = SectorPlots[i];
                myBindingTBFC1.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(BlankTextBlock, TextBlock.ForegroundProperty, myBindingTBFC1);

                // Populate Background Image
                Binding myBindingBGI = new Binding();
                myBindingBGI.Path = new PropertyPath("PlotBackgroundImage");
                myBindingBGI.Source = SectorPlots[i];
                myBindingBGI.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(BackgroundImage, Image.SourceProperty, myBindingBGI);

                //Binding myBindingTB2 = new Binding();
                //myBindingTB2.Path = new PropertyPath("");
                //myBindingTB2.Source = SectorPlots[i];
                //myBindingTB2.Mode = BindingMode.TwoWay;
                //BindingOperations.SetBinding(BlankTextBlock2, TextBlock.TextProperty, myBindingTB2);

                Binding myBindingTB3 = new Binding();
                myBindingTB3.Path = new PropertyPath("PlotTitle");
                myBindingTB3.Source = SectorPlots[i];
                myBindingTB3.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(BlankTextBlock3, TextBlock.TextProperty, myBindingTB3);
                
                //// Populate Blank Rectangle Background
                //Binding myBindingRectBG = new Binding();
                //myBindingRectBG.Path = new PropertyPath("ShowPlot4Exploring");
                //myBindingRectBG.Source = SectorPlots[i];
                //myBindingRectBG.Mode = BindingMode.TwoWay;
                //BindingOperations.SetBinding(BlankRectangle, Rectangle.VisibilityProperty, myBindingRectBG);

                // Populate Child Border
                Binding myBindingBG = new Binding();
                myBindingBG.Path = new PropertyPath("PlotColor");
                myBindingBG.Source = SectorPlots[i];
                myBindingBG.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(BlankBorder, Border.BackgroundProperty, myBindingBG);

                Binding myBindingBBrush = new Binding();
                myBindingBBrush.Path = new PropertyPath("PlotBorderColor");
                myBindingBBrush.Source = SectorPlots[i];
                myBindingBBrush.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(BlankBorder, Border.BorderBrushProperty, myBindingBBrush);

                // Set Action Canvas Property Bindings
                Binding myBindingABG = new Binding();
                myBindingABG.Path = new PropertyPath("ActionColor");
                myBindingABG.Source = SectorPlots[i];
                myBindingABG.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(BlankRectangle, Rectangle.FillProperty, myBindingABG);

                //Binding myBindingACV = new Binding();
                //myBindingACV.Path = new PropertyPath("ActionIsVisible");
                //myBindingACV.Source = SectorPlots[i];
                ////myBindingACV.Mode = BindingMode.TwoWay;
                //BindingOperations.SetBinding(ActionCanvas, Canvas.VisibilityProperty, myBindingACV);

                Binding myBindingATTB = new Binding();
                myBindingATTB.Path = new PropertyPath("ActionDisplayTime");
                myBindingATTB.Source = SectorPlots[i];
                myBindingATTB.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(TimeTextBlock, TextBlock.TextProperty, myBindingATTB);

                Binding myBindingATTBFC = new Binding();
                myBindingATTBFC.Path = new PropertyPath("PlotForeColor");
                myBindingATTBFC.Source = SectorPlots[i];
                myBindingATTBFC.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(TimeTextBlock, TextBlock.ForegroundProperty, myBindingATTBFC);

                Canvas.SetTop(BlankCanvas, GridY * 60);
                Canvas.SetLeft(BlankCanvas, GridX * 60);

                GridX++;
                Counter++;
                if (Counter == 10) { GridX = 0; GridY++; Counter = 0; }
                //BoardTiles.Add(new Tile(i, tempGridX, tempGridY, tempGrowthRate, tempGrowthSize, tempSize, tempOwner, 0));
                PerfEndTime = DateTime.Now;
                PerfTimeSpan = PerfEndTime - PerfStartTime;
                PerfCount3 += PerfTimeSpan.Milliseconds;
            }

            

            IsLoadingMapDone = 1;
            LoadingDone();

            //textBlockBioFuelmGs.Text = PerfCount.ToString() + " mGS";
            //MessageBox.Show("Perf1 = " + PerfCount.ToString() + " :: Perf2 = " + PerfCount2.ToString() + " :: Perf3 = " + PerfCount3.ToString());
        }

        private void LoadingDone()
        {
            PerfCount++;

            if (IsLoadingMapDone == 1 && IsSeedSettingsCheckDone == 1 && IsMachineSettingsCheckDone == 1 && IsPlotTypeSettingsCheckDone == 1 && IsGranarySettingsCheckDone == 1 && IsSiloSettingsCheckDone == 1 && IsMillSettingsCheckDone == 1 && IsRefinerySettingsCheckDone == 1 && IsUpgradeSettingsCheckDone == 1)
            {
                tRefinery.RefineryStatus = "Idle";

                CurrentGameUI.SectorDisplayName = "Sector " + Convert.ToInt32(SectorXDoc.Element("Sector").Attribute("SectorID").Value);
                textBlockCurrentSector.DataContext = CurrentGameUI;

                double WhereIsHomeTile = Math.Floor(Convert.ToDouble(TheUser.HomeTileNumber / 100));
                int HowMuch2Scroll = 0;
                if (WhereIsHomeTile > 4) { HowMuch2Scroll = 100; }
                if (WhereIsHomeTile > 6) { HowMuch2Scroll = 250; }

                scrollViewerBoard.ScrollToVerticalOffset(HowMuch2Scroll);
                //MessageBox.Show(WhichCheck.ToString());
                RadProgressIndicator.IsRunning = false;
                RadProgressIndicator.Content = "";
                rectShade.Visibility = System.Windows.Visibility.Collapsed;

                LoadGranaryDetails();
                LoadSiloDetails();
                LoadMillDetails();
                PopulateSkillsData();
                Start_Refinery();

                // Start Game Engine
                dt.Interval = new TimeSpan(0, 0, 0, 1, 0); // 1 Second
                dt.Tick += new EventHandler(dt_Tick);

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    dt.Start();
                });
                

                //MessageBox.Show("Done. HomeTile:" + TheUser.HomeTile);
                
            }

        }

        private void Tile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            canvasQuickLaunch.Visibility = System.Windows.Visibility.Collapsed;
            PreviousPlot.PlotBorderColor = (Brush)App.Current.Resources["MainGreen"];

            Canvas GridTile = (Canvas)sender;
            
            //TileBG = (Border)GridTile.Children[1];
            //TileTB4Size = (TextBlock)GridTile.Children[4];

            int TempTileNum = Convert.ToInt32(GridTile.Tag);
            Border TileBG = (Border)GridTile.Children[0];
            
            SelectedPlot = (Plot)SectorPlots[TempTileNum];
            SelectedPlot.PlotBorderColor = (Brush)App.Current.Resources["SelectedYellow"];

            if (SelectedPlot.Planted == 1) canvasQuickLaunch.Visibility = System.Windows.Visibility.Visible;

            //MessageBox.Show("PlotID:" + SelectedPlot.PlotID + " TileNum:" + TempTileNum);
            if (SelectedPlot.PlotID == TheUser.HomeTile)
            {
                MenuOpen = 1;
                canvasMenu.Visibility = System.Windows.Visibility.Visible;
                //canvasGarage.Visibility = System.Windows.Visibility.Visible;
                //GarageDataOpen = 1;
            }
            else
            {
                MenuOpen = 0;
                canvasMenu.Visibility = System.Windows.Visibility.Collapsed;

                if (PreviousPlot.PlotName == SelectedPlot.PlotName)
                {
                    canvasPlotIcon_Tap(sender, e);
                    canvasPlot.DataContext = SelectedPlot;
                    PopulatePlotDetails();
                }
            }

            // Finish Actions
            if (SelectedPlot.Status == 3) CheckExploreTime(1, SelectedPlot);
            if (SelectedPlot.Status == 4) CheckPlantTime(1, SelectedPlot);
            if (SelectedPlot.Status == 6) CheckPlowTime(1, SelectedPlot);

            PreviousPlot = SelectedPlot;
        }

        private void canvasPlotIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            if (PlotDataOpen == 0)
            {
                //PopulatePlotDetails();
                //PlotDataBounceIn.Begin();
                canvasPlot.Visibility = System.Windows.Visibility.Visible;
                PlotDataOpen = 1;
            }
            else if (PlotDataOpen == 1)
            {
                SelectedPlot.IsPlotSelected = 0;
                listboxSeeds2Plant.SelectedIndex = -1;
                canvasPlot.Visibility = System.Windows.Visibility.Collapsed;
                canvasPDExploreAction.Visibility = System.Windows.Visibility.Collapsed;
                canvasPDPlantAction.Visibility = System.Windows.Visibility.Collapsed;
                canvasPDPlantInfo.Visibility = System.Windows.Visibility.Collapsed;
                canvasPDPlowAction.Visibility = System.Windows.Visibility.Collapsed;
                //PlotDataBounceOut.Begin();
                PlotDataOpen = 0;
            }
            dt.Start();
        }

        private void button_Harvest_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Harvest(SelectedPlot, 0);

            canvasPlotIcon_Tap(sender, e);
        }

        private void Harvest(Plot tPlot, int IsAutoHarvest)
        {
            if (tPlot.Planted == 1)
            {
                TimeSpan TSInterval = DateTime.Now - tPlot.SeedPlantDate;
                int tSeconds = Convert.ToInt32(TSInterval.TotalSeconds);
                long CurrentSeedCount = GetHarvestCount(tSeconds, tPlot);
                tSilo.TSC = tSilo.TSC + CurrentSeedCount; // Grab current Seed Count
                CurrentSeed = new Seed(SeedsXDoc, tPlot.CurSeedType);

                string tCurSeedName = CurrentSeed.SeedName;
                int tDistance = GetDistance(tPlot.PlotGridLocation);
                double tPlantSpeed = 1 + (Convert.ToDouble(tDrill.Speed) / 100);
                tPlot.MaxPlantTime = Convert.ToInt32(tPlot.CurrentSeedCount / (tPlot.SeedGrowthRate * tPlot.PlotGrowthRate));
                int SeedFortitude = CurrentSeed.Fortitude;
                int SeedFortitudeMOD = CurrentSeed.FortitudeModified;
                int tFortitudeStart = ((SeedFortitude + SeedFortitudeMOD) * 60) + tPlot.MaxPlantTime;
                if (tPlot.IsFortitudeActive > 1) CurrentSeedCount = tPlot.CurrentSeedCount - tPlot.DecayTotal;

                DateTime rightnow = DateTime.Now;
                TimeSpan duration = new TimeSpan(0, 0, 0, tFortitudeStart);
                DateTime answer = rightnow.Add(duration);

                // Update Plot XML
                SectorXDoc.Element("Sector").Element("Tile" + tPlot.TileNum).SetElementValue("SeedPlantDate", DateTime.Now.ToString());
                SectorXDoc.Element("Sector").Element("Tile" + tPlot.TileNum).SetElementValue("FortitudeStartDate", answer.ToString());
                string tempsectorxml = SectorXDoc.ToString();
                helper.ManageFullMapXML(2, tempsectorxml);

                tPlot.SeedPlantDate = DateTime.Now;
                tPlot.HarvestCount = "0";
                tPlot.IsPlotMaxed = 0;
                tPlot.IsFortitudeActive = 0;
                tPlot.FortitudeStartDate = answer;
                tPlot.CurrentSeedVolume = 0;
                tPlot.MaxSeedCountSmall = (tPlot.CurrentSeedCount / 1000) + "k";
                tPlot.PlotStatus = "Growing";

                // Send Tile Updates to Cloud


                // Update Harvestor XML
                YourHarvestor.UseCount += 1;
                YourHarvestor.Available -= 1;
                YourHarvestor.TotalEXP += Convert.ToInt32(CurrentSeedCount);
                int BeforeHarvestLvl = YourHarvestor.Level;
                YourHarvestor.Level = helper.GetLevel(1, YourHarvestor.TotalEXP);
                int AfterHarvestLvl = YourHarvestor.Level;
                MachinesXDoc.Element("Machinery").Element("Machine" + YourHarvestor.ID).SetElementValue("Count", YourHarvestor.UseCount);
                MachinesXDoc.Element("Machinery").Element("Machine" + YourHarvestor.ID).SetElementValue("Available", YourHarvestor.Available);
                MachinesXDoc.Element("Machinery").Element("Machine" + YourHarvestor.ID).SetElementValue("TotalEXP", YourHarvestor.TotalEXP);
                MachinesXDoc.Element("Machinery").Element("Machine" + YourHarvestor.ID).SetElementValue("Level", YourHarvestor.Level);
                machineryxml = helper.SaveUpdatedXML(MachinesXDoc, "Machine.xml");
                PopulateSkillsData();

                // Add Harvested Yield to Silo
                SiloSeedActions(1, CurrentSeed.SeedID, CurrentSeedCount);

                // Add Harvest Count for Individual Seed
                int tSeedHarvestCount = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).Element("TotalTimesHarvest").Value);
                long tSeedsHarvested = Convert.ToInt64(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).Element("TotalHarvestYield").Value);
                long newHarvested = tSeedsHarvested + CurrentSeedCount;
                SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).SetElementValue("TotalTimesHarvest", tSeedHarvestCount + 1);
                SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).SetElementValue("TotalHarvestYield", newHarvested);

                // Calculate Total Points for a Seed
                int tSeedHarvestperPoint = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).Element("HarvestPerPoint").Value);
                int tBaseHPP = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).Element("BaseHPP").Value);
                int tTempCropCount = 0;
                int tTotalPoints = 0;

                while (tTempCropCount < tSeedsHarvested)
                {
                    tTempCropCount += tBaseHPP + (tBaseHPP * tTotalPoints);
                    if (tSeedsHarvested > tTempCropCount) tTotalPoints += 1;
                }

                // Calculate Available Points for a Seed
                int tPoints = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).Element("Points").Value);
                int tPointsUsed = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).Element("PointsUsed").Value);
                if (tPoints != (tTotalPoints - tPointsUsed))
                {
                    tSeedHarvestperPoint += tBaseHPP;
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).SetElementValue("HarvestPerPoint", tSeedHarvestperPoint);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).SetElementValue("TotalPoints", tTotalPoints);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurrentSeed.SeedID).SetElementValue("Points", (tTotalPoints - tPointsUsed));

                }
                seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");

                LoadSiloDetails();
                LoadGranaryDetails();

                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Orange);
                string tMessage = "+Harvested " + CurrentSeedCount.ToString("N0") + " " + CurrentSeed.SeedName + " seeds!";
                if (IsAutoHarvest == 1) tMessage = "+AutoHarvested " + tPlot.PlotName + " for " + CurrentSeedCount.ToString("N0") + " " + CurrentSeed.SeedName + " seeds!";
                textBlockHarvestUpdate.Text = tMessage;
                HarvestUpdateFade.Begin();
                if (BeforeHarvestLvl != AfterHarvestLvl)
                {
                    MessageList.Add(new Message(6, "Skill Level Up", "Harvesting has gained a level. \nNew Level\n" + YourHarvestor.Level, "Images/Combine.jpg","Collapsed",tDrill,0,0));
                }
            }
            else
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                textBlockHarvestUpdate.Text = "+This plot has not been planted yet!";
                HarvestUpdateFade.Begin();
            }
        }

        private long GetHarvestCount(int tSeconds, Plot tCurrentPlot)
        {
            //TSInterval = DateTime.Now - tCurrentPlot.SPD;
            if (tCurrentPlot.PlotFertilityRate == 0) { tCurrentPlot.PlotFertilityRate = 1; }
            if (tCurrentPlot.PlotTypeBonus == 0) { tCurrentPlot.PlotTypeBonus = 1; }
            long tempCount = Convert.ToInt64(tSeconds * tCurrentPlot.SeedGrowthRate * tCurrentPlot.PlotFertilityRate * tCurrentPlot.PlotGrowthRate * tCurrentPlot.PlotTypeBonus);

            if (tempCount > tCurrentPlot.CurrentSeedCount) tempCount = tCurrentPlot.CurrentSeedCount;

            return tempCount;
        }

        private int GetDistance(string gridlocation)
        {
            // Parse Base Location
            int tempBLint = TheUser.GlobalBaseLocation.IndexOf("|");
            int[] arrBLCoords = new int[2];
            arrBLCoords[0] = Convert.ToInt32(TheUser.GlobalBaseLocation.Substring(0, tempBLint));
            arrBLCoords[1] = Convert.ToInt32(TheUser.GlobalBaseLocation.Substring(TheUser.GlobalBaseLocation.IndexOf("|") + 1));

            // Parse Grid Numbers
            int tDistance = 0;
            int tempint = gridlocation.IndexOf("|");
            int[] arrCoords = new int[2];
            arrCoords[0] = Convert.ToInt32(gridlocation.Substring(0, tempint));
            arrCoords[1] = Convert.ToInt32(gridlocation.Substring(gridlocation.IndexOf("|") + 1));

            tDistance = (Math.Abs(arrCoords[0] - arrBLCoords[0]) + Math.Abs(arrCoords[1] - arrBLCoords[1]));

            return tDistance * 15;
        }

        private void listboxSeeds2Plant_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listboxSeeds2Plant.SelectedIndex > -1)
            {
                if (SelectedS2P.S2PSelectedBackColor == "#FF6E0000")
                {
                    SelectedS2P.S2PSelectedVisible = "Collapsed";
                    SelectedS2P.S2PSelectedForeColor = "#FF0F1D00"; // Dark Green
                    SelectedS2P.S2PSelectedBackColor = "#FF6E0000"; // Dark Red
                }
                else
                {
                    SelectedS2P.S2PSelectedVisible = "Collapsed";
                    SelectedS2P.S2PSelectedForeColor = "#FF6D6E00"; // Light Green
                    SelectedS2P.S2PSelectedBackColor = "#FF0F1D00"; // Dark Green
                }


                SelectedS2P = (Granary)listboxSeeds2Plant.SelectedItem;
                SelectedS2P.SeedGrowthRate = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedS2P.SeedID).Element("SeedGrowthRate").Value);
                SelectedS2P.SeedValue = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedS2P.SeedID).Element("SeedValue").Value) + Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedS2P.SeedID).Element("SeedValueModified").Value);
                SelectedS2P.SeedPlantBonus = 0;
                SelectedS2P.SeedsPlanted = 1;
                SelectedS2P.S2PSelectedVisible = "Visible";
                SelectedS2P.S2PSelectedForeColor = "#FF0F1D00";
                SelectedS2P.S2PSelectedBackColor = "#FF6D6E00";

                //int CurPlantLvl = tDrill.Level;
                int PlantLvlReq = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedS2P.SeedID).Element("PlantableLevel").Value);
                //textBlockPlotPlantCurPlantLvl.Text = CurPlantLvl.ToString();
                textBlockPlotPlantSGR.Text = SelectedS2P.SeedGrowthRate.ToString();
                textBlockPlotPlantSPV.Text = SelectedS2P.SeedValue.ToString("C");

                int tPlantedSeedXP = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedS2P.SeedID).Element("PlantedEXP").Value);
                int tSeedPlantSize = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedS2P.SeedID).Element("MaxPlantSize").Value);
                double tTemp = Convert.ToDouble(SelectedPlot.CurrentSeedCount) / Convert.ToDouble(tSeedPlantSize);
                double tTotalPlantedSeeds = Math.Ceiling(tTemp);
                textBlockPlotPlantSN2P.Text = tTotalPlantedSeeds.ToString();

                if (tDrill.Level < PlantLvlReq)
                {
                    SelectedS2P.S2PSelectedForeColor = "Red";
                    SelectedS2P.S2PSelectedBackColor = "#FF6E0000";
                }

                if (tTotalPlantedSeeds > SelectedS2P.SeedCount)
                {
                    textBlockPlotPlantSN2PTitle.Foreground = new SolidColorBrush(Colors.Red);
                    textBlockPlotPlantSN2P.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    textBlockPlotPlantSN2PTitle.Foreground = new SolidColorBrush(Colors.White);
                    textBlockPlotPlantSN2P.Foreground = new SolidColorBrush(Colors.White);
                }
            }
            else
            {
                if (SelectedS2P.S2PSelectedBackColor == "#FF6E0000")
                {
                    SelectedS2P.S2PSelectedVisible = "Collapsed";
                    SelectedS2P.S2PSelectedForeColor = "#FF0F1D00"; // Dark Green
                    SelectedS2P.S2PSelectedBackColor = "#FF6E0000"; // Dark Red
                }
                else
                {
                    SelectedS2P.S2PSelectedVisible = "Collapsed";
                    SelectedS2P.S2PSelectedForeColor = "#FF6D6E00"; // Light Green
                    SelectedS2P.S2PSelectedBackColor = "#FF0F1D00"; // Dark Green
                }
            }
        }

        private void Plant(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PreviousSelectedButton.BorderBrush = PreviousSelectedButton.Background;
            int tDistance = GetDistance(SelectedPlot.PlotGridLocation);
            int tPlantTotalPrice = GetPrice(tDrill, tDistance);
            double tPlantSpeed = 1 + (Convert.ToDouble(tDrill.Speed) / 100);
            int tPlantTime = Convert.ToInt32((((SelectedPlot.CurrentSeedCount / 1000) - 1) * 37 + tDistance) / tPlantSpeed);
            if (tPlantTime < 10) tPlantTime = 10;
            int tSeedPlantSize = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedS2P.SeedID).Element("MaxPlantSize").Value);
            double tTemp = Convert.ToDouble(SelectedPlot.CurrentSeedCount) / Convert.ToDouble(tSeedPlantSize);
            double tTotalSeedsReq2Plant = Math.Ceiling(tTemp);

            string newID = SelectedS2P.SeedID.ToString();
            int PlantableLevel = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedS2P.SeedID).Element("PlantableLevel").Value);

            if (SelectedPlot.Planted == 1)
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                textBlockHarvestUpdate.Text = "You have already planted in this plot!";
                HarvestUpdateFade.Begin();
            }
            else if (tDrill.Level < PlantableLevel)
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                textBlockHarvestUpdate.Text = "Your Planting Level needs to be " + PlantableLevel + " before you can plant this seed!";
                HarvestUpdateFade.Begin();
            }
            else if (newID == "")
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                textBlockHarvestUpdate.Text = "Please select a Seed to Plant!";
                HarvestUpdateFade.Begin();
            }
            else if (SelectedPlot.Status != 0)
            {
                string tAction = "";
                if (SelectedPlot.Status == 2) tAction = "planted!";
                else if (SelectedPlot.Status == 5) tAction = "plowed!";
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                textBlockHarvestUpdate.Text = "This plot is already being " + tAction;
                HarvestUpdateFade.Begin();
            }
            else if (SelectedS2P.SeedCount < tTotalSeedsReq2Plant)
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                textBlockHarvestUpdate.Text = "You need " + (tTotalSeedsReq2Plant - SelectedS2P.SeedCount) + " more " + SelectedS2P.SeedName + " to plant!";
                HarvestUpdateFade.Begin();
            }
            else if (SelectedPlot.Explored == 1)
            {
                if (tDrill.Available == 0)
                {
                    string tempAction = "You have no more Drills available for Planting.";
                    textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                    textBlockHarvestUpdate.Text = tempAction;
                    HarvestUpdateFade.Begin();
                }
                else if (tRefinery.mG >= tPlantTotalPrice)
                {
                    DateTime rightnow = DateTime.Now;
                    TimeSpan duration = new TimeSpan(0, 0, 0, tPlantTime);
                    DateTime answer = rightnow.Add(duration);

                    // Update Refinery
                    tRefinery.mG = tRefinery.mG - tPlantTotalPrice;
                    tRefinery.mGDisplay = tRefinery.mG + " mG";
                    int NewminiGallons = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("miniGallonsUsed").Value);
                    tRefinery.UsedmG = NewminiGallons + tPlantTotalPrice;
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("miniGallonsUsed", (NewminiGallons + tPlantTotalPrice));
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("mG", tRefinery.mG);
                    refineryxml = helper.SaveUpdatedXML(RefineryXDoc, "Refinery.xml");

                    // Update Drill XML
                    tDrill.UseCount += 1;
                    tDrill.Available -= 1;
                    MachinesXDoc.Element("Machinery").Element("Machine" + tDrill.ID).SetElementValue("Count", tDrill.UseCount);
                    MachinesXDoc.Element("Machinery").Element("Machine" + tDrill.ID).SetElementValue("Available", tDrill.Available);
                    machineryxml = helper.SaveUpdatedXML(MachinesXDoc, "Machine.xml");
                    PopulateSkillsData();

                    // Remove total number of seeds for Planting from Granary
                    GranaryXDoc.Element("Granary").Element("Seed" + SelectedS2P.SeedID).SetElementValue("TotalSeedCount", SelectedS2P.SeedCount - tTotalSeedsReq2Plant);
                    granaryxml = helper.SaveUpdatedXML(GranaryXDoc, "Granary.xml");
                    LoadGranaryDetails();

                    // Update Field XML
                    // Update FieldXML with the newly Explored Plot.
                    SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("CurSeedType", newID);
                    SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("Status", 2);
                    SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("SeedGrowthRate", (SelectedS2P.SeedGrowthRate + SelectedS2P.SeedPlantBonus));
                    SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("PlantCompleteDate", answer);
                    string tempsectorxml = SectorXDoc.ToString();
                    helper.ManageFullMapXML(2, tempsectorxml);

                    // Update User Stats
                    int CurrentBFUsed = Convert.ToInt32(UserXDoc.Element("UserInfo").Element("User").Element("BioFuelUsed").Value);
                    UserXDoc.Element("UserInfo").Element("User").SetElementValue("BioFuelUsed", (CurrentBFUsed + tPlantTotalPrice));
                    TheUser.BioFuelUsed = (CurrentBFUsed + tPlantTotalPrice);
                    userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");

                    SelectedPlot.PlantCompleteDate = answer;
                    SelectedPlot.Status = 2;
                    SelectedPlot.CurSeedType = Convert.ToInt32(newID);
                    SelectedPlot.SeedGrowthRate = SelectedS2P.SeedGrowthRate + SelectedS2P.SeedPlantBonus;
                    SelectedPlot.SeedValue = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedS2P.SeedID).Element("SeedValue").Value);
                    SelectedPlot.PlotBorderColor = (Brush)App.Current.Resources["PlantBorder"];
                    //SelectedPlot.PlotTitle = "";
                    SelectedPlot.ActionColor = (Brush)App.Current.Resources["PlantYellow"];
                    SelectedPlot.ActionDisplayTime = "--:--:--";
                    SelectedPlot.PlotStatus = "Planting";

                    // Change Background Image
                    Uri BGImage = new Uri("Images/Planting.jpg", UriKind.Relative);
                    ImageSource BGImageSrc = new BitmapImage(BGImage);
                    SelectedPlot.PlotBackgroundImage = BGImageSrc;

                    DeselectPlot();
                }
                else
                {
                    double tempint = tPlantTotalPrice - tRefinery.mG;
                    textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                    textBlockHarvestUpdate.Text = "-You need " + tempint.ToString("N0") + "mG of BioFuel to Plant!";
                    HarvestUpdateFade.Begin();
                }
            }
            else
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                textBlockHarvestUpdate.Text = "Select a Plot before planting!";
                HarvestUpdateFade.Begin();
            }
        }

        void CheckPlantTime(int WhichOne, Plot Plot2Update)
        {
            if (WhichOne == 1)
            {
                //string tCurSeedName = LookUpNameByIDXML(Plot2Update.CurSeedType, "Seed", seedxml, 1, "");
                int tDistance = GetDistance(Plot2Update.PlotGridLocation);
                double tPlantSpeed = 1 + (Convert.ToDouble(tDrill.Speed) / 100);
                Plot2Update.MaxPlantTime = Convert.ToInt32(Plot2Update.CurrentSeedCount / (Plot2Update.SeedGrowthRate * Plot2Update.PlotGrowthRate));
                int SeedFortitude = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + Plot2Update.CurSeedType).Element("Fortitude").Value);
                int tFortitudeStart = (SeedFortitude * 60) + Plot2Update.MaxPlantTime;

                DateTime rightnow = DateTime.Now;
                TimeSpan duration = new TimeSpan(0, 0, 0, tFortitudeStart);
                DateTime answer = rightnow.Add(duration);

                // Set Plot Planted value
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("FortitudeStartDate", answer);
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("Status", 0);
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("Planted", 1);
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("FlowerImage", "Images/" + SeedsXDoc.Element("AllSeeds").Element("Seed" + Plot2Update.CurSeedType).Element("FlowerImage").Value);
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("SeedPlantDate", rightnow);
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("SeedGrowthRate", Plot2Update.SeedGrowthRate);
                string tempsectorxml = SectorXDoc.ToString();
                helper.ManageFullMapXML(2, tempsectorxml);

                // Update Current Plot with new properties
                Plot2Update.FortitudeStartDate = answer;
                Plot2Update.SeedPlantDate = rightnow;
                Plot2Update.Status = 0;
                Plot2Update.Planted = 1;
                Plot2Update.PlotStatus = "Growing";
                Plot2Update.PlotFertilityRate = Convert.ToInt32(PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + Plot2Update.PlotTypeID).Element("FertilityBonus").Value);
                //Plot2Update.PlotColor = (Brush)App.Current.Resources[Plot2Update.PlotTypeFinal];
                Plot2Update.SeedVolumeIsVisible = "Visible";
                Plot2Update.FlowerImage = "Images/" + SeedsXDoc.Element("AllSeeds").Element("Seed" + Plot2Update.CurSeedType).Element("FlowerImage").Value;
                Plot2Update.SeedsPerSecond = (Plot2Update.SeedGrowthRate * Plot2Update.PlotFertilityRate * Plot2Update.PlotGrowthRate * Plot2Update.PlotTypeBonus) + "sps";
                Plot2Update.SeedName = SeedsXDoc.Element("AllSeeds").Element("Seed" + Plot2Update.CurSeedType).Attribute("Name").Value;
                Plot2Update.SeedID = Plot2Update.CurSeedType;
                Plot2Update.ActionDisplayTime = Plot2Update.MaxSeedCountSmall;
                Plot2Update.ActionColor = new SolidColorBrush(Colors.Green);
                Uri BGImage = new Uri(Plot2Update.FlowerImage, UriKind.Relative);
                ImageSource BGImageSrc = new BitmapImage(BGImage);
                Plot2Update.PlotBackgroundImage = BGImageSrc;

                // Check for PlotTypeBonus
                //int cpCurSeedTypeNativePlotTypeID = Convert.ToInt32(LookUpChildValueXML(TempSeedName, "Seed", seedxml, "NativePlotType"));
                //if (Plot2Update.PlotTypeID == cpCurSeedTypeNativePlotTypeID)
                //{
                //    Plot2Update.PlotTypeBonus = Convert.ToInt32(LookUpChildValueXML(TempSeedName, "Seed", seedxml, "NativeBonus"));
                //    Plot2Update.TimeCountDown = "*" + Plot2Update.TimeCountDown;
                //}

                // Update Plant Stats
                int tPlantedSeedXP = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + Plot2Update.CurSeedType).Element("PlantedEXP").Value);
                int tSeedPlantSize = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + Plot2Update.CurSeedType).Element("MaxPlantSize").Value);
                int tTotalPlantedSeeds = Convert.ToInt32(Math.Ceiling((double)Plot2Update.CurrentSeedCount / tSeedPlantSize));
                int tXP2Add = (Plot2Update.CurrentSeedCount / 1000) + (tPlantedSeedXP * tTotalPlantedSeeds);
                tDrill.Available++;
                tDrill.TotalEXP = tDrill.TotalEXP + tXP2Add;
                int BeforePlantLvl = tDrill.Level;
                tDrill.Level = helper.GetLevel(2, tDrill.TotalEXP);
                int AfterPlantLvl = tDrill.Level;
                MachinesXDoc.Element("Machinery").Element("Machine" + tDrill.ID).SetElementValue("Available", tDrill.Available);
                MachinesXDoc.Element("Machinery").Element("Machine" + tDrill.ID).SetElementValue("TotalEXP", tDrill.TotalEXP);
                MachinesXDoc.Element("Machinery").Element("Machine" + tDrill.ID).SetElementValue("Level", tDrill.Level);
                machineryxml = helper.SaveUpdatedXML(MachinesXDoc, "Machine.xml");
                PopulateSkillsData();

                // Add Plant Count for Individual Seed
                int CurrentPlantedCount = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + Plot2Update.CurSeedType).Element("TotalPlantedCount").Value);
                SeedsXDoc.Element("AllSeeds").Element("Seed" + Plot2Update.CurSeedType).SetElementValue("TotalPlantedCount", CurrentPlantedCount + 1);
                seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");

                //textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Blue);
                //textBlockHarvestUpdate.Text = "You Planted a " + Plot2Update.SeedName + " seed!";
                //HarvestUpdateFade.Begin();
                int DidSkillGainLevel = 0;
                if (BeforePlantLvl != AfterPlantLvl) { DidSkillGainLevel = 1; }
                MessageList.Add(new Message(7, "Seed Planted", "A new Seed has been planted on Plot " + Plot2Update.PlotTypeID + ".\nSeed Planted = " + Plot2Update.SeedName + "\nPlant XP = +" + tXP2Add, "Images/Planting.jpg", "Visible", tDrill, tXP2Add, DidSkillGainLevel));
                if (BeforePlantLvl != AfterPlantLvl)
                {
                    MessageList.Add(new Message(6, "Skill Level Up", "Planting has gained a level. \nNew Level\n" + tDrill.Level, "Images/Planting.jpg", "Visible", tDrill, tXP2Add, DidSkillGainLevel));
                }


                CheckYourMessages();
            }
            else if (WhichOne == 0)
            {
                // Update FieldXML with the newly Plowed Plot.
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("Status", "4");
                Plot2Update.Status = 4;
            }
        }

        private void Explore(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Update ThisUpdate = new Update();
            int tDistance = GetDistance(SelectedPlot.PlotGridLocation);
            int tExploreTotalPrice = GetPrice(tExplorer, tDistance);
            double tExploreSpeed = 1 + (Convert.ToDouble(tExplorer.ExecuteTime) / 100);
            int tExploreTime = Convert.ToInt32(((tDistance * tDistance) / 4) / tExploreSpeed);

            if (SelectedPlot.Explorable == 0)
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                textBlockHarvestUpdate.Text = "This plot is not explorable yet. Explore nearby plots first.";
                HarvestUpdateFade.Begin();
            }
            else if (SelectedPlot.Explored == 1 && (SelectedPlot.ExploredAcres == SelectedPlot.Acres))
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                textBlockHarvestUpdate.Text = "All Acres have been explored.";
                HarvestUpdateFade.Begin();
            }
            else if (tExplorer.Available == 0)
            {
                string tempAction = "You have no more Explorers available.";
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                textBlockHarvestUpdate.Text = tempAction;
                HarvestUpdateFade.Begin();
            }
            else if (SelectedPlot.Planted == 1)
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                textBlockHarvestUpdate.Text = "You cannot explore a plot that has been planted.";
                HarvestUpdateFade.Begin();
            }
            else if (SelectedPlot.Status != 0)
            {
                string tAction = "explored";
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                textBlockHarvestUpdate.Text = "This plot is already being " + tAction;
                HarvestUpdateFade.Begin();
            }
            else if (tRefinery.mG >= tExploreTotalPrice)
            {
                DateTime rightnow = DateTime.Now;
                TimeSpan duration = new TimeSpan(0, 0, 0, tExploreTime);
                DateTime answer = rightnow.Add(duration);

                // Update Explorer XML
                tExplorer.UseCount += 1;
                tExplorer.Available -= 1;
                MachinesXDoc.Element("Machinery").Element("Machine" + tExplorer.ID).SetElementValue("Count", tExplorer.UseCount);
                MachinesXDoc.Element("Machinery").Element("Machine" + tExplorer.ID).SetElementValue("Available", tExplorer.Available);
                machineryxml = helper.SaveUpdatedXML(MachinesXDoc, "Machine.xml");
                PopulateSkillsData();

                // Update Refinery
                //UpdateRefinerymG(2, tExploreTotalPrice);
                tRefinery.mG = tRefinery.mG - tExploreTotalPrice;
                tRefinery.mGDisplay = tRefinery.mG + " mG";
                int NewminiGallons = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("miniGallonsUsed").Value);
                tRefinery.UsedmG = NewminiGallons + tExploreTotalPrice;
                RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("miniGallonsUsed", (NewminiGallons + tExploreTotalPrice));
                RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("mG", tRefinery.mG);
                refineryxml = helper.SaveUpdatedXML(RefineryXDoc, "Refinery.xml");

                // Update FieldXML with the newly Explored Plot.
                SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("Status", "1");
                SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("ExploreDate", answer.ToString());
                SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("OwnerID", TheUser.UserID.ToString());
                string tempsectorxml = SectorXDoc.ToString();
                helper.ManageFullMapXML(2, tempsectorxml);

                SelectedPlot.ExploreDate = answer;
                SelectedPlot.Status = 1;
                //SelectedPlot.ActionIsVisible = "Visible";
                SelectedPlot.FOWisVisible = "Collasped";
                SelectedPlot.PlotForeColor = new SolidColorBrush(Colors.White);
                SelectedPlot.PlotBorderColor = (Brush)App.Current.Resources["ExploreBlue"];
                //SelectedPlot.PlotTitle = "";
                SelectedPlot.ActionColor = (Brush)App.Current.Resources["ExploreBorder"];
                //SelectedPlot.PlotColor = (Brush)App.Current.Resources["ExploreBorder"];
                //SelectedPlot.ActionTextBorderColor = "#FF00008D";
                //SelectedPlot.ActionTextColor = "#FF434564";
                SelectedPlot.ActionDisplayTime = "--:--:--";
                SelectedPlot.PlotStatus = "Exploring";

                // Change Background Image
                Uri BGImage = new Uri("Images/explorer_small.png", UriKind.Relative);
                ImageSource BGImageSrc = new BitmapImage(BGImage);
                SelectedPlot.PlotBackgroundImage = BGImageSrc;

                // Update User Stats
                int CurrentBFUsed = Convert.ToInt32(UserXDoc.Element("UserInfo").Element("User").Element("BioFuelUsed").Value);
                UserXDoc.Element("UserInfo").Element("User").SetElementValue("BioFuelUsed", (CurrentBFUsed + tExploreTotalPrice));
                TheUser.BioFuelUsed = (CurrentBFUsed + tExploreTotalPrice);
                userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");
                //TheUser = TheUser.DefaultUser(userxml);

                //DeselectPlot();
                canvasPlotIcon_Tap(sender, e);
            }
            else
            {
                double tempint = tExploreTotalPrice - tRefinery.mG;
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                textBlockHarvestUpdate.Text = "-You need " + tempint.ToString("N0") + "mG of BioFuel to Explore!";
                HarvestUpdateFade.Begin();
            }
        }

        private void imgPlotActionPlant_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            canvasPDPlantAction.Visibility = System.Windows.Visibility.Visible;
            canvasPDExploreAction.Visibility = System.Windows.Visibility.Collapsed;
            canvasPDPlantInfo.Visibility = System.Windows.Visibility.Collapsed;
            canvasPDPlowAction.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void imgPlotActionPlowing_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            canvasPDExploreAction.Visibility = System.Windows.Visibility.Collapsed;
            canvasPDPlantAction.Visibility = System.Windows.Visibility.Collapsed;
            canvasPDPlantInfo.Visibility = System.Windows.Visibility.Collapsed;
            canvasPDPlowAction.Visibility = System.Windows.Visibility.Visible;
        }

        private void ShowPlantInfo(object sender, System.Windows.Input.GestureEventArgs e)
        {
            canvasPDExploreAction.Visibility = System.Windows.Visibility.Collapsed;
            canvasPDPlantAction.Visibility = System.Windows.Visibility.Collapsed;
            canvasPDPlantInfo.Visibility = System.Windows.Visibility.Visible;
            canvasPDPlowAction.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void canvasGarageIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //dt.Stop();
            if (GarageDataOpen == 0)
            {
                if (MenuOpen == 1)
                {
                    MenuOpen = 0;
                    //MenuBounceOut.Begin();
                }

                //SiloDataBounceIn.Begin();
                canvasGarage.Visibility = System.Windows.Visibility.Visible;
                GarageDataOpen = 1;
            }
            else if (GarageDataOpen == 1)
            {
                //SiloDataBounceOut.Begin();
                canvasGarage.Visibility = System.Windows.Visibility.Collapsed;
                GarageDataOpen = 0;
            }
            //dt.Start();
        }

        private void PopulatePlotDetails()
        {
            // Calculate Explore Price
            int tDistance = GetDistance(SelectedPlot.PlotGridLocation);
            int tExploreTotalPrice = GetPrice(tExplorer, tDistance);
            SelectedPlot.TheExplorePrice = tExploreTotalPrice.ToString() + " mG";

            double tExploreSpeed = 1 + (Convert.ToDouble(tExplorer.ExecuteTime) / 100);
            int tExploreTime = Convert.ToInt32(((tDistance * tDistance) / 4) / tExploreSpeed);
            TimeSpan tempExploreTime = new TimeSpan(0, 0, tExploreTime);
            SelectedPlot.TheExploreTime = tempExploreTime.ToString(@"hh\:mm\:ss");
            PreviousPlot.IsPlotSelected = 0;
            SelectedPlot.IsPlotSelected = 1;

            textBlockPlotGrowthRate.Foreground = new SolidColorBrush(Colors.White);
            SelectedPlot.PlotTypeFinal = PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + SelectedPlot.PlotTypeID).Attribute("PlotName").Value;
            SelectedPlot.PlotTypeFinalID = Convert.ToInt32(PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + SelectedPlot.PlotTypeID).Attribute("PlotTypeID").Value);
            //int tDefaultMaxSeedCount = Convert.ToInt32(PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + SelectedPlot.PlotTypeID).Element("MaxSeedCount").Value);
            //textBlockPlowBonusText.Text = "Max Plot Size increased by Test " + tDefaultMaxSeedCount;
            if (SelectedPlot.IsPlotMaxed == 1) textBlockPlowBonusText.Text = "Plot Growth Rate increased by " + Convert.ToDouble(SelectedPlot.CurrentSeedCount) / 10000;
            else
            {
                int NewMaxSeedCount = (tPlow.Efficiency * SelectedPlot.PlotTypeFinalID);
                double tMSC = NewMaxSeedCount;
                double tMSC2 = (tMSC / 1000);
                string NewMessage = "Max Plot Size increased by ";
                if (NewMaxSeedCount < 1000) { NewMessage += NewMaxSeedCount.ToString(); }
                else if (NewMaxSeedCount < 1000000) { NewMessage += tMSC2 + "k"; }
                else { NewMessage += (NewMaxSeedCount / 1000000) + "m"; }
                textBlockPlowBonusText.Text = NewMessage;
            }
            //textBlockPlotCoordinate.Text = "(" + SelectedPlot.PlotGridLocation + ")";

            if (SelectedPlot.Explored == 1)
            {
                if (SelectedPlot.PlotGrowthRateModified > 0.0)
                {
                    textBlockPlotGrowthRate.Foreground = new SolidColorBrush(Colors.Yellow);
                    textBlockPlotGrowthRate.Text = "x " + SelectedPlot.PlotGrowthRate + "* (" + SelectedPlot.PlotGrowthRateModified + ")";
                }
                else textBlockPlotGrowthRate.Text = "x " + SelectedPlot.PlotGrowthRate;

                textBlockPlotPlowCurLvl.Text = tPlow.Level.ToString();
                int PlowLvlREQ = ((((SelectedPlot.CurrentSeedCount / 10) - 25) / 75) + 1);
                textBlockPlotPlowLvlReq.Text = PlowLvlREQ.ToString();
                double tPlowSpeed = 1 + (Convert.ToDouble(tPlow.Speed) / 100);
                int tPlowTime = Convert.ToInt32((((SelectedPlot.CurrentSeedCount / 1000) - 1) * 37 + tDistance) / tPlowSpeed);

                if (tPlowTime < 10) tPlowTime = 10;
                TimeSpan tempTime = new TimeSpan(0, 0, tPlowTime);
                textBlockPlotPlowTime.Text = tempTime.ToString(@"hh\:mm\:ss");

                // Calculate Plow Price
                int tPlowTotalPrice = GetPrice(tPlow, tDistance);
                textBlockPlotPlowCost.Text = tPlowTotalPrice.ToString() + " mG";

                // Calculate Plant Price
                int tPlantTotalPrice = GetPrice(tPlow, tDistance);
                textBlockPlotPlantCost.Text = tPlantTotalPrice.ToString() + " mG";
                textBlockPlotPlantCurPlantLvl.Text = tDrill.Level.ToString();

                double tPlantSpeed = 1 + (Convert.ToDouble(tDrill.Speed) / 100);
                int tPlantTime = Convert.ToInt32((((SelectedPlot.CurrentSeedCount / 1000) - 1) * 37 + tDistance) / tPlantSpeed);
                if (tPlantTime < 10) tPlantTime = 10;
                tempTime = new TimeSpan(0, 0, tPlantTime);
                textBlockPlotPlantTime.Text = tempTime.ToString(@"hh\:mm\:ss");
                int PlowSizeREQ = 75 * (tPlow.Level - 1) + 25;
                if ((SelectedPlot.CurrentSeedCount / 1000) > PlowSizeREQ)
                {
                    textBlockPlotPlowLvlReqTitle.Foreground = new SolidColorBrush(Colors.Red);
                    textBlockPlotPlowLvlReq.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            else textBlockPlotGrowthRate.Text = "NA";


            if (SelectedPlot.ExploredAcres == SelectedPlot.Acres)
            {
                imgPlotActionExplore.Visibility = System.Windows.Visibility.Collapsed;
                imgPlotActionPlowing.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                imgPlotActionExplore.Visibility = System.Windows.Visibility.Visible;
                imgPlotActionPlowing.Visibility = System.Windows.Visibility.Collapsed;
            }

            // Planted Info
            if (SelectedPlot.Planted == 1)
            {
                canvasPDPlantInfo.Visibility = System.Windows.Visibility.Visible;
                canvasPDPlantAction.Visibility = System.Windows.Visibility.Collapsed;
                canvasPDExploreAction.Visibility = System.Windows.Visibility.Collapsed;

                imgPlotActionPlant.Visibility = System.Windows.Visibility.Collapsed;
                imgPlotActionExplore.Visibility = System.Windows.Visibility.Collapsed;
                imgPlotActionHarvest.Visibility = System.Windows.Visibility.Visible;
                canvasPDPlowAction.Visibility = System.Windows.Visibility.Collapsed;
                double tGR = (SelectedPlot.SeedGrowthRate * SelectedPlot.PlotFertilityRate * SelectedPlot.PlotGrowthRate * SelectedPlot.PlotTypeBonus);
                textBlockSPM.Text = (tGR * 60).ToString("N0");
                string tempSeedName = SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedPlot.CurSeedType).Attribute("Name").Value;
                double tempSeedValue = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedPlot.CurSeedType).Element("SeedValue").Value);
                double tempSeedValueMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedPlot.CurSeedType).Element("SeedValueModified").Value);
                textBlockSeedName.Text = " " + tempSeedName;
                textBlockValue.Text = (tempSeedValue + tempSeedValueMOD).ToString("C");
                if (tempSeedValueMOD > 0) textBlockValue.Foreground = new SolidColorBrush(Colors.Yellow);
                textBlockGrowthRate.Text = SelectedPlot.SeedGrowthRate.ToString();
                double SeedFortitude = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedPlot.CurSeedType).Element("Fortitude").Value);
                double SeedFortitudeMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedPlot.CurSeedType).Element("FortitudeModified").Value);
                TimeSpan TSFortitudeTime = new TimeSpan(0, Convert.ToInt32(SeedFortitude + SeedFortitudeMOD), 0);
                textBlockFTL.Text = TSFortitudeTime.ToString(@"hh\:mm\:ss");
                textBlockMPV.Text = ((tempSeedValue + tempSeedValueMOD) * SelectedPlot.CurrentSeedCount).ToString("C0");
                textBlockCPV.Text = ((tempSeedValue + tempSeedValueMOD) * SelectedPlot.CurrentSeedCount).ToString("C0");
                double tempSeedQuality = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedPlot.CurSeedType).Element("SeedQuality").Value);
                double tempSeedQualityMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + SelectedPlot.CurSeedType).Element("SeedQualityModified").Value);
                textBlockCPQ.Text = ((tempSeedQuality + tempSeedQualityMOD) * SelectedPlot.CurrentSeedCount).ToString("N0");

                // Change Flower Image when Plot Data is opened.
                Uri uri = new Uri(SelectedPlot.FlowerImage, UriKind.Relative);
                ImageSource imgsrc1 = new BitmapImage(uri);
                imgFlower.Source = imgsrc1;
            }
            else if (SelectedPlot.Explored == 1)
            {
                imgPlotActionPlant.Visibility = System.Windows.Visibility.Visible;
                imgPlotActionHarvest.Visibility = System.Windows.Visibility.Collapsed;
                imgPlotActionExplore.Visibility = System.Windows.Visibility.Collapsed;

                canvasPDPlantAction.Visibility = System.Windows.Visibility.Visible;
                canvasPDExploreAction.Visibility = System.Windows.Visibility.Collapsed;
                canvasPDPlantInfo.Visibility = System.Windows.Visibility.Collapsed;
                canvasPDPlowAction.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (SelectedPlot.Explorable == 1)
            {
                imgPlotActionExplore.Visibility = System.Windows.Visibility.Visible;
                canvasPDExploreAction.Visibility = System.Windows.Visibility.Visible;

                imgPlotActionPlant.Visibility = System.Windows.Visibility.Collapsed;
                canvasPDPlantAction.Visibility = System.Windows.Visibility.Collapsed;
                
                imgPlotActionPlowing.Visibility = System.Windows.Visibility.Collapsed;
                canvasPDPlowAction.Visibility = System.Windows.Visibility.Collapsed;

                imgPlotActionHarvest.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                canvasPDExploreAction.Visibility = System.Windows.Visibility.Collapsed;
                imgPlotActionPlowing.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private int GetPrice(Machine whichMachine, int howfar)
        {
            int tPrice = 0;
            int tBioFuelPrice = 0;
            howfar = Math.Abs(howfar / 15);
            if (whichMachine.Price == 10000) tBioFuelPrice = whichMachine.Price / 10000;
            else tBioFuelPrice = whichMachine.Price;
            tPrice = tBioFuelPrice + (tBioFuelPrice * howfar);
            //if (whichMachine.Name == "Plow" || whichMachine.Name == "Drill") tPrice = ((SelectedPlot.CurrentSeedCount / 1000) * (SelectedPlot.CurrentSeedCount / 5000)) + (tBioFuelPrice * howfar);

            return tPrice;
        }

        private void DeselectPlot()
        {
            // Hiding the PlotData bar if they are open.
            //PreviousButton.BorderBrush = PreviousSelectedButton.Background;
            //if (PlotDataOpen == 1) PlotDataBounceOut.Begin();
            canvasPlot.Visibility = System.Windows.Visibility.Collapsed;
            PlotDataOpen = 0;
        }

        void dt_Tick(object sender, EventArgs e)
        {
            long tYield = 0;
            double tTSPS = 0.0;
            int tYieldMax = 0;
            double tBasePGR = 0.0;
            double tTotalPGR = 0.0;
            FirstLBUpdate++;
            double tEmpire = 0.0;

            foreach (Plot currentPlot in SectorPlots)
            {
                if (currentPlot.Explored > 0 || currentPlot.Status > 0) // If Plot isn't explored, don't do anything.
                {
                    // Each Tick should be doing 1 of 5 things for each Plot.
                    // 1 - Is there Action on the Plot (Exploring, Plowing, Planting).
                    // 2 - Is the Plot Planted but not at Max (so the Plot is growing crop).
                    // 3 - The Plot is Planted and at Max so the Fortitude Countdown has started.
                    // 4 - The Fortitude CountDown has finished and the Plot is in a Decaying State.
                    // 5 - The Decay has finished and the plot is waiting to be cleaned up.

                    // LeaderBoard Stats
                    tYieldMax += currentPlot.CurrentSeedCount;
                    tBasePGR += currentPlot.BaseGrowthRate;
                    tTotalPGR += (currentPlot.PlotGrowthRate);
                    //tTotalPGR += (currentPlot.PlotGrowthRate + currentPlot.PlotGrowthRateModified);
                    if (tTotalPGR < 20) tEmpire += tTotalPGR;
                    else tEmpire += 20;

                    if (currentPlot.Status > 0) // If there is Action on the Plot Status will be greater then 0.
                    {
                        if (currentPlot.Status == 1) // Plot is currently being Explored.
                        {
                            TSInterval = currentPlot.ExploreDate - DateTime.Now;
                            if (TSInterval.TotalSeconds > 0)
                            {
                                long CurrentTotalSeconds = Convert.ToInt64(TSInterval.TotalSeconds);
                                string DisplayCD = string.Empty;
                                DisplayCD = TSInterval.ToString(@"hh\:mm\:ss");
                                currentPlot.ActionDisplayTime = DisplayCD;
                                //currentPlot.ActionTitle = "Exploring";
                                //currentPlot.PlotTitle = "";
                                currentPlot.ActionColor = (Brush)App.Current.Resources["ExploreBorder"];
                                currentPlot.PlotForeColor = new SolidColorBrush(Colors.White);
                                currentPlot.PlotStatus = "Exploring";
                            }
                            else
                            { // This Plot is currently being Explored.
                                currentPlot.Status = 3; // Explore Complete Status
                                currentPlot.PlotForeColor = new SolidColorBrush(Colors.White);
                                currentPlot.ActionColor = (Brush)App.Current.Resources["ExploreBlue"];
                                currentPlot.PlotBorderColor = (Brush)App.Current.Resources["ExploreBorder"];
                                currentPlot.ActionDisplayTime = "Done"; // Explore Complete Status
                                //currentPlot.ActionTitle = "Exploring";
                                currentPlot.PlotTitle = "";
                                currentPlot.PlotStatus = "";
                                CheckExploreTime(0, currentPlot); // Sets this Plot Status to 3 which is Exploring is Complete.
                            }
                        }
                        else if (currentPlot.Status == 2) // Plot is being Planted.
                        {
                            TSInterval = currentPlot.PlantCompleteDate - DateTime.Now;
                            if (TSInterval.TotalSeconds > 0)
                            {
                                long CurrentTotalSeconds = Convert.ToInt64(TSInterval.TotalSeconds);
                                string DisplayCD = string.Empty;
                                DisplayCD = TSInterval.ToString(@"hh\:mm\:ss");
                                currentPlot.ActionDisplayTime = DisplayCD;
                                currentPlot.PlotTitle = "";
                                currentPlot.ActionColor = (Brush)App.Current.Resources["PlantYellow"];
                                currentPlot.PlotBorderColor = (Brush)App.Current.Resources["PlantBorder"];
                                currentPlot.PlotForeColor = new SolidColorBrush(Colors.White);
                                currentPlot.PlotStatus = "Planting";
                            }
                            else
                            {
                                currentPlot.Status = 4; // Plant Complete Status
                                currentPlot.ActionDisplayTime = "Done"; // Plant Complete Status
                                currentPlot.ActionColor = (Brush)App.Current.Resources["PlantYellow"];
                                currentPlot.PlotBorderColor = (Brush)App.Current.Resources["PlantBorder"];
                                currentPlot.PlotTitle = "";
                                currentPlot.PlotStatus = "";
                                CheckPlantTime(0, currentPlot);
                            }
                        }
                        else if (currentPlot.Status == 3)
                        {
                            currentPlot.TimeCountDown = "Done"; // Explore Complete Status
                            currentPlot.PlotStatus = "";
                            currentPlot.PlotBorderColor = (Brush)App.Current.Resources["ExploreBorder"];
                        }
                        else if (currentPlot.Status == 4)
                        {
                            currentPlot.PlotStatus = "";
                            currentPlot.PlotColor = (Brush)App.Current.Resources["PlantYellow"];
                            currentPlot.PlotBorderColor = (Brush)App.Current.Resources["PlantBorder"];
                            currentPlot.TimeCountDown = "Done"; // Planting Complete Status
                        }
                        else if (currentPlot.Status == 5) // Plot is being Plowed.
                        {
                            TSInterval = currentPlot.PlowCompleteDate - DateTime.Now;
                            if (TSInterval.TotalSeconds > 0)
                            {
                                long CurrentTotalSeconds = Convert.ToInt64(TSInterval.TotalSeconds);
                                string DisplayCD = string.Empty;
                                DisplayCD = TSInterval.ToString(@"hh\:mm\:ss");
                                currentPlot.ActionDisplayTime = DisplayCD;
                                //currentPlot.PlotColor = (Brush)App.Current.Resources["PlowRed"];
                                currentPlot.ActionColor = (Brush)App.Current.Resources["PlowRed"];
                                currentPlot.PlotBorderColor = (Brush)App.Current.Resources["MainGreen"];
                                currentPlot.PlotStatus = "Plowing";
                            }
                            else
                            {
                                currentPlot.ActionDisplayTime = "Done";
                                currentPlot.PlotStatus = "";
                                //currentPlot.PlotColor = (Brush)App.Current.Resources["PlowRed"];
                                currentPlot.ActionColor = (Brush)App.Current.Resources["PlowRed"];
                                currentPlot.PlotBorderColor = (Brush)App.Current.Resources["MainGreen"];
                                CheckPlowTime(0, currentPlot); // Sets this Plot Status to 6 which is Exploring is Complete.
                            }
                        }
                        else if (currentPlot.Status == 6)
                        {
                            currentPlot.TimeCountDown = "Done"; // Plowing Complete Status
                            currentPlot.PlotStatus = "";
                            //currentPlot.PlotColor = (Brush)App.Current.Resources["PlowRed"];
                            currentPlot.ActionColor = (Brush)App.Current.Resources["PlowRed"];
                            currentPlot.PlotBorderColor = (Brush)App.Current.Resources["MainGreen"];
                        }
                    }
                    else
                    {
                        if (currentPlot.Planted == 1)
                        {
                            currentPlot.PlotStatus = "Growing";
                            currentPlot.ActionColor = new SolidColorBrush(Colors.Green);

                            // Check for Seed Growth Rate MOD
                            int tempSGR = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + currentPlot.SeedID).Element("SeedGrowthRate").Value);
                            int tempSGRMOD = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + currentPlot.SeedID).Element("SeedGrowthRateModified").Value);
                            if (currentPlot.SeedGrowthRate != (tempSGR + tempSGRMOD)) currentPlot.SeedGrowthRate = (tempSGR + tempSGRMOD);
                            currentPlot.MaxPlantTime = Convert.ToInt32(currentPlot.CurrentSeedCount / (currentPlot.SeedGrowthRate * currentPlot.PlotGrowthRate));
                            double tempSeedQuality = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + currentPlot.SeedID).Element("SeedQuality").Value);
                            double tempSeedQualityMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + currentPlot.SeedID).Element("SeedQualityModified").Value);

                            currentPlot.PlotTitle = "";
                            currentPlot.TimeCountDown = "";
                            
                            // LeaderBoard Stats.
                            if (currentPlot.SeedValue == 0) currentPlot.SeedValue = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + currentPlot.SeedID).Element("SeedValueModified").Value);
                            string tempSeedValue = SeedsXDoc.Element("AllSeeds").Element("Seed" + currentPlot.SeedID).Element("SeedValueModified").Value;
                            string tempSeedValueMOD = SeedsXDoc.Element("AllSeeds").Element("Seed" + currentPlot.SeedID).Element("SeedValueModified").Value;
                            tYield += Convert.ToInt64(Convert.ToDouble(currentPlot.CurrentSeedCount) * (Convert.ToDouble(currentPlot.SeedValue) + Convert.ToDouble(tempSeedValueMOD)));
                            tTSPS += (currentPlot.SeedGrowthRate * currentPlot.PlotFertilityRate * currentPlot.PlotGrowthRate);

                            if (currentPlot.IsPlotMaxed == 0)  // Is the Plot Planted but not at Max yet.
                            {
                                // Get Current CropCount.
                                TimeSpan TSInterval = DateTime.Now - currentPlot.SeedPlantDate;
                                int tSeconds = Convert.ToInt32(TSInterval.TotalSeconds);
                                long tempCount = GetHarvestCount(tSeconds, currentPlot);
                                int tCurSeedVol = 0;

                                // Update the canvasPlot Info
                                if (currentPlot.IsPlotSelected == 1)
                                {
                                    textBlockCurSeedCount.Text = tempCount.ToString("N0"); // Update the Crop Count TextBlock in canvasPlot Info
                                    int tGTMLeft = (currentPlot.MaxPlantTime - tSeconds);
                                    if (tGTMLeft < 1) tGTMLeft = 0;
                                    TimeSpan TStime = new TimeSpan(0, 0, tGTMLeft);
                                    textBlockGTM.Text = TStime.ToString(@"hh\:mm\:ss");
                                    textBlockCPV.Text = ((Convert.ToDouble(currentPlot.SeedValue) + Convert.ToDouble(tempSeedValueMOD)) * Convert.ToDouble(tempCount)).ToString("C");
                                    textBlockCPQ.Text = ((tempSeedQuality + tempSeedQualityMOD) * tempCount).ToString("N0");
                                }

                                if (tempCount == currentPlot.CurrentSeedCount) // Check to see if the Plot is Maxed.
                                {
                                    currentPlot.IsPlotMaxed = 1;
                                    tCurSeedVol = 100;
                                    currentPlot.MaxSeedCountSmall = "MAX";
                                    currentPlot.IsFortitudeActive = 1;
                                }
                                else
                                {
                                    double tSeedVol = Convert.ToDouble(tempCount) / Convert.ToDouble(currentPlot.CurrentSeedCount);
                                    tCurSeedVol = Convert.ToInt32(tSeedVol * 100);
                                    if (currentPlot.MaxSeedCountSmall != (currentPlot.CurrentSeedCount / 1000) + "k") currentPlot.MaxSeedCountSmall = (currentPlot.CurrentSeedCount / 1000) + "k";
                                }

                                // Change the Growth Progress bar for the Plot Growth.
                                currentPlot.CurrentSeedVolume = tCurSeedVol;
                            }
                            else if (currentPlot.IsFortitudeActive == 1) // Is the Plot Planted and Maxed but Fortitude has not been drained. IsFortitudeActive = 1 means its still Active. 2 = Means it is expired.
                            {
                                // Check if the Fortitude is still Active.
                                TimeSpan FortitudeInterval = currentPlot.FortitudeStartDate - DateTime.Now;
                                if (FortitudeInterval.TotalSeconds > 0)
                                {
                                    currentPlot.PlotStatus = "Fortitude";
                                    if (currentPlot.IsPlotSelected == 1)
                                    {
                                        textBlockCurSeedCount.Text = currentPlot.CurrentSeedCount.ToString("N0");
                                        textBlockFTL.Text = FortitudeInterval.ToString(@"hh\:mm\:ss");
                                    }

                                    currentPlot.ActionColor = new SolidColorBrush(Colors.Yellow);
                                    currentPlot.PlotForeColor = new SolidColorBrush(Colors.Black);
                                }
                                else
                                {
                                    currentPlot.DecayTotal = 0;
                                    currentPlot.IsFortitudeActive = 2; // Fortitude has been drained. Start the Decay.
                                }
                            }
                            else if (currentPlot.IsFortitudeActive == 2) // IsFortitudeActive = 2 means the Planted Seeds Fortitude has been drained and the plot is now decaying.
                            {
                                TimeSpan FortitudeInterval = currentPlot.FortitudeStartDate - DateTime.Now;
                                //int tDecayRate = Convert.ToInt32(LookUpChildValueXML(currentPlot.SeedName, "Seed", seedxml, "DecayRate"));
                                int tDecayRate = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + currentPlot.SeedID).Element("DecayRate").Value);
                                currentPlot.DecayTotal = Convert.ToInt64(Math.Abs(FortitudeInterval.TotalSeconds)) * tDecayRate;
                                long tCurrentSeedCount = currentPlot.CurrentSeedCount - currentPlot.DecayTotal;
                                if (tCurrentSeedCount < 0) tCurrentSeedCount = 0;

                                // Update the Count Progress Bar Also
                                currentPlot.MaxSeedCountSmall = "Decay";
                                double tSeedVol = Convert.ToDouble(tCurrentSeedCount) / Convert.ToDouble(currentPlot.CurrentSeedCount);
                                currentPlot.CurrentSeedVolume = Convert.ToInt32(tSeedVol * 100);
                                currentPlot.PlotStatus = "Decaying";

                                if (tCurrentSeedCount < currentPlot.CurrentSeedCount && tCurrentSeedCount != 0)
                                {
                                    if (currentPlot.IsPlotSelected == 1)
                                    {
                                        textBlockFTL.Text = "00:00:00";
                                        textBlockCurSeedCount.Text = tCurrentSeedCount.ToString("N0");
                                        textBlockCPV.Text = ((Convert.ToDouble(currentPlot.SeedValue) + Convert.ToDouble(tempSeedValueMOD)) * Convert.ToDouble(tCurrentSeedCount)).ToString("C");
                                        textBlockCPQ.Text = ((tempSeedQuality + tempSeedQualityMOD) * tCurrentSeedCount).ToString("N0");
                                    }
                                    currentPlot.ActionColor = new SolidColorBrush(Colors.Red);
                                    currentPlot.PlotForeColor = new SolidColorBrush(Colors.White);
                                }
                                else
                                {
                                    currentPlot.CurrentSeedVolume = 1;
                                    currentPlot.IsFortitudeActive = 3;
                                }
                                
                            }
                            else if (currentPlot.IsFortitudeActive == 3) // IsFortitudeActive = 3 means the Decay is finished and this plot has only 1 crop left.
                            {
                                currentPlot.MaxSeedCountSmall = "Dead";
                                currentPlot.PlotStatus = "Dead";
                                currentPlot.CurrentSeedVolume = 1;
                                currentPlot.DecayTotal = currentPlot.CurrentSeedCount - 1;
                                currentPlot.ActionColor = new SolidColorBrush(Colors.Black);
                                currentPlot.PlotForeColor = new SolidColorBrush(Colors.White);

                                if (currentPlot.IsPlotSelected == 1)
                                {
                                    int tCurrentSeedCount = 1;
                                    textBlockFTL.Text = "00:00:00";
                                    textBlockCurSeedCount.Text = "1";
                                    textBlockCPV.Text = ((Convert.ToDouble(currentPlot.SeedValue) + Convert.ToDouble(tempSeedValueMOD)) * Convert.ToDouble(tCurrentSeedCount)).ToString("C");
                                    textBlockCPQ.Text = ((tempSeedQuality + tempSeedQualityMOD) * tCurrentSeedCount).ToString("N0");
                                }
                            }
                        }
                    }
                }
            }

            // =============================================================================== //
            // LeaderBoard Count
            // =============================================================================== //

            tLeaderBoardTick++;
            textBlockUserYieldWorth.Text = tYield.ToString("C") + "f";
            textBlockUserTotalSPS.Text = tTSPS.ToString("N");
            textBlockUserYieldMax.Text = (tYieldMax / 1000) + "k";
            textBlockUserBasePGR.Text = tBasePGR.ToString();
            textBlockUserTotalPGR.Text = tTotalPGR.ToString();

            if (tLeaderBoardTick > 30 || FirstLBUpdate == 1)
            {
                int TIG = 0;
                if (FirstLBUpdate == 1) TIG = 1;
                else TIG = tLeaderBoardTick;

                UpdateLeaderBoard(tYield.ToString(), tTSPS.ToString(), tYieldMax.ToString(), tBasePGR.ToString(), tTotalPGR.ToString(), tEmpire.ToString(), TIG.ToString(), TheUser.MoneySpent.ToString(), TheUser.BioFuelUsed.ToString(), tExplorer.TotalEXP.ToString(), tDrill.TotalEXP.ToString(), tPlow.TotalEXP.ToString(), YourHarvestor.TotalEXP.ToString(), TheUser.MillEXP.ToString());
                tLeaderBoardTick = 0;
            }

            // =============================================================================== //
            // End LeaderBoard
            // =============================================================================== //

            // =============================================================================== //
            // Mill Processing
            // =============================================================================== //
            if (TheUser.IsMillProcessing == 1)
            {
                MillInterval = TheUser.MillCompleteDate - DateTime.Now;
                if (MillInterval.TotalSeconds > 0)
                {
                    textBlockMillProgressTimeLeft.Text = MillInterval.ToString(@"hh\:mm\:ss");
                    if (canvasProcessCountDown.Visibility == System.Windows.Visibility.Collapsed)
                    {
                        textBlockMillProgressCount.Text = TheUser.MillSeedProcessingCount.ToString();
                        textBlockMillProgressSeed.Text = SeedsXDoc.Element("AllSeeds").Element("Seed" + TheUser.MillCurrentSeed).Attribute("Name").Value + " seed";
                    }
                }
                else
                {
                    //borderImgMillIcon.BorderBrush = new SolidColorBrush(Colors.Red);
                    textBlockMillProgressTimeLeft.Text = "Done";
                    MessageList.Add(new Message(1, "Mill Status", "Your Mill is finished processing. Go to the Mill to add the new Seeds to the Granary.", "Images/Mill.jpg", "Collapsed", tDrill, 0, 0));
                    TheUser.IsMillProcessing = 0;
                    //ShakeObject.Begin();
                }

                canvasSeedProcessing.Visibility = System.Windows.Visibility.Collapsed;
                canvasProcessCountDown.Visibility = System.Windows.Visibility.Visible;
            }
            // =============================================================================== //
            // End Mill Processing
            // =============================================================================== //

            // =============================================================================== //
            // Refinery Timer
            // =============================================================================== //
            if (tRefinery.CropinRefinery > 0 && tRefinery.mG < tRefinery.MaxmG)
            {
                if (tRefinery.RefineryStatus != "Processing") { tRefinery.RefineryStatus = "Processing"; }
                if (tRefinery.CropinRefinery > tRefinery.CropPerSec)
                {
                    textBlockRefineryHarvestLeft.Foreground = new SolidColorBrush(Colors.Cyan); // Change color of the Refinery Status box depending on the Crop Left in the Refinery.
                    if (tRefinery.CropinRefinery < 10000) textBlockRefineryHarvestLeft.Foreground = new SolidColorBrush(Colors.Yellow); // Change color of the Refinery Status box depending on the Crop Left in the Refinery.
                    tRefinery.TotalCropRefined += tRefinery.CropPerSec;
                    tRefinery.CropinRefinery -= tRefinery.CropPerSec;
                }
                else
                {
                    if (tRefinery.CropinRefinery < 10000) textBlockRefineryHarvestLeft.Foreground = new SolidColorBrush(Colors.Yellow); // Change color of the Refinery Status box depending on the Crop Left in the Refinery.
                    textBlockRefineryHarvestLeft.Foreground = new SolidColorBrush(Colors.Red); // Change color of the Refinery Status box depending on the Crop Left in the Refinery.
                    tRefinery.TotalCropRefined += tRefinery.CropPerSec;
                    tRefinery.CropinRefinery = 0;
                }
                tRefinery.CropsinthisBatch += tRefinery.CropPerSec;

                //textBlockRefineryHarvest2Refine.Text = tRefinery.CropinRefinery.ToString("N0");
                if (tRefinery.CropinRefinery > 1000000)
                {
                    double tCRLeft = Convert.ToDouble(tRefinery.CropinRefinery) / 1000000;
                    textBlockRefineryHarvestLeft.Text = tCRLeft.ToString("N") + "m";
                }
                else textBlockRefineryHarvestLeft.Text = (tRefinery.CropinRefinery / 1000) + "k";

                BatchCounter--;
                RefineryBatchInterval = new TimeSpan(0, 0, BatchCounter);
                tRefinery.SecsPerBatchLeft = RefineryBatchInterval.ToString(@"hh\:mm\:ss");

                //int PreviousRefinedmG = tRefinery.RefinedmG;
                //tRefinery.RefinedmG = Convert.ToInt32((Math.Floor((double)(tRefinery.TotalCropRefined / tRefinery.CropPermG)) / tRefinery.OutputmG));
                //if (PreviousRefinedmG != tRefinery.RefinedmG)
                if (tRefinery.CropsinthisBatch > tRefinery.CropPermG)
                {
                    // Update Refinery XML
                    tRefinery.mG = tRefinery.mG + tRefinery.OutputmG;
                    tRefinery.mGDisplay = tRefinery.mG + " mG";
                    int CurrentRefinedmG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("miniGallonsRefined").Value);
                    //int NewRefinedmG = Convert.ToInt32((Math.Floor((double)(tRefinery.TotalCropRefined / tRefinery.CropPermG)) / tRefinery.OutputmG));
                    tRefinery.CropsinthisBatch = (tRefinery.CropsinthisBatch - tRefinery.CropPermG);
                    tRefinery.RefinedmG = (CurrentRefinedmG + tRefinery.OutputmG);
                    BatchCounter = (tRefinery.CropPermG / tRefinery.CropPerSec);
                    RefineryBatchInterval = new TimeSpan(0, 0, BatchCounter);
                    tRefinery.SecsPerBatchLeft = RefineryBatchInterval.ToString(@"hh\:mm\:ss");
                    
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("miniGallonsRefined", (CurrentRefinedmG + tRefinery.OutputmG));
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("mG", tRefinery.mG);
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("CropsinthisBatch", tRefinery.CropsinthisBatch);
                }

                int CurrentmG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("mG").Value);
                if (CurrentmG != tRefinery.mG) RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("mG", tRefinery.mG);

                //string tCRBucketName = "CropRefinedLvl" + tRefinery.Level;
                long CurrentCropRefined = Convert.ToInt64(RefineryXDoc.Element("Refinery").Element("Setting").Element("CropRefinedLvl1").Value);
                long CurrentCropinRefinery = Convert.ToInt64(RefineryXDoc.Element("Refinery").Element("Setting").Element("CropinRefinery").Value);
                RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("CropinRefinery", CurrentCropinRefinery - tRefinery.CropPerSec);
                RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("CropRefinedLvl1", CurrentCropRefined + tRefinery.CropPerSec);
                RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("LastRefineDate", DateTime.Now);
                refineryxml = helper.SaveUpdatedXML(RefineryXDoc, "Refinery.xml");
            }
            else 
            {
                textBlockRefineryHarvestLeft.Foreground = new SolidColorBrush(Colors.Red); // Change color of the Refinery Status box depending on the Crop Left in the Refinery.textBlockRefineryHarvestLeft
                if (tRefinery.CropinRefinery > 1000000)
                {
                    double tCRLeft = Convert.ToDouble(tRefinery.CropinRefinery) / 1000000;
                    textBlockRefineryHarvestLeft.Text = tCRLeft.ToString("N") + "m";
                }
                else textBlockRefineryHarvestLeft.Text = (tRefinery.CropinRefinery / 1000) + "k";

                if (tRefinery.mG >= tRefinery.MaxmG)
                {
                    tRefinery.IsRefineryMaxed = 1;
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("IsRefineryMaxed", tRefinery.IsRefineryMaxed);
                    refineryxml = helper.SaveUpdatedXML(RefineryXDoc, "Refinery.xml");

                    if (tRefinery.RefineryStatus != "Maxed")
                    {
                        tRefinery.RefineryStatus = "Maxed";
                    }
                }
            }
            // =============================================================================== //
            // End Refinery Timer
            // =============================================================================== //

            CheckYourMessages();
        }

        void CheckExploreTime(int status, Plot Plot2Update)
        {
            if (status == 1)
            {
                //int tPlotTypeIDFinal = Plot2Update.PlotTypeFinalID;
                //int tPlotTypeID = Plot2Update.PlotTypeID;
                int tDefaultMaxSeedCount = Convert.ToInt32(PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + Plot2Update.PlotTypeFinalID).Element("StartSeedCount").Value);
                UpdateAdjacentPlots(SectorXDoc, Plot2Update.TileNum);
                
                // Update User Explored Plots
                int CurrentExpPlots = Convert.ToInt32(UserXDoc.Element("UserInfo").Element("User").Element("CurrentExploredPlots").Value);
                UserXDoc.Element("UserInfo").Element("User").SetElementValue("CurrentExploredPlots", (CurrentExpPlots + 1));
                TheUser.CurrentExploredPlots++;
                userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");

                if (TheUser.CurrentExploredPlots == 1)
                {
                    // Find a Grass Seed
                    GranaryXDoc.Element("Granary").Element("Seed1").SetElementValue("TotalSeedCount", 1);
                    GranaryXDoc.Element("Granary").Element("Seed1").SetElementValue("Found", 1);
                    granaryxml = helper.SaveUpdatedXML(GranaryXDoc, "Granary.xml");
                    LoadGranaryDetails();
                    MessageList.Add(new Message(2, "New Seed Found", "A new Seed was found. \nType = Grass \nSeed Growth Rate = 14", "Images/Grass.jpg", "Collapsed", tDrill, 0, 0));
                }

                // Update Current Plot with new properties
                Plot2Update.Status = 0;
                Plot2Update.Explored = 1;
                //Plot2Update.TimeCountDown = "";
                //Plot2Update.PlotTitle = Plot2Update.PlotTypeFinal;
                Plot2Update.PlotColor = (Brush)App.Current.Resources[Plot2Update.PlotTypeFinal];
                Plot2Update.ActionColor = new SolidColorBrush();
                //Plot2Update.CurrentSeedCount = (Plot2Update.CurrentSeedCount + tDefaultMaxSeedCount);
                Plot2Update.CurrentSeedCount = tDefaultMaxSeedCount;
                if (Plot2Update.CurrentSeedCount < 1000) { Plot2Update.MaxSeedCountSmall = Plot2Update.CurrentSeedCount.ToString(); }
                else if (Plot2Update.CurrentSeedCount < 1000000) 
                {
                    double tMSC = Convert.ToDouble(Plot2Update.CurrentSeedCount);
                    double tMSC2 = Convert.ToDouble(tMSC / 1000);
                    Plot2Update.MaxSeedCountSmall = tMSC2 + "k"; 
                }
                else 
                {
                    double tMSC = Convert.ToDouble(Plot2Update.CurrentSeedCount);
                    double tMSC2 = Convert.ToDouble(tMSC / 1000000);
                    Plot2Update.MaxSeedCountSmall = tMSC2 + "m"; 
                }
                Plot2Update.FertilityText = GetFertilityText(Plot2Update.PlotGrowthRate);
                //Plot2Update.ActionIsVisible = "Collapsed";
                Plot2Update.ActionTitle = Plot2Update.MaxSeedCountSmall;
                Plot2Update.ActionDisplayTime = Plot2Update.MaxSeedCountSmall;
                Plot2Update.PlotTypeDisplay = Plot2Update.PlotTypeFinal;
                // Change Background Image
                Uri BGImage = new Uri("", UriKind.Relative);
                ImageSource BGImageSrc = new BitmapImage(BGImage);
                Plot2Update.PlotBackgroundImage = BGImageSrc;

                // Update FieldXML with the newly Explored Plot.
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("Status", "0");
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("Explored", "1");
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("MaxSeedCount", PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + Plot2Update.PlotTypeFinalID).Element("MaxSeedCount").Value);
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("CurrentSeedCount", (Plot2Update.CurrentSeedCount).ToString());
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("TimeCountDown", "");
                //SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("PlotTitle", Plot2Update.PlotTypeFinal);
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("PlotColor", Plot2Update.PlotTypeFinal);
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("MaxSeedCountSmall", Plot2Update.MaxSeedCountSmall);
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("FertilityText", GetFertilityText(Plot2Update.PlotGrowthRate));
                //SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("ActionIsVisible", "Collapsed");
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("ActionDisplayTime", Plot2Update.MaxSeedCountSmall);
                string tempsectorxml = SectorXDoc.ToString();
                helper.ManageFullMapXML(2, tempsectorxml);

                // Update Explore Stats
                int tDistance = GetDistance(Plot2Update.PlotGridLocation);
                double tExploreSpeed = 1 + (Convert.ToDouble(tExplorer.ExecuteTime) / 100);
                int tExploreTime = Convert.ToInt32(((tDistance * tDistance) / 4) / tExploreSpeed);
                int BeforeExploreLvl = tExplorer.Level;
                tExplorer.Available++;
                tExplorer.TotalEXP = tExplorer.TotalEXP + tExploreTime;
                tExplorer.Level = helper.GetLevel(3, tExplorer.TotalEXP);
                int AfterExploreLvl = tExplorer.Level;
                MachinesXDoc.Element("Machinery").Element("Machine" + tExplorer.ID).SetElementValue("Available", tExplorer.Available);
                MachinesXDoc.Element("Machinery").Element("Machine" + tExplorer.ID).SetElementValue("TotalEXP", tExplorer.TotalEXP);
                MachinesXDoc.Element("Machinery").Element("Machine" + tExplorer.ID).SetElementValue("Level", tExplorer.Level);
                machineryxml = helper.SaveUpdatedXML(MachinesXDoc, "Machine.xml");
                PopulateSkillsData();

                //textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Green);
                //textBlockHarvestUpdate.Text = "+Explored a " + Plot2Update.PlotTypeFinal + "!";
                //HarvestUpdateFade.Begin();
                int DidSkillGainLevel = 0;
                if (BeforeExploreLvl != AfterExploreLvl) { DidSkillGainLevel = 1; }
                MessageList.Add(new Message(1, "Explored New Plot", "A new Plot was explored. \nType = " + Plot2Update.PlotTypeFinal + " \nGrowth Rate = " + Plot2Update.PlotGrowthRate + " \nMax Seed Count = " + Plot2Update.CurrentSeedCount + " \nExplorer XP = +" + tExploreTime, "Images/CompassMap.jpg", "Visible", tExplorer, tExploreTime, DidSkillGainLevel));
                if (DidSkillGainLevel == 1)
                {
                    MessageList.Add(new Message(6, "Skill Level Up", "Exploring has gained a level. \nNew Level\n" + tExplorer.Level, "Images/CompassMap.jpg", "Visible", tExplorer, tExploreTime, DidSkillGainLevel));
                }

                // Check for Explore Bonus
                int ExploreBonus = 100;
                int BonusGroup = MyRandomNumber.Next(0, 101);

                //textBlockPlotBonus.Text = BonusGroup.ToString();

                int[] arrBonusPCT = new int[4] { -1, BaseExploreSeedFindPCT, BaseExploreSeedFindPCT + 15, BaseExploreSeedFindPCT + 30 };
                for (int k = 0; k < 4; k++)
                {
                    if (BonusGroup >= arrBonusPCT[k]) ExploreBonus = k;
                }

                if (ExploreBonus == 0) // User found a Seed Exploring
                {
                    // Add Bonus Seed to Granary for a successful exploring mission.
                    int SeedWinner = MyRandomNumber.Next(0, AvailableSeedsList.Count + 1);
                    string SeedWinnerName = AvailableSeedsName[SeedWinner];
                    int CurrentGranarySeedCount = Convert.ToInt32(GranaryXDoc.Element("Granary").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("TotalSeedCount").Value);
                    GranaryXDoc.Element("Granary").Element("Seed" + AvailableSeedsID[SeedWinner]).SetElementValue("TotalSeedCount", CurrentGranarySeedCount + 1);
                    GranaryXDoc.Element("Granary").Element("Seed" + AvailableSeedsID[SeedWinner]).SetElementValue("Found", 1);
                    granaryxml = helper.SaveUpdatedXML(GranaryXDoc, "Granary.xml");
                    LoadGranaryDetails();

                    // Remove the Found Seed from the List
                    //while (AvailableSeedsList.IndexOf(SeedWinnerName) > -1)
                    //{
                    //    AvailableSeedsList.Remove(SeedWinnerName);
                    //}
                    int tCurrentBonusEntries = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("BonusEntries").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).SetElementValue("BonusEntries", tCurrentBonusEntries + 1);
                    seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");
                    CreateSeedList(seedxml);

                    //textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Green);
                    //textBlockHarvestUpdate.Text = "+You found a " + SeedWinnerName + " seed!";
                    //HarvestUpdateFade.Begin();
                    int FoundSeedGrowthRate = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("SeedGrowthRate").Value) + Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("SeedGrowthRateModified").Value);
                    int tPlantableLevel = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("PlantableLevel").Value);
                    string tRarity = SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("Rarity").Value;
                    string FoundSeedImage = SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("FlowerImage").Value;
                    MessageList.Add(new Message(2, "Seed Found", "A Seed was found by your Explorer. \nType = " + tRarity + "\nName = " + SeedWinnerName + "\nLevel = " + tPlantableLevel + "\nSeed Growth Rate = " + FoundSeedGrowthRate, "Images/" + FoundSeedImage, "Collapsed", tDrill, 0, 0));
                }
                else if (ExploreBonus == 1) // User found some money while Exploring
                {
                    // Update User Currency
                    int MoneyWinner = MyRandomNumber.Next(1, 101) * 1000;
                    UserXDoc.Element("UserInfo").Element("User").SetElementValue("FreeCurrency", (MoneyWinner + TheUser.FreeCurrency));
                    TheUser.FreeCurrency = TheUser.FreeCurrency + MoneyWinner;
                    userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");
                    
                    //textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Green);
                    //textBlockHarvestUpdate.Text = "+You found " + MoneyWinner + "f!";
                    //HarvestUpdateFade.Begin();
                    MessageList.Add(new Message(4, "Currency Found", "Currency was found by your Explorer. \nAmount = " + MoneyWinner + "f", "Images/SilverCoin.jpg", "Collapsed", tDrill, 0, 0));
                }

                CheckYourMessages();
            }
            else if (status == 0)
            {
                // Update FieldXML with the newly Explored Plot.
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("Status", "3");
                Plot2Update.Status = 3;
            }
        }

        private void UpdateAdjacentPlots(XDocument CurrentSector, int CurrentTileNum)
        {
            // Check if the Tile directly south of this Tile is Owned by this Player.
            int TempSouthTile = CurrentTileNum + 10;
            if (TempSouthTile < 100)
            {
                int TempOwnerID = Convert.ToInt32(CurrentSector.Element("Sector").Element("Tile" + TempSouthTile).Element("OwnerID").Value);
                if ((TempOwnerID == 0) && (SectorPlots[TempSouthTile].Explored == 0) && TheUser.HomeTile != SectorPlots[TempSouthTile].PlotID)
                { 
                    SectorPlots[TempSouthTile].Explorable = 1;
                    SectorPlots[TempSouthTile].PlotTitle = "????";
                    SectorPlots[TempSouthTile].PlotColor = (Brush)App.Current.Resources["Explorable"];
                    SectorPlots[TempSouthTile].ExploreVisible = "Visible";
                    SectorPlots[TempSouthTile].ShowPlot4Exploring = "100";
                    Uri CurrentURI = new Uri("", UriKind.Relative);
                    ImageSource imgsrc1 = new BitmapImage(CurrentURI);
                    SectorPlots[TempSouthTile].PlotBackgroundImage = imgsrc1;
                }
            };
            // Check if the Tile directly North of this Tile is Owned by this Player   
            int TempNorthTile = CurrentTileNum - 10;
            if (TempNorthTile > 0)
            {
                int TempOwnerID = Convert.ToInt32(CurrentSector.Element("Sector").Element("Tile" + TempNorthTile).Element("OwnerID").Value);
                if ((TempOwnerID == 0) && (SectorPlots[TempNorthTile].Explored == 0) && (TheUser.HomeTile != SectorPlots[TempNorthTile].PlotID))
                { 
                    SectorPlots[TempNorthTile].Explorable = 1;
                    SectorPlots[TempNorthTile].PlotTitle = "????";
                    SectorPlots[TempNorthTile].PlotColor = (Brush)App.Current.Resources["Explorable"];
                    SectorPlots[TempNorthTile].ExploreVisible = "Visible";
                    SectorPlots[TempNorthTile].ShowPlot4Exploring = "100";
                    Uri CurrentURI = new Uri("", UriKind.Relative);
                    ImageSource imgsrc1 = new BitmapImage(CurrentURI);
                    SectorPlots[TempNorthTile].PlotBackgroundImage = imgsrc1;
                }
            };
            // Check if the Tile directly East of this Tile is Owned by this Player   
            int TempEastTile = CurrentTileNum + 1;
            if (TempEastTile < 100)
            {
                int TempOwnerID = Convert.ToInt32(CurrentSector.Element("Sector").Element("Tile" + TempEastTile).Element("OwnerID").Value);
                int CurrentTileRoundDown = Convert.ToInt32(Math.Floor(Convert.ToDouble(CurrentTileNum / 10)));
                int NextTileRoundDown = Convert.ToInt32(Math.Floor(Convert.ToDouble(TempEastTile / 10)));
                if (((TempOwnerID == 0) && (CurrentTileRoundDown == NextTileRoundDown)) && (SectorPlots[TempEastTile].Explored == 0) && TheUser.HomeTile != SectorPlots[TempEastTile].PlotID)
                { 
                    SectorPlots[TempEastTile].Explorable = 1;
                    SectorPlots[TempEastTile].PlotTitle = "????";
                    SectorPlots[TempEastTile].PlotColor = (Brush)App.Current.Resources["Explorable"];
                    SectorPlots[TempEastTile].ExploreVisible = "Visible";
                    SectorPlots[TempEastTile].ShowPlot4Exploring = "100";
                    Uri CurrentURI = new Uri("", UriKind.Relative);
                    ImageSource imgsrc1 = new BitmapImage(CurrentURI);
                    SectorPlots[TempEastTile].PlotBackgroundImage = imgsrc1;
                }
            };
            // Check if the Tile directly West of this Tile is Owned by this Player   
            int TempWestTile = CurrentTileNum - 1;
            if (TempWestTile > 0)
            {
                int TempOwnerID = Convert.ToInt32(CurrentSector.Element("Sector").Element("Tile" + TempWestTile).Element("OwnerID").Value);
                int CurrentTileRoundDown = Convert.ToInt32(Math.Floor(Convert.ToDouble(CurrentTileNum / 10)));
                int NextTileRoundDown = Convert.ToInt32(Math.Floor(Convert.ToDouble(TempWestTile / 10)));
                if (((TempOwnerID == 0) && (CurrentTileRoundDown == NextTileRoundDown)) && (SectorPlots[TempWestTile].Explored == 0) && TheUser.HomeTile != SectorPlots[TempWestTile].PlotID)
                { 
                    SectorPlots[TempWestTile].Explorable = 1;
                    SectorPlots[TempWestTile].PlotTitle = "????";
                    SectorPlots[TempWestTile].PlotColor = (Brush)App.Current.Resources["Explorable"];
                    SectorPlots[TempWestTile].ExploreVisible = "Visible";
                    SectorPlots[TempWestTile].ShowPlot4Exploring = "100";
                    Uri CurrentURI = new Uri("", UriKind.Relative);
                    ImageSource imgsrc1 = new BitmapImage(CurrentURI);
                    SectorPlots[TempWestTile].PlotBackgroundImage = imgsrc1;
                }
            };
        }

        private string GetFertilityText(double fertvalue)
        {
            double curNum = fertvalue;
            string curFertText = string.Empty;
            double[] arrFertNums = new double[12] { 0.25, 0.50, 0.75, 1, 2, 5, 8, 12.99, 20, 29.99, 49.99, 100 };
            string[] arrFertility = new string[12] { "Sterile", "Barren", "Infertile", "Plentiful", "Fertile", "Virile", "Superfluous", "Veracious", "Overgrown", "Jungly", "Dense", "Lush" };

            for (int i = 0; i < 9; i++)
            {
                if (curNum >= arrFertNums[i])
                {
                    curFertText = arrFertility[i];
                }
            }

            return curFertText;// +"(" + fertvalue + ")";
        }

        private void canvas_imgBioFuel(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            if (RefineryOpen == 0)
            {
                if (MenuOpen == 1)
                {
                    MenuOpen = 0;
                    canvasMenu.Visibility = System.Windows.Visibility.Collapsed;
                }

                canvasRefinery.Visibility = System.Windows.Visibility.Visible;
                RefineryOpen = 1;
                //RefineryBounceIn.Begin();
            }
            else if (RefineryOpen == 1)
            {
                canvasRefinery.Visibility = System.Windows.Visibility.Collapsed;
                RefineryOpen = 0;
                //RefineryBounceOut.Begin();
            }
            dt.Start();
        }

        private void Plow(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int tDistance = GetDistance(SelectedPlot.PlotGridLocation);
            int tPlowTotalPrice = GetPrice(tPlow, tDistance);
            double tPlowSpeed = 1 + (Convert.ToDouble(tPlow.Speed) / 100);
            int tPlowTime = Convert.ToInt32((((SelectedPlot.CurrentSeedCount / 1000) - 1) * 37 + tDistance) / tPlowSpeed);
            if (tPlowTime < 10) tPlowTime = 10;

            int PlowLvlREQ = ((((SelectedPlot.CurrentSeedCount / 10) - 25) / 75) + 1);

            if (tPlow.Level < PlowLvlREQ)
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                //textBlockHarvestUpdate.Text = "Your Plowing Lvl needs to be " + ((((SelectedPlot.CurrentSeedCount / 10) - 25) / 75) + 2) + " before you can plow this plot!";
                textBlockHarvestUpdate.Text = "Your Plowing Lvl needs to be " + PlowLvlREQ + " before you can plow this plot!";
                HarvestUpdateFade.Begin();
            }
            else if (SelectedPlot.Status != 0)
            {
                string tAction = "";
                if (SelectedPlot.Status == 2) tAction = "planted!";
                else if (SelectedPlot.Status == 5) tAction = "plowed!";
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                textBlockHarvestUpdate.Text = "This plot is already being " + tAction;
                HarvestUpdateFade.Begin();
            }
            else if (tPlow.Available == 0)
            {
                string tempAction = "You have no more Plows available.";
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                textBlockHarvestUpdate.Text = tempAction;
                HarvestUpdateFade.Begin();
            }
            else if (SelectedPlot.PlotName != null)
            {
                if (tPlow.Available == 0)
                {
                    string tempAction = "You have no more Plows available.";
                    textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Yellow);
                    textBlockHarvestUpdate.Text = tempAction;
                    HarvestUpdateFade.Begin();
                }
                else if (tRefinery.mG >= tPlowTotalPrice)
                {
                    DateTime rightnow = DateTime.Now;
                    TimeSpan duration = new TimeSpan(0, 0, 0, tPlowTime);
                    DateTime answer = rightnow.Add(duration);

                    // Update Plow XML
                    tPlow.UseCount += 1;
                    tPlow.Available -= 1;
                    MachinesXDoc.Element("Machinery").Element("Machine" + tPlow.ID).SetElementValue("Count", tPlow.UseCount);
                    MachinesXDoc.Element("Machinery").Element("Machine" + tPlow.ID).SetElementValue("Available", tPlow.Available);
                    machineryxml = helper.SaveUpdatedXML(MachinesXDoc, "Machine.xml");
                    PopulateSkillsData();

                    // Update Refinery
                    //UpdateRefinerymG(2, tExploreTotalPrice);
                    tRefinery.mG = tRefinery.mG - tPlowTotalPrice;
                    tRefinery.mGDisplay = tRefinery.mG + " mG";
                    int NewminiGallons = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("miniGallonsUsed").Value);
                    tRefinery.UsedmG = NewminiGallons + tPlowTotalPrice;
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("miniGallonsUsed", (NewminiGallons + tPlowTotalPrice));
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("mG", tRefinery.mG);
                    refineryxml = helper.SaveUpdatedXML(RefineryXDoc, "Refinery.xml");

                    // Update User Stats
                    int CurrentBFUsed = Convert.ToInt32(UserXDoc.Element("UserInfo").Element("User").Element("BioFuelUsed").Value);
                    UserXDoc.Element("UserInfo").Element("User").SetElementValue("BioFuelUsed", (CurrentBFUsed + tPlowTotalPrice));
                    TheUser.BioFuelUsed = (CurrentBFUsed + tPlowTotalPrice);
                    userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");

                    if (SelectedPlot.IsPlotMaxed == 1) 
                    {
                        SelectedPlot.WasPlotMaxed = 1;
                        SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("WasPlotMaxed", "1");
                    }

                    // Update FieldXML with the newly Explored Plot.
                    SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("Status", "5");
                    SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("PlowCompleteDate", answer.ToString());
                    SectorXDoc.Element("Sector").Element("Tile" + SelectedPlot.TileNum).SetElementValue("Planted", "0");
                    string tempsectorxml = SectorXDoc.ToString();
                    helper.ManageFullMapXML(2, tempsectorxml);

                    SelectedPlot.PlowCompleteDate = answer;
                    SelectedPlot.Status = 5;
                    SelectedPlot.SeedVolumeIsVisible = "Collapsed";
                    //SelectedPlot.FlowerImage = "";
                    SelectedPlot.Planted = 0;
                    //SelectedPlot.ActionTitle = "Plowing";
                    //SelectedPlot.ActionIsVisible = "Visible";
                    //SelectedPlot.PlotForeColor = "White";
                    //SelectedPlot.ActionTextBorderColor = "#FF8D0000";
                    //SelectedPlot.ActionTextColor = "#FF644343";
                    SelectedPlot.ActionDisplayTime = "--:--:--";
                    //SelectedPlot.PlotColor = (Brush)App.Current.Resources["PlowRed"];
                    SelectedPlot.ActionColor = (Brush)App.Current.Resources["PlowRed"];
                    SelectedPlot.PlotBorderColor = (Brush)App.Current.Resources["MainGreen"];
                    SelectedPlot.PlotStatus = "Plowing";
                    // Change Background Image
                    Uri BGImage = new Uri("Images/Plowing.jpg", UriKind.Relative);
                    ImageSource BGImageSrc = new BitmapImage(BGImage);
                    SelectedPlot.PlotBackgroundImage = BGImageSrc;

                    DeselectPlot();
                }
                else
                {
                    double tempint = tPlowTotalPrice - tRefinery.mG;
                    textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                    textBlockHarvestUpdate.Text = "-You need " + tempint.ToString("N0") + "mG of BioFuel to Plow!";
                    HarvestUpdateFade.Begin();
                }
            }
            else
            {
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                textBlockHarvestUpdate.Text = "Select a Plot before plowing!";
                HarvestUpdateFade.Begin();
            }
        }

        void CheckPlowTime(int WhichOne, Plot Plot2Update)
        {
            if (WhichOne == 1)
            {
                // Update User Plots Plowed
                int CurrentPlotsPlowed = Convert.ToInt32(UserXDoc.Element("UserInfo").Element("User").Element("PlotsPlowed").Value);
                UserXDoc.Element("UserInfo").Element("User").SetElementValue("PlotsPlowed", (TheUser.PlotsPlowed + 1));
                TheUser.PlotsPlowed++;
                userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");

                // Update Plow Stats
                int tPlowEXP = (Plot2Update.CurrentSeedCount / 10);
                //int tPlowEXP = tPlow.Efficiency * tPlow.Level;
                tPlow.Available++;
                tPlow.TotalEXP = tPlow.TotalEXP + tPlowEXP;
                int BeforePlowLvl = tPlow.Level;
                tPlow.Level = helper.GetLevel(4, tPlow.TotalEXP);
                int AfterPlowLvl = tPlow.Level;
                MachinesXDoc.Element("Machinery").Element("Machine" + tPlow.ID).SetElementValue("Available", tPlow.Available);
                MachinesXDoc.Element("Machinery").Element("Machine" + tPlow.ID).SetElementValue("TotalEXP", tPlow.TotalEXP);
                MachinesXDoc.Element("Machinery").Element("Machine" + tPlow.ID).SetElementValue("Level", tPlow.Level);
                machineryxml = helper.SaveUpdatedXML(MachinesXDoc, "Machine.xml");
                PopulateSkillsData();

                int tPlotTypeMaxSeedCount = Convert.ToInt32(PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + Plot2Update.PlotTypeFinalID).Element("MaxSeedCount").Value);

                //int tDefaultMaxSeedCount = Convert.ToInt32(PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + Plot2Update.PlotTypeFinalID).Element("StartSeedCount").Value);
                int tMaxSeedCount2Add = tPlow.Efficiency * Plot2Update.PlotTypeFinalID;
                double tPlowMaxPlotBonus = 0.0;

                // Find out if the user gets the Plot Growth Rate boost because they Plowed a Max Planted Plot.
                TimeSpan TSInterval = DateTime.Now - Plot2Update.SeedPlantDate;
                if (TSInterval.TotalDays < 100000)
                { 
                    int tSeconds = Convert.ToInt32(TSInterval.TotalSeconds);
                    long tempCount = GetHarvestCount(tSeconds, Plot2Update);
                    //if (tempCount == Plot2Update.CurrentSeedCount) tPlowMaxPlotBonus = Convert.ToDouble(Plot2Update.CurrentSeedCount) / Convert.ToDouble(10000);
                }
                
                if (Plot2Update.WasPlotMaxed == 1)
                {
                    tPlowMaxPlotBonus = Convert.ToDouble(Plot2Update.CurrentSeedCount) / Convert.ToDouble(10000);
                    Plot2Update.WasPlotMaxed = 0;
                    SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("WasPlotMaxed", "0");
                }
                double NewPlotGrowthRate = Plot2Update.PlotGrowthRate + Convert.ToDouble(tPlowMaxPlotBonus);

                // Update Current Plot with new properties
                Plot2Update.Status = 0;
                Plot2Update.Planted = 0;
                Plot2Update.SeedPlantDate = DateTime.Now;
                Plot2Update.SeedGrowthRate = 0;
                //Plot2Update.PlotTitle = Plot2Update.PlotTypeFinal;
                //Plot2Update.PlotColor = (Brush)App.Current.Resources[Plot2Update.PlotTypeFinal];
                Plot2Update.ActionColor = new SolidColorBrush();
                Plot2Update.ActionDisplayTime = "";
                Plot2Update.PlotFertilityRate = 0;
                Plot2Update.SeedVolumeIsVisible = "Collapsed";
                //int TempMaxSeedCount2Add = Plot2Update.CurrentSeedCount;
                //Plot2Update.PlotForeColor = LookUpChildValueXML(Plot2Update.PlotTypeFinal, "PlotType", plottypesxml, "PlotForeColor");
                if (tPlowMaxPlotBonus > 0.0) Plot2Update.PlotGrowthRateModified = (Plot2Update.PlotGrowthRateModified + tPlowMaxPlotBonus);
                else Plot2Update.CurrentSeedCount = Plot2Update.CurrentSeedCount + tMaxSeedCount2Add;
                //else Plot2Update.CurrentSeedCount = Plot2Update.CurrentSeedCount * 2;
                
                //Plot2Update.ActionIsVisible = "Collapsed";
                Plot2Update.SeedsPerSecond = "";
                Plot2Update.PlotGrowthRate = NewPlotGrowthRate;

                Uri BGImage = new Uri("", UriKind.Relative);
                ImageSource BGImageSrc = new BitmapImage(BGImage);
                Plot2Update.PlotBackgroundImage = BGImageSrc;

                if (Plot2Update.CurrentSeedCount < 1000) { Plot2Update.MaxSeedCountSmall = Plot2Update.CurrentSeedCount.ToString(); }
                else if (Plot2Update.CurrentSeedCount < 1000000) 
                {
                    double tMSC = Convert.ToDouble(Plot2Update.CurrentSeedCount);
                    double tMSC2 = Convert.ToDouble(tMSC / 1000);
                    Plot2Update.MaxSeedCountSmall = tMSC2 + "k"; 
                }
                else
                {
                    double tMSC = Convert.ToDouble(Plot2Update.CurrentSeedCount);
                    double tMSC2 = Convert.ToDouble(tMSC / 1000000);
                    Plot2Update.MaxSeedCountSmall = tMSC2 + "m";
                }
                Plot2Update.IsPlotMaxed = 0;
                Plot2Update.FertilityText = GetFertilityText(Plot2Update.PlotGrowthRate);
                Plot2Update.ActionTitle = "";
                Plot2Update.ActionDisplayTime = Plot2Update.MaxSeedCountSmall;
                string PlowedMessage = "";

                // Remove Plot Planted value
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("Status", "0");
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("CurSeedType", "0");
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("SeedPlantDate", "");
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("SeedGrowthRate", "0");
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("PlotGrowthRate", NewPlotGrowthRate.ToString("N"));
                if (tPlowMaxPlotBonus > 0.0) 
                { 
                    SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("PlotGrowthRateModified", (Plot2Update.PlotGrowthRateModified + tPlowMaxPlotBonus).ToString("N"));
                    PlowedMessage = " The Plot Growth Rate has been modified. \nPlot Growth Rate to Add = " + tPlowMaxPlotBonus;
                    PlowedMessage += "\nNew Plot Growth Rate = " + NewPlotGrowthRate;
                }
                else 
                {
                    SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("CurrentSeedCount", (Plot2Update.CurrentSeedCount).ToString());
                    SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("MaxSeedCountSmall", (Plot2Update.MaxSeedCountSmall).ToString());
                    PlowedMessage = " The Plot Max Seed Count has been modified. \nPrevious Plot Max Seed Count = " + (Plot2Update.CurrentSeedCount - tMaxSeedCount2Add);
                    PlowedMessage += "\nNew Plot Max Seed Count = " + Plot2Update.CurrentSeedCount;
                }
                PlowedMessage += "\nPlow XP = +" + tPlowEXP;

                // Check for Plot Upgrade
                if (Plot2Update.CurrentSeedCount > tPlotTypeMaxSeedCount)
                {
                    Plot2Update.PlotTypeFinalID++; // Upgrade plot to the next Plot Type
                    Plot2Update.PlotTypeID++;
                    Plot2Update.PlotTypeFinal = PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + Plot2Update.PlotTypeFinalID).Attribute("PlotName").Value;
                    Plot2Update.PlotTypeDisplay = PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + Plot2Update.PlotTypeFinalID).Attribute("PlotName").Value;
                    //Plot2Update.PlotBackgroundImage
                    SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("MaxSeedCount", PlotTypeXDoc.Element("AllPlotTypes").Element("PlotType" + Plot2Update.PlotTypeFinalID).Element("MaxSeedCount").Value);
                    SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("PlotTypeFinalID", Plot2Update.PlotTypeFinalID.ToString());
                    SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("PlotTypeID", Plot2Update.PlotTypeID.ToString());
                    SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("PlotTypeFinal", Plot2Update.PlotTypeFinal);
                    SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("PlotTypeDisplay", Plot2Update.PlotTypeDisplay);
                }

                string tempsectorxml = SectorXDoc.ToString();
                helper.ManageFullMapXML(2, tempsectorxml);

                // Show Message
                //textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Blue);
                //textBlockHarvestUpdate.Text = "You Plowed plot " + Plot2Update.PlotName + "!";
                //HarvestUpdateFade.Begin();
                int DidSkillGainLevel = 0;
                if (BeforePlowLvl != AfterPlowLvl) {DidSkillGainLevel = 1;}
                MessageList.Add(new Message(7, "Plot Plowed", "A new Plot has been plowed." + PlowedMessage, "Images/Plowing.jpg", "Visible", tPlow, tPlowEXP, DidSkillGainLevel));
                if (DidSkillGainLevel == 1)
                {
                    MessageList.Add(new Message(6, "Skill Level Up", "Plowing has gained a level. \nNew Level\n" + tPlow.Level, "Images/Plowing.jpg", "Visible", tPlow, tPlowEXP, DidSkillGainLevel));
                }

                

                // Check for Bonus
                int PlowBonus = 100;
                int BonusGroup = MyRandomNumber.Next(0, 101);
                int[] arrBonusPCT = new int[2] { -1, (tPlow.Level + BasePlowSeedFindPCT) };
                for (int k = 0; k < 2; k++)
                {
                    if (BonusGroup >= arrBonusPCT[k]) PlowBonus = k;
                }

                if (PlowBonus == 0) // User found a Seed While Plowing
                {
                    // Add Bonus Seed to Granary for a successful Plowing mission.
                    int SeedWinner = MyRandomNumber.Next(0, AvailableSeedsList.Count + 1);
                    string SeedWinnerName = AvailableSeedsName[SeedWinner];
                    int CurrentGranarySeedCount = Convert.ToInt32(GranaryXDoc.Element("Granary").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("TotalSeedCount").Value);
                    GranaryXDoc.Element("Granary").Element("Seed" + AvailableSeedsID[SeedWinner]).SetElementValue("TotalSeedCount", CurrentGranarySeedCount + 1);
                    GranaryXDoc.Element("Granary").Element("Seed" + AvailableSeedsID[SeedWinner]).SetElementValue("Found", 1);
                    granaryxml = helper.SaveUpdatedXML(GranaryXDoc, "Granary.xml");
                    LoadGranaryDetails();

                    //LoadGranaryDetails(granaryxml);

                    // Remove the Found Seed from the List
                    //while (AvailableSeedsList.IndexOf(SeedWinnerName) > -1)
                    //{
                    //    AvailableSeedsList.Remove(SeedWinnerName);
                    //}
                    int tCurrentBonusEntries = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("BonusEntries").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).SetElementValue("BonusEntries", tCurrentBonusEntries - 1);
                    seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");
                    CreateSeedList(seedxml);

                    //textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Green);
                    //textBlockHarvestUpdate.Text = "+You found a " + SeedWinnerName + " seed!";
                    //HarvestUpdateFade.Begin();
                    int FoundSeedGrowthRate = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("SeedGrowthRate").Value) + Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("SeedGrowthRateModified").Value);
                    int tPlantableLevel = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("PlantableLevel").Value);
                    string tRarity = SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("Rarity").Value;
                    string FoundSeedImage = SeedsXDoc.Element("AllSeeds").Element("Seed" + AvailableSeedsID[SeedWinner]).Element("FlowerImage").Value;
                    MessageList.Add(new Message(2, "Seed Found", "A Seed was found by your Plow. \nType = " + tRarity + "\nName = " + SeedWinnerName + "\nLevel = " + tPlantableLevel + "\nSeed Growth Rate = " + FoundSeedGrowthRate, "Images/" + FoundSeedImage, "Collapsed", tDrill, 0, 0));
                }

                CheckYourMessages();
                DeselectPlot();
            }
            else if (WhichOne == 0)
            {
                // Update FieldXML with the newly Plowed Plot.
                SectorXDoc.Element("Sector").Element("Tile" + Plot2Update.TileNum).SetElementValue("Status", "6");
                Plot2Update.Status = 6;
            }
        }

        private void LoadGranaryDetails()
        {
            List<Granary> tlistboxGranary = new List<Granary>();
            //List<ViewModel> items = new List<ViewModel>();
            //XmlReader tempFieldXML = XmlReader.Create(new StringReader(temp));
            tlistboxSeed2Plant = new List<Granary>();
            string tSeedName = string.Empty;
            string tUnknownSeedName = "??????";
            string tSeedImage = string.Empty;
            string tUnknownSeedImage = "Images/Question2.jpg";
            int tSeedCount = 0;
            int tPoints = 0;
            //int tSeedID = 0;
            //int Counter = 0;
            int FoundCounter = 0;
            int TotalGranarySeedCount = 0;
            string tForeColor = "#FF6D6E00";
            string tBackColor = "#FF0F1D00";
            string tGranaryPointsColor = "";
            int tPlantLvlReq = 0;
            //string tPointsSVMODString = string.Empty;
            //string tPointsSGRMODString = string.Empty;
            //string tPointsSQMODString = string.Empty;
            //string blah = string.Empty;
            //int Counter1 = 0;
            int TotalSeeds = Convert.ToInt32(GranaryXDoc.Element("Granary").Attribute("TotalSeeds").Value);

            for (int i = 1; i < (TotalSeeds+1); i++)
            {
                int ThisSeedCount = Convert.ToInt32(GranaryXDoc.Element("Granary").Element("Seed" + i).Element("TotalSeedCount").Value);
                int ThisSeedFound = Convert.ToInt32(GranaryXDoc.Element("Granary").Element("Seed" + i).Element("Found").Value);
                int ThisSeedID = Convert.ToInt32(GranaryXDoc.Element("Granary").Element("Seed" + i).Attribute("SeedID").Value);
                tSeedImage = "Images/" + SeedsXDoc.Element("AllSeeds").Element("Seed" + i).Element("FlowerImage").Value;

                if (ThisSeedCount == 0 && ThisSeedFound == 0)
                {
                    tSeedImage = tUnknownSeedImage;
                    tSeedName = tUnknownSeedName;
                }

                if (ThisSeedCount > 0)
                {
                    tSeedName = SeedsXDoc.Element("AllSeeds").Element("Seed" + i).Attribute("Name").Value;
                    tSeedImage = "Images/" + SeedsXDoc.Element("AllSeeds").Element("Seed" + i).Element("FlowerImage").Value;
                    TotalGranarySeedCount += ThisSeedCount;
                    tPlantLvlReq = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + ThisSeedID).Element("PlantableLevel").Value);
                    int tCurDrillLevel = helper.GetLevel(2, tDrill.TotalEXP);
                    if (tPlantLvlReq > tCurDrillLevel)
                    {
                        tForeColor = "#FF0F1D00";
                        tBackColor = "#FF6E0000";
                    }

                    tlistboxSeed2Plant.Add(new Granary(tSeedName, ThisSeedCount, tSeedImage, "Collapsed", tForeColor, tBackColor, tPlantLvlReq, ThisSeedID));

                    tForeColor = "#FF6D6E00";
                    tBackColor = "#FF0F1D00";

                }

                // Add the Granary Seeds to the Granary Slide View
                string testPoints = SeedsXDoc.Element("AllSeeds").Element("Seed" + ThisSeedID).Element("Points").Value;
                if (testPoints == "") { tPoints = 0; }
                else { tPoints = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + ThisSeedID).Element("Points").Value); }
                //Counter++;
                if (ThisSeedFound > 0) FoundCounter++; // Used to calculate the Total Seeds Found in the Granary

                // Add Granary Seed
                tGranaryPointsColor = "Collapsed";
                if (tPoints > 0) tGranaryPointsColor = "Visible";
                tlistboxGranary.Add(new Granary(tSeedName, ThisSeedCount, tSeedImage, tGranaryPointsColor, ThisSeedID, tPoints));
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                double tPCT = Convert.ToDouble(FoundCounter) / Convert.ToDouble(TotalSeeds);
                textBlockGranaryCollecting.Text = FoundCounter + " of " + TotalSeeds + " (" + tPCT.ToString("P") + ")";
                textBlockGranaryPlantableSeeds.Text = TotalGranarySeedCount.ToString();
                listboxGranary.ItemsSource = tlistboxGranary;
                listboxSeeds2Plant.ItemsSource = tlistboxSeed2Plant;
            });
        }

        private void imgGranary_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            if (GranaryOpen == 0)
            {
                if (MenuOpen == 1)
                {
                    MenuOpen = 0;
                    canvasMenu.Visibility = System.Windows.Visibility.Collapsed;
                }

                canvasGranary.Visibility = System.Windows.Visibility.Visible;
                GranaryOpen = 1;
                //GranaryBounceIn.Begin();
            }
            else if (GranaryOpen == 1)
            {
                canvasGranary.Visibility = System.Windows.Visibility.Collapsed;
                GranaryOpen = 0;
                //GranaryBounceOut.Begin();
            }
            dt.Start();
        }

        private void listboxGranary_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listboxGranary.SelectedIndex > -1)
            {
                PreviousSelectedGranarySeed.SelectedColor = "#FF340049";
                SelectedGranarySeed = (Granary)listboxGranary.SelectedItem;

                SelectedGranarySeed.SelectedColor = "#FFFF0000";
                LoadSeedDetails(SelectedGranarySeed.SeedName, SelectedGranarySeed.SeedID);
                PreviousSelectedGranarySeed = SelectedGranarySeed;
            }
        }

        private void LoadSeedDetails(string tSeedName, int tSeedID)
        {
            List<ViewModel> items = new List<ViewModel>();
            // Add the Granary Seeds to the Granary Slide View
            string tSeedImage = string.Empty;
            string tPoints = string.Empty;
            string tNewSeedGrowthRate = string.Empty;
            int tSeedGrowthRate = 0;
            int tSeedGrowthRateMOD = 0;
            string tNewSeedFortitude = string.Empty;
            int tSeedFortitude = 0;
            double tSeedFortitudeMOD = 0;
            string tNewSeedValue = string.Empty;
            double tSeedValue = 0.0;
            double tSeedValueMOD = 0.0;
            string tNewSeedQuality = string.Empty;
            double tSeedQuality = 0.0;
            double tSeedQualityMOD = 0.0;
            string tPlantLevel = string.Empty;
            string tSeedPlantedCount = string.Empty;
            string tSeedHarvestCount = string.Empty;
            string tPlantXP = string.Empty;
            string tPlantSize = string.Empty;
            string tPointsSVMODString = string.Empty;
            string tPointsSGRMODString = string.Empty;
            string tPointsSQMODString = string.Empty;
            string tPointsSFMODString = string.Empty;
            string tPointsSVMODShow = string.Empty;
            string tPointsSGRMODShow = string.Empty;
            string tPointsSQMODShow = string.Empty;
            string tPointsSFMODShow = string.Empty;
            string tSVValueForeColor = "White";
            string tSGRValueForeColor = "White";
            string tSQValueForeColor = "White";
            string tSFValueForeColor = "White";
            string tRarity = string.Empty;
            string tRarityFC = string.Empty;
            string SeedIDFormatted = string.Empty;

            if (tSeedName == "??????")
            {
                tSeedImage = "Images/Question2.jpg";
                tPlantLevel = tSeedName;
                tNewSeedGrowthRate = tSeedName;
                tNewSeedValue = tSeedName;
                tNewSeedQuality = tSeedName;
                tNewSeedFortitude = tSeedName;
                tSeedPlantedCount = tSeedName;
                tSeedHarvestCount = tSeedName;
                tPoints = "??";
                tPlantXP = tSeedName;
                tPlantSize = tSeedName;
                tRarity = tSeedName;
                tRarityFC = "White";
                tPointsSVMODString = "Points required: ??";
                tPointsSGRMODString = "Points required: ??";
                tPointsSQMODString = "Points required: ??";
                tPointsSFMODString = "Points required: ??";
            }
            else
            {
                tSeedImage = "Images/" + SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("FlowerImage").Value;
                tPoints = SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("Points").Value;
                tSeedGrowthRate = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("SeedGrowthRate").Value);
                tSeedGrowthRateMOD = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("SeedGrowthRateModified").Value);
                tNewSeedGrowthRate = (tSeedGrowthRate + tSeedGrowthRateMOD).ToString();
                tSeedValue = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("SeedValue").Value);
                tSeedValueMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("SeedValueModified").Value);
                tNewSeedValue = (tSeedValue + tSeedValueMOD).ToString("C");
                tSeedQuality = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("SeedQuality").Value);
                tSeedQualityMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("SeedQualityModified").Value);
                tNewSeedQuality = (tSeedQuality + tSeedQualityMOD).ToString("N");
                tSeedFortitude = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("Fortitude").Value);
                tSeedFortitudeMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("FortitudeModified").Value);
                tNewSeedFortitude = (tSeedFortitude + tSeedFortitudeMOD).ToString("N0");
                tPlantLevel = SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("PlantableLevel").Value;
                tSeedPlantedCount = SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("TotalPlantedCount").Value;
                tSeedHarvestCount = SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("TotalHarvestYield").Value;
                tPlantXP = SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("PlantedEXP").Value;
                tPlantSize = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("MaxPlantSize").Value).ToString("N0");
                if (tSeedValueMOD > 0) tSVValueForeColor = "Yellow";
                if (tSeedGrowthRateMOD > 0) tSGRValueForeColor = "Yellow";
                if (tSeedQualityMOD > 0) tSQValueForeColor = "Yellow";
                if (tSeedFortitudeMOD > 0) tSFValueForeColor = "Yellow";
                tRarity = SeedsXDoc.Element("AllSeeds").Element("Seed" + tSeedID).Element("Rarity").Value;
                if (tRarity == "Common") tRarityFC = "White";
                else if (tRarity == "Uncommon") tRarityFC = "Silver";
                else if (tRarity == "Scarce") tRarityFC = "Blue";
                else if (tRarity == "Rare") tRarityFC = "Green";
                else if (tRarity == "Legendary") tRarityFC = "Orange";
                else { tRarityFC = "Black"; }

                if (Convert.ToInt32(tPoints) >= PointsperSVMOD) tPointsSVMODString = "Tap to use " + PointsperSVMOD;
                else tPointsSVMODString = "Points required: " + PointsperSVMOD;
                if (Convert.ToInt32(tPoints) >= PointsperSGRMOD) tPointsSGRMODString = "Tap to use " + PointsperSGRMOD;
                else tPointsSGRMODString = "Points required: " + PointsperSGRMOD;
                if (Convert.ToInt32(tPoints) >= PointsperSQMOD) tPointsSQMODString = "Tap to use " + PointsperSQMOD;
                else tPointsSQMODString = "Points required: " + PointsperSQMOD;
                if (Convert.ToInt32(tPoints) >= PointsperSFMOD) tPointsSFMODString = "Tap to use " + PointsperSFMOD;
                else tPointsSFMODString = "Points required: " + PointsperSFMOD;

                if (tSeedValueMOD > 0) tPointsSVMODShow = "(" + tSeedValueMOD + ")";
                else tPointsSVMODShow = "";
                if (tSeedGrowthRateMOD > 0) tPointsSGRMODShow = "(" + tSeedGrowthRateMOD + ")";
                else tPointsSGRMODShow = "";
                if (tSeedQualityMOD > 0) tPointsSQMODShow = "(" + tSeedQualityMOD + ")";
                else tPointsSQMODShow = "";
                if (tSeedFortitudeMOD > 0) tPointsSFMODShow = "(" + tSeedFortitudeMOD + ")";
                else tPointsSFMODShow = "";
            }

            ViewModel model = new ViewModel()
            {
                Title = tSeedName,
                ImagePath = tSeedImage,
                ReqPlantingLevel = tPlantLevel.ToString(),
                SeedGrowthRate = tNewSeedGrowthRate,
                SeedGrowthRateMOD = tSeedGrowthRateMOD.ToString(),
                SeedValue = tNewSeedValue,
                SeedValueMOD = tSeedValueMOD.ToString(),
                SeedQuality = tNewSeedQuality,
                SeedQualityMOD = tSeedQualityMOD.ToString(),
                Fortitude = tNewSeedFortitude,
                FortitudeMOD = tSeedFortitudeMOD.ToString(),
                SeedIDFormatted = tSeedID.ToString() + "/28",
                SeedID = tSeedID,
                SeedPlantCount = tSeedPlantedCount.ToString(),
                SeedHarvestCount = tSeedHarvestCount.ToString(),
                Points = tPoints.ToString(),
                PlantXP = tPlantXP,
                PlantSize = tPlantSize,
                PlantCount = tSeedPlantedCount,
                PointsSVMODString = tPointsSVMODString,
                PointsSVMODNeeded = PointsperSVMOD,
                PointsSVMODForeColor = tSVValueForeColor,
                PointsSVMODShow = tPointsSVMODShow,
                PointsSGRMODString = tPointsSGRMODString,
                PointsSGRMODNeeded = PointsperSGRMOD,
                PointsSGRMODForeColor = tSGRValueForeColor,
                PointsSGRMODShow = tPointsSGRMODShow,
                PointsSQMODString = tPointsSQMODString,
                PointsSQMODNeeded = PointsperSQMOD,
                PointsSQMODForeColor = tSQValueForeColor,
                PointsSQMODShow = tPointsSQMODShow,
                PointsSFMODString = tPointsSFMODString,
                PointsSFMODNeeded = PointsperSFMOD,
                PointsSFMODForeColor = tSFValueForeColor,
                PointsSFMODShow = tPointsSFMODShow,
                Rarity = tRarity,
                RarityForeColor = tRarityFC
            };
            items.Add(model);
            listboxGranaryDetails.ItemsSource = items;
        }

        private void LoadSiloDetails()
        {
            List<Silo> tlistboxSilo = new List<Silo>();
            string tSeedName = string.Empty;
            string tFlowerImage = string.Empty;
            double tSeedValue = 0.0;
            double tSeedValueMOD = 0.0;
            double tSeedQuality = 0.0;
            double tSeedQualityMOD = 0.0;
            double tNewSeedValue = 0.0;
            double tNewSeedQuality = 0.0;
            int tSiloSeedCount = 0;
            double tSiloSeedWorth = 0.0;
            //string tValue = "";
            int TotalSeeds = Convert.ToInt32(SiloXDoc.Element("SeedSilo").Attribute("TotalSeeds").Value);

            for (int i = 1; i < (TotalSeeds + 1); i++)
            {
                int ThisSeedCrop = Convert.ToInt32(SiloXDoc.Element("SeedSilo").Element("Seed" + i).Element("TotalSeedCrop").Value);
                int ThisSeedID = Convert.ToInt32(SiloXDoc.Element("SeedSilo").Element("Seed" + i).Attribute("SeedID").Value);
                string ThisSeedName = SiloXDoc.Element("SeedSilo").Element("Seed" + i).Attribute("SeedName").Value;
                if (ThisSeedCrop > 0)
                {
                    tSeedValue = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + ThisSeedID).Element("SeedValue").Value);
                    tSeedValueMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + ThisSeedID).Element("SeedValueModified").Value);
                    tNewSeedValue = tSeedValue + tSeedValueMOD;
                    tSeedQuality = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + ThisSeedID).Element("SeedQuality").Value);
                    tSeedQualityMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + ThisSeedID).Element("SeedQualityModified").Value);
                    tNewSeedQuality = tSeedQuality + tSeedQualityMOD;
                    tFlowerImage = "Images/" + SeedsXDoc.Element("AllSeeds").Element("Seed" + ThisSeedID).Element("FlowerImage").Value;

                    tlistboxSilo.Add(new Silo(tSeedName, tNewSeedValue, ThisSeedCrop, ThisSeedID.ToString(), tFlowerImage, tNewSeedQuality));
                }
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                listboxSilo.ItemsSource = tlistboxSilo;
                textBlockSiloCount.Text = tSiloSeedCount.ToString("N0");
                textBlockSiloWorth.Text = tSiloSeedWorth.ToString("C");
            });
        }

        private void listboxGranaryDetails_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //MessageBox.Show("Changed");
        }

        private void IncreaseSeedValue(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            { 
                ViewModel CurGranarySeed = (ViewModel)listboxGranaryDetails.SelectedItem;
                if (Convert.ToInt32(CurGranarySeed.Points) >= PointsperSVMOD)
                {
                    // Manage the Seed Points
                    int OriginalPointsUsed = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("PointsUsed").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("Points", Convert.ToInt32(CurGranarySeed.Points) - PointsperSVMOD);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("PointsUsed", (OriginalPointsUsed + PointsperSVMOD));

                    // Increase the Seed Value
                    double tSeedValue = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedValue").Value);
                    double tSeedValueMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedValueModified").Value);
                    if (tSeedValueMOD == 0) tSeedValueMOD = SeedValueBonus;
                    else tSeedValueMOD += SeedValueBonus;
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("SeedValueModified", tSeedValueMOD);
                    seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");

                    // Show the updated Granary Details
                    CurGranarySeed.Points = (Convert.ToInt32(CurGranarySeed.Points) - PointsperSVMOD).ToString();
                    CurGranarySeed.SeedValueMOD = tSeedValueMOD.ToString();
                    CurGranarySeed.SeedValue = (Convert.ToDouble(tSeedValue + tSeedValueMOD)).ToString("C") + "*";
                    CurGranarySeed.PointsSVMODForeColor = "Yellow";

                    LoadSiloDetails();
                    LoadGranaryDetails();
                    LoadSeedDetails(CurGranarySeed.Title, CurGranarySeed.SeedID);
                }
                else
                {
                    textBlockNotEnoughPoints.Margin = new Thickness(35, 0, 0, 110);
                    sBNotEnoughPoints.Begin();
                }
            }
            catch
            {

            }
        }

        private void IncreaseSeedValue_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                listboxGranaryDetails.SelectedIndex = 0;

                ViewModel CurGranarySeed = (ViewModel)listboxGranaryDetails.SelectedItem;
                if (Convert.ToInt32(CurGranarySeed.Points) >= PointsperSVMOD)
                {
                    // Manage the Seed Points
                    int OriginalPointsUsed = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("PointsUsed").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("Points", Convert.ToInt32(CurGranarySeed.Points) - PointsperSVMOD);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("PointsUsed", (OriginalPointsUsed + PointsperSVMOD));

                    // Increase the Seed Value
                    double tSeedValue = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedValue").Value);
                    double tSeedValueMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedValueModified").Value);
                    if (tSeedValueMOD == 0) tSeedValueMOD = SeedValueBonus;
                    else tSeedValueMOD += SeedValueBonus;
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("SeedValueModified", tSeedValueMOD);
                    seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");

                    // Show the updated Granary Details
                    CurGranarySeed.Points = (Convert.ToInt32(CurGranarySeed.Points) - PointsperSVMOD).ToString();
                    CurGranarySeed.SeedValueMOD = tSeedValueMOD.ToString();
                    CurGranarySeed.SeedValue = (Convert.ToDouble(tSeedValue + tSeedValueMOD)).ToString("C") + "*";
                    CurGranarySeed.PointsSVMODForeColor = "Yellow";

                    LoadSiloDetails();
                    LoadGranaryDetails();
                    LoadSeedDetails(CurGranarySeed.Title, CurGranarySeed.SeedID);
                }
                else
                {
                    textBlockNotEnoughPoints.Margin = new Thickness(35, 0, 0, 110);
                    sBNotEnoughPoints.Begin();
                }
            }
            catch
            {

            }
        }

        private void IncreaseSeedGrowthRate(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                ViewModel CurGranarySeed = (ViewModel)listboxGranaryDetails.SelectedItem;
                if (Convert.ToInt32(CurGranarySeed.Points) >= PointsperSGRMOD)
                {
                    // Increase the Seed Value
                    int tSeedGrowthRate = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedGrowthRate").Value);
                    int tSeedGrowthRateMOD = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedGrowthRateModified").Value);
                    if (tSeedGrowthRateMOD == 0) tSeedGrowthRateMOD = SeedGrowthRateBonus;
                    else tSeedGrowthRateMOD += SeedGrowthRateBonus;

                    // Show the updated Granary Details
                    CurGranarySeed.Points = (Convert.ToInt32(CurGranarySeed.Points) - PointsperSGRMOD).ToString();
                    CurGranarySeed.SeedGrowthRateMOD = tSeedGrowthRateMOD.ToString();
                    CurGranarySeed.SeedGrowthRate = (Convert.ToInt32(tSeedGrowthRate + tSeedGrowthRateMOD)).ToString("N0") + "*";
                    CurGranarySeed.PointsSGRMODForeColor = "Yellow";

                    // Manage the Seed Points
                    int OriginalPointsUsed = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("PointsUsed").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("Points", CurGranarySeed.Points);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("PointsUsed", OriginalPointsUsed + PointsperSGRMOD);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("SeedGrowthRateModified", tSeedGrowthRateMOD);
                    seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");

                    LoadGranaryDetails();
                    LoadSeedDetails(CurGranarySeed.Title, CurGranarySeed.SeedID);
                }
                else
                {
                    textBlockNotEnoughPoints.Margin = new Thickness(275, 0, 0, 110);
                    sBNotEnoughPoints.Begin();
                }
            }
            catch
            {

            }
        }

        private void IncreaseSeedGrowthRate_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            listboxGranaryDetails.SelectedIndex = 0;

            try
            {
                ViewModel CurGranarySeed = (ViewModel)listboxGranaryDetails.SelectedItem;
                if (Convert.ToInt32(CurGranarySeed.Points) >= PointsperSGRMOD)
                {
                    // Increase the Seed Value
                    int tSeedGrowthRate = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedGrowthRate").Value);
                    int tSeedGrowthRateMOD = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedGrowthRateModified").Value);
                    if (tSeedGrowthRateMOD == 0) tSeedGrowthRateMOD = SeedGrowthRateBonus;
                    else tSeedGrowthRateMOD += SeedGrowthRateBonus;

                    // Show the updated Granary Details
                    CurGranarySeed.Points = (Convert.ToInt32(CurGranarySeed.Points) - PointsperSGRMOD).ToString();
                    CurGranarySeed.SeedGrowthRateMOD = tSeedGrowthRateMOD.ToString();
                    CurGranarySeed.SeedGrowthRate = (Convert.ToInt32(tSeedGrowthRate + tSeedGrowthRateMOD)).ToString("N0") + "*";
                    CurGranarySeed.PointsSGRMODForeColor = "Yellow";

                    // Manage the Seed Points
                    int OriginalPointsUsed = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("PointsUsed").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("Points", CurGranarySeed.Points);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("PointsUsed", OriginalPointsUsed + PointsperSGRMOD);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("SeedGrowthRateModified", tSeedGrowthRateMOD);
                    seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");

                    LoadGranaryDetails();
                    LoadSeedDetails(CurGranarySeed.Title, CurGranarySeed.SeedID);
                }
                else
                {
                    textBlockNotEnoughPoints.Margin = new Thickness(275, 0, 0, 110);
                    sBNotEnoughPoints.Begin();
                }
            }
            catch
            {

            }
        }

        private void IncreaseSeedQuality(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                ViewModel CurGranarySeed = (ViewModel)listboxGranaryDetails.SelectedItem;
                if (Convert.ToInt32(CurGranarySeed.Points) >= PointsperSQMOD)
                {
                    // Increase the Seed Value
                    double tSeedQuality = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedQuality").Value);
                    double tSeedQualityMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedQualityModified").Value);
                    if (tSeedQualityMOD == 0) tSeedQualityMOD = SeedQualityBonus;
                    else tSeedQualityMOD += SeedQualityBonus;

                    // Show the updated Granary Details
                    CurGranarySeed.Points = (Convert.ToInt32(CurGranarySeed.Points) - PointsperSQMOD).ToString();
                    CurGranarySeed.SeedValueMOD = tSeedQualityMOD.ToString();
                    CurGranarySeed.SeedValue = (Convert.ToDouble(tSeedQuality + tSeedQualityMOD)).ToString("C") + "*";
                    CurGranarySeed.PointsSQMODForeColor = "Yellow";

                    // Manage the Seed Points
                    int OriginalPointsUsed = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("PointsUsed").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("Points", CurGranarySeed.Points);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("PointsUsed", (OriginalPointsUsed + PointsperSQMOD));
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("SeedQualityModified", tSeedQualityMOD);
                    seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");

                    LoadSiloDetails();
                    LoadGranaryDetails();
                    LoadSeedDetails(CurGranarySeed.Title, CurGranarySeed.SeedID);
                }
                else
                {
                    textBlockNotEnoughPoints.Margin = new Thickness(500, 0, 0, 110);
                    sBNotEnoughPoints.Begin();
                }
            }
            catch { }
        }

        private void IncreaseSeedQuality_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            listboxGranaryDetails.SelectedIndex = 0;

            try
            {
                ViewModel CurGranarySeed = (ViewModel)listboxGranaryDetails.SelectedItem;
                if (Convert.ToInt32(CurGranarySeed.Points) >= PointsperSQMOD)
                {
                    // Increase the Seed Value
                    double tSeedQuality = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedQuality").Value);
                    double tSeedQualityMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("SeedQualityModified").Value);
                    if (tSeedQualityMOD == 0) tSeedQualityMOD = SeedQualityBonus;
                    else tSeedQualityMOD += SeedQualityBonus;

                    // Show the updated Granary Details
                    CurGranarySeed.Points = (Convert.ToInt32(CurGranarySeed.Points) - PointsperSQMOD).ToString();
                    CurGranarySeed.SeedValueMOD = tSeedQualityMOD.ToString();
                    CurGranarySeed.SeedValue = (Convert.ToDouble(tSeedQuality + tSeedQualityMOD)).ToString("C") + "*";
                    CurGranarySeed.PointsSQMODForeColor = "Yellow";

                    // Manage the Seed Points
                    int OriginalPointsUsed = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("PointsUsed").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("Points", CurGranarySeed.Points);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("PointsUsed", (OriginalPointsUsed + PointsperSQMOD));
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("SeedQualityModified", tSeedQualityMOD);

                    LoadSiloDetails();
                    LoadGranaryDetails();
                    LoadSeedDetails(CurGranarySeed.Title, CurGranarySeed.SeedID);
                }
                else
                {
                    textBlockNotEnoughPoints.Margin = new Thickness(500, 0, 0, 110);
                    sBNotEnoughPoints.Begin();
                }
            }
            catch { }
        }

        private void IncreaseSeedFortitude(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                ViewModel CurGranarySeed = (ViewModel)listboxGranaryDetails.SelectedItem;
                if (Convert.ToInt32(CurGranarySeed.Points) >= PointsperSFMOD)
                {
                    // Manage the Seed Points
                    int OriginalPointsUsed = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("PointsUsed").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("Points", Convert.ToInt32(CurGranarySeed.Points) - PointsperSFMOD);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("PointsUsed", OriginalPointsUsed + PointsperSFMOD);

                    // Increase the Seed Value
                    double tSeedFortitude = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("Fortitude").Value);
                    double tSeedFortitudeMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("FortitudeModified").Value);
                    if (tSeedFortitudeMOD == 0) tSeedFortitudeMOD = SeedFortitudeBonus;
                    else tSeedFortitudeMOD += SeedFortitudeBonus;
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("FortitudeModified", tSeedFortitudeMOD);
                    seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");

                    // Show the updated Granary Details
                    CurGranarySeed.Points = (Convert.ToInt32(CurGranarySeed.Points) - PointsperSFMOD).ToString();
                    CurGranarySeed.SeedValueMOD = tSeedFortitudeMOD.ToString();
                    CurGranarySeed.SeedValue = (Convert.ToDouble(tSeedFortitude + tSeedFortitudeMOD)).ToString("C") + "*";
                    CurGranarySeed.PointsSFMODForeColor = "Yellow";

                    LoadSiloDetails();
                    LoadGranaryDetails();
                    LoadSeedDetails(CurGranarySeed.Title, CurGranarySeed.SeedID);
                }
                else
                {
                    textBlockNotEnoughPoints.Margin = new Thickness(500, 0, 0, 10);
                    sBNotEnoughPoints.Begin();
                }
            }
            catch { }
        }

        private void IncreaseSeedFortitude_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            listboxGranaryDetails.SelectedIndex = 0;

            try
            {
                ViewModel CurGranarySeed = (ViewModel)listboxGranaryDetails.SelectedItem;
                if (Convert.ToInt32(CurGranarySeed.Points) >= PointsperSFMOD)
                {
                    // Manage the Seed Points
                    int OriginalPointsUsed = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("PointsUsed").Value);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("Points", Convert.ToInt32(CurGranarySeed.Points) - PointsperSFMOD);
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("PointsUsed", (OriginalPointsUsed + PointsperSFMOD));

                    // Increase the Seed Value
                    double tSeedFortitude = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("Fortitude").Value);
                    double tSeedFortitudeMOD = Convert.ToDouble(SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).Element("FortitudeModified").Value);
                    if (tSeedFortitudeMOD == 0) tSeedFortitudeMOD = SeedFortitudeBonus;
                    else tSeedFortitudeMOD += SeedFortitudeBonus;
                    SeedsXDoc.Element("AllSeeds").Element("Seed" + CurGranarySeed.SeedID).SetElementValue("FortitudeModified", tSeedFortitudeMOD);
                    seedxml = helper.SaveUpdatedXML(SeedsXDoc, "Seeds.xml");

                    // Show the updated Granary Details
                    CurGranarySeed.Points = (Convert.ToInt32(CurGranarySeed.Points) - PointsperSFMOD).ToString();
                    CurGranarySeed.SeedValueMOD = tSeedFortitudeMOD.ToString();
                    CurGranarySeed.SeedValue = (Convert.ToDouble(tSeedFortitude + tSeedFortitudeMOD)).ToString("C") + "*";
                    CurGranarySeed.PointsSFMODForeColor = "Yellow";

                    LoadSiloDetails();
                    LoadGranaryDetails();
                    LoadSeedDetails(CurGranarySeed.Title, CurGranarySeed.SeedID);
                }
                else
                {
                    textBlockNotEnoughPoints.Margin = new Thickness(500, 0, 0, 10);
                    sBNotEnoughPoints.Begin();
                }
            }
            catch { }
        }

        private void SiloSell_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (listboxSilo.SelectedIndex > -1)
            {
                int tSeedCount = SelectedSiloFlower.SeedCount;
                string tSeedName = SelectedSiloFlower.SeedName;
                // Don't need these Seed Value MOD cause it is done when the Silo is Loaded.
                double tSeedValue = SelectedSiloFlower.SeedValue;
                double Currency2Add = Convert.ToDouble(tSeedCount) * tSeedValue;

                TheUser.FreeCurrency = TheUser.FreeCurrency + Currency2Add;
                UserXDoc.Element("UserInfo").Element("User").SetElementValue("FreeCurrency", (Currency2Add + TheUser.FreeCurrency));
                userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");
                //textBlockFreeCurrency.Text = tUser.FreeCurrency.ToString("C0") + "f";

                SiloSeedActions(2, SelectedSiloFlower.SeedID, tSeedCount);
                LoadSiloDetails();

                // Clear current text
                textBlockSiloSeedNameSell.Text = string.Empty;
                textBlockSiloSeedCountSell.Text = string.Empty;
                textBlockSiloSeedWorthSell.Text = string.Empty;
                textBlockSiloSeedTotalSell.Text = string.Empty;
                Uri uri = new Uri("", UriKind.Relative);
                ImageSource imgsrc1 = new BitmapImage(uri);
                imgSellFlower.Source = imgsrc1;

                textBlockSiloSeedNameSend.Text = string.Empty;
                textBlockSiloSeedCountSend.Text = string.Empty;
                textBlockSiloSeedWorthSend.Text = string.Empty;
                textBlockSiloSeedTotalSend.Text = string.Empty;
                imgSendFlower.Source = imgsrc1;
            }
        }

        private void SiloSeedActions(int WhichOne, int TheSeedID, long TheSeedCount)
        {
            // Add and Remove Seeds from the Silo
            long CurrentSeedCrop = Convert.ToInt64(SiloXDoc.Element("SeedSilo").Element("Seed" + TheSeedID).Element("TotalSeedCrop").Value);
            long NewSeedCrop = 0;
            if (WhichOne == 1)
            {
                NewSeedCrop = CurrentSeedCrop + TheSeedCount;
            }
            else if (WhichOne == 2)
            {
                NewSeedCrop = CurrentSeedCrop - TheSeedCount;
            }
            SiloXDoc.Element("SeedSilo").Element("Seed" + TheSeedID).SetElementValue("TotalSeedCrop", NewSeedCrop);
            siloxml = helper.SaveUpdatedXML(SiloXDoc, "Silo.xml");
        }

        private void canvasSiloIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            if (SiloDataOpen == 0)
            {
                if (MenuOpen == 1)
                {
                    MenuOpen = 0;
                    canvasMenu.Visibility = System.Windows.Visibility.Collapsed;
                }

                //SiloDataBounceIn.Begin();
                canvasSilo.Visibility = System.Windows.Visibility.Visible;
                SiloDataOpen = 1;
            }
            else if (SiloDataOpen == 1)
            {
                //SiloDataBounceOut.Begin();
                canvasSilo.Visibility = System.Windows.Visibility.Collapsed;
                SiloDataOpen = 0;
            }
            dt.Start();
        }

        private void canvasMillIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            if (MillOpen == 0)
            {
                if (MenuOpen == 1)
                {
                    MenuOpen = 0;
                    canvasMenu.Visibility = System.Windows.Visibility.Collapsed;
                }

                radNumericUpDown.MaxValue = 0;
                //MillDataBounceIn.Begin();
                canvasMill.Visibility = System.Windows.Visibility.Visible;
                MillOpen = 1;
            }
            else if (MillOpen == 1)
            {
                //MillDataBounceOut.Begin();
                canvasMill.Visibility = System.Windows.Visibility.Collapsed;
                MillOpen = 0;
                listboxMill.SelectedIndex = -1;
                textBlockMillSeedsAvailable.Text = "Select a seed...";
                textBlockMillRequired.Text = "Select a seed...";
                textBlockRefineryScraps.Text = "0 cP";
            }
            dt.Start();
        }

        private void GoToMill(object sender, System.Windows.Input.GestureEventArgs e)
        {
            canvasSiloIcon_Tap(sender, e);
            canvasMillIcon_Tap(sender, e);
        }

        private void SiloAction_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Border TempBorder = (Border)sender;
            TextBlock TempTB = (TextBlock)TempBorder.Child;

            if (TempTB.Text == "Tap to Sell") SiloSell_Tap(sender, e);
            if (TempTB.Text == "Send to Mill") Send2Mill_Tap(sender, e);
        }

        private void Send2Mill_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (listboxSilo.SelectedIndex > -1)
            {
                // Remove Seeds in Silo
                double tSeedValue = SelectedSiloFlower.SeedValue;
                int tSeedCount = SelectedSiloFlower.SeedCount;
                string tSeedName = SelectedSiloFlower.SeedName;
                double tSeedQuality = SelectedSiloFlower.SeedQuality;
                int tNewSeedCount = Convert.ToInt32(tSeedCount * tSeedQuality);

                SiloSeedActions(2, SelectedSiloFlower.SeedID, tSeedCount);
                LoadSiloDetails();

                // Add Seeds to Mill
                int CurrentTotalSeedCrop = Convert.ToInt32(MillXDoc.Element("Mill").Element("Seed" + SelectedSiloFlower.SeedID).Element("TotalSeedCrop").Value);
                MillXDoc.Element("Mill").Element("Seed" + SelectedSiloFlower.SeedID).SetElementValue("TotalSeedCrop", (CurrentTotalSeedCrop + tNewSeedCount));
                millxml = helper.SaveUpdatedXML(MillXDoc, "Mill.xml");
                LoadMillDetails();

                textBlockSiloSeedNameSell.Text = string.Empty;
                textBlockSiloSeedCountSell.Text = string.Empty;
                textBlockSiloSeedWorthSell.Text = string.Empty;
                textBlockSiloSeedTotalSell.Text = string.Empty;
                Uri uri = new Uri("", UriKind.Relative);
                ImageSource imgsrc1 = new BitmapImage(uri);
                imgSellFlower.Source = imgsrc1;

                textBlockSiloSeedNameSend.Text = string.Empty;
                textBlockSiloSeedCountSend.Text = string.Empty;
                textBlockSiloSeedWorthSend.Text = string.Empty;
                textBlockSiloSeedTotalSend.Text = string.Empty;
                imgSendFlower.Source = imgsrc1;
            }
        }

        private void LoadMillDetails()
        {
            List<Mill> tlistboxMill = new List<Mill>();
            string tSeedName = string.Empty;
            string tSeedImage = string.Empty;
            int tCropCount = 0;
            int tSeedCount = 0;
            int tHarvest2Seed = 0;

            int TotalSeeds = Convert.ToInt32(MillXDoc.Element("Mill").Attribute("TotalSeeds").Value);

            for (int i = 1; i < (TotalSeeds + 1); i++)
            {
                int ThisSeedCrop = Convert.ToInt32(MillXDoc.Element("Mill").Element("Seed" + i).Element("TotalSeedCrop").Value);
                int ThisSeedID = Convert.ToInt32(MillXDoc.Element("Mill").Element("Seed" + i).Attribute("SeedID").Value);
                string ThisSeedName = SiloXDoc.Element("SeedSilo").Element("Seed" + i).Attribute("SeedName").Value;
                if (ThisSeedCrop > 0)
                {
                    tCropCount = Convert.ToInt32(MillXDoc.Element("Mill").Element("Seed" + ThisSeedID).Element("TotalSeedCrop").Value);
                    tHarvest2Seed = Convert.ToInt32(MillXDoc.Element("Mill").Element("Seed" + ThisSeedID).Element("Harvest2Seed").Value);
                    tSeedCount = Convert.ToInt32(GranaryXDoc.Element("Granary").Element("Seed" + i).Element("TotalSeedCount").Value);
                    tSeedImage = "Images/" + MillXDoc.Element("Mill").Element("Seed" + ThisSeedID).Element("FlowerImage").Value;

                    tlistboxMill.Add(new Mill(ThisSeedID, ThisSeedName, ThisSeedCrop, tSeedImage, tHarvest2Seed, tSeedCount));
                }
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                listboxMill.ItemsSource = tlistboxMill;
                radNUDMillSend2Refinery.MaxValue = 0;
            });
        }

        private void listboxSilo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedSiloFlower = (Silo)listboxSilo.SelectedItem;
            if (listboxSilo.SelectedIndex > -1)
            {
                double tSeedValue = SelectedSiloFlower.SeedValue;
                int tSeedCount = SelectedSiloFlower.SeedCount;
                //double tSeedValueMOD = Convert.ToDouble(LookUpChildValueXML(SelectedSiloFlower.SeedName, "Seed", seedxml, "SeedValueModified"));
                //double tNewSeedValue = tSeedValue + tSeedValueMOD;

                Uri uri = new Uri(SelectedSiloFlower.FlowerImage, UriKind.Relative);
                ImageSource imgsrc1 = new BitmapImage(uri);
                imgSellFlower.Source = imgsrc1;
                textBlockSiloSeedNameSell.Text = SelectedSiloFlower.SeedName;
                textBlockSiloSeedCountSell.Text = SelectedSiloFlower.SeedCount.ToString();
                textBlockSiloSeedWorthSell.Text = "x " + tSeedValue.ToString("C") + "f";
                textBlockSiloSeedTotalSell.Text = (SelectedSiloFlower.SeedCount * tSeedValue).ToString("C");

                imgSendFlower.Source = imgsrc1;
                textBlockSiloSeedNameSend.Text = SelectedSiloFlower.SeedName;
                textBlockSiloSeedCountSend.Text = SelectedSiloFlower.SeedCount.ToString();
                textBlockSiloSeedWorthSend.Text = "x " + Math.Round(SelectedSiloFlower.SeedQuality, 2).ToString("N");
                textBlockSiloSeedTotalSend.Text = (SelectedSiloFlower.SeedCount * SelectedSiloFlower.SeedQuality).ToString("N0");
            }
        }

        private void listboxMill_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedMillSeed = (Mill)listboxMill.SelectedItem;
            if (listboxMill.SelectedIndex > -1)
            {
                textBlockMillSeedsAvailable.Text = SelectedMillSeed.CropCount.ToString("N0");
                textBlockMillRequired.Text = SelectedMillSeed.Harvest2Seed.ToString("N0");
                TimeSpan MillProcessTime = new TimeSpan(0, 0, 0, TheUser.MillTime);
                textBlockMillHarvestedTitle.Text = MillProcessTime.ToString();
                int tMaxValue = SelectedMillSeed.CropCount / SelectedMillSeed.Harvest2Seed;
                if (TheUser.MillLevel < tMaxValue) tMaxValue = TheUser.MillLevel;
                radNumericUpDown.MaxValue = tMaxValue;

                radNumericUpDown.ValueChanged += radNumericUpDown_ValueChanged;
                canvasSeedProcessing.Visibility = System.Windows.Visibility.Visible;

                radNUDMillSend2Refinery.MaxValue = SelectedMillSeed.CropCount;
                radNUDMillSend2Refinery.Change = 10000;
                radNUDMillSend2Refinery.Value = 0;
                radNUDMillSend2Refinery.ValueChanged += radNUDMillSend2Refinery_ValueChanged;

                textBlockRefineryScraps.Text = "0 cP";
            }
            else if (listboxMill.SelectedIndex == -1)
            {
                radNUDMillSend2Refinery.MaxValue = 0;
                radNUDMillSend2Refinery.Change = 0;
                radNUDMillSend2Refinery.Value = 0;
            }
        }
        
        private void radNumericUpDown_ValueChanged(object sender, Telerik.Windows.Controls.ValueChangedEventArgs<double> e)
        {
            int tMillSeconds = TheUser.MillTime * Convert.ToInt32(radNumericUpDown.Value);
            TimeSpan tMillProcessTime = new TimeSpan(0, 0, 0, tMillSeconds);
            textBlockMillHarvestedTitle.Text = tMillProcessTime.ToString();
        }

        private void radNUDMillSend2Refinery_ValueChanged(object sender, Telerik.Windows.Controls.ValueChangedEventArgs<double> e)
        {
            textBlockRefineryScraps.Text = Convert.ToInt32(radNUDMillSend2Refinery.Value).ToString("N0") + " cP";
            if (SelectedMillSeed.CropCount > 0)
            {
                if (radNUDMillSend2Refinery.Value > 0) SelectedMillSeed.Harvest2Send2Refinery = Convert.ToInt32(radNUDMillSend2Refinery.Value);
            }
        }

        private void Mill_Send2Refinery(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Convert.ToInt32(radNUDMillSend2Refinery.Value) > 0)
            {
                // Update Refinery XML
                int Seconds2CompleteRefining = ((tRefinery.CropinRefinery + Convert.ToInt32(radNUDMillSend2Refinery.Value)) / tRefinery.CropPerSec);
                DateTime rightnow = DateTime.Now;
                TimeSpan duration = new TimeSpan(0, 0, 0, Seconds2CompleteRefining);
                DateTime answer = rightnow.Add(duration);

                tRefinery.RefineCompleteDate = answer;
                tRefinery.CropinRefinery = tRefinery.CropinRefinery + SelectedMillSeed.Harvest2Send2Refinery;
                RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("RefineCompleteDate", answer);
                RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("CropinRefinery", tRefinery.CropinRefinery);
                refineryxml = helper.SaveUpdatedXML(RefineryXDoc, "Refinery.xml");
                //tRefinery = helper.LoadRefinery(RefineryXDoc, BaseRefineTime);

                // Update Mill details
                int CurrentMillSeedCrop = Convert.ToInt32(MillXDoc.Element("Mill").Element("Seed" + SelectedMillSeed.SeedID).Element("TotalSeedCrop").Value);
                MillXDoc.Element("Mill").Element("Seed" + SelectedMillSeed.SeedID).SetElementValue("TotalSeedCrop", (CurrentMillSeedCrop - SelectedMillSeed.Harvest2Send2Refinery));
                millxml = helper.SaveUpdatedXML(MillXDoc, "Mill.xml");
                LoadMillDetails();

                radNUDMillSend2Refinery.Value = 0;
                textBlockRefineryScraps.Text = "0 cP";
            }
        }

        private void Mill(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (listboxMill.SelectedIndex > -1)
            {
                if (SelectedMillSeed.CropCount >= SelectedMillSeed.Harvest2Seed)
                {
                    DateTime rightnow = DateTime.Now;
                    int tempTotalSeconds = TheUser.MillTime * Convert.ToInt32(radNumericUpDown.Value);
                    TimeSpan duration = new TimeSpan(0, 0, 0, tempTotalSeconds);
                    DateTime answer = rightnow.Add(duration);

                    // Update User Milling details
                    UserXDoc.Element("UserInfo").Element("User").SetElementValue("MillCompleteDate", answer);
                    UserXDoc.Element("UserInfo").Element("User").SetElementValue("MillProcessing", 1);
                    UserXDoc.Element("UserInfo").Element("User").SetElementValue("MillCurrentSeed", SelectedMillSeed.SeedID);
                    UserXDoc.Element("UserInfo").Element("User").SetElementValue("MillSeedProcessingCount", Convert.ToInt32(radNumericUpDown.Value));
                    TheUser.MillCompleteDate = answer;
                    TheUser.IsMillProcessing = 1;
                    TheUser.MillCurrentSeed = SelectedMillSeed.SeedID;
                    TheUser.MillSeedProcessingCount = Convert.ToInt32(radNumericUpDown.Value);
                    userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");

                    // Update Mill details
                    int CurrentMillSeedCrop = Convert.ToInt32(MillXDoc.Element("Mill").Element("Seed" + SelectedMillSeed.SeedID).Element("TotalSeedCrop").Value);
                    MillXDoc.Element("Mill").Element("Seed" + SelectedMillSeed.SeedID).SetElementValue("TotalSeedCrop", (CurrentMillSeedCrop - (SelectedMillSeed.Harvest2Seed * radNumericUpDown.Value)));
                    millxml = helper.SaveUpdatedXML(MillXDoc, "Mill.xml");
                    LoadMillDetails();
                    listboxMill.SelectedIndex = -1;

                    //borderImgMillIcon.BorderThickness = new Thickness(2);
                    //borderImgMillIcon.BorderBrush = new SolidColorBrush(Colors.Green);

                    textBlockMillProgressTimeLeft.Text = duration.ToString();
                    textBlockMillProgressCount.Text = TheUser.MillSeedProcessingCount.ToString();
                    textBlockMillProgressSeed.Text = SeedsXDoc.Element("AllSeeds").Element("Seed" + TheUser.MillCurrentSeed).Attribute("Name").Value + " seed";
                    textBlockRefineryScraps.Text = "Select harvest...";
                    radNUDMillSend2Refinery.MaxValue = 0;

                    canvasProcessCountDown.Visibility = System.Windows.Visibility.Visible;
                    //canvasSeedProcessing.Visibility = System.Windows.Visibility.Collapsed;

                }
            }
        }

        private void borderMillMaxS2R_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            radNUDMillSend2Refinery.Value = radNUDMillSend2Refinery.MaxValue;
        }

        private void textBlockMillProgressTimeLeft_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (textBlockMillProgressTimeLeft.Text == "Done")
            {
                CompleteMilling();
            }
        }

        private void CompleteMilling()
        {
            //ShakeObject.Stop();

            // Add Seed(s) to Granary
            int CurrentGranarySeedCount = Convert.ToInt32(GranaryXDoc.Element("Granary").Element("Seed" + TheUser.MillCurrentSeed).Element("TotalSeedCount").Value);
            GranaryXDoc.Element("Granary").Element("Seed" + TheUser.MillCurrentSeed).SetElementValue("TotalSeedCount", CurrentGranarySeedCount + TheUser.MillSeedProcessingCount);
            granaryxml = helper.SaveUpdatedXML(GranaryXDoc, "Granary.xml");
            LoadGranaryDetails();
            LoadMillDetails();

            // Update User Milling Stats
            int tSeedXP = Convert.ToInt32(SeedsXDoc.Element("AllSeeds").Element("Seed" + TheUser.MillCurrentSeed).Element("PlantedEXP").Value);
            int tMillCount = Convert.ToInt32(UserXDoc.Element("UserInfo").Element("User").Element("MillCount").Value);
            int tMillXP = Convert.ToInt32(UserXDoc.Element("UserInfo").Element("User").Element("MillEXP").Value);
            int BeforeMillLvl = TheUser.MillLevel;
            TheUser.MillEXP = (TheUser.MillSeedProcessingCount * tSeedXP) + tMillXP;
            TheUser.MillLevel = helper.GetLevel(5, TheUser.MillEXP);
            int AfterMillLvl = TheUser.MillLevel;
            int CurrentMillSeedProcessingCount = TheUser.MillSeedProcessingCount;
            UserXDoc.Element("UserInfo").Element("User").SetElementValue("MillEXP", (TheUser.MillSeedProcessingCount * tSeedXP) + tMillXP);
            UserXDoc.Element("UserInfo").Element("User").SetElementValue("MillProcessing", 0);
            UserXDoc.Element("UserInfo").Element("User").SetElementValue("MillCurrentSeed", "");
            UserXDoc.Element("UserInfo").Element("User").SetElementValue("MillSeedProcessingCount", 0);
            UserXDoc.Element("UserInfo").Element("User").SetElementValue("MillCount", (tMillCount + 1));
            TheUser.IsMillProcessing = 0;
            string tSeedName = SeedsXDoc.Element("AllSeeds").Element("Seed" + TheUser.MillCurrentSeed).Attribute("Name").Value;
            TheUser.MillCurrentSeed = 0;
            TheUser.MillSeedProcessingCount = 0;
            userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");
            PopulateSkillsData();

            // Reset Mill so it can process again
            canvasProcessCountDown.Visibility = System.Windows.Visibility.Collapsed;
            textBlockMillProgressTimeLeft.Text = "00:00:00";
            textBlockMillHarvestedTitle.Text = "00:00:00";
            string tGrammar = " seed has been";
            if (CurrentMillSeedProcessingCount > 1) tGrammar = " seeds have been";
            int DidSkillGainLevel = 0;
            if (BeforeMillLvl != AfterMillLvl) { DidSkillGainLevel = 1; }
            Machine tMill = new Machine();
            tMill.ID = 5;
            tMill.TotalEXP = TheUser.MillEXP;
            tMill.Level = TheUser.MillLevel;
            MessageList.Add(new Message(10, "Mill Complete", "The Mill has completed milling. \n" + CurrentMillSeedProcessingCount + " new " + tSeedName + tGrammar + " added to the Granary.\nAdded XP = +" + (CurrentMillSeedProcessingCount * tSeedXP), "Images/Mill.jpg", "Visible", tMill, (CurrentMillSeedProcessingCount * tSeedXP), DidSkillGainLevel));
            if (BeforeMillLvl != AfterMillLvl)
            {
                MessageList.Add(new Message(6, "Skill Level Up", "Milling has gained a level. \nNew Level\n" + TheUser.MillLevel, "Images/Mill.jpg", "Visible", tMill, (CurrentMillSeedProcessingCount * tSeedXP), DidSkillGainLevel));
            }
        }

        private void Ellipse_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            if (MenuOpen == 0)
            {
                MenuOpen = 1;
                canvasMenu.Visibility = System.Windows.Visibility.Visible;
                //MenuBounceIn.Begin();
            }
            else if (MenuOpen == 1)
            {
                MenuOpen = 0;
                canvasMenu.Visibility = System.Windows.Visibility.Collapsed;
                //MenuBounceOut.Begin();
            }
            dt.Start();
        }

        private void canvasMainMenu_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            NavigationService.Navigate(new Uri("/Login.xaml?Return=1", UriKind.Relative));
        }

        private void canvasUserIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            if (UserDataOpen == 0)
            {
                if (MenuOpen == 1)
                {
                    MenuOpen = 0;
                    canvasMenu.Visibility = System.Windows.Visibility.Collapsed;
                }

                //UserDataBounceIn.Begin();
                canvasUser.Visibility = System.Windows.Visibility.Visible;
                UserDataOpen = 1;
            }
            else if (UserDataOpen == 1)
            {
                //UserDataBounceOut.Begin();
                canvasUser.Visibility = System.Windows.Visibility.Collapsed;
                UserDataOpen = 0;
            }
            dt.Start();
        }

        private void imgGarage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            if (GarageDataOpen == 0)
            {
                if (MenuOpen == 1)
                {
                    MenuOpen = 0;
                    canvasMenu.Visibility = System.Windows.Visibility.Collapsed;
                }

                canvasGarage.Visibility = System.Windows.Visibility.Visible;
                GarageDataOpen = 1;
            }
            else if (GarageDataOpen == 1)
            {
                canvasGarage.Visibility = System.Windows.Visibility.Collapsed;
                UserDataOpen = 0;
            }
            dt.Start();
        }

        private void PopulateSkillsData()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                //tUser.SetPrice(3);
                textBlockUserName.Text = TheUser.UserName + "(" + TheUser.UserID + ")";
                //textBlockCurrentFC.Text = TheUser.FreeCurrency.ToString("C0") + "f";
                textBlockUserMoney.Text = TheUser.FreeCurrency.ToString("C0") + "f";

                //tUser.GetLevel(5, tUser.MillEXP);
                TheUser.MillLevel = helper.GetLevel(5, TheUser.MillEXP);
                if (TheUser.IsMillProcessing == 1)
                {
                    textBlockMillProgressCount.Text = TheUser.MillSeedProcessingCount.ToString();
                    textBlockMillProgressSeed.Text = SeedsXDoc.Element("AllSeeds").Element("Seed" + TheUser.MillCurrentSeed).Attribute("Name").Value + " seed";
                }

                // Populate the Skills Class
                List<Skills> skillitems = new List<Skills>();

                // Add Planting to Skills SlideView
                //double PTCurEXP = tDrill.TotalEXP - tUser.GetNextLevelXP(2, (tDrill.Level - 1));
                double PTCurEXP = tDrill.TotalEXP - helper.GetNextLevelXP(2, (tDrill.Level - 1));
                //double PTNextEXP = tUser.GetNextLevelXP(2, tDrill.Level) - tUser.GetNextLevelXP(2, (tDrill.Level - 1));
                double PTNextEXP = helper.GetNextLevelXP(2, tDrill.Level) - helper.GetNextLevelXP(2, (tDrill.Level - 1));
                double PTlevelPCT = (PTCurEXP / PTNextEXP) * 100;
                Skills tPlantSkill = new Skills()
                {
                    SkillTitlePlusLevel = "Plant " + "Lvl" + tDrill.Level,
                    ImagePath = "Images/Planting.jpg",
                    SkillDescription = "The higher your Planting level the better seeds you can plant.",
                    SkillBonusText = "",
                    SkillBonusNumber = "",
                    Level = tDrill.Level,
                    NextLevel = tDrill.Level + 1,
                    EXP = tDrill.TotalEXP,
                    //NextEXP = tUser.GetNextLevelXP(2, tDrill.Level),
                    NextEXP = helper.GetNextLevelXP(2, tDrill.Level),
                    NextLevelPCT = PTlevelPCT * 2
                };
                skillitems.Add(tPlantSkill);

                // Add Plowing to Skills SlideView
                double PWCurEXP = tPlow.TotalEXP - helper.GetNextLevelXP(4, (tPlow.Level - 1));
                double PWNextEXP = helper.GetNextLevelXP(4, tPlow.Level) - helper.GetNextLevelXP(4, (tPlow.Level - 1));
                double PWlevelPCT = (PWCurEXP / PWNextEXP) * 100;
                Skills tPlowSkill = new Skills()
                {
                    SkillTitlePlusLevel = "Plow " + "Lvl" + tPlow.Level,
                    ImagePath = "Images/Plowing.jpg",
                    SkillDescription = "Plowing has a " + BasePlowSeedFindPCT + "% chance of finding a new seed. This % goes up with your Plow Level",
                    SkillBonusText = "New Seed %:",
                    SkillBonusNumber = (tPlow.Level + BasePlowSeedFindPCT) + "%",
                    Level = tPlow.Level,
                    NextLevel = tPlow.Level + 1,
                    EXP = tPlow.TotalEXP,
                    NextEXP = helper.GetNextLevelXP(4, tPlow.Level),
                    NextLevelPCT = PWlevelPCT * 2
                };
                skillitems.Add(tPlowSkill);

                // Add Harvesting to Skills SlideView
                double HCurEXP = YourHarvestor.TotalEXP - helper.GetNextLevelXP(1, (YourHarvestor.Level - 1));
                double HNextEXP = helper.GetNextLevelXP(1, YourHarvestor.Level) - helper.GetNextLevelXP(1, (YourHarvestor.Level - 1));
                double HlevelPCT = (HCurEXP / HNextEXP) * 100;
                Skills tHarvestSkill = new Skills()
                {
                    SkillTitlePlusLevel = "Harvest " + "Lvl" + YourHarvestor.Level,
                    ImagePath = "Images/Combine.jpg",
                    SkillDescription = "",
                    SkillBonusText = "",
                    SkillBonusNumber = "",
                    Level = YourHarvestor.Level,
                    NextLevel = YourHarvestor.Level + 1,
                    EXP = YourHarvestor.TotalEXP,
                    NextEXP = helper.GetNextLevelXP(1, YourHarvestor.Level),
                    NextLevelPCT = HlevelPCT * 2
                };
                skillitems.Add(tHarvestSkill);
                // Add Exploring to Skills SlideView
                double ECurEXP = tExplorer.TotalEXP - helper.GetNextLevelXP(3, (tExplorer.Level - 1));
                double ENextEXP = helper.GetNextLevelXP(3, tExplorer.Level) - helper.GetNextLevelXP(3, (tExplorer.Level - 1));
                double ElevelPCT = (ECurEXP / ENextEXP) * 100;
                Skills tExploreSkill = new Skills()
                {
                    SkillTitlePlusLevel = "Explore " + "Lvl" + tExplorer.Level,
                    ImagePath = "Images/CompassMap.jpg",
                    SkillDescription = "Exploring will increase your Farm size and it gives you an opportunity to find NEW seeds to plant.",
                    SkillBonusText = "Chance to find Seed:",
                    SkillBonusNumber = BaseExploreSeedFindPCT + "%",
                    Level = tExplorer.Level,
                    NextLevel = tExplorer.Level + 1,
                    EXP = tExplorer.TotalEXP,
                    NextEXP = helper.GetNextLevelXP(3, tExplorer.Level),
                    NextLevelPCT = ElevelPCT * 2
                };
                skillitems.Add(tExploreSkill);

                // Add Milling to Skills SlideView
                double MCurEXP = TheUser.MillEXP - helper.GetNextLevelXP(5, (TheUser.MillLevel - 1));
                double MNextEXP = helper.GetNextLevelXP(5, TheUser.MillLevel) - helper.GetNextLevelXP(5, (TheUser.MillLevel - 1));
                double MlevelPCT = (MCurEXP / MNextEXP) * 100;
                Skills tMillSkill = new Skills()
                {
                    SkillTitlePlusLevel = "Mill " + "Lvl" + TheUser.MillLevel,
                    ImagePath = "Images/Mill.jpg",
                    SkillDescription = "The higher your Milling level, the more Plantable seeds you can produce at one time.",
                    SkillBonusText = "",
                    SkillBonusNumber = "",
                    Level = TheUser.MillLevel,
                    NextLevel = TheUser.MillLevel + 1,
                    EXP = TheUser.MillEXP,
                    NextEXP = helper.GetNextLevelXP(5, TheUser.MillLevel),
                    NextLevelPCT = MlevelPCT * 2
                };
                skillitems.Add(tMillSkill);

                radUserSlideView.ItemsSource = skillitems;
            });
        }

        private void CheckYourMessages()
        {
            CurrentMessage = new Message();
            // Shows New Messages for user
            if (MessageList.Count > 0)
            {
                CurrentMessage = MessageList[0];
                CurrentMessage.MessageCount = "Message Count: " + MessageList.Count();
                canvasMessage.DataContext = CurrentMessage;
                MessageOpen = 1;
                canvasMessage.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                canvasMessage.Visibility = System.Windows.Visibility.Collapsed;
                MessageOpen = 0;
            }
        }

        private void Refinery_ShowUpgrade(object sender, System.Windows.Input.GestureEventArgs e)
        {
            canvasRefineryUpgradeComponents.Visibility = System.Windows.Visibility.Visible;
            canvasRefineryDefinition.Visibility = System.Windows.Visibility.Collapsed;
            canvasRefineryStats.Visibility = System.Windows.Visibility.Collapsed;
            //borderRefineryClick2Upgrade.Visibility = System.Windows.Visibility.Collapsed;
            //borderRefineryHideUpgrade.Visibility = System.Windows.Visibility.Visible;
        }

        private void RefineryUpgrades(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Border TempRefBorder = new Border();
            TempRefBorder = (Border)sender;

            string WhichProp = TempRefBorder.Tag.ToString();
            int CurrentPropLvl = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element(WhichProp + "Lvl").Value);

            long FreeCurrencyCost = Convert.ToInt64(UpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (CurrentPropLvl + 1)).Attribute("UpgradeFree").Value);
            if (TheUser.FreeCurrency >= FreeCurrencyCost)
            {
                // Update User Currency
                int MoneyWinner = MyRandomNumber.Next(1, 101) * 1000;
                UserXDoc.Element("UserInfo").Element("User").SetElementValue("FreeCurrency", (TheUser.FreeCurrency - FreeCurrencyCost));
                UserXDoc.Element("UserInfo").Element("User").SetElementValue("MoneySpent", (TheUser.FreeCurrency + FreeCurrencyCost));
                TheUser.FreeCurrency = TheUser.FreeCurrency - FreeCurrencyCost;
                userxml = helper.SaveUpdatedXML(UserXDoc, "User.xml");

                // Update Refinery XML
                int CurrentPropValue = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element(WhichProp).Value);
                int PropValue2Add = Convert.ToInt32(UpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (CurrentPropLvl + 1)).Attribute("UpgradeValue").Value);
                long UpgradeFree = Convert.ToInt32(UpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (CurrentPropLvl + 2)).Attribute("UpgradeFree").Value);
                long UpgradeValue = Convert.ToInt32(UpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (CurrentPropLvl + 2)).Attribute("UpgradeValue").Value);
                if (WhichProp == "RefineRate")
                {
                    tRefinery.RefineRate = CurrentPropValue + PropValue2Add;
                    tRefinery.RefineRateLvl = CurrentPropLvl + 1;
                    tRefinery.RefineRateFCC = (UpgradeFree).ToString("C");
                    tRefinery.RefineRateNextLvlDisplay = WhichProp + " + " + UpgradeValue;

                    tRefineTime = Convert.ToInt32(Math.Ceiling((double)(BaseRefineTime / tRefinery.RefineRate)));
                    tRefinery.CropPerSec = tRefinery.CropPermG / tRefineTime;
                    int Seconds2CompleteRefining = (tRefinery.CropinRefinery / tRefinery.CropPerSec);
                    DateTime rightnow = DateTime.Now;
                    TimeSpan duration = new TimeSpan(0, 0, 0, Seconds2CompleteRefining);
                    DateTime answer = rightnow.Add(duration);
                    tRefinery.RefineCompleteDate = answer;
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("RefineCompleteDate", answer);
                    
                    // Reset Batch Timer
                    TimeSpan TSRefinery1 = new TimeSpan(0, 0, (tRefinery.CropPermG / tRefinery.CropPerSec));
                    tRefinery.SecsPerBatch = TSRefinery1.ToString(@"hh\:mm\:ss");
                    tRefinery.SecsPerBatchLeft = (tRefinery.CropPermG / tRefinery.CropPerSec).ToString();
                    BatchCounter = (tRefinery.CropPermG / tRefinery.CropPerSec) - BatchCounter;
                }
                else if (WhichProp == "MaxmG")
                {
                    tRefinery.MaxmG = CurrentPropValue + PropValue2Add;
                    tRefinery.MaxmGLvl = CurrentPropLvl + 1;
                    tRefinery.MaxmGFCC = (UpgradeFree).ToString("C");
                    tRefinery.MaxmGNextLvlDisplay = WhichProp + " + " + UpgradeValue;
                }
                else if (WhichProp == "CropPermG")
                {
                    tRefinery.CropPermG = CurrentPropValue + PropValue2Add;
                    tRefinery.CropPermGLvl = CurrentPropLvl + 1;
                    tRefinery.CropPermGFCC = (UpgradeFree).ToString("C");
                    tRefinery.CropPermGNextLvlDisplay = WhichProp + " + " + UpgradeValue;

                    // Reset Batch Timer
                    TimeSpan TSRefinery1 = new TimeSpan(0, 0, (tRefinery.CropPermG / tRefinery.CropPerSec));
                    tRefinery.SecsPerBatch = TSRefinery1.ToString(@"hh\:mm\:ss");
                    tRefinery.SecsPerBatchLeft = (tRefinery.CropPermG / tRefinery.CropPerSec).ToString();
                    BatchCounter = (tRefinery.CropPermG / tRefinery.CropPerSec) - BatchCounter;
                }

                //int CurrentRefinedmG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("miniGallonsRefined").Value);
                //int tRefinedmG = Convert.ToInt32((Math.Floor((double)tRefinery.TotalCropRefined / tRefinery.CropPermG) / tRefinery.OutputmG));
                RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue(WhichProp, (CurrentPropValue + PropValue2Add));
                RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue(WhichProp + "Lvl", (CurrentPropLvl + 1));
                int IsSecondaryProp = Convert.ToInt32(UpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (CurrentPropLvl + 1)).Attribute("IsSecondaryProp").Value);
                if (IsSecondaryProp == 1)
                {
                    int tSecondaryValue = Convert.ToInt32(UpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (CurrentPropLvl + 1)).Attribute("SecondaryValue").Value);
                    string tSecondaryProp = UpgradeXDoc.Element("AllUpgrades").Element("Upgrade5").Element(WhichProp).Element("Level" + (CurrentPropLvl + 1)).Attribute("SecondaryProp").Value;
                    tRefinery.OutputmG = tSecondaryValue;
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue(tSecondaryProp, tSecondaryValue);
                }
                refineryxml = helper.SaveUpdatedXML(RefineryXDoc, "Refinery.xml");
                //tRefinery = helper.LoadRefinery(RefineryXDoc, BaseRefineTime, UpgradeXDoc);
                MessageList.Add(new Message(1, "Refinery Upgrade", "The Refinery has been upgraded. \nOriginal value for " + WhichProp + " = " + CurrentPropValue + " \nNew Value for " + WhichProp + " = " + (CurrentPropValue + PropValue2Add), "Images/BioFuel.jpg", "Collapsed", tDrill, 0, 0));
                CheckYourMessages();
            }
            else
            {
                long tempint = Convert.ToInt64(FreeCurrencyCost - TheUser.FreeCurrency);
                textBlockHarvestUpdate.Foreground = new SolidColorBrush(Colors.Red);
                textBlockHarvestUpdate.Text = "You need " + tempint.ToString("C") + " more money to upgrade!";
                HarvestUpdateFade.Begin();
            }
        }

        private void Start_Refinery()
        {
            try
            {
                textBlockBioFuelmGs.DataContext = "";
                // Update Refinery
                string StartDateCheck = RefineryXDoc.Element("Refinery").Element("Setting").Element("RefineStartDate").Value;
                string CompleteDateCheck = RefineryXDoc.Element("Refinery").Element("Setting").Element("RefineCompleteDate").Value;
                string LastRefineDate = RefineryXDoc.Element("Refinery").Element("Setting").Element("LastRefineDate").Value;
                int tRefineRate = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("RefineRate").Value);
                int CurrentRefinedmG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("miniGallonsRefined").Value);
                int tCropPermG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("CropPermG").Value);
                int tCropinRefinery = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("CropinRefinery").Value);
                int CropRefined = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("CropRefinedLvl1").Value);
                int tMaxmG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("MaxmG").Value);
                int tOutputmG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("OutputmG").Value);
                int tIsRefineryMaxed = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("IsRefineryMaxed").Value);
                tRefineTime = Convert.ToInt32(Math.Ceiling((double)(BaseRefineTime / tRefineRate)));
                int CropRefinedWhileGone = 0;
                int tRefinedmG = 0;

                // Load Upgrade Data onto Refinery Screen

                int tCropPS = (tCropPermG / tRefineTime); // Crops Per Second;
                //if (StartDateCheck == "")
                //{
                //    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("RefineStartDate", DateTime.Now);
                //}
                //if (CompleteDateCheck == "")
                //{
                //    int Seconds2CompleteRefining = (tCropinRefinery / tCropPS);
                //    DateTime rightnow = DateTime.Now;
                //    TimeSpan duration = new TimeSpan(0, 0, 0, Seconds2CompleteRefining);
                //    DateTime answer = rightnow.Add(duration);
                //    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("RefineCompleteDate", answer);
                //}
                //tRefinery.RefineryStatus = "Idle";

                if (tCropinRefinery > 0 && tIsRefineryMaxed == 0) // Calculate how much Crop was refined while user was gone.
                {
                    // Find out if the Crop Refined While Gone exceeds the MaxmG
                    DateTime Now = DateTime.Now;
                    if (LastRefineDate == "") LastRefineDate = DateTime.Now.ToString();
                    TimeSpan tGoneHowLong = Now - Convert.ToDateTime(LastRefineDate);
                    //if (tGoneHowLong.TotalSeconds > tDuration.TotalSeconds) tGoneHowLong = tDuration;
                    int NewCropRefined = Convert.ToInt32(tGoneHowLong.TotalSeconds * tCropPS);
                    if (NewCropRefined > tCropinRefinery)
                    {
                        CropRefinedWhileGone = tCropinRefinery; // If Crop Refined While Gone is more then was in the Refinery, then set Crop Refined While Gone = to Crop in Refinery
                    }
                    else
                    {
                        CropRefinedWhileGone = NewCropRefined;
                    }
                    tCropinRefinery = (tCropinRefinery - CropRefinedWhileGone);
                    CropRefined += CropRefinedWhileGone;

                    int RefinedmGWhileGone = (Convert.ToInt32((Math.Floor((double)CropRefinedWhileGone / tCropPermG))) * tOutputmG);
                    int tmG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("mG").Value);
                    if ((tmG + RefinedmGWhileGone) >= tMaxmG)
                    {
                        tmG = tMaxmG;
                        tRefinedmG = CurrentRefinedmG + (tMaxmG - tmG);
                    }
                    else
                    {
                        tmG += RefinedmGWhileGone;
                        tRefinedmG = CurrentRefinedmG + RefinedmGWhileGone;
                    }

                    //if (StartDateCheck != "" && CompleteDateCheck != "")
                    //{
                    //    // Find out when Refinery would completely refine all the Crop left.
                    //    DateTime tRightNow = DateTime.Now;
                    //    TimeSpan tDuration = Convert.ToDateTime(CompleteDateCheck) - tRightNow; // How much time has elapsed since the Refinery started.
                    //    tRefinery.RefineryStatus = "Processing";

                    //    if (tDuration.TotalSeconds < 1) // Refinery refined all crops while user was gone.
                    //    {
                    //        CropRefinedWhileGone = tCropinRefinery;
                    //        CropRefined += tCropinRefinery;
                    //        tCropinRefinery = 0;
                    //        tRefinery.RefineryStatus = "Idle";
                    //    }
                    //    else if (tDuration.TotalSeconds > 0 && LastRefineDate != "")
                    //    {
                    //        DateTime Now = DateTime.Now;
                    //        TimeSpan tGoneHowLong = Now - Convert.ToDateTime(LastRefineDate);

                    //        if (tGoneHowLong.TotalSeconds > tDuration.TotalSeconds) tGoneHowLong = tDuration;

                    //        int NewCropRefined = Convert.ToInt32(tGoneHowLong.TotalSeconds * tCropPS);
                    //        CropRefinedWhileGone = NewCropRefined;
                    //        CropRefined += NewCropRefined;
                    //        tCropinRefinery = (tCropinRefinery - NewCropRefined);
                    //    }

                    //    //int CurrentRefinedmG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("miniGallonsRefined").Value);
                    //    int tRefinedmG = (Convert.ToInt32((Math.Floor((double)CropRefinedWhileGone / tCropPermG))) * tOutputmG);
                    //    int tmG = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("mG").Value);
                    //    if (tmG < tMaxmG)
                    //    { 
                    //        if ((tRefinedmG + tmG) > tMaxmG)
                    //        {
                    //            //int mGOverFlow = (tRefinedmG - tmG) - tMaxmG;
                    //            //int CropOverFlow = tCropPermG * mGOverFlow;

                    //            int WhileGoneRefinedmG = (tMaxmG - tmG);
                    //            int OriginalCropRefined = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("CropRefinedLvl1").Value);
                    //            CropRefined = ((WhileGoneRefinedmG / tOutputmG) * tCropPermG) + OriginalCropRefined;
                    //            int OriginalCropLeft = Convert.ToInt32(RefineryXDoc.Element("Refinery").Element("Setting").Element("CropinRefinery").Value);
                    //            tCropinRefinery = OriginalCropLeft - ((WhileGoneRefinedmG / tOutputmG) * tCropPermG);
                    //            tRefinedmG = CurrentRefinedmG + WhileGoneRefinedmG; // Set Refined mG to the Max mG the Refinery can hold.
                    //        }
                    //        else
                    //        {
                    //            tRefinedmG = (Convert.ToInt32((Math.Floor((double)CropRefinedWhileGone / tCropPermG))) * tOutputmG) + CurrentRefinedmG;
                    //        }
                    //    }

                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("CropinRefinery", tCropinRefinery);
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("CropRefinedLvl1", CropRefined);
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("miniGallonsRefined", tRefinedmG);
                    RefineryXDoc.Element("Refinery").Element("Setting").SetElementValue("mG", tmG);
                }

                refineryxml = helper.SaveUpdatedXML(RefineryXDoc, "Refinery.xml");
                tRefinery = helper.LoadRefinery(RefineryXDoc, BaseRefineTime, UpgradeXDoc);
                BatchCounter = (tRefinery.CropPermG / tRefinery.CropPerSec);

                // Load Refinery
                canvasRefinery.DataContext = tRefinery;
                textBlockBioFuelmGs.DataContext = tRefinery;
            }
            catch
            {
                MessageBox.Show("Refinery Failed to Start!");
                NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                //buttonRetry.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Refinery_ShowDetails(object sender, System.Windows.Input.GestureEventArgs e)
        {
            canvasRefineryUpgradeComponents.Visibility = System.Windows.Visibility.Collapsed;
            canvasRefineryDefinition.Visibility = System.Windows.Visibility.Visible;
            canvasRefineryStats.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Refinery_ShowStats(object sender, System.Windows.Input.GestureEventArgs e)
        {
            canvasRefineryUpgradeComponents.Visibility = System.Windows.Visibility.Collapsed;
            canvasRefineryDefinition.Visibility = System.Windows.Visibility.Collapsed;
            canvasRefineryStats.Visibility = System.Windows.Visibility.Visible;
        }

        private void UpdateLeaderBoard(string yieldworth, string totalsps, string yieldmax, string basepgr, string totalpgr, string empire, string tig, string moneyspent, string bfu, string explorexp, string plantxp, string plowxp, string harvestxp, string millxp)
        {
            int TheOption = 3;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_PetProjectAppServices.php?Option={0}&UserName={1}&ExpPlots={2}&FreeCur={3}&yw={4}&tsps={5}&ymax={6}&bpgr={7}&tpgr={8}&empire={9}&tig={10}&moneyspent={11}&bfu={12}&explorexp={13}&plantxp={14}&plowxp={15}&harvestxp={16}&millxp={17}", TheOption, TheUser.UserName, TheUser.CurrentExploredPlots, TheUser.FreeCurrency, yieldworth, totalsps, yieldmax, basepgr, totalpgr, empire, tig, moneyspent, bfu, explorexp, plantxp, plowxp, harvestxp, millxp);

            WebClient wcULB = new WebClient();
            wcULB.DownloadStringAsync(new Uri(newURL));
            wcULB.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcULB_DownloadStringCompleted);
        }

        private void wcULB_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;
                if (s == "1")
                {
                    runLastUpdate.Text = "Last Update: " + DateTime.Now.ToString();
                }
            }
            catch
            {

            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            dt.Stop();
            if (PlotDataOpen == 1)
            {
                PlotDataOpen = 0;
            }
            if (UserDataOpen == 1)
            {
                UserDataOpen = 0;
            }
            if (SiloDataOpen == 1)
            {
                SiloDataOpen = 0;
            }
            if (GranaryOpen == 1)
            {
                GranaryOpen = 0;
            }
            if (MillOpen == 1)
            {
                MillOpen = 0;
            }
            if (RefineryOpen == 1)
            {
                RefineryOpen = 0;
            }
            if (MenuOpen == 1)
            {
                MenuOpen = 0;
            }
            if (GarageDataOpen == 1)
            {
                GarageDataOpen = 0;
            }
            dt.Start();
            e.Cancel = true;
        }

        private void canvasMessage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            dt.Stop();
            MessageList.Remove(CurrentMessage);

            if (MessageOpen == 0)
            {
                if (MenuOpen == 1)
                {
                    MenuOpen = 0;
                    canvasMenu.Visibility = System.Windows.Visibility.Collapsed;
                }

                canvasMessage.Visibility = System.Windows.Visibility.Visible;
                MessageOpen = 1;
            }
            else if (MessageOpen == 1)
            {
                CheckYourMessages();
            }
            dt.Start();
        }

        private void imgHarvest_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            Harvest(SelectedPlot, 0);

            //canvasPlotIcon_Tap(sender, e);
        }




    }
}