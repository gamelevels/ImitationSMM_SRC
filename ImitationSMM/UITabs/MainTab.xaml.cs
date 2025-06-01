using DataObjects;
using DataObjects.models;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImitationSMM.UITabs
{
    /// <summary>
    /// Interaction logic for MainTab.xaml
    /// </summary>
    public partial class MainTab : Page
    {
        List<PlatformConstraint> constraints = null;
        List<API> apis = null;
        bool changedHandle = false;
        private MainTab()
        {
            InitializeComponent();
            manager = MainManager.GetMainManager();

            try
            {
                apis = manager.APIManager.GetAPIs();
                constraints = manager.PlatformManager.GetPlatformConstraints();
                foreach (Platform plat in manager.PlatformManager.GetUserPlatforms(manager.LoggedInUser.LevelInfo.Level))
                {
                    // No addrange ??
                    cbPlatform.Items.Add(plat.PlatformName);
                }
            }
            catch
            {
                MessageBox.Show("Unable to load backend data, please try again later", "Critical Application Error");
                CloseWindow.CloseParent();
            }

            // May add configuration for this in admin dashboard
            cbPlatform.SelectedIndex = 0;
            tbRequestNums.Text = $"0-{manager.LoggedInUser.LevelInfo.Level * 2500}";
            sliderRequest.Maximum = manager.LoggedInUser.LevelInfo.Level * 2500;
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            if (rbTOS.IsChecked == false)
            {
                MessageBox.Show("You must agree to ToS before continuing", "Terms of Service Notification");
                goto end;
            }

            string selectedPlatform = cbPlatform.Text.ToLower();
            string selectedFill = cbFillType.Text.ToLower();

            if (selectedFill.Contains("custom") && tbRequestNums.Text.Contains('-'))
            {
                MessageBox.Show("Please enter a valid request number", "Invalid Request Notification");
                goto end;
            }

            foreach (API api in apis)
            {
                APIRequest request = new APIRequest()
                {
                    URL = api.Link,
                    Handle = tbHandle.Text,
                    // Looking at this now, shouldve uploaded objects to the comboboxes, but this works
                    FillType = selectedFill.Split(' ')[1].Split(' ')[0].Replace("[", "").Replace("]", ""),
                    Platform = selectedPlatform,
                    Requests = Convert.ToInt32(tbRequestNums.Text)
                };
                try
                {
                    APIResponse response = await manager.APIManager.SendAPIRequest(request);
                    tbRequestHistory.Text += $"API Result: {response.result}, API Message: {response.message}\n";
                    manager.LogManager.InsertAPIHistory(new APILog()
                    {
                        Link = api.Link,
                        UsedBy = manager.LoggedInUser.Username,
                        Response = response.message,
                        Date = DateTime.Now
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to request one of our backend APIs, please try again later " + ex.Message, "Request error");
                }
            }

            try
            {
                manager.LogManager.InsertUserHistory(new UserHistory()
                {
                    Username = manager.LoggedInUser.Username,
                    RequestHandle = tbRequestHistory.Text,
                    RequestType = selectedFill,
                    RequestDetails = selectedPlatform
                });
            }
            catch
            {
                // pointless, also dont need to tell them it failed
                // would rewrite method but too late in project
                goto end;
            }

        end: btnStart.IsEnabled = true;
        }

        private void cbPlatform_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = "";
            if (cbPlatform.SelectedItem != null && cbPlatform.SelectedIndex != -1)
            {
                selected = cbPlatform.SelectedItem.ToString();
                cbFillType.Items.Clear();
                platformDisplay.Content = selected;
            }

            List<PlatformConstraint> straints = constraints.FindAll(c => c.PlatformName.Contains(selected));
            foreach (PlatformConstraint constraint in straints)
            {
                cbFillType.Items.Add($"{constraint.PlatformName}: {constraint.RequestType} [{constraint.RequestCount}]");
            }
            cbFillType.Items.Add($"{selected}: [CUSTOM]");

            cbFillType.SelectedIndex = 0;

            if (!changedHandle)
            {
                tbHandle.Text = PlaceholderHandle.URLS[selected];
            }
        }

        private void cbFillType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = "";
            if (cbFillType.SelectedItem != null && cbFillType.SelectedIndex != -1)
            {
                selected = cbFillType.SelectedItem.ToString();
                fillTypeDisplay.Content = selected;
            }

            sliderRequest.IsEnabled = selected.Contains("CUSTOM") ? true : false;

            string output = Regex.Replace(selected, "[^.0-9]", ""); // drop everything but request count
            if (output != string.Empty)
            {
                tbRequestNums.Text = output;
                sliderRequest.Value = Convert.ToInt32(output);
            }
        }

        private void tbHandle_TextChanged(object sender, TextChangedEventArgs e)
        {
            // when they erase example text
            // this text box doesnt support placeholder text
            if (tbHandle.Text.Length == 0)
            {
                changedHandle = true;
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(AppVars.appInfo.TOS, "Terms of Service");
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbRequestNums.Text.Contains("-") || cbFillType.SelectedItem.ToString().ToLower().Contains("custom"))
            {
                tbRequestNums.Text = ((int)sliderRequest.Value).ToString();
            }
        }

        private async void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (API api in apis)
            {
                APIRequest request = new APIRequest()
                {
                    URL = api.Link,
                    Handle = tbHandle.Text
                };
                try
                {
                    APIResponse response = await manager.APIManager.SendAPICancelRequest(request);
                    if (response == null)
                    {
                        throw new ApplicationException("Unable to cancel request");
                    }
                    tbRequestHistory.Text += $"API Result: {response.result}, API Message: {response.message}\n";
                    manager.LogManager.InsertAPIHistory(new APILog()
                    {
                        Link = api.Link,
                        UsedBy = manager.LoggedInUser.Username,
                        Response = response.message,
                        Date = DateTime.Now
                    });

                    manager.LogManager.InsertUserHistory(new UserHistory()
                    {
                        Username = manager.LoggedInUser.Username,
                        RequestHandle = tbRequestHistory.Text,
                        RequestType = "Cancel",
                        RequestDetails = "Cancel"
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to request one of our backend APIs, please try again later " + ex.Message, "Request error");
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbRequestHistory.Text = "";
        }



        private static MainTab instance;
        public static MainTab GetMainTab()
        {
            return instance ?? (instance = new MainTab());
        }

        private MainManager manager;
    }
}
