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
    /// Interaction logic for UpdateWebhook.xaml
    /// </summary>
    public partial class UpdateWebhook : Window
    {
        Webhook webhook = null;

        private MainManager manager;
        public UpdateWebhook(Webhook webhook)
        {
            InitializeComponent();
            this.webhook = webhook;
            manager = MainManager.GetMainManager();
            tbLink.Text = webhook.Link;
            tbTitle.Text = webhook.Title;
            btnEnabled.Background = webhook.Enabled ? Brushes.LimeGreen : Brushes.Brown;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
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
            webhook.Title = tbTitle.Text;
            webhook.Enabled = EnableValue(btnEnabled);
            try
            {
                manager.WebhookManager.UpdateWebhook(webhook);
                MessageBox.Show("Webhook information has been updated", "Webhook updated");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
