using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public class ChatbotEngine
    {
        // Stores the user's name
        private string _userName = string.Empty;

        // Remembers the last topic so follow-ups work
        private string _lastTopic = string.Empty;

        public ChatbotEngine(string userName)
        {
            _userName = userName;
        }

        // Takes user input and returns a response
        public string GetResponse(string input)
        {
            // If the input is empty return a fallback message
            if (string.IsNullOrWhiteSpace(input))
            {
                return "🤔 I didn't catch that. Could you rephrase?\n" +
                       "💡 Try typing 'help' to see what I can do.";
            }

            // Check for follow-up requests e.g. "tell me more"
            if (IsFollowUp(input))
            {
                if (_lastTopic != string.Empty)
                {
                    string moreTip = ResponseBank.PickRandom(_lastTopic);
                    return "🔄 Here's another tip on " + _lastTopic + ":\n\n" + moreTip;
                }
                else
                {
                    return "💡 I'm not sure which topic to continue on.\n" +
                           "Could you remind me what you'd like more info about?";
                }
            }

            // Step 2 - Check exact match questions e.g. "how are you"
            foreach (KeyValuePair<string, string> entry in ResponseBank.ExactResponses)
            {
                if (input.IndexOf(entry.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return entry.Value;
                }
            }

            // Step 3 - Check for help command
            if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                return ResponseBank.HelpResponse;
            }

            // Step 4 - Check for keyword topics e.g. "password", "phishing"
            foreach (KeyValuePair<string, List<string>> entry in ResponseBank.TopicResponses)
            {
                if (input.IndexOf(entry.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Remember this topic for follow-up requests
                    _lastTopic = entry.Key;

                    // Use the delegate to pick a random response
                    ResponseSelector selector = ResponseBank.GetRandomPicker(entry.Key);
                    string response = selector(input);

                    return response + "\n\n💬 Want to know more? Type 'tell me more' or ask about another topic!";
                }
            }

            // Step 5 - Nothing matched, return a helpful fallback
            return "🤔 I'm not sure I understand that.\n" +
                   "💡 Try typing 'help' to see a list of topics I can assist with.";
        }

        // Checks if the user is asking for a follow-up on the last topic
        private bool IsFollowUp(string input)
        {
            string lower = input.ToLower().Trim();

            foreach (string keyword in ResponseBank.FollowUpKeywords)
            {
                if (lower.Contains(keyword))
                {
                    return true;
                }
            }

            return false;
        }
    }
}