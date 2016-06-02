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
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        //fields
        List<string> messages = new List<string>();
        string sender;
        string lastMessage = "";
        public event RoutedEventHandler SendMessageButtonClick; //event

        //constractor
        public Chat()
        {
            InitializeComponent();
        }

        //sender (usename) getting and setting
        public string Sender
        {
            set { sender = value; }
            get { return sender; }
        }

        //setting messages
        public void SetMessages (string[] values)
        {
                foreach(var item in values) 
                    this.messages.Add(item);
        }

        //adding the previous messages to the textbox
        public void ShowOldMessages()
        {
            ShowMessages(messagesLabel, () => messagesLabel.Text = "");
                foreach (var item in messages)
                    ShowMessages(messagesLabel, () => messagesLabel.Text = messagesLabel.Text  + item);
        }

        //lastmessage getting
        public string LastMessage
        {
            get { return lastMessage; }
        }

        //adding a message ("message") to the list and showing it on the textbox
        public void AddMessage(string message)
        {
            this.messages.Add(message);
            ShowMessages(messagesLabel, () => messagesLabel.Text = "");
            foreach (var item in messages)
                ShowMessages(messagesLabel, () => messagesLabel.Text = messagesLabel.Text + item);
        }

        //sending the message written in the textbox
        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = "        "+this.sender + ": " + messageTextBox.Text + " [" + DateTime.Now + "]"+ "\n";
            lastMessage = message;
            messageTextBox.Clear();
            if (SendMessageButtonClick!=null)
                SendMessageButtonClick(this, new RoutedEventArgs());
        }

        //showing messages using invoking (uiElement = the element we invoke, action = showing the messages)
        private void ShowMessages(UIElement uiElement, Action action)
        {
            if (!uiElement.Dispatcher.CheckAccess())
                uiElement.Dispatcher.Invoke(action);
            else
                action();
        }

    }
}
