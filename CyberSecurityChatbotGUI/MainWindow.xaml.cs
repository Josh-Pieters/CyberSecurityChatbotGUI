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
        // Quiz state fields
        private List<QuizQuestion> _quizQuestions = new List<QuizQuestion>();
        private int _currentQuestionIndex = 0;
        private int _score = 0;
        private bool _quizActive = false;
        private bool _answerSelected = false;

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
            LoadTaskList();
            LoadActivityLog();
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

            ActivityLog.Log("Session started for user: " + _userName);

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

            //Adds log to the activity log for the chat message sent
            ActivityLog.Log("Chat message sent: \"" + input + "\"");

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

        // Save task button clicked
        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleBox.Text.Trim();
            string description = TaskDescriptionBox.Text.Trim();
            string reminder = TaskReminderBox.Text.Trim();

            // Validate title and description
            if (string.IsNullOrWhiteSpace(title))
            {
                ShowTaskFeedback("⚠️ Please enter a title for the task.", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                ShowTaskFeedback("⚠️ Please enter a description for the task.", false);
                return;
            }

            // Save to database
            bool saved = DatabaseHelper.AddTask(title, description, reminder);

            if (saved)
            {
                ShowTaskFeedback("✅ Task saved successfully!", true);

                //Adds log when task is saved
                ActivityLog.Log("Task added: \"" + title + "\"" +
                (string.IsNullOrWhiteSpace(reminder) ? "" : " | Reminder: " + reminder));

                // Clear the form
                TaskTitleBox.Clear();
                TaskDescriptionBox.Clear();
                TaskReminderBox.Clear();

                // Refresh the task list
                LoadTaskList();
            }
            else
            {
                ShowTaskFeedback("❌ Failed to save task. Check your database connection.", false);
            }
        }

        // Refresh task list button clicked
        private void RefreshTasks_Click(object sender, RoutedEventArgs e)
        {
            LoadTaskList();
            ShowTaskFeedback("🔄 Task list refreshed.", true);
        }

        // Loads all tasks from the database and displays them
        private void LoadTaskList()
        {
            // Clear the current list
            TaskListPanel.Children.Clear();

            List<CyberTask> tasks = DatabaseHelper.GetAllTasks();

            // Show a message if there are no tasks yet
            if (tasks.Count == 0)
            {
                TextBlock empty = new TextBlock();
                empty.Text = "📭 No tasks yet. Add one using the form above!";
                empty.Foreground = new SolidColorBrush(Color.FromRgb(0x8B, 0x94, 0x9E));
                empty.FontSize = 13;
                empty.FontFamily = new FontFamily("Consolas");
                empty.Margin = new Thickness(8);
                TaskListPanel.Children.Add(empty);
                return;
            }

            // Build a card for each task
            foreach (CyberTask task in tasks)
            {
                Border card = BuildTaskCard(task);
                TaskListPanel.Children.Add(card);
            }
        }

        // Builds a styled card UI element for a single task
        private Border BuildTaskCard(CyberTask task)
        {
            // Card border
            Border card = new Border();
            card.Background = new SolidColorBrush(Color.FromRgb(0x1C, 0x23, 0x33));
            card.CornerRadius = new CornerRadius(8);
            card.BorderBrush = task.IsCompleted
                ? new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53))
                : new SolidColorBrush(Color.FromRgb(0x00, 0xD4, 0xFF));
            card.BorderThickness = new Thickness(1);
            card.Padding = new Thickness(14, 10, 14, 10);
            card.Margin = new Thickness(0, 0, 0, 8);

            // Main stack inside the card
            StackPanel stack = new StackPanel();

            // Top row — title and status
            Grid topRow = new Grid();
            topRow.Margin = new Thickness(0, 0, 0, 6);

            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = GridLength.Auto;
            topRow.ColumnDefinitions.Add(col1);
            topRow.ColumnDefinitions.Add(col2);

            // Task title
            TextBlock titleText = new TextBlock();
            titleText.Text = (task.IsCompleted ? "✅ " : "⏳ ") + task.Title;
            titleText.Foreground = task.IsCompleted
                ? new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53))
                : new SolidColorBrush(Color.FromRgb(0x00, 0xD4, 0xFF));
            titleText.FontSize = 14;
            titleText.FontWeight = FontWeights.Bold;
            titleText.FontFamily = new FontFamily("Consolas");
            Grid.SetColumn(titleText, 0);

            // Task ID label
            TextBlock idText = new TextBlock();
            idText.Text = "ID: " + task.TaskId;
            idText.Foreground = new SolidColorBrush(Color.FromRgb(0x4A, 0x55, 0x68));
            idText.FontSize = 11;
            idText.FontFamily = new FontFamily("Consolas");
            idText.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetColumn(idText, 1);

            topRow.Children.Add(titleText);
            topRow.Children.Add(idText);

            // Description
            TextBlock descText = new TextBlock();
            descText.Text = "📝 " + task.Description;
            descText.Foreground = new SolidColorBrush(Color.FromRgb(0xE6, 0xED, 0xF3));
            descText.FontSize = 12;
            descText.FontFamily = new FontFamily("Consolas");
            descText.TextWrapping = TextWrapping.Wrap;
            descText.Margin = new Thickness(0, 0, 0, 4);

            // Reminder
            TextBlock reminderText = new TextBlock();
            string reminderDisplay = string.IsNullOrWhiteSpace(task.ReminderDate) ? "None" : task.ReminderDate;
            reminderText.Text = "⏰ Reminder: " + reminderDisplay;
            reminderText.Foreground = new SolidColorBrush(Color.FromRgb(0x8B, 0x94, 0x9E));
            reminderText.FontSize = 11;
            reminderText.FontFamily = new FontFamily("Consolas");
            reminderText.Margin = new Thickness(0, 0, 0, 8);

            // Action buttons row
            StackPanel btnRow = new StackPanel();
            btnRow.Orientation = Orientation.Horizontal;

            // Only show Complete button if task is not done
            if (!task.IsCompleted)
            {
                Button completeBtn = new Button();
                completeBtn.Content = "✅ Mark Complete";
                completeBtn.Style = (Style)FindResource("ActionBtn");
                completeBtn.Foreground = new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53));
                completeBtn.Tag = task.TaskId;
                completeBtn.Click += CompleteTask_Click;
                btnRow.Children.Add(completeBtn);
            }

            // Delete button
            Button deleteBtn = new Button();
            deleteBtn.Content = "🗑️ Delete";
            deleteBtn.Style = (Style)FindResource("ActionBtn");
            deleteBtn.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0x6B, 0x6B));
            deleteBtn.Tag = task.TaskId;
            deleteBtn.Click += DeleteTask_Click;
            btnRow.Children.Add(deleteBtn);

            // Add everything to the stack
            stack.Children.Add(topRow);
            stack.Children.Add(descText);
            stack.Children.Add(reminderText);
            stack.Children.Add(btnRow);

            card.Child = stack;
            return card;
        }

        // Mark task as complete button clicked
        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int taskId = (int)btn.Tag;

            bool success = DatabaseHelper.CompleteTask(taskId);

            if (success)
            {
                ShowTaskFeedback("✅ Task marked as complete!", true);
                ActivityLog.Log("Task ID " + taskId + " marked as completed.");
                LoadTaskList();
            }
            else
            {
                ShowTaskFeedback("❌ Could not complete task. Try again.", false);
            }
        }

        // Delete task button clicked
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int taskId = (int)btn.Tag;

            bool success = DatabaseHelper.DeleteTask(taskId);

            if (success)
            {
                ShowTaskFeedback("🗑️ Task deleted.", true);
                ActivityLog.Log("Task ID " + taskId + " deleted.");
                LoadTaskList();
            }
            else
            {
                ShowTaskFeedback("❌ Could not delete task. Try again.", false);
            }
        }

        // Shows a feedback message below the task form
        private void ShowTaskFeedback(string message, bool isSuccess)
        {
            TaskFeedback.Text = message;
            TaskFeedback.Foreground = isSuccess
                ? new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53))
                : new SolidColorBrush(Color.FromRgb(0xFF, 0x6B, 0x6B));
            TaskFeedback.Visibility = Visibility.Visible;
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

        // ── QUIZ TAB EVENTS ──────────────────────────────────────────────────

        // Start or restart the quiz
        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            ActivityLog.Log("Quiz started.");
            // Load and shuffle questions
            _quizQuestions = QuizBank.GetAllQuestions();
            _currentQuestionIndex = 0;
            _score = 0;
            _quizActive = true;
            _answerSelected = false;

            // Update UI
            StartQuizBtn.Content = "🔄 Restart Quiz";
            NextQuestionBtn.Visibility = Visibility.Collapsed;
            FeedbackBorder.Visibility = Visibility.Collapsed;

            // Show first question
            ShowQuestion(_currentQuestionIndex);
        }

        // Shows the question at the given index
        private void ShowQuestion(int index)
        {
            if (index >= _quizQuestions.Count)
            {
                ShowFinalScore();
                return;
            }

            QuizQuestion question = _quizQuestions[index];
            _answerSelected = false;

            // Update question text and counter
            QuestionText.Text = question.QuestionText;
            QuestionCounter.Text = "Question " + (index + 1) + " of " + _quizQuestions.Count;
            QuizSubtitle.Text = question.Type == QuestionType.TrueFalse
                ? "True / False Question"
                : "Multiple Choice Question";

            // Hide feedback and next button
            FeedbackBorder.Visibility = Visibility.Collapsed;
            NextQuestionBtn.Visibility = Visibility.Collapsed;

            // Clear old answer buttons and build new ones
            AnswerPanel.Children.Clear();

            foreach (string option in question.Options)
            {
                Button answerBtn = new Button();
                answerBtn.Content = option;
                answerBtn.Tag = option.Substring(0, 1); // Store just "A", "B", "C" or "D"
                answerBtn.Style = (Style)FindResource("ActionBtn");
                answerBtn.Foreground = new SolidColorBrush(Color.FromRgb(0xE6, 0xED, 0xF3));
                answerBtn.FontSize = 13;
                answerBtn.FontFamily = new FontFamily("Consolas");
                answerBtn.HorizontalAlignment = HorizontalAlignment.Stretch;
                answerBtn.HorizontalContentAlignment = HorizontalAlignment.Left;
                answerBtn.Margin = new Thickness(0, 0, 0, 6);
                answerBtn.Padding = new Thickness(14, 10, 14, 10);
                answerBtn.Click += AnswerBtn_Click;
                AnswerPanel.Children.Add(answerBtn);
            }
        }

        // Answer button clicked
        private void AnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            // Prevent answering twice
            if (_answerSelected)
                return;

            _answerSelected = true;

            Button clickedBtn = (Button)sender;
            string selectedAnswer = clickedBtn.Tag.ToString();
            QuizQuestion question = _quizQuestions[_currentQuestionIndex];

            bool isCorrect = selectedAnswer == question.CorrectAnswer;

            if (isCorrect)
                _score++;

            // Update score display
            ScoreText.Text = "Score: " + _score + " / " + _quizQuestions.Count;

            // Colour the clicked button green or red
            clickedBtn.Background = isCorrect
                ? new SolidColorBrush(Color.FromRgb(0x1A, 0x3A, 0x2A))
                : new SolidColorBrush(Color.FromRgb(0x3A, 0x1A, 0x1A));

            clickedBtn.BorderBrush = isCorrect
                ? new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53))
                : new SolidColorBrush(Color.FromRgb(0xFF, 0x6B, 0x6B));

            // Highlight the correct answer if they got it wrong
            if (!isCorrect)
            {
                foreach (Button btn in AnswerPanel.Children)
                {
                    if (btn.Tag.ToString() == question.CorrectAnswer)
                    {
                        btn.Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x3A, 0x2A));
                        btn.BorderBrush = new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53));
                    }
                }
            }

            // Show feedback
            string resultPrefix = isCorrect ? "✅ Correct! " : "❌ Incorrect. ";
            FeedbackText.Text = resultPrefix + question.Explanation;
            FeedbackText.Foreground = isCorrect
                ? new SolidColorBrush(Color.FromRgb(0x39, 0xD3, 0x53))
                : new SolidColorBrush(Color.FromRgb(0xFF, 0x6B, 0x6B));
            FeedbackBorder.Visibility = Visibility.Visible;

            // Show next button
            bool isLastQuestion = _currentQuestionIndex == _quizQuestions.Count - 1;
            NextQuestionBtn.Content = isLastQuestion ? "See Results 🏆" : "Next ➤";
            NextQuestionBtn.Visibility = Visibility.Visible;
        }

        // Next question button clicked
        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            _currentQuestionIndex++;

            if (_currentQuestionIndex >= _quizQuestions.Count)
                ShowFinalScore();
            else
                ShowQuestion(_currentQuestionIndex);
        }

        // Shows the final score screen
        private void ShowFinalScore()
        {
            int total = _quizQuestions.Count;
            int percentage = (_score * 100) / total;

            ActivityLog.Log("Quiz completed. Score: " + _score + "/" + total +
                    " (" + percentage + "%)");

            // Pick a feedback message based on the score
            string feedback = string.Empty;

            if (percentage == 100)
                feedback = "🏆 Perfect score! You're a cybersecurity expert!";
            else if (percentage >= 80)
                feedback = "🌟 Great job! You have strong cybersecurity knowledge!";
            else if (percentage >= 60)
                feedback = "👍 Good effort! Keep learning to stay safe online.";
            else if (percentage >= 40)
                feedback = "📚 Keep studying! Cybersecurity knowledge is important.";
            else
                feedback = "💪 Don't give up! Review the topics and try again.";

            // Display final results
            QuestionText.Text = "Quiz Complete!\n\n" +
                                "Your Score: " + _score + " out of " + total + "\n" +
                                "Percentage: " + percentage + "%\n\n" +
                                feedback;

            QuizSubtitle.Text = "Quiz finished!";
            QuestionCounter.Text = "All " + total + " questions answered";

            // Clear answer buttons and hide next button
            AnswerPanel.Children.Clear();
            NextQuestionBtn.Visibility = Visibility.Collapsed;
            FeedbackBorder.Visibility = Visibility.Collapsed;

            _quizActive = false;
        }

        // ── LOG TAB EVENTS ────────────────────────────────────────────────────

        // Refresh log button clicked
        private void RefreshLog_Click(object sender, RoutedEventArgs e)
        {
            LoadActivityLog();
        }

        // Clear log button clicked
        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            ActivityLog.Clear();
            LoadActivityLog();
        }

        // Loads and displays all log entries in the Log tab
        private void LoadActivityLog()
        {
            // Clear the current display
            LogPanel.Children.Clear();

            List<string> entries = ActivityLog.GetLog();

            // Update the entry counter
            LogCountText.Text = entries.Count + " entries";

            // Show empty message if no entries
            if (entries.Count == 0)
            {
                TextBlock empty = new TextBlock();
                empty.Text = "📭 No activity recorded yet.\n" +
                             "Start chatting, add tasks, or take the quiz!";
                empty.Foreground = new SolidColorBrush(Color.FromRgb(0x8B, 0x94, 0x9E));
                empty.FontSize = 13;
                empty.FontFamily = new FontFamily("Consolas");
                empty.Margin = new Thickness(8);
                LogPanel.Children.Add(empty);
                return;
            }

            // Build a card for each log entry
            // Show newest entries at the top by reversing the list
            List<string> reversed = new List<string>(entries);
            reversed.Reverse();

            int displayNumber = entries.Count;

            foreach (string entry in reversed)
            {
                Border card = new Border();
                card.Background = new SolidColorBrush(Color.FromRgb(0x1C, 0x23, 0x33));
                card.CornerRadius = new CornerRadius(6);
                card.BorderBrush = new SolidColorBrush(Color.FromRgb(0x30, 0x36, 0x3D));
                card.BorderThickness = new Thickness(1);
                card.Padding = new Thickness(12, 8, 12, 8);
                card.Margin = new Thickness(0, 0, 0, 6);

                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                // Entry number
                TextBlock number = new TextBlock();
                number.Text = displayNumber + ". ";
                number.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0xD4, 0xFF));
                number.FontSize = 12;
                number.FontFamily = new FontFamily("Consolas");
                number.FontWeight = FontWeights.Bold;
                number.Margin = new Thickness(0, 0, 6, 0);

                // Entry text
                TextBlock entryText = new TextBlock();
                entryText.Text = entry;
                entryText.Foreground = new SolidColorBrush(Color.FromRgb(0xE6, 0xED, 0xF3));
                entryText.FontSize = 12;
                entryText.FontFamily = new FontFamily("Consolas");
                entryText.TextWrapping = TextWrapping.Wrap;

                stack.Children.Add(number);
                stack.Children.Add(entryText);
                card.Child = stack;
                LogPanel.Children.Add(card);

                displayNumber--;
            }
        }
    }
 }
