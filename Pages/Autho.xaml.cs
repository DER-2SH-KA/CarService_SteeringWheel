using CarService_SteeringWheel.DB;
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
using System.Windows.Threading;

namespace CarService_SteeringWheel.Pages
{
    /// <summary>
    /// Логика взаимодействия для Aurho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        private static Int32 _click; // Count of tries to enter into System.
        // private readonly static demonEntities db = new demonEntities();
        private string captchaText = ""; // Captcha value.

        public Autho()
        {
            InitializeComponent();
            textBoxLogin.Focus(); // Set focus on Login TextBox Field.
            _click = 0;
        }

        /// <summary>
        /// Navigate Guest User to Client Page.
        /// </summary>
        private void BtnEnterAsGuest_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Pages.Client());
        }

        /// <summary>
        /// Decoration and Show Captcha UI.
        /// </summary>
        /// <param name="captchaText">String Captcha value</param>
        private void GenerateCaptcha(string captchaText)
        {
            textBlockCaptcha.Visibility = Visibility.Visible;
            textBoxCaptcha.Visibility = Visibility.Visible;

            textBlockCaptcha.Text = captchaText;
            textBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }

        /// <summary>
        /// Try to find User by Login and Password.
        /// </summary>
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            /*_click++;
            string login = textBoxLogin.Text.Trim();
            string password = pswBoxPassword.Password.Trim();
            string hashedPassword = HashPasswords.HashPasswords.toHashSHA256(password);

            // textBoxCaptcha.Text = hashedPassword;
            // ShowHashPasswordInMessageBox(password);

            // Find User by Login and Password.
            Users user = HelperDemon.findUserByLoginAndPassword(
                login, hashedPassword
            );

            Roles role;

            // MessageBox.Show($"User: {user.user_login}");

            if (1 == _click)
            {
                // If user was founded.
                if (user != null)
                {
                    role = user.Roles;

                    MessageBox.Show(
                        $"Вы вошли под: {role.role_name}"
                    );

                    textBoxLogin.Text = "";
                    pswBoxPassword.Password = "";
                    captchaText = "";
                    textBlockCaptcha.Text = "Капчта: ";
                    textBoxCaptcha.Text = "";

                    textBlockCaptcha.Visibility = Visibility.Hidden;
                    textBoxCaptcha.Visibility = Visibility.Hidden;

                    textBoxLogin.Focus();

                    LoadPage(user);

                    // _click = 0;
                    // NavigationService.Navigate( new DFAPage(user) ); // Navigate to 2FA Page.

                }
                else
                {
                    captchaText = CaptchaGenerator.GenerateCaptchaText(6); // Generate Captcha.
                }
            }
            else if (0 == _click % 3)
            {
                MessageBox.Show($"clicks: {_click} == 3");
                DisableFiledsAndShowTimer();
            }
            else if (1 < _click)
            {
                // Show Captcha Fields and Try Enter into System Again.

                GenerateCaptcha(captchaText);
                // MessageBox.Show($"Капча: {textBlockCaptcha.Text}\nМоё поле: {textBoxCaptcha.Text}");
                if (user != null
                    && HashPasswords.HashPasswords.toHashSHA256(textBoxCaptcha.Text) ==
                    HashPasswords.HashPasswords.toHashSHA256(textBlockCaptcha.Text))
                {
                    role = user.Roles;
                    MessageBox.Show(
                        $"Вы вошли под: {role.role_name}"
                    );

                    textBoxLogin.Text = "";
                    pswBoxPassword.Password = "";
                    captchaText = "";
                    textBlockCaptcha.Text = "Капчта: ";
                    textBoxCaptcha.Text = "";

                    textBlockCaptcha.Visibility = Visibility.Hidden;
                    textBoxCaptcha.Visibility = Visibility.Hidden;

                    textBoxLogin.Focus();

                    LoadPage(user);
                }
                else
                {
                    MessageBox.Show(
                        $"Введите данные заново!"
                    );
                    captchaText = CaptchaGenerator.GenerateCaptchaText(6);
                    GenerateCaptcha(captchaText);
                }
            }*/
        }

        /*/// <summary>
        /// Go to Page by User Role value.
        /// </summary>
        /// <param name="user"></param>
        private void LoadPage(
            Users user
        )
        {
            Roles role = user.Roles;
            _click = 0;
            switch (role.role_name.ToString())
            {
                case "Клиент":
                    NavigationService.Navigate(
                        new Client(
                            user,
                            role
                        )
                    );
                    break;

                case "Админ":
                    NavigationService.Navigate(
                        new Admin(
                            user,
                            role
                        )
                    );
                    break;

                default:
                    MessageBox.Show("Такой роли нет!");
                    break;
            }
        }*/
    }
}
