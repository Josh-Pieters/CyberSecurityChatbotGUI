using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public static class ActivityLog
    {
        // Stores the last 10 actions as log entries
        private static List<string> _log = new List<string>();

        // Maximum number of entries to keep
        private const int MaxEntries = 10;

        // ── Add a new entry to the log ───────────────────────────────────────
        public static void Log(string action)
        {
            // Build the entry with a timestamp
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string entry = "[" + timestamp + "] " + action;

            // Add to the log
            _log.Add(entry);

            // If the log exceeds the max, remove the oldest entry
            if (_log.Count > MaxEntries)
            {
                _log.RemoveAt(0);
            }
        }

        // ── Get all log entries ──────────────────────────────────────────────
        public static List<string> GetLog()
        {
            return _log;
        }

        // ── Get the log as a formatted string for display ────────────────────
        public static string GetLogAsString()
        {
            if (_log.Count == 0)
            {
                return "📭 No activity recorded yet.\n" +
                       "Start chatting, add tasks, or take the quiz to see entries here!";
            }

            string result = "📜 Recent Activity (last " + _log.Count + " actions):\n\n";

            for (int i = 0; i < _log.Count; i++)
            {
                result = result + (i + 1) + ". " + _log[i] + "\n";
            }

            return result;
        }

        // ── Clear the log ────────────────────────────────────────────────────
        public static void Clear()
        {
            _log.Clear();
        }

        // ── Check if the log has any entries ─────────────────────────────────
        public static bool HasEntries()
        {
            return _log.Count > 0;
        }
    }
}