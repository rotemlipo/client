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
        private string username;
        private byte[] userDetails;
        public Login()
        {
            InitializeComponent();
        }
        public string Details
        {
            get { return this.username; }
        }
        public byte[] UserDetails
        {
            get { return this.userDetails; }
        }
        public event RoutedEventHandler LogInButtonClick;
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
