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
            // Step 1 - Validate the input first
            string validationError = ErrorHandler.ValidateInput(input);
            if (validationError != string.Empty)
            {
                return validationError;
            }

            // Step 2 - Run NLP intent detection first
            // This catches natural language requests before keyword matching
            NlpIntent intent = NlpProcessor.DetectIntent(input);
            if (intent != NlpIntent.None)
            {

                if (intent == NlpIntent.ShowActivityLog)
                {
                    return ActivityLog.GetLogAsString();
                }

                string nlpResponse = NlpProcessor.BuildIntentResponse(intent, input, Memory.UserName);
                if (nlpResponse != string.Empty)
                {
                    return nlpResponse;
                }
            }

            // Step 3 - Detect the user's sentiment e.g. worried, curious
            Sentiment sentiment = SentimentDetector.Detect(input);
            string empathy = SentimentDetector.GetEmpathyPrefix(sentiment);

            // Step 4 - Check if the user is expressing an interest in a topic
            // e.g. "I'm interested in privacy"
            string learnedTopic = Memory.TryLearnInterest(input);

            if (learnedTopic != string.Empty)
            {
                string ack = "💾 Great! I'll remember that you're interested in " + learnedTopic + ".\n" +
                             "It's a crucial part of staying safe online.\n\n";

                ResponseSelector selector = ResponseBank.GetRandomPicker(learnedTopic);
                string tip = selector(input);

                _lastTopic = learnedTopic;

                return ack + tip + "\n\n💬 Type 'tell me more' for another tip!";
            }

            // Step 5 - Check for follow-up requests e.g. "tell me more"
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

            // Step 6 - Check exact match questions e.g. "how are you"
            foreach (KeyValuePair<string, string> entry in ResponseBank.ExactResponses)
            {
                if (input.IndexOf(entry.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return empathy + entry.Value;
                }
            }

            // Step 7 - Check for help command
            if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                return ResponseBank.HelpResponse;
            }

            // Step 8 - Check for keyword topics e.g. "password", "phishing"
            foreach (KeyValuePair<string, List<string>> entry in ResponseBank.TopicResponses)
            {
                if (input.IndexOf(entry.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    _lastTopic = entry.Key;

                    ResponseSelector selector = ResponseBank.GetRandomPicker(entry.Key);
                    string response = selector(input);

                    string hint = Memory.GetPersonalisedHint();

                    return empathy + hint + response + "\n\n💬 Type 'tell me more' for another tip!";
                }
            }

            // Step 9 - Check for typos before giving up
            string typoSuggestion = ErrorHandler.GetTypoSuggestion(input);
            if (typoSuggestion != string.Empty)
            {
                return typoSuggestion;
            }

            // Step 10 - Nothing matched, return a random fallback message
            return ErrorHandler.GetFallback();
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