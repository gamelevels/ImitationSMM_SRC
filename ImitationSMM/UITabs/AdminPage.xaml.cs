using DataObjects.models;
using Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private static AdminPage instance;
        public static AdminPage GetAdminPage()
        {
            return instance ?? (instance = new AdminPage());
        }

        private MainManager manager;
        private AdminPage()
        {
            InitializeComponent();
            manager = MainManager.GetMainManager();
        }

        private void updateDisplays()
        {
            try
            {
                // Completely forgot to make a generate register tokens pages
                // Demonstrated with a token that is in the register tokens table

                if (tabUser.IsSelected)
                {
                    List<User> users = manager.UserManager.GetUsers();
                    dgUsers.ItemsSource = users;
                }
                if (tabAPI.IsSelected)
                {
                    List<API> apis = manager.APIManager.GetAPIs();
                    dgAPI.ItemsSource = apis;
                }
                if (tabWebhook.IsSelected)
                {
                    List<Webhook> webhooks = manager.WebhookManager.GetWebhooks();
                    dgWebhooks.ItemsSource = webhooks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to populate datagrids: {ex.Message}", "Population error");
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateDisplays();
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItems.Count != 0)
            {
                new UpdateUser((User)dgUsers.SelectedItem, true).ShowDialog();
                updateDisplays();
            }
        }

        private void dgAPI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAPI.SelectedItems.Count != 0)
            {
                new UpdateAPI((API)dgAPI.SelectedItem).ShowDialog();
                updateDisplays();
            }
        }

        private void dgWebhooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgWebhooks.SelectedItems.Count != 0)
            {
                new UpdateWebhook((Webhook)dgWebhooks.SelectedItem).ShowDialog();
                updateDisplays();
            }
        }
    }
}
