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

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

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
            this.btnNewUser.Visibility = Visibility.Visible; //showing login user control and new user button 
            this.network = new Network();
            this.network.NewConnection += NewConnection; //adding the method NewConnection to the event
            this.flagGroups = false;
        }

        #endregion

        #region Event Handlers

        //sending login details to the server for checking them
        private void SendLoginDetails(object sender, RoutedEventArgs e)
        {
            network.SendMessages(this.ucLogin.UserDetails);
        }

        //sending register details to the server for checking them and saving them
        private void SendRegisterDetails(object sender, RoutedEventArgs e)
        {
            network.SendMessages(this.ucRegister.UserDetails);
        }

        //showing register user control
        private void btnNewUserClick(object sender, RoutedEventArgs e)
        {
            this.HideAllUserControl();
            this.ucRegister.Visibility = Visibility.Visible;
        }

        //sending the server a request to send all users usernames
        private void btnNewGroupClick(object sender, RoutedEventArgs e)
        {
            byte[] mess = Encoding.ASCII.GetBytes(Convert.ToString((int)Network.eNetworkCommands.GetUsersCommand));
            network.SendMessages(mess);
        }

        //sending the server group details - groupname and the users that are chosen
        private void CreateNewGroup(object sender, RoutedEventArgs e)
        {
            string newGroupComand = Convert.ToString((int)Network.eNetworkCommands.NewGroupCommand);
            string newGroupData = newGroupComand + " " + this.ucNewGroup.GroupName+" "+user; //adding user - the client itself, it shouldnt choose himself.
            foreach (var item in this.ucNewGroup.UserList)
                newGroupData += " " + item;
            byte[] groupDataBytes = Encoding.ASCII.GetBytes(newGroupData);
            network.SendMessages(groupDataBytes);
        }

        //sending the server the chosen group name
        private void ChooseGroup(object sender, RoutedEventArgs e)
        {
            string chosenGroup = Convert.ToString((int)Network.eNetworkCommands.GroupIsOK) + " " + this.ucChatRoomList.ChosenGroup;
            byte[] groupNameBytes = Encoding.ASCII.GetBytes(chosenGroup);
            network.SendMessages(groupNameBytes);
        }

        //sending the server a chat message
        private void SendMessage (object sender, RoutedEventArgs e)
        {
            string message = Convert.ToString((int)Network.eNetworkCommands.Messages)+" "+this.ucChat.LastMessage;
            network.SendMessages(Encoding.ASCII.GetBytes(message));
        }

        #endregion

        #region Private Methods

        //handling data from the server (the data is "mess")
        private void NewConnection(string mess)
        {
            //invalid information - showing a suitable messagebox
            //login = the username doesnt exist or the password is incorrect
            //register = the username already exists
            if (mess == Convert.ToString((int)Network.eNetworkCommands.Error) && !flag)
            {
                MessageBox.Show("invalid information, try again");
            }

            //login or register information were valid. user is connected, username is saved. showing chatroomlist user control
            else if ((mess == Convert.ToString((int)Network.eNetworkCommands.LoginCommand) && !flag) || (mess == Convert.ToString((int)Network.eNetworkCommands.RegisterCommand) && !flag))
            {
                this.flag = true;
                MessageBox.Show("welcome! please choose a group to chat at or create a new one");
                if (mess == Convert.ToString((int)Network.eNetworkCommands.LoginCommand))
                    user = this.ucLogin.Details;
                else if (mess == Convert.ToString((int)Network.eNetworkCommands.RegisterCommand))
                    user = this.ucRegister.Details;
                this.HideAllUserControl();
                this.ChangeVisibility(this.ucChatRoomList, () => this.ucChatRoomList.Visibility = Visibility.Visible);
                this.ChangeVisibility(this.btnNewGroup, () => this.btnNewGroup.Visibility = Visibility.Visible);
            }

            //adding all the groups the user is a part of to the chatrooms list
            else if (mess[0] == ';')
            {
                mess = mess.Substring(1);
                this.ucChatRoomList.AddItems(mess.Split(';'));
            }

            //adding all the users to the listbox in the newgroup user control and showing the user control
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
                this.ucNewGroup.Init(usersList, user);
                this.ChangeVisibility(this.ucNewGroup, () => this.ucNewGroup.Visibility = Visibility.Visible);
            }

            //invalid groupname. showing a suitable messagebox
            else if (mess[0].ToString() == Convert.ToString((int)Network.eNetworkCommands.ErrorGroup))
            {
                MessageBox.Show("group name already exists");
            }

            //groupname is valid. showing shat user control
            else if (mess[0].ToString() == Convert.ToString((int)Network.eNetworkCommands.GroupIsOK))
            {
                MessageBox.Show("welcome, you can chat now");
                this.HideAllUserControl();
                this.ChangeVisibility(this.ucChat, () => this.ucChat.Visibility = Visibility.Visible);
                this.ucChat.Sender = user;
            }

            //connecting to a existing group - getting all the previous messages in this chatgroup, showing the chat user control and the previous messages.
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

            //showing a chat message gotten from another user
            else if (mess[0].ToString() == Convert.ToString((int)Network.eNetworkCommands.ChatMessage))
            {
                if (mess.Substring(2, user.Length) != user)
                    ucChat.AddMessage(mess.Substring(1));
            }

        }

        //hiding all the user controls (using invoking)
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

        //changing visibility using invoking (the uieEement is the element we invoke and the action is the changing visibility action)
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