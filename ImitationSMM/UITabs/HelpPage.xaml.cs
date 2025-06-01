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
    /// Interaction logic for HelpPage.xaml
    /// </summary>
    public partial class HelpPage : Page
    {
        private static HelpPage instance;
        public static HelpPage GetHelpPage()
        {
            return instance ?? (instance = new HelpPage());
        }
        private HelpPage()
        {
            InitializeComponent();
        }

        private void btnVisitServer_Click(object sender, RoutedEventArgs e)
        {
            // Would redirect to website, or server of some sort
            Process.Start(DiscordUtils.DiscordInvite);
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(AppVars.appInfo.Help.Replace("\n", Environment.NewLine), "Help Menu");
        }

        private void btnTOS_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(AppVars.appInfo.TOS, "Terms of Service");
        }

        private async void btnSubmitReview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await DiscordUtils.LogToDiscord("review", $"Review Message: {tbReview.Text}\nRating: {(int)slideRate.Value}/10");
            }
            catch
            {
                MessageBox.Show("Unable to submit review, please try again later");
            }
        }

        private void slideRate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblRate.Content = $"{(int)slideRate.Value}/10";
        }
    }
}
