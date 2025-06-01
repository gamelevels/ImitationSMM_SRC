using DataObjects;
using DataObjects.models;
using ImitationSMM.UITabs;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImitationSMM
{
    /// <summary>
    /// Interaction logic for HubWindow.xaml
    /// </summary>
    public partial class HubWindow : Window
    {
        private MainManager manager;
        public HubWindow()
        {
            InitializeComponent();
            manager = MainManager.GetMainManager();
            try
            {
                AppVars.appInfo.UserCount = manager.ApplicationManager.GetUserCount();
                AppVars.appInfo.PopularPlatform = manager.PlatformManager.GetPopularPlatform();
            }
            catch
            {
                MessageBox.Show("Unable to load crucial backend data, please try again later", "Critical Application Error");
                this.Close();
            }

            if(manager.LoggedInUser.LevelInfo.Level != 10)
            {
                btnAdmin.Visibility = Visibility.Hidden;
            }

            if(manager.LoggedInUser.Settings.EnableMOTD)
            {
                Task.Run(() => MessageBox.Show(AppVars.appInfo.MOTD, "Message of the day"));
            }
        }

        private void rectHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMainTab_Click(object sender, RoutedEventArgs e)
        {
            tabDisplayPanel.Navigate(MainTab.GetMainTab());
        }

        private void btnAppInfo_Click(object sender, RoutedEventArgs e)
        {
            tabDisplayPanel.Navigate(InfoPage.GetInfoPage());
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            tabDisplayPanel.Navigate(HelpPage.GetHelpPage());
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            tabDisplayPanel.Navigate(SettingsPage.GetSettingsPage());

        }

        private void btnTickets_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Feature to be implemented!");
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            tabDisplayPanel.Navigate(AdminPage.GetAdminPage());
        }
    }
}
