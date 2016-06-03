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
    /// Interaction logic for NewGroup.xaml
    /// </summary>
    public partial class NewGroup : UserControl
    {
        //constructor
        public NewGroup()
        {
            InitializeComponent();
        }

        //adding the usernames to the listbox. we dont add the user itself - he shouldnt pick himself. we use invoking.
        public void Init(List<string> users,string user)
        {
            foreach (var item in users)
            {
                if (item != user)
                    this.usersBox.Dispatcher.Invoke((Action)(() => this.usersBox.Items.Add(item)));
            }
            
        }

        public event RoutedEventHandler NewGroupButtonClick; //event
        
        //sending the server new group's details through the main window
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (NewGroupButtonClick != null)
                NewGroupButtonClick(this, new RoutedEventArgs());
        }

        //returning all the users that are chosen
        public List<string> UserList
        {
            get
            {
                List<string> userList = new List<string>();
                foreach (var item in usersBox.SelectedItems)
                    userList.Add(item.ToString());

                return userList;
            }
        }

        //returning the group's name
        public string GroupName 
        { 
            get 
            { 
                return UserGrouptextBox.Text; 
            } 
        }
    }
}
