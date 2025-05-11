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

            textBlockCaptcha.Visibility = Visibility.Hidden;
            textBoxCaptcha.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Navigate Guest User to Client Page.
        /// </summary>
        private void BtnEnterAsGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.Client(null));
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
            string login = textBoxLogin.Text.Trim();
            string password = pswBoxPassword.Password.Trim();

            // textBoxCaptcha.Text = hashedPassword;
            // ShowHashPasswordInMessageBox(password);

            // Find User by Login and Password.
            User user = SqlHelper.findUserByLoginAndPassword(
                login, password
            );

            // MessageBox.Show($"User: {user.user_login}");

            if (1 > _click)
            {
                // If user was founded.
                if (user != null)
                {
                    MessageBox.Show(
                        $"Вы вошли под: {user.Role.RoleName.ToString()}"
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
                    MessageBox.Show("Вы ввели неверно логин или пароль!");
                    captchaText = GenerateCaptchaText(6);
                }
            }
        }

        /// <summary>
        /// Go to Page by User Role value.
        /// </summary>
        /// <param name="user"></param>
        private void LoadPage(
            User user
        )
        {
            Role role = user.Role;
            _click = 0;
            switch (role.RoleName.ToString())
            {
                case "Клиент":
                    NavigationService.Navigate(
                        new Pages.Client(
                            user
                        )
                    );
                    break;

                case "Менеджер":
                    NavigationService.Navigate(new Client(user));
                    break;

                /*case "Админ":
                    NavigationService.Navigate(
                        new Admin(
                            user,
                            role
                        )
                    );
                    break;*/

                default:
                    MessageBox.Show("Такой роли нет!");
                    break;
            }
        }

        private static readonly Random RD = new Random();
        private const string CHARACTERS =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// Generate new Catpcha by length.
        /// </summary>
        /// <param name="length">Int32 Length of Captcha</param>
        /// <returns>String Generated Captcha</returns>
        public static string GenerateCaptchaText(int length)
        {
            string result = "";

            try
            {
                if (1 > length)
                {
                    throw new ArgumentException(
                        "Длина текста капчи должна быть больше нуля!"
                    );
                }

                StringBuilder captchaTextByStringBuilder = new StringBuilder(length);

                // Generate Catpcha.
                int index = -1;
                for (int i = 0; i < length; i++)
                {
                    index = RD.Next(CHARACTERS.Length);
                    captchaTextByStringBuilder.Append(CHARACTERS[index]);
                }

                result = captchaTextByStringBuilder.ToString();
            }
            catch (ArgumentException aex)
            {
                Console.WriteLine(aex);
                result = "Длина меньше нуля!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = ex.Message;
            }
            finally
            {
                Console.WriteLine("Работа модуля капчи завершена");
            }

            return result;
        }
    }
}
