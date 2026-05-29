using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public class MemoryStore
    {
        // Stores the user's name
        public string UserName = string.Empty;

        // General purpose memory storage - stores any key/value pair
        // e.g. "favourite_topic" -> "privacy"
        private Dictionary<string, string> _memory = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Phrases that signal the user is expressing interest in a topic
        private List<string> _interestPhrases = new List<string>
        {
            "i'm interested in",
            "i am interested in",
            "i care about",
            "i want to learn about",
            "i love",
            "my favourite topic is",
            "i worry about",
            "i'm concerned about"
        };

        // Topics to watch for when detecting interest
        private List<string> _knownTopics = new List<string>
        {
            "password",
            "phishing",
            "safe browsing",
            "malware",
            "social engineering",
            "2fa",
            "privacy",
            "scam"
        };

        // Saves a value into memory
        public void Remember(string key, string value)
        {
            _memory[key] = value;
        }

        // Retrieves a value from memory
        // Returns empty string if not found
        public string Recall(string key)
        {
            if (_memory.ContainsKey(key))
                return _memory[key];
            else
                return string.Empty;
        }

        // Checks if a key exists in memory
        public bool HasMemory(string key)
        {
            return _memory.ContainsKey(key);
        }

        // Scans the user's input to detect if they mentioned an interest
        // e.g. "I'm interested in privacy" -> saves "privacy" as favourite topic
        // Returns the topic name if found, empty string if not
        public string TryLearnInterest(string input)
        {
            string lower = input.ToLower();

            foreach (string phrase in _interestPhrases)
            {
                // Check if the input contains this interest phrase
                int index = lower.IndexOf(phrase, StringComparison.OrdinalIgnoreCase);

                if (index >= 0)
                {
                    // Get everything after the interest phrase
                    string afterPhrase = lower.Substring(index + phrase.Length).Trim();

                    // Check if a known topic appears after the phrase
                    foreach (string topic in _knownTopics)
                    {
                        if (afterPhrase.Contains(topic))
                        {
                            // Save it to memory and return it
                            Remember("favourite_topic", topic);
                            return topic;
                        }
                    }
                }
            }

            // Nothing found
            return string.Empty;
        }

        // Builds a personalised hint to add to responses
        // e.g. "Since you're interested in privacy, this is especially relevant!"
        public string GetPersonalisedHint()
        {
            string topic = Recall("favourite_topic");

            if (topic != string.Empty && UserName != string.Empty)
            {
                return "💾 Since you're interested in " + topic + ", " + UserName + ", this tip is especially relevant for you!\n\n";
            }
            else if (topic != string.Empty)
            {
                return "💾 Since you're interested in " + topic + ", this tip is especially relevant!\n\n";
            }

            // No favourite topic stored yet
            return string.Empty;
        }
    }
}