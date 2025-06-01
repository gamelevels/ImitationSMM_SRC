using DataObjects;
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
    /// Interaction logic for InfoPage.xaml
    /// </summary>
    public partial class InfoPage : Page
    {
        private static InfoPage instance;
        public static InfoPage GetInfoPage()
        {
            return instance ?? (instance = new InfoPage());
        }

        private MainManager manager;
        private InfoPage()
        {
            InitializeComponent();
            manager = MainManager.GetMainManager();


            // Not using account ids, this will be fine for the purpose
            lblAccountID.Content = manager.LoggedInUser.Username.Length.ToString();
            
            lblUsername.Content = manager.LoggedInUser.Username;
            lblLevel.Content = manager.LoggedInUser.LevelInfo.Level;
            lblHandleCD.Content = manager.LoggedInUser.LevelInfo.HandleCooldown;
            lblExpiration.Content = manager.LoggedInUser.Expiration;

            tbMOTD.Text = AppVars.appInfo.MOTD;
            lblPopularPlatform.Content = AppVars.appInfo.PopularPlatform;
            lblRegisteredUsers.Content = AppVars.appInfo.UserCount;

            // Don't need to tell users account count failed to retreive
            // Just set it as 0
            loadAccount();
            lblAppVersion.Content = $"v{AppVars.appInfo.AppVersion}";
        }
        public async void loadAccount()
        {
            int accs = 0;
            try
            {
                accs = await manager.APIManager.GetAccountCount();
            }
            catch { }
            lblSMMAccounts.Content = accs;
        }
    }
}
