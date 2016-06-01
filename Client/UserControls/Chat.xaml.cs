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
        List<string> messages = new List<string>();
        string sender;
        string lastMessage = "";
        public event RoutedEventHandler SendMessageButtonClick;
        public Chat()
        {
            InitializeComponent();
        }
        public string Sender
        {
            set { sender = value; }
            get { return sender; }
        }
        public void SetMessages (string[] values)
        {
                foreach(var item in values) 
                    this.messages.Add(item);
        }
        public void ShowOldMessages()
        {
                foreach (var item in messages)
                    ShowMessages(messagesLabel, () => messagesLabel.Text = messagesLabel.Text + "\n" + item);
        }
        public string LastMessage
        {
            get { return lastMessage; }
        }
        public void AddMessage(string message)
        {
            this.messages.Add(message);
            ShowMessages(messagesLabel, () => messagesLabel.Text =  messagesLabel.Text +"\n"+ message ); 

        }
        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = this.sender + ": " + messageTextBox.Text + " [" + DateTime.Now + "]";
            lastMessage = message;
            messageTextBox.Clear();
            if (SendMessageButtonClick!=null)
                SendMessageButtonClick(this, new RoutedEventArgs());
        }
        private void ShowMessages(UIElement uiElement, Action action)
        {
            if (!uiElement.Dispatcher.CheckAccess())
                uiElement.Dispatcher.Invoke(action);
            else
                action();
        }

    }
}
