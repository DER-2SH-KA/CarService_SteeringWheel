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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        List<Product> productList = new List<Product>(); //Создаем пустой лист, к которому можно будет обращаться во всех методах
        // Ссылок: 1
        public OrderPage(List<Product> products, User user)
        {
            InitializeComponent();

            DataContext = this;
            productList = products;
            LViewOrder.ItemsSource = productList;

            cmbPickupPoint.ItemsSource = SqlHelper.findPickupPoints();

            if (user != null)                                                               //Добавляем проверку на пользователя, если пользователь есть в системе,
                txtUser.Text = user.UserSurname.ToString() + user.UserName.ToString() 
                    + " " + user.UserPatronymic.ToString();
        }

        public string Total
        {
            get
            {
                var total = productList.Sum(p => Convert.ToDouble(p.ProductCost) - Convert.ToDouble(p.ProductCost) * Convert.ToDouble(p.ProductDiscountAmount / 100.00));
                return total.ToString();
            }
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                productList.Remove(LViewOrder.SelectedItem as Product);
            LViewOrder.Items.Refresh();
        }

        private void btnOrderSave_Click(object sender, RoutedEventArgs e)
        {
            var productArticle = productList.Select(p => p.ProductArticleNumber).ToArray();
            Random random = new Random(); 
            var date = DateTime.Now;
            if (productList.Any(p => p.ProductQuantityInStock < 3)) 
            {
                date = date.AddDays(6);
            }
            else
                date = date.AddDays(3);
            //
            if (cmbPickupPoint.SelectedItem == null)
            {
                MessageBox.Show("Выберите пункт выдачи!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Order newOrder = new Order()
                {
                    OrderStatus = "Новый",
                    OrderDate = DateTime.Now,
                    OrderPickupPoint = cmbPickupPoint.SelectedIndex + 1,
                    OrderDeliveryDate = date,
                    ReceiptCode = random.Next(100, 1000),
                    ClientFullName = txtUser.Text,
                };

                SqlHelper.GetContext().Order.Add(newOrder);

                for (int i = 0; i < productArticle.Count(); i++)
                {
                    OrderProduct newOrderProduct = new OrderProduct()
                    {
                        OrderID = newOrder.OrderID,
                        ProductArticleNumber = productArticle[i],
                        ProductCount = 1
                    };
                    SqlHelper.GetContext().OrderProduct.Add(newOrderProduct);
                }

                SqlHelper.GetContext().SaveChanges();
                MessageBox.Show("Заказ оформлен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new OrderTicketPage(newOrder, productList));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
