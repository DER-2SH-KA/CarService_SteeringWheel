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
using CarService_SteeringWheel.Windows;

namespace CarService_SteeringWheel.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        private User _currUser = null;
        private List<Product> products;
        private List<Product> fullProducts;
        private string[] sortList = new string[]
        {
            "Без сортировки",
            "Стоимость по возрастанию",
            "Стоимость по убыванию"
        };
        private string[] filterDiscountList = new string[]
        {
            "Все диапазоны",
            "0%-9,99%",
            "10%-14,99%",
            "15% и более"
        };

        /*public Client()
        {
            InitializeComponent();

            try
            {
                products = SqlHelper.findProducts();
                fullProducts = new List<Product>(products);

                txtBlockAllCount.Text = fullProducts.Count.ToString();
                txtBlockResultAmountCount.Text = products.Count.ToString();

                lViewProduct.ItemsSource = products;

                cmbSorting.ItemsSource = sortList;
                cmbFilter.ItemsSource = filterDiscountList;

                User();
            }
            catch (Exception ex) { Console.Error.WriteLine(ex.Message); }
        }*/

        public Client(User user)
        {
            InitializeComponent();

            try
            {
                _currUser = user;

                products = SqlHelper.findProducts();
                fullProducts = new List<Product>(products);

                txtBlockAllCount.Text = fullProducts.Count.ToString();
                txtBlockResultAmountCount.Text = products.Count.ToString();

                lViewProduct.ItemsSource = products;

                cmbSorting.ItemsSource = sortList;
                cmbFilter.ItemsSource = filterDiscountList;

                User();
            }
            catch (Exception ex) { Console.Error.WriteLine(ex.Message); }
        }

        public void UpdateData()
        {
            var _products = fullProducts;

            switch (cmbSorting.SelectedIndex)
            {
                case 1:
                    _products = _products
                        .OrderBy(p =>
                            Convert.ToDouble(p.CostWithDiscount)
                        )
                        .ToList();
                    break;

                case 2:
                    _products = _products
                        .OrderByDescending(p => 
                            Convert.ToDouble(p.CostWithDiscount)
                        )
                        .ToList();
                    break;

                default:
                    break;
            }

            switch (cmbFilter.SelectedIndex)
            {
                case 1:
                    _products = _products
                        .Where( x => 
                            x.ProductDiscountAmount  >= 0 && 
                            x.ProductDiscountAmount < 10
                        ).ToList(); 
                    break;

                case 2:
                    _products = _products
                        .Where(x =>
                            x.ProductDiscountAmount >= 10 &&
                            x.ProductDiscountAmount < 15
                        ).ToList();
                    break;

                case 3:
                    _products = _products
                        .Where(x =>
                            x.ProductDiscountAmount >= 15
                        ).ToList();
                    break;
            }

            _products = _products
                .Where( x => 
                    x.ProductName
                        .ToLower()
                        .Contains( txtSearch.Text.Trim().ToLower() ) 
                )
                .ToList();
            products = _products;
            txtBlockResultAmountCount.Text = products.Count.ToString();
            lViewProduct.ItemsSource = products;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void User()
        {
            if (_currUser != null)
                txtBlockFullName.Text = _currUser.UserSurname.ToString() + 
                    _currUser.UserName.ToString() + 
                    " " + _currUser.UserPatronymic.ToString();
            else
                txtBlockFullName.Text = "Гость";
        }

        List<Product> orderProducts = new List<Product>();

        private void btnAddProduct_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            orderProducts.Add(lViewProduct.SelectedItem as Product);

            if (orderProducts.Count > 0)
            {
                btnOrder.Visibility = Visibility.Visible;
            }
        }

        private void btnOrder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OrderWindow order = new OrderWindow(orderProducts, _currUser);
            order.ShowDialog();
        }
    }
}
