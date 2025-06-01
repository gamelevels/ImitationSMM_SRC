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
using System.Windows.Shapes;

namespace ImitationSMM
{
    /// <summary>
    /// Interaction logic for UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Window
    {
        User user = null;
        private MainManager manager;
        public UpdateUser(User user, bool isAdmin = false)
        {
            InitializeComponent();
            this.user = user;
            manager = MainManager.GetMainManager();

            tbUsername.Text = user.Username;
            tbUsername.IsEnabled = false;

            tbLevel.Text = user.LevelInfo.Level.ToString();
            lblDate.Content = user.Expiration;
            dpDate.SelectedDate = user.Expiration;
            btnEnabled.Background = user.Enabled ? Brushes.LimeGreen : Brushes.Brown;
            btnMOTDEnabled.Background = user.Settings.EnableMOTD ? Brushes.LimeGreen : Brushes.Brown;

            if(user.LevelInfo.Level != 10 && isAdmin == false)
            {
                tbLevel.IsEnabled = false;
                btnEnabled.IsEnabled = false;  
                dpDate.IsEnabled = false;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEnabled_Click(object sender, RoutedEventArgs e)
        {
            btnEnabled.Background = (btnEnabled.Background == Brushes.LimeGreen) ? Brushes.Brown : Brushes.LimeGreen;
        }

        private void btnMOTDEnabled_Click(object sender, RoutedEventArgs e)
        {
            btnMOTDEnabled.Background = (btnMOTDEnabled.Background == Brushes.LimeGreen) ? Brushes.Brown : Brushes.LimeGreen;
        }

        private void dpDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            lblDate.Content = dpDate.SelectedDate.ToString().Split(' ')[0];
        }

        private void rectHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }

        private bool EnableValue(Control control)
        {
            return control.Background == Brushes.LimeGreen;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int num = 0;
            if(int.TryParse(tbLevel.Text, out num))
            {
                user.LevelInfo.Level = num;
            }
            else
            {
                MessageBox.Show("Please enter level as a number");
                return;
            }

            user.Expiration = dpDate.SelectedDate.Value;
            user.Enabled = EnableValue(btnEnabled);
            user.Settings.EnableMOTD = EnableValue(btnMOTDEnabled);

            try
            {
                manager.UserManager.UpdateUserSettings(user);
                manager.UserManager.UpdateUser(user);
                MessageBox.Show("Account information has been updated", "Account updated");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
