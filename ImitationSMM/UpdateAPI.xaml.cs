using DataObjects.models;
using Logic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ImitationSMM
{
    /// <summary>
    /// Interaction logic for UpdateAPI.xaml
    /// </summary>
    public partial class UpdateAPI : Window
    {
        APIManager apiManager = null;
        API api = null;

        private MainManager manager;
        public UpdateAPI(API api)
        {
            InitializeComponent();
            this.api = api;
            manager = MainManager.GetMainManager();
            tbLink.Text = api.Link;
            tbLink.IsEnabled = false;

            tbPlaceholder.Text = api.ParameterPlaceHolder;
            btnEnabled.Background = api.Enabled ? Brushes.LimeGreen : Brushes.Brown;

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool EnableValue(Control control)
        {
            return control.Background == Brushes.LimeGreen;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            api.ParameterPlaceHolder = tbPlaceholder.Text;
            api.Enabled = EnableValue(btnEnabled);
            try
            {
                apiManager.UpdateAPI(api);
                MessageBox.Show("API information has been updated", "API updated");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rectHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnEnabled_Click(object sender, RoutedEventArgs e)
        {
            btnEnabled.Background = (btnEnabled.Background == Brushes.LimeGreen) ? Brushes.Brown : Brushes.LimeGreen;
        }
    }
}
