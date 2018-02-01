using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace EmpireBuilding.Pages
{
    public partial class LBList : PhoneApplicationPage
    {
        string UserID = string.Empty;
        string UserName = string.Empty;

        public LBList()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            UserID = NavigationContext.QueryString["UserID"];
            UserName = NavigationContext.QueryString["UserName"];
        }

        private void Go2StatsLeaderBoard(object sender, System.Windows.Input.GestureEventArgs e)
        {

            NavigationService.Navigate(new Uri("/Pages/EmpireLeaderboard.xaml?UserID=" + UserID + "&UserName=" + UserName, UriKind.Relative));
        }

        private void Go2UsageLeaderBoard(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UsageLeaderboard.xaml?UserID=" + UserID + "&UserName=" + UserName, UriKind.Relative));
        }

        private void Go2SkillsLeaderBoard(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SkillsLeaderboard.xaml?UserID=" + UserID + "&UserName=" + UserName, UriKind.Relative));
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Login.xaml?Return=1", UriKind.Relative));
        }
    }
}