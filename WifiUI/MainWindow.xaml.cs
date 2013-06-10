using NetSh;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace WifiUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        /// <summary>
        /// Constructs the window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            trayIcon.TrayLeftMouseDown += trayIcon_TrayLeftMouseDown;
            WifiProfiles = new ObservableCollection<WifiProfile>();
            listView.ItemsSource = WifiProfiles;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Represents the list of Wifi profiles to display.
        /// </summary>
        public ObservableCollection<WifiProfile> WifiProfiles { get; set; }

        #endregion

        #region Private Methods

        private void RefreshItemsSource()
        {
            WifiProfiles.Clear();

            List<WifiProfile> wifiProfiles = NetShWrapper.GetWifiProfiles();

            /*
             * Iterate over NetSh results to display them in the view.
             */
            foreach (WifiProfile wifiProfile in wifiProfiles)
            {
                WifiProfiles.Add(wifiProfile);
            }

            listView.Items.Refresh();
        }

        #endregion

        #region Events

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void trayIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            RefreshItemsSource();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            NetShWrapper.DeleteWifiProfile((listView.SelectedItem as WifiProfile).Name);
        }

        private void DeleteAutoOpen_Click(object sender, RoutedEventArgs e)
        {
            List<WifiProfile> wifiProfiles = NetShWrapper.GetWifiProfiles();

            /*
             * Iterate over NetSh results to remove bad profiles.
             */
            foreach (WifiProfile wifiProfile in wifiProfiles.Where(NetShWrapper.IsOpenAndAutoWifiProfile))
            {
                NetShWrapper.DeleteWifiProfile(wifiProfile.Name);
            }
        }

        #endregion
    }
}
