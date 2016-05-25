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
        public NewGroup()
        {
            InitializeComponent();
        }

        public void Init(List<string> users)
        {
            this.usersBox.Dispatcher.Invoke((Action)(() => users.ForEach(x => this.usersBox.Items.Add(x))));
        }

        public event RoutedEventHandler NewGroupButtonClick;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (NewGroupButtonClick != null)
                NewGroupButtonClick(this, new RoutedEventArgs());
        }


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

        public string GroupName { get { return UserGrouptextBox.Text; } }
    }
}
