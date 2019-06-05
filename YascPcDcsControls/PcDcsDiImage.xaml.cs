using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace YascPcDcsControls
{
    /// <summary>
    /// Interaction logic for PcDcsDiImage.xaml
    /// </summary>
    public partial class PcDcsDiImage : PcDcsDigital
    {
        BitmapImage OnImage;
        BitmapImage OffImage;

        public PcDcsDiImage()
        {
            InitializeComponent();
        }

        override protected void DiUpdated()
        {
            if (OpcDi==0)
            {
                Image.Source = OffImage;
            } else
            {
                Image.Source = OnImage;
            }
        }

        private void PcDcsDigital_Loaded(object sender, RoutedEventArgs e)
        {
            OnImage = new BitmapImage(new Uri("On.png", UriKind.Relative));
            OffImage = new BitmapImage(new Uri("Off.png", UriKind.Relative));
            if (OpcDi == 0)
            {
                Image.Source = OffImage;
            }
            else
            {
                Image.Source = OnImage;
            }
        }

        private void PcDcsDigital_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpcWindow w;
            var p = this.Parent;
            w = p as OpcWindow;
            while (w == null)
            {
                p = VisualTreeHelper.GetParent(p);
                w = p as OpcWindow;
            }

            w.opc.Write( new string[] { this.OpcName }, new object[] { (this.OpcDi+1)%2 }, out int[] error);
        }
    }
}
