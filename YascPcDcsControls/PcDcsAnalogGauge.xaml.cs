using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for PcDcsAnalogGauge.xaml
    /// </summary>
    public partial class PcDcsAnalogGauge : PcDcsAnalog
    {
        public PcDcsAnalogGauge()
        {
            InitializeComponent();
        }

        override protected void PvUpdated()
        {
            Debug.WriteLine($"PcDcsAnalogGauge::{OpcPV}");
            this.Gauge.Text = this.OpcPV.ToString();
        }
    }
}
