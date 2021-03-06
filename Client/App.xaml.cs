﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Network network;

        //checking if the server is up and if it doesnt showing a suitable messagebox
        protected override void OnStartup(StartupEventArgs e)
        {
            bool serverisUp = false;
            bool tryAgain = true;
            do
            {
                try
                {
                    network = new Network();
                    serverisUp = true;
                }
                catch { }

                if (!serverisUp)
                {
                    var result = MessageBox.Show("Server is not up, do you want to try to reconnect?", "Server down", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    tryAgain = result == MessageBoxResult.Yes;
                }

            } while (!serverisUp && tryAgain);

            if (serverisUp)
            {
                base.OnStartup(e);
                this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            }
            else
                this.Shutdown(1);
            
        }

    }
}
