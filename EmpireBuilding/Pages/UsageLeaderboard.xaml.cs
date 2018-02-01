using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Xml.Linq;
using EmpireBuilding.CodeFiles;

namespace EmpireBuilding.Pages
{
    public partial class UsageLeaderboard : PhoneApplicationPage
    {
        int TheOption = 0;
        string UserID = string.Empty;
        string UserName = string.Empty;
        string TheField = string.Empty;
        string FieldFormat = string.Empty;

        public UsageLeaderboard()
        {
            InitializeComponent();
        }

        private void pivotLeaderBoards_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            ProgressIndicator.IsRunning = true;
            ProgressIndicator.Content = "Connecting...";

            textBlockConnectError.Visibility = System.Windows.Visibility.Collapsed;
            UserID = NavigationContext.QueryString["UserID"];
            UserName = NavigationContext.QueryString["UserName"];
            TheOption = 4;

            // Field Formats
            // 0 = Number
            // 1 = Dollar Value
            // 2 = Value with 2 Decimal Places

            if (e.Item == pivotTIG)
            {
                TheField = "TimeInGame";
                FieldFormat = "3";
            }
            else if (e.Item == pivotMoneySpent)
            {
                TheField = "MoneySpent";
                FieldFormat = "1";
            }
            else if (e.Item == pivotBFU)
            {
                TheField = "BioFuelUsed";
                FieldFormat = "0";
            }

            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&UserID={1}&TheField={2}&FieldFormat={3}", TheOption, UserID, TheField, FieldFormat);

            WebClient wcGetLBData = new WebClient();
            wcGetLBData.DownloadStringAsync(new Uri(newURL));
            wcGetLBData.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcGetLBData_DownloadStringCompleted);
        }

        private void wcGetLBData_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                ProgressIndicator.Content = "Downloading...";
                string s = e.Result;
                TextReader tr = new StringReader(s);
                XDocument LeaderboardXDoc = XDocument.Load(tr);
                List<Leaderboard> tlistboxLeaderBoard = new List<Leaderboard>();
                string tRankNum = "0";
                string tValue = string.Empty;
                string tUserName = string.Empty;
                string tBackground = string.Empty;
                string tForeground = string.Empty;
                int tValueFormat = 0;
                int tUserRank = 0;

                int TotalUserCount = Convert.ToInt32(LeaderboardXDoc.Element("Leaderboards").Attribute("LeaderboardCount").Value);

                for (int i = 1; i < (TotalUserCount + 1); i++)
                {
                    tRankNum = LeaderboardXDoc.Element("Leaderboards").Element("LBItem" + i).Attribute("Rank").Value;
                    tValue = LeaderboardXDoc.Element("Leaderboards").Element("LBItem" + i).Attribute("Value").Value;
                    tUserName = LeaderboardXDoc.Element("Leaderboards").Element("LBItem" + i).Attribute("UserName").Value;
                    tBackground = LeaderboardXDoc.Element("Leaderboards").Element("LBItem" + i).Attribute("BG").Value;
                    tForeground = LeaderboardXDoc.Element("Leaderboards").Element("LBItem" + i).Attribute("FG").Value;
                    tValueFormat = Convert.ToInt32(LeaderboardXDoc.Element("Leaderboards").Element("LBItem" + i).Attribute("ValueFormat").Value);

                    if (tUserName == UserName)
                    {
                        tUserRank = Convert.ToInt32(tRankNum);
                        tForeground = "Yellow";
                    }

                    tlistboxLeaderBoard.Add(new Leaderboard(tRankNum, tValue, tValueFormat, tUserName, tBackground, tForeground));
                }

                if (TheField == "TimeInGame")
                {
                    listboxLBTIG.ItemsSource = tlistboxLeaderBoard;
                }
                else if (TheField == "MoneySpent")
                {
                    listboxLBMS.ItemsSource = tlistboxLeaderBoard;
                }
                else if (TheField == "BioFuelUsed")
                {
                    listboxLBBFU.ItemsSource = tlistboxLeaderBoard;
                }
                textBlockLBUserName.Text = UserName + " Rank:";
                textBlockLBRank.Text = tUserRank.ToString() + " of " + tRankNum;

                ProgressIndicator.Content = "Complete...";
                ProgressIndicator.IsRunning = false;
            }
            catch
            {
                ProgressIndicator.Content = "Error...";
                ProgressIndicator.IsRunning = false;

                textBlockConnectError.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/LBList.xaml?UserID=" + UserID + "&UserName=" + UserName, UriKind.Relative));
        }
    }
}