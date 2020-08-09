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
using BusinessLogic;

namespace BikeShopApp
{
    /// <summary>
    /// TestPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TestPage : Page
    {
        public TestPage()
        {
            InitializeComponent();

            InitListBox();
        }

        private void InitListBox()
        {
            Random rand = new Random();
            string[] names = { "LHY", "KGR", "YYJ", "ODU", "BGH", "LCS" };
            List<Car> lists = new List< Car > ();
            for (int i = 0; i < 10; i++)
            {
                byte red = (byte)(i % 3 == 0 ? 255 : (i * 50) % 255);
                byte green = 0;
                byte blue = (byte)(i % 3 == 0 ? (i * 90) % 255 : 255) ;
                int index = rand.Next(names.Length);
                lists.Add(new Car
                {
                    Speed = i * 30,
                    Color = Color.FromRgb(red, green, blue),
                    Driver = new Human { Name = names[index], HasDrivingLicense = true}
                });
            }
            //LstCar.DataContext = lists;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Car car = new Car();
            car.Speed = 100;
            car.Color = Colors.Black;
            car.Driver = new Human { Name = "Nick", HasDrivingLicense = true};

            //this.DataContext = car;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hell world!");
        }
    }
}
