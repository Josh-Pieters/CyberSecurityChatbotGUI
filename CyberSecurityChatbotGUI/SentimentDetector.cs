using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    // The different moods we can detect
    public enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Happy,
        Confused
    }

    public static class SentimentDetector
    {
        private static Random _rng = new Random();

        // Keywords that indicate each sentiment
        private static Dictionary<Sentiment, List<string>> _keywords = new Dictionary<Sentiment, List<string>>
        {
            {
                Sentiment.Worried, new List<string>
                {
                    "worried", "scared", "afraid", "nervous", "anxious",
                    "fear", "terrified", "concerned", "unsafe", "threatened"
                }
            },
            {
                Sentiment.Curious, new List<string>
                {
                    "curious", "wondering", "interested", "want to know",
                    "how does", "tell me about", "explain", "learn"
                }
            },
            {
                Sentiment.Frustrated, new List<string>
                {
                    "frustrated", "annoyed", "angry", "upset", "fed up",
                    "useless", "hate", "stupid", "not working", "waste"
                }
            },
            {
                Sentiment.Happy, new List<string>
                {
                    "happy", "great", "awesome", "love", "excellent",
                    "amazing", "thank", "thanks", "helpful", "wonderful"
                }
            },
            {
                Sentiment.Confused, new List<string>
                {
                    "confused", "don't understand", "not sure", "lost",
                    "unclear", "what do you mean", "i don't get it"
                }
            }
        };

        // Empathetic responses for each sentiment
        // Multiple options so it doesn't always say the same thing
        private static Dictionary<Sentiment, List<string>> _empathyPrefixes = new Dictionary<Sentiment, List<string>>
        {
            {
                Sentiment.Worried, new List<string>
                {
                    "💙 It's completely understandable to feel that way. You're not alone — let me help put your mind at ease.\n\n",
                    "💙 I hear you and your concern is valid. Scammers can be very convincing. Here's what you should know:\n\n",
                    "💙 Feeling worried about online safety is actually a healthy response — it means you're paying attention. Here are some tips:\n\n"
                }
            },
            {
                Sentiment.Curious, new List<string>
                {
                    "🌟 Great question! I love the curiosity — here's what you need to know:\n\n",
                    "🌟 Fantastic that you're keen to learn more. Here's the scoop:\n\n",
                    "🌟 Curiosity is the first step to staying safe online! Here's some useful info:\n\n"
                }
            },
            {
                Sentiment.Frustrated, new List<string>
                {
                    "😤 I understand your frustration — cybersecurity can feel overwhelming. Let me try to help:\n\n",
                    "😤 I'm sorry you're having a hard time. Let me break this down simply:\n\n",
                    "😤 That sounds really frustrating. Let's work through this together:\n\n"
                }
            },
            {
                Sentiment.Happy, new List<string>
                {
                    "😊 Glad to hear you're in good spirits! Here's some useful info:\n\n",
                    "😊 Love the positive energy! Here's what I've got for you:\n\n"
                }
            },
            {
                Sentiment.Confused, new List<string>
                {
                    "🤔 No worries at all — let me explain this as clearly as possible:\n\n",
                    "🤔 Happy to clear that up! Here's a simple breakdown:\n\n",
                    "🤔 Confusion is totally normal with this topic. Let me walk you through it:\n\n"
                }
            }
        };

        // Scans input and returns the detected sentiment
        public static Sentiment Detect(string input)
        {
            string lower = input.ToLower();

            foreach (KeyValuePair<Sentiment, List<string>> entry in _keywords)
            {
                foreach (string keyword in entry.Value)
                {
                    if (lower.Contains(keyword))
                    {
                        return entry.Key;
                    }
                }
            }

            // No sentiment detected
            return Sentiment.Neutral;
        }

        // Returns a random empathetic prefix for the given sentiment
        // Returns empty string if sentiment is neutral
        public static string GetEmpathyPrefix(Sentiment sentiment)
        {
            if (sentiment == Sentiment.Neutral)
                return string.Empty;

            if (_empathyPrefixes.ContainsKey(sentiment))
            {
                List<string> options = _empathyPrefixes[sentiment];
                int index = _rng.Next(options.Count);
                return options[index];
            }

            return string.Empty;
        }
    }
}