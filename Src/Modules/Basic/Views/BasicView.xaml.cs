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

namespace Basic.Views
{
    /// <summary>
    /// Logika interakcji dla klasy BasicView.xaml
    /// </summary>
    public partial class BasicView : UserControl
    {
        public BasicView()
        {
            InitializeComponent();
        }

        //private void ListView_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    var fff = sender as ListView;
        //    Console.WriteLine(fff.SelectedItem);
        //}

        //private void ListViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    Console.WriteLine("rightclick");
        //}
    }
}
