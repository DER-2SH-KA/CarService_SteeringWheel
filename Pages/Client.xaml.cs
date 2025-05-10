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

namespace CarService_SteeringWheel.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        private User _currUser = null;

        public Client()
        {
            InitializeComponent();

            try
            {
                List<Product> products = SqlHelper.findProducts();
                lViewProduct.ItemsSource = products;
            }
            catch (Exception ex) { Console.Error.WriteLine(ex.Message); }
        }
        public Client(User user)
        {
            InitializeComponent();

            try
            {
                _currUser = user;

                List<Product> products = SqlHelper.findProducts();
                lViewProduct.ItemsSource = products;
            }
            catch (Exception ex) { Console.Error.WriteLine(ex.Message); }
        }
    }
}
