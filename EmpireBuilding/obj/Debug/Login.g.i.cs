﻿#pragma checksum "C:\Users\jthoma\SkyDrive\EmpireBuilding\EmpireBuilding\Login.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1E2E6DE6B90A36A948784721FF86797E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows.Controls;


namespace EmpireBuilding {
    
    
    public partial class Login : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard CheckingUserName;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Canvas newUserCanvas;
        
        internal System.Windows.Controls.TextBlock textBlockBackground;
        
        internal System.Windows.Controls.TextBox textBoxUserName;
        
        internal System.Windows.Controls.TextBlock textBlockStatus;
        
        internal System.Windows.Controls.Button buttonCreateUser;
        
        internal System.Windows.Controls.Canvas returnCanvas;
        
        internal System.Windows.Controls.Button buttonResumeGame;
        
        internal System.Windows.Controls.Button buttonNewGame;
        
        internal System.Windows.Controls.Button buttonLeaderBoard;
        
        internal System.Windows.Controls.TextBlock textBlockUserName;
        
        internal System.Windows.Controls.TextBlock textblockLoginLoading;
        
        internal System.Windows.Controls.Button buttonRetryLoad;
        
        internal Telerik.Windows.Controls.RadBusyIndicator RadProgressIndicator;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/EmpireBuilding;component/Login.xaml", System.UriKind.Relative));
            this.CheckingUserName = ((System.Windows.Media.Animation.Storyboard)(this.FindName("CheckingUserName")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.newUserCanvas = ((System.Windows.Controls.Canvas)(this.FindName("newUserCanvas")));
            this.textBlockBackground = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockBackground")));
            this.textBoxUserName = ((System.Windows.Controls.TextBox)(this.FindName("textBoxUserName")));
            this.textBlockStatus = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockStatus")));
            this.buttonCreateUser = ((System.Windows.Controls.Button)(this.FindName("buttonCreateUser")));
            this.returnCanvas = ((System.Windows.Controls.Canvas)(this.FindName("returnCanvas")));
            this.buttonResumeGame = ((System.Windows.Controls.Button)(this.FindName("buttonResumeGame")));
            this.buttonNewGame = ((System.Windows.Controls.Button)(this.FindName("buttonNewGame")));
            this.buttonLeaderBoard = ((System.Windows.Controls.Button)(this.FindName("buttonLeaderBoard")));
            this.textBlockUserName = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockUserName")));
            this.textblockLoginLoading = ((System.Windows.Controls.TextBlock)(this.FindName("textblockLoginLoading")));
            this.buttonRetryLoad = ((System.Windows.Controls.Button)(this.FindName("buttonRetryLoad")));
            this.RadProgressIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("RadProgressIndicator")));
        }
    }
}

