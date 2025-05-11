using CarService_SteeringWheel.DB;
using CarService_SteeringWheel.Windows;
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
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
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

        public Admin(User user)
        {
            InitializeComponent();

            try
            {
                _currUser = user;

                products = SqlHelper.findProducts();
                fullProducts = new List<Product>(products);

                txtAllAmount.Text = fullProducts.Count.ToString();
                txtResultAmount.Text = products.Count.ToString();

                LViewProduct.ItemsSource = products;

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
                        .Where(x =>
                            x.ProductDiscountAmount >= 0 &&
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
                .Where(x =>
                    x.ProductName
                        .ToLower()
                        .Contains(txtSearch.Text.Trim().ToLower())
                )
                .ToList();
            products = _products;
            txtResultAmount.Text = products.Count.ToString();
            LViewProduct.ItemsSource = products;
        }

        private void txtSearch_TextChanged1(object sender, TextChangedEventArgs e)
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
                txtFullName.Text = _currUser.UserSurname.ToString() +
                    _currUser.UserName.ToString() +
                    " " + _currUser.UserPatronymic.ToString();
            else
                txtFullName.Text = "Гость";
        }

        List<Product> orderProducts = new List<Product>();

        private void btnAddProduct_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            orderProducts.Add(LViewProduct.SelectedItem as Product);

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

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(
                new AddEditProductPage(LViewProduct.SelectedItem as Product)
            );
        }

        private void btnAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(
                new AddEditProductPage(null)
            );
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                SqlHelper.GetContext()
                    .ChangeTracker
                    .Entries()
                    .ToList()
                    .ForEach(x => x.Reload());
                LViewProduct.ItemsSource = SqlHelper.findProducts();
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }
    }
}
