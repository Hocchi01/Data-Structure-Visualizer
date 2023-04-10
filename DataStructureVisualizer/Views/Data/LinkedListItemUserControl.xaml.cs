﻿using System;
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

namespace DataStructureVisualizer.Views.Data
{
    /// <summary>
    /// Interaction logic for LinkedListItemUserControl.xaml
    /// </summary>
    public partial class LinkedListItemUserControl : UserControl
    {
        // public double NextPointerLength { get; set; } = 35;
        public LinkedListItemUserControl()
        {
            InitializeComponent();
        }
        public LinkedListItemUserControl(double nextPointerLength) : this()
        {
            next.Length = nextPointerLength;
        }
    }
}
