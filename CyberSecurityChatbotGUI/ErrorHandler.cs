using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public static class ErrorHandler
    {
        private static Random _rng = new Random();

        // Different fallback messages so it doesn't always say the same thing
        private static List<string> _fallbackMessages = new List<string>
        {
            "🤔 I'm not sure I understand that.\n" +
            "💡 Try typing 'help' to see a list of topics I can assist with.",

            "🤖 I didn't quite catch that. Could you rephrase?\n" +
            "💡 You can ask me about: password, phishing, malware, privacy, or scams.",

            "❓ Hmm, I'm not familiar with that topic.\n" +
            "💡 Type 'help' to see everything I can help you with.",

            "🔍 I couldn't find anything on that.\n" +
            "💡 Try asking about: safe browsing, 2fa, social engineering, or phishing."
        };

        // Returns a random fallback message for unrecognised input
        public static string GetFallback()
        {
            int index = _rng.Next(_fallbackMessages.Count);
            return _fallbackMessages[index];
        }

        // Checks if the input looks like it could be a cybersecurity topic
        // but was just typed slightly wrong
        // e.g. "pasword" instead of "password"
        public static string GetTypoSuggestion(string input)
        {
            string lower = input.ToLower();

            // Common typos and near-matches mapped to the correct topic
            Dictionary<string, string> typoMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "pasword",    "password" },
                { "passward",   "password" },
                { "passwrod",   "password" },
                { "phising",    "phishing" },
                { "phshing",    "phishing" },
                { "malwear",    "malware" },
                { "malwre",     "malware" },
                { "privcy",     "privacy" },
                { "privacy",    "privacy" },
                { "scam",       "scam" },
                { "browing",    "safe browsing" },
                { "browsinng",  "safe browsing" }
            };

            foreach (KeyValuePair<string, string> entry in typoMap)
            {
                if (lower.Contains(entry.Key))
                {
                    return "💡 Did you mean '" + entry.Value + "'? Try typing that and I'll help you out!";
                }
            }

            // No typo detected
            return string.Empty;
        }

        // Validates that the input is safe to process
        // Returns an error message if something is wrong, empty string if fine
        public static string ValidateInput(string input)
        {
            // Check if input is empty
            if (string.IsNullOrWhiteSpace(input))
            {
                return "🤖 Please type something so I can help you!";
            }

            // Check if input is too long
            if (input.Length > 500)
            {
                return "⚠️ That message is too long. Please keep it under 500 characters.";
            }

            // Check if input is just numbers with no letters
            bool hasLetter = false;
            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    hasLetter = true;
                    break;
                }
            }

            if (!hasLetter)
            {
                return "🤖 I only understand text. Please type a question or topic name!";
            }

            // Input looks fine
            return string.Empty;
        }
    }
}