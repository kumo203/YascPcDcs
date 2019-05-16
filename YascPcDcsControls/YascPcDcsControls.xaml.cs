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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PcDcsControl : UserControl
    {
        readonly string separator = ".";

        public PcDcsControl()
        {
            InitializeComponent();
        }

        [Category("Opc")]
        [Description("OPC Server Name")]
        public string OpcServerName
        {
            get { return (string)GetValue(OpcServerNameProperty); }
            set { SetValue(OpcServerNameProperty, value); }
        }

        public static readonly DependencyProperty OpcServerNameProperty =
            DependencyProperty.Register("OpcServerName", typeof(string), typeof(PcDcsControl), new PropertyMetadata(""));


        [Category("Opc")]
        [Description("OPC Register Name")]
        public string OpcRegisterName
        {
            get { return (string)GetValue(OpcRegisterNameProperty); }
            set { SetValue(OpcRegisterNameProperty, value); }
        }

        public static readonly DependencyProperty OpcRegisterNameProperty =
            DependencyProperty.Register("OpcRegisterName", typeof(string), typeof(PcDcsControl), new PropertyMetadata(""));

        public string OpcName
        {
            get => OpcServerName + separator + OpcRegisterName;
        }

        [Category("Opc")]
        [Description("OPC Analog Value")]
        public int OpcAnalogValue
        {
            get { return (int)GetValue(OpcAnalogValueProperty); }
            set { SetValue(OpcAnalogValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpcAnalogValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpcAnalogValueProperty =
            DependencyProperty.Register("OpcAnalogValue", typeof(int), typeof(PcDcsControl), new PropertyMetadata(0, ValueChanged));

        static private void ValueChanged(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var ths = (target as PcDcsControl);
            ths.TextBlock.Text = ths.OpcAnalogValue.ToString();
        }

    }
}
