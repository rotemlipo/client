using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.UserControls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        //fields
        private string username;
        private byte[] userDetails;

        //constructor
        public Login()
        {
            InitializeComponent();
        }

        //details (string type) getter
        public string Details
        {
            get { return this.username; }
        }

        //details (bytes array) getter
        public byte[] UserDetails
        {
            get { return this.userDetails; }
        }

        public event RoutedEventHandler LogInButtonClick; //event

        //sending the details that the user typed in to the server through the main window
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            string loginDetails = Convert.ToString((int)Network.eNetworkCommands.LoginCommand) + " " + textBox1.Text + " " + passwordBox1.Password;
            username = textBox1.Text;
            userDetails = Encoding.ASCII.GetBytes(loginDetails);
            if (LogInButtonClick != null)
                LogInButtonClick(this, new RoutedEventArgs());
        }

    }
}
