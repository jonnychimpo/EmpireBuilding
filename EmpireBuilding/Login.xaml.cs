using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using EmpireBuilding.CodeFiles;
using System.IO;
using System.Xml.Linq;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace EmpireBuilding
{
    public partial class Login : PhoneApplicationPage
    {
        string userxml = string.Empty;
        int IsUserLoaded = 1;
        int TheOption = 0;
        int DefaultUserCurrency = 500000;
        int DefaultPaidCurrency = 0;
        int DefaultCurExPlots = 0;
        User LoginUser = new User();
        int WhereFailed = 0;
        string theUserName = string.Empty;

        public Login()
        {
            InitializeComponent();

            userxml = helper.CheckFiles("User.xml", 1, 0, "");
            IsolatedStorageFile ISOstore = IsolatedStorageFile.GetUserStoreForApplication();
            if ((ISOstore.FileExists("User.xml") == false) || (userxml.Length < 100))
            {
                IsUserLoaded = 0;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemTray.IsVisible = false;
            textBlockStatus.Text = "";
            RadProgressIndicator.IsRunning = false;

            if (IsUserLoaded == 1)
            {
                Return2MainMenu(1);
            }
            else
            {
                newUserCanvas.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Return2MainMenu(int resuming)
        {
            textblockLoginLoading.Visibility = System.Windows.Visibility.Collapsed;

            newUserCanvas.Visibility = System.Windows.Visibility.Collapsed;
            returnCanvas.Visibility = System.Windows.Visibility.Visible;
            if (resuming == 1) buttonResumeGame.Visibility = System.Windows.Visibility.Visible;
            LoginUser = helper.LoadUser(userxml);
            string tempName = LoginUser.UserName;
            textBlockUserName.Text = tempName;
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            textBlockBackground.Text = string.Empty;
        }

        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (textBoxUserName.Text == "") textBlockBackground.Text = "Username";
            else CheckUserName();
        }

        private void CheckUserName()
        {
            CheckingUserName.Begin();
            RadProgressIndicator.IsRunning = true;
            TheOption = 1;
            theUserName = textBoxUserName.Text;
            WhereFailed = 1;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&UserName={1}", TheOption, theUserName);

            WebClient b = new WebClient();
            b.DownloadStringAsync(new Uri(newURL));
            b.DownloadStringCompleted += new DownloadStringCompletedEventHandler(b_DownloadStringCompleted);
        }

        private void b_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;
                if (s != "0")
                {
                    CheckingUserName.Stop();
                    RadProgressIndicator.IsRunning = false;
                    textBlockStatus.Foreground = (Brush)App.Current.Resources["MainRed"];
                    textBlockStatus.Text = "Username is taken. Try another one!";
                }
                else
                {
                    textBlockStatus.Foreground = (Brush)App.Current.Resources["LoadingGreen"];
                    textBlockStatus.Text = "Username is available!";
                    buttonCreateUser.Visibility = System.Windows.Visibility.Visible;
                    CheckingUserName.Stop();
                    RadProgressIndicator.IsRunning = false;
                }
            }
            catch
            {
                MessageBox.Show("You failed to connect.");
                //buttonRetry.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void buttonCreateUser_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TheOption = 2;
            WhereFailed = 2;
            string newURL = String.Format("http://www.myicaddy.com/petproject/AJAX_EmpireBuildingServices.php?Option={0}&UserName={1}", TheOption, theUserName);

            WebClient wcCreateUser = new WebClient();
            wcCreateUser.DownloadStringAsync(new Uri(newURL));
            wcCreateUser.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcCreateUser_DownloadStringCompleted);
        }

        private void wcCreateUser_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string s = e.Result;
                TextReader tr = new StringReader(s);
                XDocument xdoc = XDocument.Load(tr);

                if (s != string.Empty)
                {
                    int tUserID = Convert.ToInt32(xdoc.Element("User").Element("UserID").Value);
                    string tUserName = xdoc.Element("User").Element("UserName").Value;

                    userxml = helper.CheckFiles("User.xml", 1, 3, "");
                    userxml = helper.CreateNewUser(userxml, "1.00", tUserName, tUserID, DefaultUserCurrency, DefaultPaidCurrency, DefaultCurExPlots, 0, 0, 0);

                    LoginUser = helper.LoadUser(userxml);

                    //SyncSettings();
                    //ISXMLVersion = myFunctions.ReadXMLVersion(settingxml, "AllSettings");
                    //textBlockSyncSetting.Text = "Settings: Phone v" + ISXMLVersion;
                    //ProgressIndicator.IsRunning = true;

                    string tempName = LoginUser.UserName;
                    textBlockUserName.Text = tempName;
                    newUserCanvas.Visibility = System.Windows.Visibility.Collapsed;
                    returnCanvas.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch
            {
                MessageBox.Show("You failed to connect.");
                //buttonRetry.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void buttonNewGame_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            IsolatedStorageFile ISOstore = IsolatedStorageFile.GetUserStoreForApplication();
            if (ISOstore.FileExists("Field.xml") == true)
            {
                RadMessageBox.Show("New Empire?", MessageBoxButtons.YesNo, "Starting a new game will erase your existing Empire. Do you really want to start a New Game?", closedHandler: (args) =>
                {
                    if (args.ButtonIndex == 0)
                    {
                        // Navigate to the MainPage with the Option to delete all the existing XML files and start over.
                        NavigationService.Navigate(new Uri("/MainPage.xaml?NewGame=1", UriKind.Relative));
                    }
                });
            }
            else
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml?NewGame=1", UriKind.Relative));
            }
        }

        private void buttonResumeGame_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            RadProgressIndicator.IsRunning = true;
            NavigationService.Navigate(new Uri("/MainPage.xaml?Resume=1", UriKind.Relative));
        }

        private void buttonLeaderBoard_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/LBList.xaml?UserID=" + LoginUser.UserID + "&UserName=" + LoginUser.UserName, UriKind.Relative));
        }


    }
}