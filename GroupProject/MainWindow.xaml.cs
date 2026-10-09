using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GroupProject.Models;

namespace GroupProject;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<Product> _products = new();

    public MainWindow()
    {
        InitializeComponent();

        // Временные данные для проверки интерфейса.
        _products.Add(new Product
        {
            Id = 1,
            Name = "Кроссовки",
            Category = "Спортивная обувь",
            Price = 5990,
            Stock = 12
        });

        _products.Add(new Product
        {
            Id = 2,
            Name = "Ботинки",
            Category = "Зимняя обувь",
            Price = 8990,
            Stock = 5
        });

        ProductsGrid.ItemsSource = _products;
        CollectionViewSource.GetDefaultView(_products).Filter = MatchesSearch;
    }

    private bool MatchesSearch(object item)
    {
        return item is Product product &&
               product.Name.Contains(
                   SearchBox.Text,
                   StringComparison.CurrentCultureIgnoreCase);
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        CollectionViewSource.GetDefaultView(_products).Refresh();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ProductDialog
        {
            Owner = this
        };

        if (dialog.ShowDialog() == true && dialog.ResultProduct is not null)
        {
            dialog.ResultProduct.Id = _products.Count == 0
                ? 1
                : _products.Max(product => product.Id) + 1;

            _products.Add(dialog.ResultProduct);
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductsGrid.SelectedItem is not Product selected)
        {
            MessageBox.Show("Сначала выберите товар.");
            return;
        }

        var dialog = new ProductDialog
        {
            Owner = this
        };

        dialog.LoadProduct(selected);

        if (dialog.ShowDialog() == true && dialog.ResultProduct is not null)
        {
            selected.Name = dialog.ResultProduct.Name;
            selected.Category = dialog.ResultProduct.Category;
            selected.Price = dialog.ResultProduct.Price;
            selected.Stock = dialog.ResultProduct.Stock;

            ProductsGrid.Items.Refresh();
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductsGrid.SelectedItem is Product selected)
        {
            _products.Remove(selected);
        }
        else
        {
            MessageBox.Show("Сначала выберите товар.");
        }
    }
}