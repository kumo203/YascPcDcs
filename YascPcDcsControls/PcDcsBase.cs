using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace YascPcDcsControls
{
    public class PcDcsBase : UserControl
    {
        readonly string separator = ".";

        [Category("Opc")]
        [Description("OPC Server Name")]
        public string OpcServerName
        {
            get { return (string)GetValue(OpcServerNameProperty); }
            set { SetValue(OpcServerNameProperty, value); }
        }

        public static readonly DependencyProperty OpcServerNameProperty =
            DependencyProperty.Register("OpcServerName", typeof(string), typeof(PcDcsBase), new PropertyMetadata(""));

        [Category("Opc")]
        [Description("OPC Register Name")]
        public string OpcRegisterName
        {
            get { return (string)GetValue(OpcRegisterNameProperty); }
            set { SetValue(OpcRegisterNameProperty, value); }
        }

        public static readonly DependencyProperty OpcRegisterNameProperty =
            DependencyProperty.Register("OpcRegisterName", typeof(string), typeof(PcDcsBase), new PropertyMetadata(""));

        public string OpcName
        {
            get => OpcServerName + separator + OpcRegisterName;
        }

    }

    public class PcDcsAnalog : PcDcsBase
    {
        virtual protected void PvUpdated()
        {
            Debug.WriteLine($"PcDcsAnalog::{OpcPV}");
        }
        virtual protected void SvUpdated()
        {
            Debug.WriteLine($"PcDcsAnalog::{OpcSV}");
        }
        virtual protected void MvUpdated()
        {
            Debug.WriteLine($"PcDcsAnalog::{OpcMV}");
        }

        [Category("Opc")]
        [Description("OPC PV")]
        public int OpcPV
        {
            get { return (int)GetValue(OpcPVProperty); }
            set { SetValue(OpcPVProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpcPV.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpcPVProperty =
            DependencyProperty.Register("OpcPV", typeof(int), typeof(PcDcsAnalog), new PropertyMetadata(0, PvChanged));

        static private void PvChanged(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var ths = (target as PcDcsAnalog);
            ths?.PvUpdated();
        }

        [Category("Opc")]
        [Description("OPC MV")]
        public int OpcMV
        {
            get { return (int)GetValue(OpcMVProperty); }
            set { SetValue(OpcMVProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpcMv.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpcMVProperty =
            DependencyProperty.Register("OpcMV", typeof(int), typeof(PcDcsAnalog), new PropertyMetadata(0, MvChanged));

        static private void MvChanged(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var ths = (target as PcDcsAnalog);
            ths?.MvUpdated();
        }

        public int OpcSV
        {
            get { return (int)GetValue(OpcSVProperty); }
            set { SetValue(OpcSVProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OpcSV.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpcSVProperty =
            DependencyProperty.Register("OpcSV", typeof(int), typeof(PcDcsAnalog), new PropertyMetadata(0, SvChanged));
        static private void SvChanged(DependencyObject target,
        DependencyPropertyChangedEventArgs e)
        {
            var ths = (target as PcDcsAnalog);
            ths?.SvUpdated();
        }
    }

    public class PcDcsDigital : PcDcsBase
    {
    }
}
