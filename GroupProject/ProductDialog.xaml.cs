using System.Globalization;
using System.Windows;
using GroupProject.Models;

namespace GroupProject;

public partial class ProductDialog : Window
{
    public Product? ResultProduct { get; private set; }

    public ProductDialog()
    {
        InitializeComponent();
    }

    public void LoadProduct(Product product)
    {
        NameBox.Text = product.Name;
        CategoryBox.Text = product.Category;
        PriceBox.Text = product.Price.ToString(CultureInfo.CurrentCulture);
        StockBox.Text = product.Stock.ToString();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text) ||
            string.IsNullOrWhiteSpace(CategoryBox.Text))
        {
            MessageBox.Show("Введите название и категорию.");
            return;
        }

        if (!decimal.TryParse(
                PriceBox.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out decimal price) || price < 0)
        {
            MessageBox.Show("Введите корректную цену.");
            return;
        }

        if (!int.TryParse(StockBox.Text, out int stock) || stock < 0)
        {
            MessageBox.Show("Введите целое количество не меньше нуля.");
            return;
        }

        ResultProduct = new Product
        {
            Name = NameBox.Text.Trim(),
            Category = CategoryBox.Text.Trim(),
            Price = price,
            Stock = stock
        };

        DialogResult = true;
    }
}