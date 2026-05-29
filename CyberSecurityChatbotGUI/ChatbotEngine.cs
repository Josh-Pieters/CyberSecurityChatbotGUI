using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public class ChatbotEngine
    {
        // Memory stores the user's name and interests
        public MemoryStore Memory = new MemoryStore();

        // Remembers the last topic discussed for follow-ups
        private string _lastTopic = string.Empty;

        public ChatbotEngine(string userName)
        {
            Memory.UserName = userName;
        }

        // Main method — takes user input and returns a response
        public string GetResponse(string input)
        {
            // If the input is empty return a fallback message
            if (string.IsNullOrWhiteSpace(input))
            {
                return "🤔 I didn't catch that. Could you rephrase?\n" +
                       "💡 Try typing 'help' to see what I can do.";
            }

            // Step 1 - Detect the user's sentiment e.g. worried, curious
            Sentiment sentiment = SentimentDetector.Detect(input);
            string empathy = SentimentDetector.GetEmpathyPrefix(sentiment);

            // Step 2 - Check if the user is expressing an interest in a topic
            // e.g. "I'm interested in privacy"
            string learnedTopic = Memory.TryLearnInterest(input);

            if (learnedTopic != string.Empty)
            {
                // Acknowledge the interest and give a tip on that topic
                string ack = "💾 Great! I'll remember that you're interested in " + learnedTopic + ".\n" +
                             "It's a crucial part of staying safe online.\n\n";

                ResponseSelector selector = ResponseBank.GetRandomPicker(learnedTopic);
                string tip = selector(input);

                _lastTopic = learnedTopic;

                return ack + tip + "\n\n💬 Type 'tell me more' for another tip!";
            }

            // Step 3 - Check for follow-up requests e.g. "tell me more"
            if (IsFollowUp(input))
            {
                if (_lastTopic != string.Empty)
                {
                    string moreTip = ResponseBank.PickRandom(_lastTopic);
                    return empathy + "🔄 Here's another tip on " + _lastTopic + ":\n\n" + moreTip;
                }
                else
                {
                    return "💡 I'm not sure which topic to continue on.\n" +
                           "Could you remind me what you'd like more info about?";
                }
            }

            // Step 4 - Check exact match questions e.g. "how are you"
            foreach (KeyValuePair<string, string> entry in ResponseBank.ExactResponses)
            {
                if (input.IndexOf(entry.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return empathy + entry.Value;
                }
            }

            // Step 5 - Check for help command
            if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                return ResponseBank.HelpResponse;
            }

            // Step 6 - Check for keyword topics e.g. "password", "phishing"
            foreach (KeyValuePair<string, List<string>> entry in ResponseBank.TopicResponses)
            {
                if (input.IndexOf(entry.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Remember this topic for follow-up requests
                    _lastTopic = entry.Key;

                    // Use the delegate to pick a random response
                    ResponseSelector selector = ResponseBank.GetRandomPicker(entry.Key);
                    string response = selector(input);

                    // Add empathy prefix and personalised memory hint
                    string hint = Memory.GetPersonalisedHint();

                    return empathy + hint + response + "\n\n💬 Type 'tell me more' for another tip!";
                }
            }

            // Step 7 - Nothing matched, return a helpful fallback
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