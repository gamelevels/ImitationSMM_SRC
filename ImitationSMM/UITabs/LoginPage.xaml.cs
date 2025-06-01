using DataObjects;
using DataObjects.models;
using Logic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImitationSMM.UITabs
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private MainManager mainManager;

        public LoginPage()
        {
            InitializeComponent();
            mainManager = MainManager.GetMainManager();

            try
            {
                AppVars.appInfo = mainManager.ApplicationManager.SetApplicationInformation();
                AppVars.appInfo.Help = AppVars.appInfo.Help.Replace("\\n", Environment.NewLine);
                AppVars.appInfo.TOS = AppVars.appInfo.TOS.Replace("\\n", Environment.NewLine);

                foreach (Webhook wh in mainManager.WebhookManager.GetWebhooks())
                {
                    AppVars.appInfo.Webhooks.Add(wh);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to load program information: {ex.Message}");
            }
            tbUsername.Text = "username";
            tbPassword.Password = "mypassword";
        }

        private void lblRegister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterPage());

        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = false;
            string uname = tbUsername.Text, passw = tbPassword.Password;
            if (uname.ValidString() && passw.ValidString())
            {
                try
                {
                    UserVM user = mainManager.UserManager.LoginUser(uname, passw);
                    // Could and shouldve combined all logging into one method
                    // But works for what it needs to be for project
                    await DiscordUtils.LogToDiscord("login", $"{uname} has logged in");
                    mainManager.LogManager.InsertUserLog(new UserLog()
                    {
                        Username = uname,
                        LogType = "Login",
                        LogDetails = $"{uname} has logged in"
                    });
                    mainManager.LoggedInUser = user;

                    new HubWindow().Show();
                    CloseWindow.CloseParent();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Login Failed");
                    tbUsername.Focus();
                    goto end;
                }
            }
            else
            {
                MessageBox.Show("Username or password is too short to be valid", "Login error");
                tbUsername.Focus();
            }
        end:
            btnLogin.IsEnabled = true;
        }

        private void cbViewPassword_Click(object sender, RoutedEventArgs e)
        {
            if (cbViewPassword.IsChecked == true)
            {
                tbPassword.Visibility = Visibility.Collapsed;
                tbPasswordTxt.Visibility = Visibility.Visible;
                tbPasswordTxt.Focus();
            }
            else
            {
                tbPassword.Visibility = Visibility.Visible;
                tbPasswordTxt.Visibility = Visibility.Collapsed;
                tbPassword.Focus();
            }
        }
    }
}
