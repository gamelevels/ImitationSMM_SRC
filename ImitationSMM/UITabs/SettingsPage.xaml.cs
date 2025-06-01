using DataObjects.models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImitationSMM.UITabs
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private MainManager manager;
        private static SettingsPage instance;
        public static SettingsPage GetSettingsPage()
        {
            return instance ?? (instance = new SettingsPage());
        }
        public SettingsPage()
        {
            InitializeComponent();
            manager = MainManager.GetMainManager();
            btnMOTD.Background = manager.LoggedInUser.Settings.EnableMOTD ? Brushes.LimeGreen : Brushes.Brown; 
        }

        private void btnMOTD_Click(object sender, RoutedEventArgs e)
        {
            // Unfortunately the wpf controls are terrible
            // So I had to resort to doing something like this. 
            btnMOTD.Background = (btnMOTD.Background == Brushes.LimeGreen) ? Brushes.Brown : Brushes.LimeGreen;
            manager.LoggedInUser.Settings.EnableMOTD = (btnMOTD.Background == Brushes.Brown) ? false : true;
            manager.UserManager.UpdateUserSettings(manager.LoggedInUser);
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<UserHistory> userHistory = manager.LogManager.GetUserHistoryLogs(manager.LoggedInUser);
                foreach (UserHistory item in userHistory)
                {
                    tbRequestHistory.Text += $"{item.Username}: {item.RequestDetails} - [{item.Date}]\n";
                }
            }
            catch
            {
                MessageBox.Show("Unable to populate your request history at this time", "Request history fail");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            new UpdateUser(manager.LoggedInUser).ShowDialog();
        }
    }
}
