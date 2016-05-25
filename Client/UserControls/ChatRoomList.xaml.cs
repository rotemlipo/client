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
    /// Interaction logic for ChatRoomList.xaml
    /// </summary>
    public partial class ChatRoomList : UserControl
    {
        string chosenGroup;
        public event RoutedEventHandler ChosingGroup;
        public ChatRoomList()
        {
            InitializeComponent();
        }
        public string ChosenGroup
        {
            get { return this.chosenGroup; }
        }

        public void AddItems(string[] list)
        {
            this.groupBox.Dispatcher.Invoke((Action)(() =>
            {
                for (int i = 0; i < list.Length; i++)
                    this.groupBox.Items.Add(list[i]);
            }));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            chosenGroup = groupBox.SelectedItem.ToString();
            while (chosenGroup == null)
                MessageBox.Show("please pick a group");
            if (ChosingGroup != null)
                ChosingGroup(this, new RoutedEventArgs());


        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
