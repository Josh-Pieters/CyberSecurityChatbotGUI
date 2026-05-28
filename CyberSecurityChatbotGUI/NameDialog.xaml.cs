using System.Windows;
using System.Windows.Input;

namespace CyberSecurityChatbot
{
    public partial class NameDialog : Window
    {
        // This stores the name so MainWindow can access it after the dialog closes
        public string EnteredName = string.Empty;

        public NameDialog()
        {
            InitializeComponent();
        }

        // When the Start button is clicked
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text.Trim();

            // If the textbox is empty, show the error message
            if (string.IsNullOrWhiteSpace(name))
            {
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            // Save the name and close the dialog
            EnteredName = name;
            DialogResult = true;
            Close();
        }

        // If the user presses Enter, treat it the same as clicking the button
        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StartBtn_Click(sender, e);
            }
        }
    }
}