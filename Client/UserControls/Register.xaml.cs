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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        //fields
        private string username;
        private byte[] userDetails;

        //constructor
        public Register()
        {
            InitializeComponent();
        }

        //register details (string type) getter
        public string Details
        {
            get { return this.username; }
        }

        //register details (bytes array type) getter
        public byte[] UserDetails
        {
            get { return this.userDetails; }
        }

        public event RoutedEventHandler RegisterButtonClick; //event

        //sending the register details to the server through the main window
        private void button1_Click(object sender, RoutedEventArgs e)
        {
             e.Handled = true;
            string registerDetails = Convert.ToString((int)Network.eNetworkCommands.RegisterCommand)+" " + textBox1.Text + " " + textBox2.Text + " " + textBox3.Text + " " + passwordBox1.Password;
            username = textBox3.Text;
            userDetails = Encoding.ASCII.GetBytes(registerDetails);
            if (RegisterButtonClick != null)
                RegisterButtonClick(this, new RoutedEventArgs());
        }
    }
}
