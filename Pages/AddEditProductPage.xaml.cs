using CarService_SteeringWheel.DB;
using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        Product product = new Product();
        public string[] CategoryList = new string[]
        {
            "Аксессуары",
            "Автозапчасти",
            "Автосервис",
            "Съемники подшипников",
            "Ручные инструменты",
            "Зарядные устройства"
        };

        public AddEditProductPage(Product currentProduct)
        {
            InitializeComponent();

            if (currentProduct != null)
            {
                product = currentProduct;

                btnDeleteProduct.Visibility = Visibility.Visible;
                txtArticle.IsEnabled = false;
            }

            DataContext = product;
            cmbCategory.ItemsSource = CategoryList;
        }

        private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog();

            GetImageDialog.Filter = "Файлы изображений: (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg";
            GetImageDialog.InitialDirectory = "C:\\Users\\yulia\\Desktop\\Rul\\Rul\\Resources";
            if (GetImageDialog.ShowDialog() == true)
            {
                product.ProductImage = GetImageDialog.FileName;
            }
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотите удалить {product.ProductName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    SqlHelper.GetContext().Product.Remove(product);
                    SqlHelper.GetContext().SaveChanges();
                    MessageBox.Show("Запись удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (product.ProductCost < 0)
                errors.AppendLine("Стоимость не может быть отрицательной!");
            if (product.MinCount < 0)
                errors.AppendLine("Минимальное количество не может быть отрицательным!");             //Прописываем проверки по заданию
            if (product.ProductDiscountAmount > product.MaxDiscountAmount)
                errors.AppendLine("Действующая скидка на товар не может быть больше максимальной скидки!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString()); //Выводим ошибки
                return;
            }

            if (txtArticle.Text.Trim() != null && !txtArticle.Text.Trim().Equals("") && 
                txtArticle.IsEnabled == true)
            {
                try
                {
                    product.ProductArticleNumber = txtArticle.Text.Trim();
                    product.ProductName = txtTitle.Text.Trim();
                    product.ProductDescription = txtDescription.Text.Trim();
                    product.ProductCategory = cmbCategory.SelectedIndex.ToString();
                    product.ProductManufacturer = txtManufacturer.Text.Trim();
                    product.ProductCost = Convert.ToDecimal(txtCost.Text.Trim());
                    product.ProductDiscountAmount = Convert.ToByte(txtDiscount.Text.Trim());
                    product.ProductQuantityInStock = Convert.ToInt32(txtCountInStock.Text.Trim());
                    product.ProductStatus = "Новый";
                    product.Unit = txtUnit.Text.Trim();
                    product.MaxDiscountAmount = Convert.ToByte(txtMaxDiscount.Text.Trim());
                    product.Supplier = txtSupplier.Text.Trim();
                    product.CountInPack = Convert.ToInt32(txtCountInPack.Text.Trim());
                    product.MinCount = Convert.ToInt32(txtMinCount.Text.Trim());

                    SqlHelper.GetContext().Product.Add(product); //Добавляем объект в БД
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка входных данных!");
                }
            }

            try
            {
                SqlHelper.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information); //Сохраняем данные в БД
                NavigationService.GoBack(); //Переходим на предыдущую страницу
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); //Выводим ошибки
            }
        }
    }
}
