﻿using DataStructureVisualizer.Common.AnimationLib;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataStructureVisualizer.Views.Data
{
    /// <summary>
    /// Interaction logic for ArrayItemUserControl.xaml
    /// </summary>
    public partial class ArrayItemUserControl : SuccessiveItemUserControl
    {
        public ArrayItemUserControl()
        {
            InitializeComponent();

            ValueItem = valueItem;
        }

        
    }
}
