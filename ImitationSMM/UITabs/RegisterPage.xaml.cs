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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private MainManager mainManager;
        public RegisterPage()
        {
            InitializeComponent();
            mainManager = MainManager.GetMainManager();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            btnRegister.IsEnabled = false;
            string uname = tbUsername.Text, passw = tbPassword.Password, confPassw = tbConfirmPassword.Password, regToken = tbToken.Text;
            if (uname.ValidString() && passw.ValidString() && regToken.ValidGuid())
            {
                if (passw != confPassw)
                {
                    MessageBox.Show("Passwords do not match", "Register Fail");
                    goto end;
                }

                try
                {
                    User findUser = mainManager.UserManager.FindUser(uname);
                    if (findUser != null)
                    {
                        throw new ArgumentException("User already exists");
                    }

                    Guid newToken = Guid.Parse(regToken);
                    RegisterToken registerToken = mainManager.UserManager.GetToken(newToken);
                    if (registerToken == null || registerToken.UsedBy != null)
                    {
                        throw new ArgumentException("Token has already been claimed");
                    }

                    User newUser = new User()
                    {
                        Username = uname,
                        LevelInfo = new Levels() { Level = registerToken.Level },
                        PasswordHash = mainManager.UserManager.Hash256(passw),
                        Register = new RegisterToken()
                        {
                            Token = registerToken.Token
                        },
                        Expiration = DateTime.Now.AddDays(registerToken.Days),
                    };

                    if (mainManager.UserManager.RegisterUser(newUser))
                    {
                        MessageBox.Show("Register successful, please goto the login page", "Register Complete");
                        await DiscordUtils.LogToDiscord("register", $"New Register:\nUsername: {uname}\nRegister Token: {regToken}");
                        mainManager.LogManager.InsertUserLog(new UserLog()
                        {
                            Username = uname,
                            LogType = "Register",
                            LogDetails = $"{uname} has registered with token {registerToken.Token}"
                        });
                    }
                    else
                    {
                        throw new ArgumentException("Unable to complete register, try again later");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Register Failed");
                    tbUsername.Focus();
                    goto end;
                }
            }
            else
            {
                tbUsername.Focus();
            }
        end:
            btnRegister.IsEnabled = true;
        }

        private void lblLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage());
        }

        private void cbShowPassword_Click(object sender, RoutedEventArgs e)
        {
            // Changing password char of the passwordbox erases the entered password
            // So doing this to keep text and display clear text password
            if (cbShowPassword.IsChecked == true)
            {
                tbPassword.Visibility = Visibility.Collapsed;
                tbConfirmPassword.Visibility = Visibility.Collapsed;
                tbPasswordTxt.Visibility = Visibility.Visible;
                tbConfirmPasswordTxt.Visibility = Visibility.Visible;
            }
            else
            {
                tbPassword.Visibility = Visibility.Visible;
                tbConfirmPassword.Visibility = Visibility.Visible;
                tbPasswordTxt.Visibility = Visibility.Collapsed;
                tbConfirmPasswordTxt.Visibility = Visibility.Collapsed;
            }
        }
    }
}
