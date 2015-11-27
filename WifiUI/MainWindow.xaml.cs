using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NetSh;

namespace WifiUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            trayIcon.TrayLeftMouseDown += trayIcon_TrayLeftMouseDown;
            WifiProfiles = new ObservableCollection<WifiProfile>();
            listView.ItemsSource = WifiProfiles;
        }
        
        /// <summary>
        /// Represents the list of Wifi profiles to display.
        /// </summary>
        public ObservableCollection<WifiProfile> WifiProfiles { get; set; }
        
        private async Task RefreshItemsSourceAsync()
        {
            listView.Visibility = Visibility.Collapsed;
            loadingBorder.Visibility = Visibility.Visible;

            WifiProfiles.Clear();

            List<WifiProfile> wifiProfiles = await NetShWrapper.GetWifiProfilesAsync();
            
            foreach (WifiProfile wifiProfile in wifiProfiles)
            {
                WifiProfiles.Add(wifiProfile);
            }

            listView.Items.Refresh();

            listView.Visibility = Visibility.Visible;
            loadingBorder.Visibility = Visibility.Collapsed;
        }

        #region Events

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        async void trayIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            await RefreshItemsSourceAsync();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            NetShWrapper.DeleteWifiProfile((listView.SelectedItem as WifiProfile).Name);
        }

        private async void DeleteAutoOpen_Click(object sender, RoutedEventArgs e)
        {
            List<WifiProfile> wifiProfiles = await NetShWrapper.GetWifiProfilesAsync();
            
            foreach (WifiProfile wifiProfile in wifiProfiles.Where(NetShWrapper.IsOpenAndAutoWifiProfile))
            {
                NetShWrapper.DeleteWifiProfile(wifiProfile.Name);
            }
        }

        #endregion
    }
}
