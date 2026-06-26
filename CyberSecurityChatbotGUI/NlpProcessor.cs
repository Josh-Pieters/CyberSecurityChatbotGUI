using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    // The different intents the NLP can detect
    public enum NlpIntent
    {
        None,
        AddTask,
        ViewTasks,
        SetReminder,
        StartQuiz,
        ShowActivityLog,
        AskCybersecurityTopic,
        GreetBot,
        ThankBot
    }

    public static class NlpProcessor
    {
        // ── Intent keyword maps ──────────────────────────────────────────────
        // Each intent has a list of phrases that signal it
        private static Dictionary<NlpIntent, List<string>> _intentKeywords =
            new Dictionary<NlpIntent, List<string>>
        {
            {
                NlpIntent.AddTask, new List<string>
                {
                    "add a task",
                    "add task",
                    "create a task",
                    "create task",
                    "new task",
                    "make a task",
                    "i need to",
                    "remind me to",
                    "set a task",
                    "log a task"
                }
            },
            {
                NlpIntent.ViewTasks, new List<string>
                {
                    "view tasks",
                    "show tasks",
                    "see my tasks",
                    "list tasks",
                    "what are my tasks",
                    "show my tasks",
                    "my tasks",
                    "pending tasks",
                    "task list"
                }
            },
            {
                NlpIntent.SetReminder, new List<string>
                {
                    "set a reminder",
                    "remind me",
                    "set reminder",
                    "add a reminder",
                    "reminder for",
                    "don't let me forget",
                    "notify me"
                }
            },
            {
                NlpIntent.StartQuiz, new List<string>
                {
                    "start quiz",
                    "take the quiz",
                    "begin quiz",
                    "quiz me",
                    "test my knowledge",
                    "start the quiz",
                    "i want to do the quiz",
                    "open quiz",
                    "play quiz"
                }
            },
            {
                NlpIntent.ShowActivityLog, new List<string>
                {
                    "show activity log",
                    "activity log",
                    "what have you done",
                    "show log",
                    "view log",
                    "recent actions",
                    "what have you done for me",
                    "show history",
                    "what did you do"
                }
            },
            {
                NlpIntent.GreetBot, new List<string>
                {
                    "hello",
                    "hi",
                    "hey",
                    "good morning",
                    "good afternoon",
                    "good evening",
                    "howzit",
                    "greetings"
                }
            },
            {
                NlpIntent.ThankBot, new List<string>
                {
                    "thank you",
                    "thanks",
                    "thank u",
                    "appreciate it",
                    "that helped",
                    "cheers"
                }
            }
        };

        // ── Detect the intent from the user's input ──────────────────────────
        // Returns the best matching intent or NlpIntent.None
        public static NlpIntent DetectIntent(string input)
        {
            string lower = input.ToLower().Trim();

            foreach (KeyValuePair<NlpIntent, List<string>> entry in _intentKeywords)
            {
                foreach (string phrase in entry.Value)
                {
                    if (lower.Contains(phrase))
                    {
                        return entry.Key;
                    }
                }
            }

            return NlpIntent.None;
        }

        // ── Extract a topic from the input if one is mentioned ───────────────
        // e.g. "Can you remind me to update my password?" -> "password"
        public static string ExtractTopic(string input)
        {
            string lower = input.ToLower();

            List<string> knownTopics = new List<string>
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

            foreach (string topic in knownTopics)
            {
                if (lower.Contains(topic))
                {
                    return topic;
                }
            }

            return string.Empty;
        }

        // ── Extract a task title from a natural language request ─────────────
        // e.g. "Add a task to enable two-factor authentication" -> "Enable two-factor authentication"
        public static string ExtractTaskTitle(string input)
        {
            string lower = input.ToLower();

            // List of prefixes to strip to find the actual task
            List<string> prefixesToStrip = new List<string>
            {
                "add a task to ",
                "add task to ",
                "add a task - ",
                "add task - ",
                "create a task to ",
                "create task to ",
                "new task - ",
                "new task to ",
                "i need to ",
                "remind me to ",
                "set a task to ",
                "log a task to "
            };

            foreach (string prefix in prefixesToStrip)
            {
                if (lower.StartsWith(prefix))
                {
                    // Return the original input with the prefix removed
                    // Use original input (not lower) to preserve casing
                    string extracted = input.Substring(prefix.Length).Trim();

                    // Capitalise the first letter
                    if (extracted.Length > 0)
                    {
                        extracted = char.ToUpper(extracted[0]) + extracted.Substring(1);
                    }

                    return extracted;
                }
            }

            // Could not extract a clean title
            return string.Empty;
        }

        // ── Extract a reminder timeframe from input ──────────────────────────
        // e.g. "remind me in 3 days" -> "in 3 days"
        public static string ExtractReminder(string input)
        {
            string lower = input.ToLower();

            // Common reminder timeframe patterns
            List<string> reminderPhrases = new List<string>
            {
                "in 1 day",
                "in 2 days",
                "in 3 days",
                "in 4 days",
                "in 5 days",
                "in 6 days",
                "in 7 days",
                "in a week",
                "in one week",
                "tomorrow",
                "next week",
                "in a month",
                "today"
            };

            foreach (string phrase in reminderPhrases)
            {
                if (lower.Contains(phrase))
                {
                    return phrase;
                }
            }

            return string.Empty;
        }

        // ── Build a natural response for a detected intent ───────────────────
        public static string BuildIntentResponse(NlpIntent intent, string input, string userName)
        {
            string extractedTopic = ExtractTopic(input);
            string extractedTitle = ExtractTaskTitle(input);
            string extractedReminder = ExtractReminder(input);

            switch (intent)
            {
                case NlpIntent.AddTask:
                    if (extractedTitle != string.Empty)
                    {
                        return "📋 I detected a task request!\n\n" +
                               "Suggested title: \"" + extractedTitle + "\"\n\n" +
                               "Head over to the 📋 Tasks tab to save this task. " +
                               "I've pre-filled the title for you based on what you said!";
                    }
                    return "📋 It sounds like you want to add a task!\n" +
                           "Head over to the 📋 Tasks tab to add and manage your cybersecurity tasks.";

                case NlpIntent.ViewTasks:
                    return "📋 You can view all your tasks in the 📋 Tasks tab!\n" +
                           "Click it at the top to see your pending and completed tasks.";

                case NlpIntent.SetReminder:
                    if (extractedReminder != string.Empty)
                    {
                        return "⏰ Got it! I detected a reminder request for: " + extractedReminder + ".\n\n" +
                               "Head to the 📋 Tasks tab, add your task, and enter '" +
                               extractedReminder + "' in the Reminder field!";
                    }
                    return "⏰ Sure! Head to the 📋 Tasks tab and fill in the Reminder field " +
                           "when adding a task. You can enter something like 'in 3 days' or 'tomorrow'.";

                case NlpIntent.StartQuiz:
                    return "🎮 Great! Head over to the 🎮 Quiz tab at the top and click " +
                           "'▶ Start Quiz' to test your cybersecurity knowledge!";

                case NlpIntent.ShowActivityLog:
                    return "📜 Head over to the 📜 Log tab to see a full record of recent actions!\n" +
                           "It tracks tasks added, quiz attempts, and more.";

                case NlpIntent.GreetBot:
                    return "👋 Hey there, " + userName + "! Great to hear from you.\n" +
                           "How can I help you stay safe online today?\n" +
                           "💡 Try asking about phishing, passwords, or type 'help' for all topics!";

                case NlpIntent.ThankBot:
                    return "😊 You're welcome, " + userName + "! " +
                           "Staying safe online is what I'm here for.\n" +
                           "Let me know if you have any more questions!";

                default:
                    return string.Empty;
            }
        }
    }
}