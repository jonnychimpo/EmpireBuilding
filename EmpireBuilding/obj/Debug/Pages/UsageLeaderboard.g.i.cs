﻿#pragma checksum "C:\Users\jthoma\SkyDrive\EmpireBuilding\EmpireBuilding\Pages\UsageLeaderboard.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A98F994E01A65C8A94282812D81F8D52"
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
using Microsoft.Phone.Shell;
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


namespace EmpireBuilding.Pages {
    
    
    public partial class UsageLeaderboard : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem menuItem1;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock textBlockConnectError;
        
        internal Microsoft.Phone.Controls.Pivot pivotUsageLeaderBoards;
        
        internal Microsoft.Phone.Controls.PivotItem pivotTIG;
        
        internal System.Windows.Controls.ListBox listboxLBTIG;
        
        internal Microsoft.Phone.Controls.PivotItem pivotMoneySpent;
        
        internal System.Windows.Controls.ListBox listboxLBMS;
        
        internal Microsoft.Phone.Controls.PivotItem pivotBFU;
        
        internal System.Windows.Controls.ListBox listboxLBBFU;
        
        internal System.Windows.Controls.TextBlock textBlockLBUserName;
        
        internal System.Windows.Controls.TextBlock textBlockLBRank;
        
        internal Telerik.Windows.Controls.RadBusyIndicator ProgressIndicator;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/EmpireBuilding;component/Pages/UsageLeaderboard.xaml", System.UriKind.Relative));
            this.menuItem1 = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("menuItem1")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.textBlockConnectError = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockConnectError")));
            this.pivotUsageLeaderBoards = ((Microsoft.Phone.Controls.Pivot)(this.FindName("pivotUsageLeaderBoards")));
            this.pivotTIG = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("pivotTIG")));
            this.listboxLBTIG = ((System.Windows.Controls.ListBox)(this.FindName("listboxLBTIG")));
            this.pivotMoneySpent = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("pivotMoneySpent")));
            this.listboxLBMS = ((System.Windows.Controls.ListBox)(this.FindName("listboxLBMS")));
            this.pivotBFU = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("pivotBFU")));
            this.listboxLBBFU = ((System.Windows.Controls.ListBox)(this.FindName("listboxLBBFU")));
            this.textBlockLBUserName = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockLBUserName")));
            this.textBlockLBRank = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockLBRank")));
            this.ProgressIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("ProgressIndicator")));
        }
    }
}

