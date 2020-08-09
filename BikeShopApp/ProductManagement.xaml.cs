using System.Diagnostics;
using System.Windows.Controls;

namespace BikeShopApp
{
    /// <summary>
    /// ProductManagement.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProductManagement : Page
    {
        ProductsFactory factory;

        public ProductManagement()
        {
            InitializeComponent();

            factory = new ProductsFactory();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            GrdProducts.ItemsSource = factory.FindProducts(TxtSearch.Text);
        }

        
        private void TxtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                GrdProducts.ItemsSource = factory.FindProducts(TxtSearch.Text);
            }
        }

        
    }
}
