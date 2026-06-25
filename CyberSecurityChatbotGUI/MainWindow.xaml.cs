using CyberSecurityChatbotGUI;
using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CyberSecurityChatbot
{
    public partial class MainWindow : Window
    {
        // Stores the user's name entered in the dialog
        private string _userName = string.Empty;
        private ChatbotEngine _engine = null!;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        // Runs when the window first opens
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PlayVoiceGreeting();
            AskForName();
        }

        // Plays the WAV greeting file
        private void PlayVoiceGreeting()
        {
            string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

            if (File.Exists(audioPath))
            {
                SoundPlayer player = new SoundPlayer(audioPath);
                player.Play();
            }
        }

        // Opens the name dialog and shows the welcome message
        private void AskForName()
        {
            NameDialog dialog = new NameDialog();
            dialog.Owner = this;
            dialog.ShowDialog();

            // If they entered a name use it, otherwise default to "User"
            if (string.IsNullOrWhiteSpace(dialog.EnteredName))
                _userName = "User";
            else
                _userName = dialog.EnteredName;

            _engine = new ChatbotEngine(_userName);

            // Show welcome message in the chat
            AddBotMessage("👋 Hello, " + _userName + "! I'm CyberBot — your cybersecurity assistant.\n\nLogic coming soon — stay tuned!");

            // Update the status bar at the bottom
            StatusText.Text = "🔒 Chatting as " + _userName;

            //Checks if the database is connected and shows a message in the chat
            if (DatabaseHelper.TestConnection())
                AddBotMessage("✅ Database connected successfully!");
            else
                AddBotMessage("❌ Database connection failed. Check SQL Server is running.");
        }

        // Send button clicked
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        // Enter key pressed in the input box
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        // Updates the placeholder and character counter as the user types
        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(InputBox.Text))
                Placeholder.Visibility = Visibility.Visible;
            else
                Placeholder.Visibility = Visibility.Collapsed;

            CharCount.Text = InputBox.Text.Length + " / 500";
        }

        // Quick topic chip button clicked
        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string topic = btn.Tag.ToString();
            InputBox.Text = topic;
            SendMessage();
        }

        // Core send logic — gets input, shows user bubble, gets bot response
        private void SendMessage()
        {
            string input = InputBox.Text.Trim();

            // Do nothing if the input is empty
            if (string.IsNullOrWhiteSpace(input))
                return;

            // Show the user's message
            AddUserMessage(input);
            
            // Clear the input box
            InputBox.Clear();

            // Get the response from the engine
            string response = _engine.GetResponse(input);

            // Show the bot's response
            AddBotMessage(response);

            // Update the status bar with the last topic if there was one
            if (_engine.Memory.HasMemory("favourite_topic"))
            {
                string topic = _engine.Memory.Recall("favourite_topic");
                StatusText.Text = "🔒 " + _userName + " | Favourite topic: " + topic;
            }

            // Scroll to the bottom
            ChatScroll.ScrollToEnd();
        }

        // Creates and adds a green user message bubble to the chat
        private void AddUserMessage(string text)
        {
            // Outer container pushes bubble to the right
            Grid container = new Grid();
            container.Margin = new Thickness(60, 4, 4, 4);

            // The bubble itself
            Border bubble = new Border();
            bubble.Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x3A, 0x2A));
            bubble.CornerRadius = new CornerRadius(14, 14, 4, 14);
            bubble.Padding = new Thickness(14, 10, 14, 10);
            bubble.HorizontalAlignment = HorizontalAlignment.Right;
            bubble.BorderBrush = new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53));
            bubble.BorderThickness = new Thickness(1);

            // Stack holds the name label and message text
            StackPanel stack = new StackPanel();

            // Name label e.g. "👤 John"
            TextBlock nameLabel = new TextBlock();
            nameLabel.Text = "👤 " + _userName;
            nameLabel.Foreground = new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53));
            nameLabel.FontSize = 11;
            nameLabel.FontFamily = new FontFamily("Consolas");
            nameLabel.FontWeight = FontWeights.Bold;
            nameLabel.Margin = new Thickness(0, 0, 0, 4);

            // The actual message
            TextBlock message = new TextBlock();
            message.Text = text;
            message.Foreground = new SolidColorBrush(Color.FromRgb(0xE6, 0xED, 0xF3));
            message.FontSize = 13;
            message.FontFamily = new FontFamily("Consolas");
            message.TextWrapping = TextWrapping.Wrap;

            // Add label and message into the stack
            stack.Children.Add(nameLabel);
            stack.Children.Add(message);

            // Put the stack inside the bubble, bubble inside container
            bubble.Child = stack;
            container.Children.Add(bubble);

            // Add to the chat panel
            ChatPanel.Children.Add(container);
        }

        // Creates and adds a blue CyberBot message bubble to the chat
        private void AddBotMessage(string text)
        {
            // Outer container pushes bubble to the left
            Grid container = new Grid();
            container.Margin = new Thickness(4, 4, 60, 4);

            // The bubble itself
            Border bubble = new Border();
            bubble.Background = new SolidColorBrush(Color.FromRgb(0x1F, 0x3A, 0x4A));
            bubble.CornerRadius = new CornerRadius(14, 14, 14, 4);
            bubble.Padding = new Thickness(14, 10, 14, 10);
            bubble.HorizontalAlignment = HorizontalAlignment.Left;
            bubble.BorderBrush = new SolidColorBrush(Color.FromRgb(0x00, 0xD4, 0xFF));
            bubble.BorderThickness = new Thickness(1);
            bubble.MaxWidth = 650;

            // Stack holds the bot name label and message text
            StackPanel stack = new StackPanel();

            // Bot name label
            TextBlock nameLabel = new TextBlock();
            nameLabel.Text = "🤖 CyberBot";
            nameLabel.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0xD4, 0xFF));
            nameLabel.FontSize = 11;
            nameLabel.FontFamily = new FontFamily("Consolas");
            nameLabel.FontWeight = FontWeights.Bold;
            nameLabel.Margin = new Thickness(0, 0, 0, 4);

            // The actual message
            TextBlock message = new TextBlock();
            message.Text = text;
            message.Foreground = new SolidColorBrush(Color.FromRgb(0xE6, 0xED, 0xF3));
            message.FontSize = 13;
            message.FontFamily = new FontFamily("Consolas");
            message.TextWrapping = TextWrapping.Wrap;
            message.LineHeight = 20;

            // Add label and message into the stack
            stack.Children.Add(nameLabel);
            stack.Children.Add(message);

            // Put the stack inside the bubble, bubble inside container
            bubble.Child = stack;
            container.Children.Add(bubble);

            // Add to the chat panel
            ChatPanel.Children.Add(container);

            // Fade the bubble in smoothly
            bubble.Opacity = 0;
            DoubleAnimation fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250));
            bubble.BeginAnimation(UIElement.OpacityProperty, fade);
        }
    }
}