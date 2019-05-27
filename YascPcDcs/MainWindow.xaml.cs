using DxpSimpleAPI;
using OpcRcw.Da;
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
using System.Windows.Threading;
using YascPcDcsControls;

namespace YascPcDcs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DxpSimpleClass opc = new DxpSimpleClass();
        string opcHost = "localhost";
        DispatcherTimer timer;
        Dictionary<string, List<UserControl>> opcName2Controls = new Dictionary<string, List<UserControl>>();
        string[] opcNames;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            opc.EnumServerList(opcHost, out string[] ServerNameArray);
            this.Title = ServerNameArray[0];
            opc.Connect(opcHost, this.Title);

            GetChildrenControl(this);
            opcNames = opcName2Controls.Keys.ToArray();

            SetupTimer();
        }
        private void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1); // every 1sec

            timer.Tick += new EventHandler(TimerMethod);
            timer.Start();

            this.Closing += new CancelEventHandler(StopTimer);

        }

        private void TimerMethod(object sender, EventArgs e)
        {
            opc.Read(opcNames, out object[] oValueArray, out short[] wQualityArray, out FILETIME[] fTimeArray, out int[] nErrorArray);
            for (int i=0; i<opcNames.Length; i++)
            {
#if DEBUG
                Debug.WriteLine($"{opcNames[i]}::{oValueArray[i]}");
#endif
                foreach (var c in opcName2Controls[opcNames[i]])
                {
                    (c as PcDcsAnalogGauge).OpcAnalogValue = Convert.ToInt32(oValueArray[i]);
                    (c as PcDcsAnalogGauge).OpcPV = Convert.ToInt32(oValueArray[i]);
                }
            }

        }


        private void StopTimer(object sender, CancelEventArgs e)
        {
            timer.Stop();
        }

        private void GetChildrenControl(FrameworkElement a)
        {
            if (a==null)
            {
                return;
            }
            foreach (var c in LogicalTreeHelper.GetChildren(a))
            {
                GetChildrenControl(c as FrameworkElement);

                var ctr = c as PcDcsAnalogGauge;

                if (ctr != null)
                {
                    Debug.WriteLine(ctr.OpcName);
                    if ( opcName2Controls.Keys.Contains(ctr.OpcName) ) {
                        opcName2Controls[ctr.OpcName].Add(ctr);
                    } else {
                        opcName2Controls.Add(ctr.OpcName, new List<UserControl>() { ctr });
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            opc.Disconnect();
        }

    }
}
