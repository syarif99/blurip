﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BluRip
{
    /// <summary>
    /// Interaktionslogik für ShutdownWindow.xaml
    /// </summary>
    public partial class ShutdownWindow : Window
    {
        private DispatcherTimer timer;
        private int countdown = 120;

        public ShutdownWindow()
        {
            InitializeComponent();
            try
            {
                labelShutdownCounter.Content = ResFormat("LabelShutdownCounter", countdown);
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += new EventHandler(TimerTick);
                timer.Start();
            }
            catch (Exception)
            {
            }
        }

        private string ResFormat(string key, params object[] para)
        {
            try
            {
                return String.Format((string)App.Current.Resources[key], para);
            }
            catch (Exception)
            {
                return "Unknown resource";
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                labelShutdownCounter.Content = ResFormat("LabelShutdownCounter", countdown);
                countdown--;
                if (countdown == 0)
                {
                    DialogResult = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                timer.Stop();
            }
            catch (Exception)
            {
            }
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                timer.Stop();
                DialogResult = true;
            }
            catch (Exception)
            {
            }
        }
    }
}