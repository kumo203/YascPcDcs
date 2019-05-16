using DxpSimpleAPI;
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

namespace YascPcDcs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DxpSimpleClass opc = new DxpSimpleClass();
        string opcHost = "localhost";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            opc.EnumServerList(opcHost, out string[] ServerNameArray);
            this.Title = ServerNameArray[0];

            opc.Connect(opcHost, this.Title);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            opc.Disconnect();
        }

    }
}
