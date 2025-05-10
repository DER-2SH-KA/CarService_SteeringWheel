using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace CarService_SteeringWheel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Loading MainWindow Window 
        /// and Set Autho page from Pafes directory.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            FrmMain.Navigate(new Pages.Autho());

        }

        /// <summary>
        /// Hide or Show Back Button to first object in Frame's history.
        /// </summary>
        private void FrmMain_ContentRendered(object sender, EventArgs e)
        {
            if (FrmMain.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible; // If history of Frame is't Empty.
            }
            else
            {
                btnBack.Visibility = Visibility.Hidden; // If history of Frame is Empty
            }
        }

        /// <summary>
        /// Go to the Laft object in Frame's history.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrmMain.GoBack();
        }
    }
}
