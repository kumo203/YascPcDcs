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

        [Category("Opc")]
        [Description("On Image Path")]
        public string OnImagePath
        {
            get { return (string)GetValue(OnImagePathProperty); }
            set { SetValue(OnImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnImagePathProperty =
            DependencyProperty.Register("OnImagePath", typeof(string), typeof(PcDcsDiImage), new PropertyMetadata("")
                );

        [Category("Opc")]
        [Description("Off Image Path")]
        public string OffImagePath
        {
            get { return (string)GetValue(OffImagePathProperty); }
            set { SetValue(OffImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OffImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OffImagePathProperty =
            DependencyProperty.Register("OffImagePath", typeof(string), typeof(PcDcsDiImage), new PropertyMetadata(""));

        override protected void DiUpdated()
        {
            if (OpcDi==0)
            {
                Image.Source = OnImage;
            } else
            {
                Image.Source = OffImage;
            }
        }

        private void PcDcsDigital_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(OnImagePath))
            {
                OnImage = new BitmapImage(new Uri("On.png", UriKind.Relative));
            }
            if (!string.IsNullOrEmpty(OffImagePath))
            {
                OffImage = new BitmapImage(new Uri("Off.png", UriKind.Relative));
            }
            if (OpcDi == 0)
            {
                Image.Source = OffImage;
            }
            else
            {
                Image.Source = OnImage;
            }
        }
    }
}
