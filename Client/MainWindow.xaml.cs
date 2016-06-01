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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Fields
        string user = "";
        bool flag,flagGroups;
        Network network;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            this.flag = false;
            this.ucLogin.Visibility = Visibility.Visible;
            this.btnNewUser.Visibility = Visibility.Visible;
            this.network = new Network();
            this.network.NewConnection += NewConnection;
            this.flagGroups = false;
        }

        #endregion

        #region Event Handlers

        private void SendLoginDetails(object sender, RoutedEventArgs e)
        {
            network.SendMessages(this.ucLogin.UserDetails);
        }

        private void SendRegisterDetails(object sender, RoutedEventArgs e)
        {
            network.SendMessages(this.ucRegister.UserDetails);
        }

        private void btnNewUserClick(object sender, RoutedEventArgs e)
        {
            this.HideAllUserControl();
            this.ucRegister.Visibility = Visibility.Visible;
        }

        private void btnNewGroupClick(object sender, RoutedEventArgs e)
        {
            byte[] mess = Encoding.ASCII.GetBytes(Convert.ToString((int)Network.eNetworkCommands.GetUsersCommand));
            network.SendMessages(mess);
        }

        private void CreateNewGroup(object sender, RoutedEventArgs e)
        {
            string newGroupComand = Convert.ToString((int)Network.eNetworkCommands.NewGroupCommand);
            string newGroupData = newGroupComand + " " + this.ucNewGroup.GroupName;
            foreach (var item in this.ucNewGroup.UserList)
                newGroupData += " " + item;
            byte[] groupDataBytes = Encoding.ASCII.GetBytes(newGroupData);
            network.SendMessages(groupDataBytes);
        }

        private void ChooseGroup(object sender, RoutedEventArgs e)
        {
            string chosenGroup = Convert.ToString((int)Network.eNetworkCommands.GroupIsOK) + " " + this.ucChatRoomList.ChosenGroup;
            byte[] groupNameBytes = Encoding.ASCII.GetBytes(chosenGroup);
            network.SendMessages(groupNameBytes);
        }

        private void SendMessage (object sender, RoutedEventArgs e)
        {
            string message = Convert.ToString((int)Network.eNetworkCommands.Messages)+" "+this.ucChat.LastMessage;
            network.SendMessages(Encoding.ASCII.GetBytes(message));
        }

        #endregion

        #region Private Methods

        private void NewConnection(string mess)
        {
            
            if (mess == Convert.ToString((int)Network.eNetworkCommands.Error) && !flag)
            {
                flag = true;
                MessageBox.Show("invalid information, try again");
            }
            else if ((mess == Convert.ToString((int)Network.eNetworkCommands.LoginCommand) && !flag) || (mess == Convert.ToString((int)Network.eNetworkCommands.RegisterCommand) && !flag))
            {
                this.flag = true;
                MessageBox.Show("login succeeded");

                if (mess == Convert.ToString((int)Network.eNetworkCommands.LoginCommand))
                    user = this.ucLogin.Details;
                else if (mess == Convert.ToString((int)Network.eNetworkCommands.RegisterCommand))
                    user = this.ucRegister.Details;

                this.HideAllUserControl();
                this.ChangeVisibility(this.ucChatRoomList, () => this.ucChatRoomList.Visibility = Visibility.Visible);
                this.ChangeVisibility(this.btnNewGroup, () => this.btnNewGroup.Visibility = Visibility.Visible);
            }
            else if (mess[0] == ';')
            {
                mess = mess.Substring(1);
                this.ucChatRoomList.AddItems(mess.Split(';'));
            }
            else if (mess[0].ToString() == Convert.ToString((int)Network.eNetworkCommands.GetUsersCommand))
            {
                List<string> usersList = new List<string>();
                if (mess.Length > 1)
                {
                    mess = mess.Substring(2);
                    string[] users = mess.Split(' ');
                    for (int i = 0; i < users.Length; i++)
                    {
                        usersList.Add(users[i]);
                    }
                }
                this.HideAllUserControl();
                this.ucNewGroup.Init(usersList);
                this.ChangeVisibility(this.ucNewGroup, () => this.ucNewGroup.Visibility = Visibility.Visible);
            }

            else if (mess[0].ToString() == Convert.ToString((int)Network.eNetworkCommands.ErrorGroup))
                MessageBox.Show("group name already exists");
            else if (mess[0].ToString() == Convert.ToString((int)Network.eNetworkCommands.GroupIsOK))
            {
                MessageBox.Show("welcome, you can chat now");
                this.HideAllUserControl();
                this.ChangeVisibility(this.ucChat, () => this.ucChat.Visibility = Visibility.Visible);
            }
            else if (mess[0].ToString() == Convert.ToString((int)Network.eNetworkCommands.Messages))
            {
                this.HideAllUserControl();
                this.ChangeVisibility(this.ucChat, () => this.ucChat.Visibility = Visibility.Visible);
                MessageBox.Show("whelcome, you can shat now");
                this.ucChat.Sender = this.user;
                mess = mess.Substring(2);
                string[] messArr = mess.Split(';');
                this.ucChat.SetMessages(messArr);
                this.ucChat.ShowOldMessages();
            }
            else if (mess[0].ToString() == Convert.ToString((int)Network.eNetworkCommands.ChatMessage))
            {
                    ucChat.AddMessage(mess.Substring(1));
            }

        }

        private void HideAllUserControl()
        {
            this.ChangeVisibility(this.ucLogin, () => this.ucLogin.Visibility = Visibility.Collapsed);
            this.ChangeVisibility(this.ucRegister, () => this.ucRegister.Visibility = Visibility.Collapsed);
            this.ChangeVisibility(this.ucChatRoomList, () => this.ucChatRoomList.Visibility = Visibility.Collapsed);
            this.ChangeVisibility(this.ucChat, () => this.ucChat.Visibility = Visibility.Collapsed);
            this.ChangeVisibility(this.ucNewGroup, () => this.ucNewGroup.Visibility = Visibility.Collapsed);
            this.ChangeVisibility(this.btnNewUser, () => this.btnNewUser.Visibility = Visibility.Collapsed);
            this.ChangeVisibility(this.btnNewGroup, () => this.btnNewGroup.Visibility = Visibility.Collapsed);
        }

        private void ChangeVisibility(UIElement uiElement, Action action)
        {
            if (!uiElement.Dispatcher.CheckAccess())
                uiElement.Dispatcher.Invoke(action);
            else
                action();
        }

        #endregion

    }
}